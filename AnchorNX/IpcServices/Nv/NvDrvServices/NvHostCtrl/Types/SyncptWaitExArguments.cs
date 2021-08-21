using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrl.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct SyncptWaitExArguments {
		public SyncptWaitArguments Input;
		public uint Value;
	}
}