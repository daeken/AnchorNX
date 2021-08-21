using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct FreeSpaceArguments {
		public ulong Offset;
		public uint Pages;
		public uint PageSize;
	}
}