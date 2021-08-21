using System;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu.Types {
	[Flags]
	enum AddressSpaceFlags : uint {
		FixedOffset = 1,
		RemapSubRange = 0x100
	}
}