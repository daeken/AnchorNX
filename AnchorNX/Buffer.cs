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
			VirtMem.GetSpan<T>(Address + (ulong) (offset * ElementSize), Core.Current.Cpu)[..((Size - offset * ElementSize) / ElementSize)];

		public ReadOnlySpan<T> SafeSpanFrom(int offset) {
			var addr = Address + (ulong) (offset * ElementSize);
			var end = addr + (ulong) (Size - offset * ElementSize) - 1;
			if((addr >> 12) == (end >> 12))
				return SpanFrom(offset);
			var data = new byte[Size - offset * ElementSize];
			(this + offset).CopyTo(data);
			return MemoryMarshal.Cast<byte, T>(data);
		}

		public Buffer<OtherT> As<OtherT>() where OtherT : struct => new(Address, (ulong) Size);
		public Buffer<OtherT> As<OtherT>(int size) where OtherT : struct => new(Address, (ulong) size);
		public Buffer<OtherT> As<OtherT>(ulong size) where OtherT : struct => new(Address, size);
		public Span<T> Span => SpanFrom(0);
		public ReadOnlySpan<T> SafeSpan => SafeSpanFrom(0);
		public static implicit operator T(Buffer<T> buffer) => buffer.Value;

		public static Buffer<T> operator +(Buffer<T> buffer, int offset) =>
			new(buffer.Address + (ulong) (buffer.ElementSize * offset),
				(ulong) buffer.Size - (ulong) (buffer.ElementSize * offset));

		public static Buffer<T> operator +(Buffer<T> buffer, uint offset) =>
			new(buffer.Address + (ulong) (buffer.ElementSize * offset),
				(ulong) buffer.Size - (ulong) (buffer.ElementSize * offset));

		public void Hexdump(Logger logger) => As<byte>().SafeSpan.Hexdump(logger);

		public void CopyFrom(ReadOnlySpan<byte> data) {
			if(typeof(T) != typeof(byte)) {
				As<byte>().CopyFrom(data);
				return;
			}
			var tc = (ulong) Math.Min(data.Length, Length);
			for(var i = 0UL; i < tc;) {
				var pageOff = 0x1000 - ((Address + i) & 0xFFF);
				var tr = (int) Math.Min(tc, Math.Min(pageOff, tc - i));
				var cs = VirtMem.GetSpan<byte>(Address + i, Core.Current.Cpu)[..tr];
				data[(int) i .. (int) (i + (ulong) tr)].CopyTo(cs);
				i += (ulong) Math.Min(tr, cs.Length);
			}
		}

		public void CopyTo(Span<byte> data) {
			if(typeof(T) != typeof(byte)) {
				As<byte>().CopyTo(data);
				return;
			}
			var tc = (ulong) Math.Min(data.Length, Length);
			for(var i = 0UL; i < tc;) {
				var pageOff = 0x1000 - ((Address + i) & 0xFFF);
				var tr = (int) Math.Min(tc, Math.Min(pageOff, tc - i));
				var cs = VirtMem.GetSpan<byte>(Address + i, Core.Current.Cpu)[..tr];
				cs.CopyTo(data[(int) i .. (int) (i + (ulong) tr)]);
				i += (ulong) Math.Min(tr, cs.Length);
			}
		}

		public IEnumerator<T> GetEnumerator() {
			var count = Size / Marshal.SizeOf<T>();
			for(var i = 0; i < count; ++i)
				yield return this[i];
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}