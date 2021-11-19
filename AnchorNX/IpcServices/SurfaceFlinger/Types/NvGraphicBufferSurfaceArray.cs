using System;
using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	[StructLayout(LayoutKind.Explicit)]
	struct NvGraphicBufferSurfaceArray {
		[FieldOffset(0x0)] readonly NvGraphicBufferSurface Surface0;

		[FieldOffset(0x58)] readonly NvGraphicBufferSurface Surface1;

		[FieldOffset(0xb0)] readonly NvGraphicBufferSurface Surface2;

		public NvGraphicBufferSurface this[int index] {
			get {
				if(index == 0)
					return Surface0;
				if(index == 1)
					return Surface1;
				if(index == 2) return Surface2;

				throw new IndexOutOfRangeException();
			}
		}

		public int Length => 3;
	}
}