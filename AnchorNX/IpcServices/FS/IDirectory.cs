using System;
using LibHac.Fs;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IDirectory {
		public readonly LibHac.Fs.Fsa.IDirectory Backing;
		
		public IDirectory(LibHac.Fs.Fsa.IDirectory backing) => Backing = backing;

		public override void Read(out ulong read, Buffer<byte> entries) {
			Backing.GetEntryCount(out var count).ThrowIfFailure();
			var mc = Math.Min(count, entries.Length / 0x310);
			var sp = new DirectoryEntry[mc];
			Backing.Read(out var _read, sp).ThrowIfFailure();
			read = (ulong) _read;
			entries.CopyFrom(((Span<DirectoryEntry>) sp).As<DirectoryEntry, byte>());
		}

		public override ulong GetEntryCount() {
			Backing.GetEntryCount(out var count).ThrowIfFailure();
			return (ulong) count;
		}
	}
}