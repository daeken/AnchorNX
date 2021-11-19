using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using LibHac.Common.Keys;
using LibHac.Crypto;
using LibHac.Fs;
using LibHac.FsSystem;

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
		static readonly Dictionary<string, int> BisKeyIds = new() {
			["PRODINFO"] = 0, 
			["PRODINFOF"] = 0, 
			["SAFE"] = 1, 
			["SYSTEM"] = 2, 
			["USER"] = 3, 
		};
		
		readonly Dictionary<string, (AesXtsKey? Key, long Start, long Size)> Partitions = new();
		readonly Stream Fp;

		public NandImage(KeySet keys, Stream fp) {
			Fp = fp;
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
				var keyId = BisKeyIds.GetValueOrDefault(name, -1);
				Partitions[name] = (
					keyId == -1 ? null : keys.BisKeys[keyId], 
					(long) entry.LbaStart * 0x200, (long) entry.LbaEnd * 0x200
				);
			}
		}

		public Stream GetStreamFor(string name) {
			var (kid, start, end) = Partitions[name];
			if(kid == null) throw new NotImplementedException();
			var ns = new StreamStorage(Fp, true);
			var es = new SubStorage(ns, start, end - start);
			var xtss = new Aes128XtsStorage(es, kid.Value, 0x4000, true);
			var cxts = new CachedStorage(xtss, 0x4000, true);
			return new StorageStream(cxts, FileAccess.Read, true);
		}

		public byte[] ReadAll(string name) {
			var (kid, start, end) = Partitions[name];
			if(kid != null) {
				var s = GetStreamFor(name);
				var data = new byte[s.Length];
				s.Read(data);
				return data;
			} else {
				var data = new byte[end - start];
				Fp.Position = start;
				Fp.Read(data, 0, (int) (end - start));
				return data;
			}
		}
	}
}