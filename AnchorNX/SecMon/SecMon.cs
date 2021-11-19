using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using IronVisor;

namespace AnchorNX.SecMon {
	public class SecMon {
		static readonly Logger Logger = new("SecMon");
		static Action<string> Log = Logger.Log;
		
		static Random Rng = new(0x_EADBEEF);
		public void Call(Vcpu cpu, int func) {
			Log($"SecMon call {func}, 0x{cpu.X[0]:X}");
			switch(func, cpu.X[0]) {
				case (1, 0xc4000003): // PowerOnCPU
					cpu.X[0] = 0;
					var cpuNum = cpu.X[1];
					var ep = cpu.X[2];
					var arg = cpu.X[3];
					Log($"Starting cpu{cpuNum} at 0x{ep:X} with argument 0x{arg:X}");
					new Thread(() => {
						Log($"Cpu started {cpuNum}");
						var core = new Core(ep, arg);
						core.Run();
					}).Start();
					break;
				case (0, 0xc3000002): // GetConfig
				case (1, 0xc3000004): // GetConfig
					ulong val = cpu.X[1] switch {
						1 => 1, // DisableProgramVerification
						3 => 44, // SecurityEngineInterruptNumber
						5 => 0, // HardwareType -- Icosa
						6 => 1, // HardwareState -- Production
						7 => 0, // IsRecoveryBoot
						8 => 0x0065474E6C38000A, // DeviceId
						10 => 1, // MemoryMode
						11 => 1, // IsDevelopmentFunctionEnabled
						12 => 1 << 8, // KernelConfiguration -- CallShowErrorOnPanic
						14 => 0, // QuestState
						16 => 0, // DeviceUniqueKeyGeneration
						
						// Exosphere extensions!
						65000 => (19UL << 48) | (5UL << 40) | (0xBUL << 32) | (12UL << 24), // ExosphereApiVersion
						65009 => 115200, // ExosphereLogConfiguration
						
						_ => throw new NotImplementedException($"Unhandled configuration option: {cpu.X[1]}")
					};
					cpu.X[0] = 0;
					cpu.X[1] = cpu.X[2] = cpu.X[3] = cpu.X[4] = val;
					break;
				case (0, 0xC300100D): { // DecryptDeviceUniqueData
					var accessKey = BitConverter.GetBytes(cpu.X[1]).Concat(BitConverter.GetBytes(cpu.X[2])).ToArray();
					var keySource = BitConverter.GetBytes(cpu.X[6]).Concat(BitConverter.GetBytes(cpu.X[7])).ToArray();
					cpu.X[0] = DecryptDeviceUniqueData(
						accessKey, keySource, 
						(uint) cpu.X[3], cpu.X[4], cpu.X[5]
					);
					break;
				}
				case (0, 0xC3000610): { // PrepareEsDeviceUniqueKey
					Log("Stub for PrepareEsDeviceUniqueKey hit");
					cpu.X[0] = cpu.X[1] = 0;
					Box.InterruptDistributor.SendInterrupt(3, 44);
					break;
				}
				case (0, 0xC3000003): { // GetResult
					Log("Stub for GetResult");
					cpu.X[0] = cpu.X[1] = 0;
					break;
				}
				case (0, 0xC3000404): { // GetResultData
					Log("Stub for GetResultData");
					var dataAddr = cpu.X[2];
					var dataSize = (int) cpu.X[3];
					var mem = PhysMem.GetSpan<byte>(dataAddr)[..(int) dataSize];
					Enumerable.Range(0, (int) dataSize).Select(_ => (byte) 0).ToArray().CopyTo(mem);
					cpu.X[0] = cpu.X[1] = 0;
					break;
				}
				case (0, 0xC3000008): { // LoadAesKey
					Log("Stub for LoadAesKey");
					cpu.X[0] = 0;
					break;
				}
				case (0, 0xC3000011): { // LoadPreparedAesKey
					Log("Stub for LoadPreparedAesKey");
					cpu.X[0] = 0;
					break;
				}
				case (0, 0xC3000009): { // ComputeAes
					Log("Stub for ComputeAes");
					cpu.X[0] = cpu.X[1] = 0;
					Box.InterruptDistributor.SendInterrupt(3, 44);
					break;
				}
				case (0, 0xc3000006): // GenerateRandomBytes
				case (1, 0xc3000005): // GenerateRandomBytes
					var size = cpu.X[1];
					for(var i = 0; i < (int) size; i += 8)
						cpu.X[1 + i / 4] = (ulong) Rng.NextInt64();
					cpu.X[0] = 0;
					break;
				case (0, 0xc3000007): { // GenerateAesKek
					var kekSource = BitConverter.GetBytes(cpu.X[1]).Concat(BitConverter.GetBytes(cpu.X[2])).ToArray();
					var (ret, kek) = GenerateAesKek(kekSource, Math.Max(((int) cpu.X[3]) - 1, 0), (uint) cpu.X[4]);
					cpu.X[0] = ret;
					cpu.X[1] = BitConverter.ToUInt64(kek, 0);
					cpu.X[2] = BitConverter.ToUInt64(kek, 8);
					break;
				}
				case (1, 0xc3000007): // SetKernelCarveoutRegion
					cpu.X[0] = 0;
					break;
				case (1, 0xc3000008): // ReadWriteRegister
					var write = cpu.X[2] != 0;
					var value = 0UL;
					Log($"ReadWriteRegister -- {(write ? "write to" : "read from")} 0x{cpu.X[1]:X}");
					var success = write
						? MmioDevice.Write(cpu.X[1], 2, cpu.X[3])
						: MmioDevice.Read(cpu.X[1], 2, out value);
					if(!success)
						throw new NotImplementedException($"{(cpu.X[2] == 0 ? "Read from" : "Write to")} register 0x{cpu.X[1]:X} -- UNIMPLEMENTED");
					if(!write) cpu.X[1] = value;
					cpu.X[0] = 0;
					break;
				default:
					throw new NotImplementedException($"Unhandled SMC call -- id{func} 0x{cpu.X[0]:X}");
			}
		}

