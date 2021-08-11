using System;
using System.IO;
using IronVisor;

namespace AnchorNX {
	class Program {
		static void Main(string[] args) {
			Box.Vm = new Vm();
			PhysMem.Map(0x8000_0000, 0x1_0000_0000);

			var kspan = PhysMem.GetSpan<byte>(0x8006_0000);
			File.ReadAllBytes("Kernel.bin").CopyTo(kspan);
			/*var suppressAt = 0x54;
			kspan[suppressAt + 0] = 0x02;
			kspan[suppressAt + 1] = 0x00;
			kspan[suppressAt + 2] = 0x00;
			kspan[suppressAt + 3] = 0xd4;*/

			var core = new Core(0x8006_0000);
			core.Run();
		}
	}
}