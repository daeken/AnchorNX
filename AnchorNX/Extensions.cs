using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace AnchorNX {
	public static class Extensions {
		public static bool HasBit(this uint v, int bit) => (v & (1U << bit)) != 0;
		
		const string Printable = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-[]{}`~!@#$%^&*()-=\\|;:'\",./<>?";
		public static void Hexdump(this ReadOnlySpan<byte> _buffer, Logger logger) {
			var buffer = _buffer.ToArray();
			logger.WithLock(() => {
				for(var i = 0; i < buffer.Length; i += 16) {
					Console.Write($"{i:X4} | ");
					for(var j = 0; j < 16; ++j) {
						Console.Write(i + j >= buffer.Length ? $"   " : $"{buffer[i + j]:X2} ");
						if(j == 7) Console.Write(" ");
					}
					Console.Write("| ");
					for(var j = 0; j < 16; ++j) {
						if(i + j >= buffer.Length) break;
						Console.Write(Printable.Contains((char) buffer[i + j]) ? new string((char) buffer[i + j], 1) : ".");
						if(j == 7) Console.Write(" ");
					}
					Console.WriteLine();
				}
				Console.WriteLine($"{buffer.Length:X4}");
			});
		}

		public static void Hexdump(this Span<byte> _buffer, Logger logger) =>
			((ReadOnlySpan<byte>) _buffer).Hexdump(logger);

		public static void Hexdump(this byte[] _buffer, Logger logger) =>
			((ReadOnlySpan<byte>) _buffer).Hexdump(logger);

		public static Span<U> As<T, U>(this Span<T> span) where T : struct where U : struct =>
			MemoryMarshal.Cast<T, U>(span);

		public static void Debug(this string message, Action<string> log) => log(message);

		public static Memory<TTo> Cast<TFrom, TTo>(this Memory<TFrom> from)
			where TFrom : unmanaged
			where TTo : unmanaged
		{
			if (typeof(TFrom) == typeof(TTo)) return (Memory<TTo>)(object)from;

			return new CastMemoryManager<TFrom, TTo>(from).Memory;
		}
		
		sealed class CastMemoryManager<TFrom, TTo> : MemoryManager<TTo>
			where TFrom : unmanaged
			where TTo : unmanaged
		{
			readonly Memory<TFrom> _from;

			public CastMemoryManager(Memory<TFrom> from) => _from = from;

			public override Span<TTo> GetSpan()
				=> MemoryMarshal.Cast<TFrom, TTo>(_from.Span);

			protected override void Dispose(bool disposing) { }
			public override MemoryHandle Pin(int elementIndex = 0)
				=> throw new NotSupportedException();
			public override void Unpin()
				=> throw new NotSupportedException();
		}
	}
}