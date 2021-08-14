using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace AnchorNX {
	public static class Extensions {
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