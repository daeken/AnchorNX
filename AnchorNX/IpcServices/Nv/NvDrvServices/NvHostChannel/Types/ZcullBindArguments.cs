using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct ZcullBindArguments {
		public ulong GpuVirtualAddress;
		public uint Mode;
		public uint Reserved;
	}
}