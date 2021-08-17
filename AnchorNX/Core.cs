using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using IronVisor;

namespace AnchorNX {
	[StructLayout(LayoutKind.Sequential)]
	unsafe struct HosExceptionContext {
		public fixed ulong X[31];
		public ulong SP, PC, Spsr, Tpidr;
	}
	
	public class Core {
		static readonly Logger Logger = new("Core");
		static Action<string> Log = Logger.Log;
		
		static readonly ThreadLocal<Core> CoreTls = new();
		public static Core Current => CoreTls.Value;
		public static int CurrentId => Current?.Id ?? -1;
		static int CoreCount = 0;
		
		static object OSLock = false; // OS Lock status
		public readonly Vcpu Cpu;
		public int Id;

		const ulong CntFrq = 19200000;
		public ulong CntpCval;
		public ulong CntPct {
			get {
				var ticks = (double) Box.Stopwatch.ElapsedTicks / Stopwatch.Frequency;
				return (ulong) (ticks * CntFrq);
			}
		}
		public bool TimerInterruptEnabled;

		bool _Interrupt;
		public bool Interrupt {
			get => _Interrupt;
			set {
				_Interrupt = value;
				if(value && Id != CurrentId)
					Cpu.ForceExit();
			}
		}

		bool Terminated;
		public void Terminate() {
			Terminated = true;
			Cpu.ForceExit();
		}

		bool HookedInterrupts;
		
		public Core(ulong entryPoint, ulong arg = 0) {
			if(CoreCount == 4) throw new Exception();
			Id = CoreCount++;
			Box.Cores[Id] = this;
			CoreTls.Value = this;
			Cpu = Box.Vm.CreateVcpu();
			Cpu.CPSR = 0b0101; // EL1
			Cpu.PC = entryPoint;
			Cpu.X[0] = arg;
			Cpu.TrapDebugExceptions = false;
			Cpu.TrapDebugRegisterAccesses = false;
			
			new Thread(() => {
				while(true) {
					Thread.Sleep(1);
					if(TimerInterruptEnabled && CntPct >= CntpCval) {
						TimerInterruptEnabled = false;
						Box.InterruptDistributor.SendInterrupt(Id, 30);
					}
				}
			}).Start();
		}

