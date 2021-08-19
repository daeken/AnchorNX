using System;
using System.IO;

namespace NandExtract {
	class Program {
		static void Main(string[] args) {
			var nand = new NandImage(File.OpenRead(args[0]));
		}
	}
}