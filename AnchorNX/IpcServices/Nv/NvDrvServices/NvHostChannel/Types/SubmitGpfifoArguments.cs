using System.Runtime.InteropServices;
using AnchorNX.IpcServices.Nns.Nvdrv.Types;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct SubmitGpfifoArguments {
		public long Address;
		public int NumEntries;
		public SubmitGpfifoFlags Flags;
		public NvFence Fence;
	}
}