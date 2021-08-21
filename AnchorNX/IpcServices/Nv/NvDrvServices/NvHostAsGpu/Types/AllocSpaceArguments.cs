using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct AllocSpaceArguments {
		public uint Pages;
		public uint PageSize;
		public AddressSpaceFlags Flags;
		public uint Padding;
		public ulong Offset;
	}
}