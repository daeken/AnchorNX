﻿using System;
using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	[StructLayout(LayoutKind.Sequential, Size = 0x10)]
	struct Rect : IEquatable<Rect> {
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public int Width => Right - Left;
		public int Height => Bottom - Top;

		public Rect(int width, int height) {
			Left = 0;
			Top = 0;
			Right = width;
			Bottom = height;
		}

		public bool IsEmpty() {
			return Width <= 0 || Height <= 0;
		}

		public bool Intersect(Rect other, out Rect result) {
			result = new Rect {
				Left = Math.Max(Left, other.Left),
				Top = Math.Max(Top, other.Top),
				Right = Math.Min(Right, other.Right),
				Bottom = Math.Min(Bottom, other.Bottom)
			};

			return !result.IsEmpty();
		}

		public void MakeInvalid() {
			Right = -1;
			Bottom = -1;
		}

		public static bool operator ==(Rect x, Rect y) {
			return x.Equals(y);
		}

		public static bool operator !=(Rect x, Rect y) {
			return !x.Equals(y);
		}

		public override bool Equals(object obj) {
			return obj is Rect rect && Equals(rect);
		}

		public bool Equals(Rect cmpObj) {
			return Left == cmpObj.Left && Top == cmpObj.Top && Right == cmpObj.Right && Bottom == cmpObj.Bottom;
		}

		public override int GetHashCode() {
			return HashCode.Combine(Left, Top, Right, Bottom);
		}
	}
}