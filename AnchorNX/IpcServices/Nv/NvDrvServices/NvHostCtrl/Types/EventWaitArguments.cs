using System.Runtime.InteropServices;
using AnchorNX.IpcServices.Nns.Nvdrv.Types;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrl.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct EventWaitArguments {
		public NvFence Fence;
		public int Timeout;
		public uint Value;
	}
}