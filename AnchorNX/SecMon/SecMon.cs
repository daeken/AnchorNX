using System;
using System.Threading;
using IronVisor;

namespace AnchorNX.SecMon {
	public class SecMon {
		static Random Rng = new(0x_EADBEEF);
		public void Call(Vcpu cpu, int func) {
			switch(func, cpu.X[0]) {
				case (1, 0xc4000003): // PowerOnCPU
					cpu.X[0] = 0;
					var cpuNum = cpu.X[1];
					var ep = cpu.X[2];
					var arg = cpu.X[3];
					Console.WriteLine($"Starting cpu{cpuNum} at 0x{ep:X} with argument 0x{arg:X}");
					new Thread(() => {
						Console.WriteLine($"Cpu started {cpuNum}");
						var core = new Core(ep, arg);
						core.Run();
					}).Start();
					break;
				case (1, 0xc3000004): // GetConfig
					ulong val;
					switch(cpu.X[1]) {
						case 10: // MemoryMode
							val = 1;
							break;
						case 12: // KernelConfiguration
							val = 0;
							break;
						default:
							throw new NotImplementedException($"Unhandled configuration option: {cpu.X[1]}");
					}
					cpu.X[0] = 0;
					cpu.X[1] = cpu.X[2] = cpu.X[3] = cpu.X[4] = val;
					break;
				case (1, 0xc3000005): // GenerateRandomBytes
					var size = cpu.X[1];
					for(var i = 0; i < (int) size; i += 8)
						cpu.X[1 + i / 4] = (ulong) Rng.NextInt64();
					cpu.X[0] = 0;
					break;
				case (1, 0xc3000008): // ReadWriteRegister
					switch(cpu.X[2], cpu.X[1]) {
						case (0, 0x70019050): // MC_EMEM_CFG
							cpu.X[1] = 0x1000; // 4GB physical RAM
							break;
						default:
							throw new NotImplementedException($"{(cpu.X[2] == 0 ? "Read from" : "Write to")} register 0x{cpu.X[1]:X} -- UNIMPLEMENTED");
					}
					cpu.X[0] = 0;
					break;
				default:
					throw new NotImplementedException($"Unhandled SMC call -- id{func} 0x{cpu.X[0]:X}");
			}
		}
	}
}