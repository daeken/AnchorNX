using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.Types {
	[StructLayout(LayoutKind.Sequential, Size = 0x20)]
	struct NvStatus {
		public uint MemoryValue1;
		public uint MemoryValue2;
		public uint MemoryValue3;
		public uint MemoryValue4;
		public long Padding1;
		public long Padding2;
	}
}