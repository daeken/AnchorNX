using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrlGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct GetGpuTimeArguments {
		public ulong Timestamp;
		public ulong Reserved;
	}
}