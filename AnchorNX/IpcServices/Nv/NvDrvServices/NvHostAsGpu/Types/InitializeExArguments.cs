﻿using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct InitializeExArguments {
		public uint Flags;
		public int AsFd;
		public uint BigPageSize;
		public uint Reserved;
		public ulong Unknown0;
		public ulong Unknown1;
		public ulong Unknown2;
	}
}