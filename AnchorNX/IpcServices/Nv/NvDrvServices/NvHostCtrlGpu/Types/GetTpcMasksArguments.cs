using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrlGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct GetTpcMasksArguments {
		public int MaskBufferSize;
		public int Reserved;
		public long MaskBufferAddress;
		public int TpcMask;
		public int Padding;
	}
}