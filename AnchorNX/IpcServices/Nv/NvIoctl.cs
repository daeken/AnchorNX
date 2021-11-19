using System;
using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv {
	[StructLayout(LayoutKind.Sequential)]
	struct NvIoctl {
		public const int NvHostCustomMagic = 0x00;
		public const int NvMapCustomMagic = 0x01;
		public const int NvDispCtrlMagic = 0x02;
		public const int NvHdcpUpMagic = 0x03;
		public const int NvSchedCtrlMagic = 0x06;
		public const int NvGpuAsMagic = 0x41;
		public const int NvGpuMagic = 0x47;
		public const int NvHostMagic = 0x48;

		const int NumberBits = 8;
		const int TypeBits = 8;
		const int SizeBits = 14;
		const int DirectionBits = 2;

		const int NumberShift = 0;
		const int TypeShift = NumberShift + NumberBits;
		const int SizeShift = TypeShift + TypeBits;
		const int DirectionShift = SizeShift + SizeBits;

		const int NumberMask = (1 << NumberBits) - 1;
		const int TypeMask = (1 << TypeBits) - 1;
		const int SizeMask = (1 << SizeBits) - 1;
		const int DirectionMask = (1 << DirectionBits) - 1;

		[Flags]
		public enum Direction : uint {
			None = 0,
			Read = 1,
			Write = 2
		}

		public uint RawValue;

		public NvIoctl(uint rawValue) => RawValue = rawValue;

		public uint Number => (RawValue >> NumberShift) & NumberMask;
		public uint Type => (RawValue >> TypeShift) & TypeMask;
		public uint Size => (RawValue >> SizeShift) & SizeMask;
		public Direction DirectionValue => (Direction) ((RawValue >> DirectionShift) & DirectionMask);
	}
}