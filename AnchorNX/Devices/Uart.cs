using System;

namespace AnchorNX.Devices {
	public class Uart : MmioDevice {
		public override (ulong, ulong) AddressRange => (0x70006000, 0x70006fff);
		public override bool ZeroFaker => true;

		/*[Mmio(0x70006000)]
		byte ThrDlab_0_0 {
			set {
				throw new NotImplementedException();
			}
		}*/
	}
}