using System;
using System.IO;
using System.Runtime.InteropServices;
using DiscUtils;
using DiscUtils.Fat;
using DiscUtils.Streams;
using LibHac.Common;
using LibHac.Common.Keys;
using LibHac.Fs;
using LibHac.Fs.Fsa;
using LibHac.FsSystem;
using LibHac.FsSystem.Save;
using Directory = System.IO.Directory;
using Path = System.IO.Path;

namespace NandExtract {
	class Program {
		static KeySet Keyset;
		static void Main(string[] args) {
			Keyset = ExternalKeyReader.ReadKeyFile(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".switch/prod.keys"));
			var nand = new NandImage(Keyset, File.OpenRead(args[0]));

			var root = args[1];
			System.IO.Directory.CreateDirectory(root);
			System.IO.Directory.CreateDirectory(Path.Join(root, "raw"));
			File.WriteAllBytes(Path.Join(root, "raw", "BCPKG2-1-Normal-Main.bin"), nand.ReadAll("BCPKG2-1-Normal-Main"));
			File.WriteAllBytes(Path.Join(root, "raw", "PRODINFO.bin"), nand.ReadAll("PRODINFO"));
			File.WriteAllBytes(Path.Join(root, "raw", "PRODINFOF.bin"), nand.ReadAll("PRODINFOF"));

			foreach(var pn in new[] { "SYSTEM", "USER" }) {
				var stream = nand.GetStreamFor(pn);
				var fs = new FatFileSystem(stream);
				var rdir = fs.GetDirectoryInfo(@"\");
				Extract(rdir, Path.Join(root, pn.ToLower()));
			}
		}

		static void Extract(DiscDirectoryInfo dir, string path) {
			Directory.CreateDirectory(path);
			foreach(var sdir in dir.GetDirectories()) {
				if(sdir.Name.EndsWith(".nca")) {
					var ncaf = sdir.GetFiles()[0];
					var ncas = ncaf.OpenRead();
					using var fp = File.OpenWrite(Path.Join(path, sdir.Name));
					ncas.CopyTo(fp);
				} else if(sdir.Name.ToLower() == "save") {
					foreach(var fin in sdir.GetFiles()) {
						var sfs = new SaveDataFileSystem(Keyset, new StreamStorage(fin.OpenRead(), true),
							IntegrityCheckLevel.ErrorOnInvalid, true);
						sfs.Extract(Path.Join(path, "save", fin.Name, "0"));
						sfs.Extract(Path.Join(path, "save", fin.Name, "1"));
						var ed = MemoryMarshal.Cast<SaveDataExtraData, byte>(new SaveDataExtraData[1]);
						File.WriteAllBytes(Path.Join(path, "save", fin.Name, "ExtraData0"), ed.ToArray());
						File.WriteAllBytes(Path.Join(path, "save", fin.Name, "ExtraData1"), ed.ToArray());
					}
				} else
					Extract(sdir, Path.Join(path, sdir.Name));
			}

			foreach(var fin in dir.GetFiles()) {
				var s = fin.OpenRead();
				using var fp = File.OpenWrite(Path.Join(path, fin.Name));
				s.CopyTo(fp);
			}
		}
	}
}