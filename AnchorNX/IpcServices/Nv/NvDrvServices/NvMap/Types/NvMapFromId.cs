using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap {
	[StructLayout(LayoutKind.Sequential)]
	struct NvMapFromId {
		public int Id;
		public int Handle;
	}
}