using System;

namespace AnchorNX.Devices {
	public class Uart : MmioDevice {
		public override (ulong, ulong) AddressRange => (0x70006000, 0x70006fff);
		public override bool ZeroFaker => true;

		public string Buffer = "";

		[Mmio(0x70006000)]
		uint ThrDlab_0_0 {
			set {
				Console.WriteLine($"UART char: 0x{value & 0xFF}");
				var ch = (char) (value & 0xFF);
				if(ch == '\n') {
					Console.WriteLine($"UART message: '{Buffer.TrimEnd('\r')}'");
					Buffer = "";
				} else {
					Buffer += ch;
				}
			}
		}
	}
}