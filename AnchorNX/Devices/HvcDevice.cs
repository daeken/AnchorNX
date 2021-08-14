using System;
using System.Threading;

namespace AnchorNX.Devices {
	public class HvcDevice : MmioDevice {
		public override (ulong Start, ulong End) AddressRange => (0x5700_0000, 0x5700_FFFF);

		[Mmio(0x57000000)]
		ulong Trigger {
			set => Box.HvcProxy.OnWake();
		}

		[Mmio(0x57000008)]
		ulong LogMessage {
			set {
				var tspan = VirtMem.GetSpan<byte>(value, Core.Current.Cpu);
				var msg = "";
				for(var i = 0; tspan[i] != 0; ++i)
					msg += (char) tspan[i];
				Console.WriteLine($"HvcProxy message: {msg}");
			}
		}
	}
}