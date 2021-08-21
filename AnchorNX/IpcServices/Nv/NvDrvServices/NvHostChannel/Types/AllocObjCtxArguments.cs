using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct AllocObjCtxArguments {
		public uint ClassNumber;
		public uint Flags;
		public ulong ObjectId;
	}
}