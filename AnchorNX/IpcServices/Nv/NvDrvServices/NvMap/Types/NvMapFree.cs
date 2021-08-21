using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap {
	[StructLayout(LayoutKind.Sequential)]
	struct NvMapFree {
		public int Handle;
		public int Padding;
		public ulong Address;
		public int Size;
		public int Flags;
	}
}