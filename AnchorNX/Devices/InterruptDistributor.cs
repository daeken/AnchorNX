using System;

namespace AnchorNX.Devices {
	public class InterruptDistributor : MmioDevice {
		public override (ulong, ulong) AddressRange => (0x50041000, 0x50041FFF);

		[Mmio(0x50041000)]
		uint Ctlr {
			get => throw new NotImplementedException();
			set { }
		}

		[Mmio(0x50041004)]
		uint Typer => 6 | (3 << 5) | (1 << 10) | (0x1F << 11);

		[Mmio(0x50041080)] MmioArray<uint> IGroupR = new(32, writable: true);
		[Mmio(0x50041100)] MmioArray<uint> IsEnableR = new(32, writable: true);
		[Mmio(0x50041180)] MmioArray<uint> IcEnableR = new(32, writable: true);
		[Mmio(0x50041280)] MmioArray<uint> IcPendR = new(32, writable: true);
		[Mmio(0x50041380)] MmioArray<uint> IcActiveR = new(32, writable: true);
		[Mmio(0x50041400)] MmioArray<byte> IPriorityR = new(1020, writable: true);
		[Mmio(0x50041800)] MmioArray<byte> ITargetsR = new(1020, writable: true);
		[Mmio(0x50041C00)] MmioArray<uint> ICfgR = new(32, writable: true);
		
		[Mmio(0x50041F00)] uint Sgir { get; set; }
	}
}