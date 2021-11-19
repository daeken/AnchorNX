using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LibHac;
using LibHac.Boot;
using LibHac.Common.Keys;
using LibHac.Diag;
using LibHac.Fs;
using LibHac.FsSystem;
using LibHac.Kernel;
using IniHeader = LibHac.Kernel.InitialProcessBinaryReader.IniHeader;

namespace AnchorNX {
	public class Package2Packager {
		const int IniPayloadIndex = 1;

		readonly byte[] KernelData;
		readonly int IniOffset, IniSize, IniPointerOffset;

		readonly Dictionary<string, byte[]> Kips = new();

		readonly List<string> ToRemove = new();
		readonly List<byte[]> ToAdd = new();
		readonly Dictionary<string, List<(int Address, uint Compare, uint Replace)>> TextPatches = new();
		
		public Package2Packager(string fn) {
			var reader = new Package2StorageReader();
			var raw = new LocalStorage(fn, FileAccess.Read);
			raw.GetSize(out var rsize).ThrowIfFailure();
			var sub = new SubStorage(raw, 0x4000, rsize - 0x4000);
			reader.Initialize(Box.KeySet, sub).ThrowIfFailure();

			Debug.Assert(reader.Header.Meta.PayloadSizes[IniPayloadIndex] == 0); // TODO: Support pre-inlined kernels

			reader.OpenKernel(out var kstorage).ThrowIfFailure();
			
			kstorage.GetSize(out var size);
			KernelData = new byte[size];
			kstorage.Read(0, KernelData).ThrowIfFailure();

			// TODO: Add search logic
			IniOffset = 0x68000;
			IniPointerOffset = 0x180;
			var iniHeader = MemoryMarshal.Cast<byte, IniHeader>(((Span<byte>) KernelData)[IniOffset..])[0];
			Debug.Assert(iniHeader.Magic == 0x31494E49); // INI1
			IniSize = iniHeader.Size;

			var iniReader = new InitialProcessBinaryReader();
			iniReader.Initialize(new SubStorage(new MemoryStorage(KernelData), IniOffset, IniSize)).ThrowIfFailure();

			for(var i = 0; i < iniReader.ProcessCount; ++i) {
				iniReader.OpenKipStorage(out var ks, i).ThrowIfFailure();
				var kr = new KipReader();
				kr.Initialize(ks).ThrowIfFailure();
				var name = kr.Name.ToString();
				Debug.Assert(!Kips.ContainsKey(name));
				ks.GetSize(out var ksize);
				var kdata = Kips[name] = new byte[ksize];
				ks.Read(0, kdata).ThrowIfFailure();
				Console.WriteLine($"Kip: '{name}'");
			}

			KernelData = File.ReadAllBytes("mesosphere_debug.bin");
			IniPointerOffset = 0x8;
		}

		public Package2Packager Add(byte[] kip) {
			ToAdd.Add(kip);
			return this;
		}

		public Package2Packager Remove(string name) {
			Debug.Assert(Kips.ContainsKey(name));
			ToRemove.Add(name);
			return this;
		}

		public Package2Packager TextReplace(string name, ulong addr, uint compare, uint replace) {
			if(!TextPatches.TryGetValue(name, out var list))
				list = TextPatches[name] = new();
			list.Add(((int) addr, compare, replace));
			return this;
		}

