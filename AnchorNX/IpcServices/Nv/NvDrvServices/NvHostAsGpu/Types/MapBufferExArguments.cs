using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct MapBufferExArguments {
		public AddressSpaceFlags Flags;
		public int Kind;
		public int NvMapHandle;
		public int PageSize;
		public ulong BufferOffset;
		public ulong MappingSize;
		public ulong Offset;
	}
}