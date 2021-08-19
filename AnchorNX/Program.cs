using System;
using System.IO;
using IronVisor;

namespace AnchorNX {
	class Program {
		static void Main(string[] args) {
			var packager = new Package2Packager("BCPKG2-1-Normal-Main.img");
			packager.Remove("FS");
			packager.Remove("boot");
			packager.Add(File.ReadAllBytes("HvcProxy.kip"));
			var package2 = packager.BuildPackage2();

			Box.DisabledTitles.Add(0x0100000000000006); // usb
			Box.DisabledTitles.Add(0x010000000000001D); // pcie
			Box.DisabledTitles.Add(0x010000000000000A); // bus
			Box.DisabledTitles.Add(0x010000000000001A); // pcv
			Box.DisabledTitles.Add(0x0100000000000019); // nvservices
			Box.DisabledTitles.Add(0x0100000000000010); // ptm
			Box.DisabledTitles.Add(0x0100000000000013); // hid
			Box.DisabledTitles.Add(0x0100000000000014); // audio
			Box.DisabledTitles.Add(0x0100000000000016); // wlan
			Box.DisabledTitles.Add(0x010000000000000B); // bluetooth
			Box.DisabledTitles.Add(0x0100000000000012); // bsdsockets
			Box.DisabledTitles.Add(0x0100000000000020); // nfc
			Box.DisabledTitles.Add(0x010000000000002A); // btm
			
			Box.Vm = new Vm();
			PhysMem.Map(0x8000_0000, 0x1_0000_0000);
			PhysMem.Map(0x5701_0000, 0xF_0000);

			Box.HvcProxy = new();

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

			var core = new Core(0x8006_0000);
			core.Run();
		}
	}
}