		byte[] DoReplacements(string name, byte[] data) {
			if(!TextPatches.TryGetValue(name, out var list)) return data;

			var uar = MemoryMarshal.Cast<byte, uint>(data);
			var toff = (int) uar[0x20 / 4] + 0x100;
			var tsiz = (int) uar[0x28 / 4];
			var text = data[toff..(toff + tsiz)];
			if(((uar[0x1C / 4] >> 24) & 1) == 1)
				text = DecompressBlz(text);

			var ut = MemoryMarshal.Cast<byte, uint>(text);
			foreach(var (addr, comp, repl) in list) {
				if(ut[addr / 4] == comp)
					ut[addr / 4] = repl;
				else
					Console.WriteLine($"[{name}] Expected value at 0x{addr:X} to be 0x{comp:X} -- got 0x{ut[addr / 4]:X}");
			}
			text = MemoryMarshal.Cast<uint, byte>(ut).ToArray();

			if(text.Length > tsiz) {
				var diff = text.Length - tsiz;
				var tdat = new byte[data.Length + diff];
				uar[0x1C / 4] ^= 1U << 24;
				uar[0x24 / 4] = uar[0x28 / 4] = (uint) text.Length;
				data = MemoryMarshal.Cast<uint, byte>(uar).ToArray();
				/*uar[0x30 / 4] += (uint) diff;
				uar[0x40 / 4] += (uint) diff;
				uar[0x50 / 4] += (uint) diff;*/
				Array.Copy(data, 0, tdat, 0, toff);
				Array.Copy(text, 0, tdat, toff, text.Length);
				Array.Copy(data, toff + tsiz, tdat, toff + text.Length, data.Length - (toff + tsiz));
				return tdat;
			}

			Array.Copy(text, 0, data, toff, text.Length);
			return data;
		}

		static byte[] DecompressBlz(byte[] compressed) {
			var additionalSize = BitConverter.ToInt32(compressed, compressed.Length - 4);
			var headerSize = BitConverter.ToInt32(compressed, compressed.Length - 8);
			var totalCompSize = BitConverter.ToInt32(compressed, compressed.Length - 12);

			var decompressed = new byte[totalCompSize + additionalSize];

			var inOffset = totalCompSize - headerSize;
			var outOffset = totalCompSize + additionalSize;

			while(outOffset > 0) {
				var control = compressed[--inOffset];
				for(var i = 0; i < 8; i++) {
					if((control & 0x80) != 0) {
						if(inOffset < 2) throw new InvalidDataException("KIP1 decompression out of bounds!");

						inOffset -= 2;

						var segmentValue = BitConverter.ToUInt16(compressed, inOffset);
						var segmentSize = ((segmentValue >> 12) & 0xF) + 3;
						var segmentOffset = (segmentValue & 0x0FFF) + 3;

						if(outOffset < segmentSize)
							segmentSize = outOffset;

						outOffset -= segmentSize;

						for(var j = 0; j < segmentSize; j++)
							decompressed[outOffset + j] = decompressed[outOffset + j + segmentOffset];
					} else {
						if(inOffset < 1) throw new InvalidDataException("KIP1 decompression out of bounds!");

						decompressed[--outOffset] = compressed[--inOffset];
					}

					control <<= 1;
					if(outOffset == 0) return decompressed;
				}
			}

			return decompressed;
		}

		public byte[] BuildPackage2() {
			var kips = Kips.Where(x => !ToRemove.Contains(x.Key))
				.Select(x => DoReplacements(x.Key, x.Value))
				.Concat(ToAdd).ToList();
			
			var pklen = (uint) KernelData.Length;
			if((pklen & 0xFFF) != 0) pklen = (pklen & ~0xFFFU) + 0x1000;
			var iniSize = Unsafe.SizeOf<IniHeader>() + kips.Select(x => x.Length).Sum();
			var tlen = (uint) ((int) pklen + iniSize);
			if((tlen & 0xFFF) != 0) tlen = (tlen & ~0xFFFU) + 0x1000;
			var data = new byte[(int) tlen];
			Span<byte> dspan = data;
			KernelData.CopyTo(data, 0);
			if(IniPointerOffset != 0x8)
				for(var i = 0; i < IniSize; ++i)
					data[IniOffset + i] = 0xDE;

			var iniPointer = MemoryMarshal.Cast<byte, uint>(dspan[IniPointerOffset..]);
			iniPointer[0] = pklen;

			var iniHeader = new IniHeader {
				Magic = 0x31494E49, 
				Size = iniSize, 
				ProcessCount = kips.Count, 
				Reserved = 0
			};
			MemoryMarshal.Cast<byte, IniHeader>(dspan[(int) pklen..])[0] = iniHeader;

			var ip = (int) pklen + Unsafe.SizeOf<IniHeader>();
			foreach(var kip in kips) {
				kip.CopyTo(data, ip);
				ip += kip.Length;
			}

			return data;
		}
	}
}