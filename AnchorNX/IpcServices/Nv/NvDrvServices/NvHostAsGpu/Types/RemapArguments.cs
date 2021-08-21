using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct RemapArguments {
		public ushort Flags;
		public ushort Kind;
		public int NvMapHandle;
		public uint MapOffset;
		public uint GpuOffset;
		public uint Pages;
	}
}