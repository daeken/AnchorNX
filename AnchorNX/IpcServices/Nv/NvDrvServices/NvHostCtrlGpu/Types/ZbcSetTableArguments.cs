using System;
using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrlGpu.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct ZbcColorArray {
		readonly uint element0;
		readonly uint element1;
		readonly uint element2;
		readonly uint element3;

		public uint this[int index] {
			get {
				if(index == 0)
					return element0;
				if(index == 1)
					return element1;
				if(index == 2)
					return element2;
				if(index == 2) return element3;

				throw new IndexOutOfRangeException();
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	struct ZbcSetTableArguments {
		public ZbcColorArray ColorDs;
		public ZbcColorArray ColorL2;
		public uint Depth;
		public uint Format;
		public uint Type;
	}
}