using System;
using System.Linq;
using IronVisor;

namespace AnchorNX {
	public class Core {
		static object OSLock = false; // OS Lock status
		public readonly Vcpu Cpu;
		public Core(ulong entryPoint, ulong arg = 0) {
			Cpu = Box.Vm.CreateVcpu();
			Cpu.CPSR = 0b0100; // EL1
			Cpu.PC = entryPoint;
			Cpu.X[0] = arg;
			Cpu.TrapDebugExceptions = false;
			Cpu.TrapDebugRegisterAccesses = false;
		}

		public void Run() {
			while(true) {
				Console.WriteLine($"Running from {Cpu.PC:X}");
				var exit = Cpu.Run();
				Console.WriteLine($"Exited from {Cpu[SysReg.ELR_EL1]:X} ! {exit.Reason} {exit.Syndrome:X} {exit.VirtualAddress:X}");
				Console.WriteLine($"PC   {Cpu.PC:X}");
				Console.WriteLine($"PPC  {VirtMem.Translate(Cpu.PC, Cpu):X}");
				Console.WriteLine($"CPSR {Cpu.CPSR:X}");
				Console.WriteLine($"X0   {Cpu.X[0]:X}");
				Console.WriteLine($"X1   {Cpu.X[1]:X}");
				Console.WriteLine($"X2   {Cpu.X[2]:X}");
				Console.WriteLine($"X3   {Cpu.X[3]:X}");
				Console.WriteLine($"X8   {Cpu.X[8]:X}");
				switch(exit.Reason) {
					case ExitReason.Exception:
						var esr = exit.Syndrome;
						var far = exit.VirtualAddress;
						var pfar = exit.PhysicalAddress;
						var ec = (ExceptionCode) (esr >> 26);
						switch(ec) {
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
								
								Console.WriteLine($"{op0} {op2} {op1} {crn} {rt} {crm}");

								switch(op0, op2, op1, crn, crm) { // OSLAR_EL1
									case (0b10, 0b100, 0b000, 0b0001, 0b0000):
										lock(OSLock)
											if(write)
												OSLock = (Cpu.X[rt] & 1) == 1;
											else
												Cpu.X[rt] = (bool) OSLock ? 1UL : 0;
										break;
									default:
										var mspan = VirtMem.GetSpan<byte>(Cpu.PC, Cpu);
										Console.WriteLine($"MSR/MRS: {op0} {op2} {op1} {crn} {crm}");
										throw new NotImplementedException($"Unhandled MSR/MRS: {mspan[0]:X2} {mspan[1]:X2} {mspan[2]:X2} {mspan[3]:X2}");
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
									
									throw new Exception($"Data abort {(write ? "writing to" : "reading from")} 0x{far:X} (bits: {bits} physical address: 0x{pfar:X})");
								} else
									throw new Exception($"Data abort accessing 0x{far:X} (physical address: 0x{pfar:X})");
							default:
								throw new NotImplementedException($"Unimplemented exception code: {ec} 0b{Convert.ToString((uint) ec, 2)} -- 0x{Cpu.PC:X}");
						}
						break;
					default:
						throw new NotImplementedException($"Unhandled exit {exit.Reason}");
				}
			}
		}
	}
}