		public unsafe void Run() {
			try {
				while(true) {
					if(Box.Initialized && !HookedInterrupts) {
						HookedInterrupts = true;
						var vbar = Cpu[SysReg.VBAR_EL1];
						var hea = vbar - 0x60800 + 0xA1320;
						Log($"HEA: {hea:X}");
						var syn = VirtMem.GetSpan<uint>(hea, Cpu);
						syn[0] = 0xd4000022;
					}
					
					var interrupted = false;
					if(Terminated) return;
					if(Interrupt) {
						Interrupt = false;
						Cpu.IrqPending = true;
						interrupted = true;
					}

					Log($"Core {Id} running from {Cpu.PC:X} -- Irq: {Cpu.IrqPending}");

					var exit = Cpu.Run();
					Log($"Core {CurrentId} Exited from {Cpu.PC:X} ! {exit.Reason} {exit.Syndrome:X} 0x{VirtMem.Translate(Cpu.PC, Cpu):X}");

					if(Terminated) return;

					switch(exit.Reason) {
						case ExitReason.Exception:
							var esr = exit.Syndrome;
							var far = exit.VirtualAddress;
							var pfar = exit.PhysicalAddress;
							var ec = (ExceptionCode) (esr >> 26);
							Log($"Exception: {ec}");
							switch(ec) {
								case ExceptionCode.TrappedWf_:
									if((esr & 1) == 0) {
										if(!Interrupt && Box.InterruptController.IsInterruptActive(Id)) {
											Interrupt = true;
											break;
										}
										while(!Interrupt)
											Thread.Sleep(1);
									}
									Cpu.PC += 4;
									break;
								case ExceptionCode.HyperCall:
									Logger.WithLock(() => {
										Log($"Got HVC; usermode exception");
										var hec = VirtMem.GetSpan<HosExceptionContext>(Cpu.X[0], Cpu)[0];
										for(var i = 0; i < 31; ++i)
											Log($"X{i}: 0x{hec.X[i]:X}");
										Log($"SP: 0x{hec.SP:X}");
										Log($"PC: 0x{hec.PC:X}");
										Log("Instruction data:");
										var cs = VirtMem.GetSpan<byte>(hec.PC, Cpu);
										for(var i = 0; i < 16; ++i)
											Console.Write($"{cs[i]:X2} ");
										Console.WriteLine();
										Environment.Exit(0);
									});
									break;
								case ExceptionCode.MonitorCall:
									var smc = (int) (esr & 0xFFFF);
									Box.SecMon.Call(Cpu, smc);
									Cpu.PC += 4;
									break;
								case ExceptionCode.MsrMrsTrap: {
									var op0 = (uint) (esr >> 20) & 0b11;
									var op2 = (uint) (esr >> 17) & 0b111;
									var op1 = (uint) (esr >> 14) & 0b111;
									var crn = (uint) (esr >> 10) & 0b1111;
									var rt = (int) (esr >> 5) & 0b11111;
									var crm = (uint) (esr >> 1) & 0b1111;
									var write = (esr & 1) == 0;

									switch (op0, op2, op1, crn, crm) {
										case (0b11, 0b001, 0b011, 0b1110, 0b0000): // CNTPCT_EL0
											if(write) throw new NotImplementedException();
											var cntpct = Cpu.X[rt] = CntPct;
											//Log($"CntPct: 0x{cntpct:X}");
											break;
										case (0b11, 0b001, 0b011, 0b1110, 0b0010): // CNTP_CTL_EL0
											if(!write) throw new NotImplementedException();
											//Log($"CNTP_CTL_EL0 write: 0x{Cpu.X[rt]:X}");
											var val = Cpu.X[rt];
											TimerInterruptEnabled = (val & 1) != 0;
											break;
										case (0b11, 0b010, 0b011, 0b1110, 0b0010): // CNTP_CVAL_EL0
											if(write)
												CntpCval = Cpu.X[rt];
											else
												Cpu.X[rt] = CntpCval;
											//Log($"CNTP_CVAL_EL0 {(write ? "write" : "read")}: 0x{CntpCval:X}");
											break;
										case (0b10, 0b100, 0b000, 0b0001, 0b0000): // OSLAR_EL1
											lock(OSLock)
												if(write)
													OSLock = (Cpu.X[rt] & 1) == 1;
												else
													Cpu.X[rt] = (bool) OSLock ? 1UL : 0;
											break;
										default:
											var mspan = VirtMem.GetSpan<byte>(Cpu.PC, Cpu);
											Log($"MSR/MRS: {op0} {op2} {op1} {crn} {crm}");
											throw new NotImplementedException(
												$"Unhandled MSR/MRS: {mspan[0]:X2} {mspan[1]:X2} {mspan[2]:X2} {mspan[3]:X2}");
									}

									Cpu.PC += 4;

									break;
								}
								case ExceptionCode.DataAbort:
									if(((esr >> 24) & 1) == 1) {
										var size = (int) ((esr >> 22) & 3);
										var bits = 8 << size;
										var write = ((esr >> 6) & 1) == 1;
										var srt = (int) ((esr >> 16) & 0b11111);

										if(MmioDevice.Devices.TryGetValue(pfar & ~0xFFFUL, out var dev)) {
											if(write) {
												var value = Cpu.X[srt];
												if(dev.Set(pfar, size, value)) {
													Cpu.PC += 4;
													break;
												}
											} else {
												if(dev.Get(pfar, size, out var value)) {
													Cpu.X[srt] = value;
													Cpu.PC += 4;
													break;
												}
											}
										}

										if(write)
											throw new Exception($"Data abort writing 0x{Cpu.X[srt]:X} to 0x{far:X} (bits: {bits} physical address: 0x{pfar:X})");
										else
											throw new Exception($"Data abort reading from 0x{far:X} (bits: {bits} physical address: 0x{pfar:X})");
									} else {
										var insn = VirtMem.GetSpan<uint>(Cpu.PC, Cpu)[0];
										if(InterpretLdSt(insn)) {
											Cpu.PC += 4;
											break;
										}

										throw new Exception(
											$"Data abort accessing 0x{far:X} (physical address: 0x{pfar:X})");
									}
								default:
									throw new NotImplementedException(
										$"Unimplemented exception code: {ec} 0b{Convert.ToString((uint) ec, 2)} -- 0x{Cpu.PC:X}");
							}

							break;
						case ExitReason.Canceled:
							/*if(interrupted) {
								Log("Rethrowing interrupt");
								//Interrupt = true;
							}*/
							break;
						default:
							throw new NotImplementedException($"Unhandled exit {exit.Reason}");
					}
				}
			} catch(Exception e) {
				foreach(var core in Box.Cores)
					core.Terminate();
				Thread.Sleep(1000);
				Logger.WithLock(() => {
					Log($"Core {CurrentId} threw an exception: {e}");
					Log($"Last PC: 0x{Cpu.PC:X}");
					Log("Instruction data: ");
					var span = VirtMem.GetSpan<byte>(Cpu.PC, Cpu);
					for(var i = 0; i < 16; ++i) {
						Console.Write($"{span[i]:X2} ");
					}
					Console.WriteLine();
				});
			}
		}

		bool InterpretLdSt(uint inst) {
			bool Write(ulong addr, int sizeIndex, ulong value) =>
				MmioDevice.Write(VirtMem.Translate(addr, Cpu), sizeIndex, value);

			bool Read(ulong addr, int sizeIndex, out ulong value) =>
				MmioDevice.Read(VirtMem.Translate(addr, Cpu), sizeIndex, out value);

			if((inst & 0b10_111_1_11_11_1_000000000_11_00000_00000) == 0b10_111_0_00_00_0_000000000_01_00000_00000) {
				var size = (inst >> 30) & 0x1U;
				var imm = (inst >> 12) & 0x1FFU;
				var rd = (inst >> 5) & 0x1FU;
				var rs = (inst >> 0) & 0x1FU;
				var simm = (imm & (1UL << 8)) != 0 ? imm - (1L << 9) : imm;
				var address = rd == 31 ? Cpu.SP : Cpu.X[(int) rd];
				if(!Write(address, size == 0 ? 2 : 3, Cpu.X[(int) rs])) return false;

				address = (ulong) ((long) address + simm);

				if(rd == 31) Cpu.SP = address;
				else Cpu.X[(int) rd] = address;

				return true;
			}
			return false;
		}
	}
}