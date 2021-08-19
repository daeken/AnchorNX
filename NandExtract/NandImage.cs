using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace NandExtract {
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	struct GptHeader {
		public ulong Signature; // 00
		public uint Revision, Size, Crc, _1; // 08
		public ulong MyLba, AltLba, FirstUseLba, LastUseLba; // 18
		public ulong Guid0, Guid1; // 38
		public ulong PartEntLba; // 48
		public uint NumPartEnts, PartEntSize, PartEntsCrc; // 50
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	unsafe struct GptEntry {
		public ulong TypeGuid0, TypeGuid1;
		public ulong PartGuid0, PartGuid1;
		public ulong LbaStart, LbaEnd, Attrs;
		public fixed ushort Name[36];

		public string GetName() {
			var name = "";
			for(var i = 0; i < 36; ++i) {
				var c = Name[i];
				if(c == 0)
					break;
				name += (char) c;
			}
			return name;
		}
	}
	
	public class NandImage {
		readonly Dictionary<string, (int KeyId, long Start, long Size)> Partitions = new();
		
		public NandImage(Stream fp) {
			Span<GptHeader> hs = stackalloc GptHeader[1];
			fp.Seek(0x200, SeekOrigin.Begin);
			fp.Read(MemoryMarshal.Cast<GptHeader, byte>(hs));
			var header = hs[0];
			Debug.Assert(header.Signature == 0x5452415020494645); // 'EFI PART'
			Debug.Assert(header.MyLba == 1);
			fp.Seek(0x200 * (long) header.AltLba, SeekOrigin.Begin);
			fp.Read(MemoryMarshal.Cast<GptHeader, byte>(hs));
			Debug.Assert(hs[0].Signature == 0x5452415020494645); // 'EFI PART'

			fp.Seek(0x200 * (long) header.PartEntLba, SeekOrigin.Begin);
			Span<GptEntry> entries = stackalloc GptEntry[(int) header.NumPartEnts];
			fp.Read(MemoryMarshal.Cast<GptEntry, byte>(entries));
			foreach(var entry in entries) {
				var name = entry.GetName();
				Console.WriteLine($"Foo? '{name}'");
			}
		}
	}
}