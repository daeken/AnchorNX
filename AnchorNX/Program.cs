using System;
using System.IO;
using IronVisor;

namespace AnchorNX {
	class Program {
		static void Main(string[] args) {
			var packager = new Package2Packager("BCPKG2-1-Normal-Main.img");
			packager.Remove("FS");
			var package2 = packager.BuildPackage2();
			
			Box.Vm = new Vm();
			PhysMem.Map(0x8000_0000, 0x1_0000_0000);

			var firstBreak = true;
			Console.CancelKeyPress += (_, evt) => {
				if(!firstBreak) return;
				firstBreak = false;
				for(var i = 0; i < 4; ++i)
					Box.Cores[i].Terminate();
				evt.Cancel = true;
			};

			var kspan = PhysMem.GetSpan<byte>(0x8006_0000);
			package2.CopyTo(kspan);
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