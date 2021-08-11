using System;
using IronVisor;

namespace AnchorNX {
	public static class VirtMem {
		static ulong CurTCR = 0;
		static ulong T1TopMask, T1BottomMask, T0BottomMask;
		static int T1Size, T0Size;
		static int PageBits0, PageBits1;

		public static ulong Translate(ulong addr, Vcpu cpu) {
			if((cpu[SysReg.SCTLR_EL1] & 1) == 0) return addr;
			
			var tcr = cpu[SysReg.TCR_EL1];
			if(tcr != CurTCR) {
				CurTCR = tcr;
				var t1sz = (tcr >> 16) & 0b111111;
				T1Size = 64 - (int) t1sz;
				T1TopMask = ~((1UL << T1Size) - 1);
				T1BottomMask = ~T1TopMask;
				var t0sz = tcr & 0b111111;
				T0Size = 64 - (int) t0sz;
				T0BottomMask = (1UL << T0Size) - 1;

				PageBits0 = ((tcr >> 14) & 3) switch {
					0b00 => 12, 
					0b01 => 16, 
					0b10 => 14, 
					_ => throw new Exception()
				};
				
				PageBits1 = ((tcr >> 30) & 3) switch {
					0b01 => 14, 
					0b10 => 12, 
					0b11 => 16, 
					_ => throw new Exception()
				};
			}

			var isTop = (addr & T1TopMask) == T1TopMask;
			
			var pt = cpu[isTop ? SysReg.TTBR1_EL1 : SysReg.TTBR0_EL1];
			var size = isTop ? T1Size : T0Size;
			var pageBits = isTop ? PageBits1 : PageBits0;
			var pageIndex = addr & ((1UL << pageBits) - 1);
			var level = size > 39 ? 0 : 1;
			addr >>= pageBits;
			size -= pageBits;
			while(size != 0) {
				var csize = size % 9 == 0 ? 9 : size % 9;
				size -= csize;
				var mask = (1UL << csize) - 1;
				var index = (int) ((addr >> size) & mask);

				var pte = PhysMem.GetSpan<ulong>(pt + (ulong) (index * 8))[0];
				if((pte & 1) == 0)
					throw new Exception($"Invalid PTE! Address 0x{addr:X} -- {index} -- L{level}");

				switch(pte & 3) {
					case 0b11 when level == 0: // Table descriptor
						throw new NotImplementedException();
					case 0b01: // Block entry
						throw new NotImplementedException();
					case 0b11: // Table entry
						pt = ((pte & 0x0000_FFFF_FFFF_FFFFUL) >> pageBits) << pageBits;
						break;
					default:
						throw new Exception($"Unknown PTE type {pte & 3}");
				}

				level++;
			}

			return pt + pageIndex;
		}

		public static Span<T> GetSpan<T>(ulong addr, Vcpu cpu) where T : struct =>
			PhysMem.GetSpan<T>(Translate(addr, cpu));
	}
}