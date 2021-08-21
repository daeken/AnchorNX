using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap {
	[StructLayout(LayoutKind.Sequential)]
	struct NvMapParam {
		public int Handle;
		public NvMapHandleParam Param;
		public int Result;
	}
}