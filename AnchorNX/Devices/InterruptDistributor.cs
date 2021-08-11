using System;

namespace AnchorNX.Devices {
	public class InterruptDistributor : MmioDevice {
		public override (ulong, ulong) AddressRange => (0x50041000, 0x50041FFF);
		public override bool ZeroFaker => true;

		[Mmio(0x50041000)]
		uint Ctlr {
			get => throw new NotImplementedException();
			set { }
		}

		[Mmio(0x50041004)]
		uint Typer {
			get => 0;
			set { }
		}
	}
}