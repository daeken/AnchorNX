using System.Runtime.InteropServices;
using AnchorNX.IpcServices.Nns.Nvdrv.Types;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct AllocGpfifoExArguments {
		public uint NumEntries;
		public uint NumJobs;
		public uint Flags;
		public NvFence Fence;
		public uint Reserved1;
		public uint Reserved2;
		public uint Reserved3;
	}
}