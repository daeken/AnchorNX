using System;
using System.Text.RegularExpressions;

namespace AnchorNX.Devices {
	public class Uart : MmioDevice {
		static readonly Logger Logger = new("Uart");
		static Action<string> Log = Logger.Log;
		
		public override (ulong, ulong) AddressRange => (0x70006000, 0x70006fff);
		public override bool ZeroFaker => true;

		public string Buffer = "";

		readonly Regex KProcMatcher = new(@"KProcess::Run\(\) pid=([0-9]+) name=([^ ]+)");

		[Mmio(0x70006000)]
		uint ThrDlab_0_0 {
			set {
				var ch = (char) (value & 0xFF);
				if(ch == '\n') {
					if(KProcMatcher.Match(Buffer) is { Success: true } m)
						Box.PidNames[ulong.Parse(m.Groups[1].Value)] = m.Groups[2].Value;
					Log($"Message: '{Buffer.TrimEnd('\r')}'");
					Buffer = "";
				} else {
					Buffer += ch;
				}
			}
		}

		[Mmio(0x70006014)] uint Lsr => 0x40;
	}
}