using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrlGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct GetActiveSlotMaskArguments {
		public int Slot;
		public int Mask;
	}
}