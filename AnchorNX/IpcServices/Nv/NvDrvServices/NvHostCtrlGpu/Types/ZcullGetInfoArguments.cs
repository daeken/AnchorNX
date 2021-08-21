using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrlGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct ZcullGetInfoArguments {
		public int WidthAlignPixels;
		public int HeightAlignPixels;
		public int PixelSquaresByAliquots;
		public int AliquotTotal;
		public int RegionByteMultiplier;
		public int RegionHeaderSize;
		public int SubregionHeaderSize;
		public int SubregionWidthAlignPixels;
		public int SubregionHeightAlignPixels;
		public int SubregionCount;
	}
}