		enum KeyType {
			Default = 0, 
			NormalOnly = 1, 
			RecoveryOnly = 2, 
			NormalAndRecovery = 3, 
		}

		enum SealKey {
			LoadAesKey, 
			DecryptDeviceUniqueData, 
			ImportLotusKey, 
			ImportEsDeviceKey, 
			ReencryptDeviceUniqueData, 
			ImportSslKey, 
			ImportEsClientCertKey, 
		}

		static readonly byte[][] SealKeySources = {
			new byte[] { 0xF4, 0x0C, 0x16, 0x26, 0x0D, 0x46, 0x3B, 0xE0, 0x8C, 0x6A, 0x56, 0xE5, 0x82, 0xD4, 0x1B, 0xF6 },
			new byte[] { 0x7F, 0x54, 0x2C, 0x98, 0x1E, 0x54, 0x18, 0x3B, 0xBA, 0x63, 0xBD, 0x4C, 0x13, 0x5B, 0xF1, 0x06 },
			new byte[] { 0xC7, 0x3F, 0x73, 0x60, 0xB7, 0xB9, 0x9D, 0x74, 0x0A, 0xF8, 0x35, 0x60, 0x1A, 0x18, 0x74, 0x63 },
			new byte[] { 0x0E, 0xE0, 0xC4, 0x33, 0x82, 0x66, 0xE8, 0x08, 0x39, 0x13, 0x41, 0x7D, 0x04, 0x64, 0x2B, 0x6D },
			new byte[] { 0xE1, 0xA8, 0xAA, 0x6A, 0x2D, 0x9C, 0xDE, 0x43, 0x0C, 0xDE, 0xC6, 0x17, 0xF6, 0xC7, 0xF1, 0xDE },
			new byte[] { 0x74, 0x20, 0xF6, 0x46, 0x77, 0xB0, 0x59, 0x2C, 0xE8, 0x1B, 0x58, 0x64, 0x47, 0x41, 0x37, 0xD9 },
			new byte[] { 0xAA, 0x19, 0x0F, 0xFA, 0x4C, 0x30, 0x3B, 0x2E, 0xE6, 0xD8, 0x9A, 0xCF, 0xE5, 0x3F, 0xB3, 0x4B },
		};

