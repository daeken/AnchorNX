using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AnchorNX {
	public class Buffer<T> : IEnumerable<T> where T : struct {
		public readonly ulong Address;
		public readonly int Size, ElementSize;

		public int Length => Size / ElementSize;
		public Buffer<T> End => this + Length;

		public ref T Value => ref Span[0];
		public ref T this[int index] => ref Span[index];

		public Buffer(ulong address, ulong size) {
			Address = address;
			Size = (int) size;
			ElementSize = Marshal.SizeOf<T>();
		}

		public Span<T> SpanFrom(int offset) =>
			VirtMem.GetSpan<T>(Address + (ulong) offset, Core.Current.Cpu)[..((Size - offset) / ElementSize)];

		public Buffer<OtherT> As<OtherT>() where OtherT : struct => new(Address, (ulong) Size);
		public Buffer<OtherT> As<OtherT>(int size) where OtherT : struct => new(Address, (ulong) size);
		public Buffer<OtherT> As<OtherT>(ulong size) where OtherT : struct => new(Address, size);
		public Span<T> Span => SpanFrom(0);
		public static implicit operator Span<T>(Buffer<T> buffer) => buffer.Span;
		public static implicit operator T(Buffer<T> buffer) => buffer.Value;

		public static Buffer<T> operator +(Buffer<T> buffer, int offset) =>
			new(buffer.Address + (ulong) (buffer.ElementSize * offset),
				(ulong) buffer.Size - (ulong) (buffer.ElementSize * offset));

		public static Buffer<T> operator +(Buffer<T> buffer, uint offset) =>
			new(buffer.Address + (ulong) (buffer.ElementSize * offset),
				(ulong) buffer.Size - (ulong) (buffer.ElementSize * offset));

		public void Hexdump(Logger logger) => As<byte>().Span.Hexdump(logger);

		public IEnumerator<T> GetEnumerator() {
			var count = Size / Marshal.SizeOf<T>();
			for(var i = 0; i < count; ++i)
				yield return this[i];
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}