using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using LibHac;
using LibHac.Boot;
using LibHac.Common.Keys;
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
		
		public Package2Packager(string fn) {
			var reader = new Package2StorageReader();
			var keyset = ExternalKeyReader.ReadKeyFile(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".switch/prod.keys"));
			var raw = new LocalStorage(fn, FileAccess.Read);
			raw.GetSize(out var rsize).ThrowIfFailure();
			var sub = new SubStorage(raw, 0x4000, rsize - 0x4000);
			reader.Initialize(keyset, sub).ThrowIfFailure();

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

		public byte[] BuildPackage2() {
			var kips = Kips.Where(x => !ToRemove.Contains(x.Key))
				.Select(x => x.Value).Concat(ToAdd).ToList();
			
			var pklen = (uint) KernelData.Length;
			if((pklen & 0xFFF) != 0) pklen = (pklen & ~0xFFFU) + 0x1000;
			var iniSize = Marshal.SizeOf<IniHeader>() + kips.Select(x => x.Length).Sum();
			var tlen = (uint) ((int) pklen + iniSize);
			if((tlen & 0xFFF) != 0) tlen = (tlen & ~0xFFFU) + 0x1000;
			var data = new byte[(int) tlen];
			Span<byte> dspan = data;
			KernelData.CopyTo(data, 0);
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

			var ip = (int) pklen + Marshal.SizeOf<IniHeader>();
			foreach(var kip in kips) {
				kip.CopyTo(data, ip);
				ip += kip.Length;
			}

			return data;
		}
	}
}