using System;

namespace AnchorNX.Devices {
	public class Uart : MmioDevice {
		static readonly Logger Logger = new("Uart");
		static Action<string> Log = Logger.Log;
		
		public override (ulong, ulong) AddressRange => (0x70006000, 0x70006fff);
		public override bool ZeroFaker => true;

		public string Buffer = "";

		[Mmio(0x70006000)]
		uint ThrDlab_0_0 {
			set {
				var ch = (char) (value & 0xFF);
				if(ch == '\n') {
					Log($"Message: '{Buffer.TrimEnd('\r')}'");
					Buffer = "";
				} else {
					Buffer += ch;
				}
			}
		}
	}
}