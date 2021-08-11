using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using IronVisor;

namespace AnchorNX {
	public class PhysMem {
		public static readonly (BoundMemory Memory, uint Offset)[] Mappings = new (BoundMemory Memory, uint Offset)[8388608];

		public static void Map(ulong baseAddr, ulong size) {
			while(size != 0) {
				var csize = size > 0x4000_0000 ? 0x4000_0000 : (uint) size;
				size -= csize;

				var bm = Box.Vm.Map(baseAddr, csize, MemoryFlags.RWX);
				for(var i = 0U; i < csize; i += 0x4000)
					Mappings[(baseAddr + i) >> 14] = (bm, i);
				baseAddr += csize;
			}
		}

		public static Span<T> GetSpan<T>(ulong addr) where T : struct {
			var (bm, offset) = Mappings[addr >> 14];
			if(bm == null) throw new Exception();
			var noff = (int) ((addr & 0x3FFFUL) + offset);
			var tspan = bm.AsSpan<byte>()[noff..];
			return MemoryMarshal.Cast<byte, T>(tspan);
		}
	}
}