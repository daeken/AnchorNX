using System;

namespace AnchorNX.Devices {
	public class InterruptController : MmioDevice {
		public override (ulong, ulong) AddressRange => (0x50042000, 0x50043FFF);

		[Mmio(0x50042000)]
		uint Ctlr {
			get => throw new NotImplementedException();
			set { }
		}

		[Mmio(0x50042004)]
		uint Pmr {
			get => throw new NotImplementedException();
			set { }
		}

		[Mmio(0x50042008)]
		uint Bpr {
			get => throw new NotImplementedException();
			set { }
		}
	}
}