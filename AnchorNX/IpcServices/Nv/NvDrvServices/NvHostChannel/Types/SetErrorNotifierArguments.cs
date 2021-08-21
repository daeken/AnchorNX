using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct SetErrorNotifierArguments {
		public ulong Offset;
		public ulong Size;
		public uint Mem;
		public uint Reserved;
	}
}