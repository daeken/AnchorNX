namespace AnchorNX.Devices {
	public class PowerManagementController : MmioDevice {
		public override (ulong Start, ulong End) AddressRange => (0x7000E000, 0x7000EFFF);
		public override bool ZeroFaker => true;
	}
}