		static readonly byte[][] KeyTypeSources = {
			new byte[] { 0x4D, 0x87, 0x09, 0x86, 0xC4, 0x5D, 0x20, 0x72, 0x2F, 0xBA, 0x10, 0x53, 0xDA, 0x92, 0xE8, 0xA9 },
			new byte[] { 0x25, 0x03, 0x31, 0xFB, 0x25, 0x26, 0x0B, 0x79, 0x8C, 0x80, 0xD2, 0x69, 0x98, 0xE2, 0x22, 0x77 },
			new byte[] { 0x76, 0x14, 0x1D, 0x34, 0x93, 0x2D, 0xE1, 0x84, 0x24, 0x7B, 0x66, 0x65, 0x55, 0x04, 0x65, 0x81 }, 
			new byte[] { 0xAF, 0x3D, 0xB7, 0xF3, 0x08, 0xA2, 0xD8, 0xA2, 0x08, 0xCA, 0x18, 0xA8, 0x69, 0x46, 0xC9, 0x0B }
		};

		static readonly byte[][] SealKeyMasks = {
			new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
			new byte[] { 0xA2, 0xAB, 0xBF, 0x9C, 0x92, 0x2F, 0xBB, 0xE3, 0x78, 0x79, 0x9B, 0xC0, 0xCC, 0xEA, 0xA5, 0x74 },
			new byte[] { 0x57, 0xE2, 0xD9, 0x45, 0xE4, 0x92, 0xF4, 0xFD, 0xC3, 0xF9, 0x86, 0x38, 0x89, 0x78, 0x9F, 0x3C },
			new byte[] { 0xE5, 0x4D, 0x9A, 0x02, 0xF0, 0x4F, 0x5F, 0xA8, 0xAD, 0x76, 0x0A, 0xF6, 0x32, 0x95, 0x59, 0xBB },
			new byte[] { 0x59, 0xD9, 0x31, 0xF4, 0xA7, 0x97, 0xB8, 0x14, 0x40, 0xD6, 0xA2, 0x60, 0x2B, 0xED, 0x15, 0x31 },
			new byte[] { 0xFD, 0x6A, 0x25, 0xE5, 0xD8, 0x38, 0x7F, 0x91, 0x49, 0xDA, 0xF8, 0x59, 0xA8, 0x28, 0xE6, 0x75 },
			new byte[] { 0x89, 0x96, 0x43, 0x9A, 0x7C, 0xD5, 0x59, 0x55, 0x24, 0xD5, 0x24, 0x18, 0xAB, 0x6C, 0x04, 0x61 },
		};

		(uint Return, byte[] Kek) GenerateAesKek(byte[] kekSource, int generation, uint options) {
			var isDeviceUnique = (options & 1) == 1;
			var keyType = (KeyType) ((options >> 1) & 0b1111);
			var sealKey = (SealKey) ((options >> 5) & 0b111);
			var reserved = options >> 8;
			Debug.Assert(reserved == 0);

			var kts = KeyTypeSources[(int) keyType];
			var skm = SealKeyMasks[(int) sealKey];

			var staticSource = kts.Zip(skm).Select(x => x.First ^ x.Second).ToArray();
			var sealKeySource = SealKeySources[(int) sealKey];

			/*var slot = isDeviceUnique
				? PrepareDeviceMasterKey(generation)
				: PrepareMasterKey(generation);*/
			return (0, new byte[16]);
		}

		int PrepareDeviceMasterKey(int generation) {
			if(generation == 0)
				return 15; // AesKeySlot_Device
			throw new NotImplementedException();
		}

		uint DecryptDeviceUniqueData(byte[] accessKey, byte[] keySource, uint options, ulong dataAddr, ulong dataSize) {
			Log("Stubbed DecryptDeviceUniqueData");
			var mem = PhysMem.GetSpan<byte>(dataAddr)[..(int) dataSize];
			Enumerable.Range(0, (int) dataSize).Select(_ => (byte) 0xFF).ToArray().CopyTo(mem);
			return 0;
		}
	}
}