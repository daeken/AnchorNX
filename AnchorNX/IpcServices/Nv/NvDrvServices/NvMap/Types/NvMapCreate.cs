using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap {
	[StructLayout(LayoutKind.Sequential)]
	struct NvMapCreate {
		public int Size;
		public int Handle;
	}
}