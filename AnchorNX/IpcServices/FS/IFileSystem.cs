using System;
using System.Text;
using LibHac.Common;
using LibHac.Fs;
using LibHac.FsSystem;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IFileSystem {
		static readonly Logger Logger = new("IFileSystem");
		new static Action<string> Log = Logger.Log;
		
		readonly LibHac.Fs.Fsa.IFileSystem Backing;

		public IFileSystem(LibHac.Fs.Fsa.IFileSystem backing) => Backing = backing;
		
		public override void CreateFile(uint mode, ulong size, Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.CreateFile [0]".Debug(Log);
		public override void DeleteFile(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.DeleteFile [1]".Debug(Log);
		public override void CreateDirectory(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.CreateDirectory [2]".Debug(Log);
		public override void DeleteDirectory(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.DeleteDirectory [3]".Debug(Log);
		public override void DeleteDirectoryRecursively(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.DeleteDirectoryRecursively [4]".Debug(Log);
		public override void RenameFile(Buffer<byte> old_path, Buffer<byte> new_path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.RenameFile [5]".Debug(Log);
		public override void RenameDirectory(Buffer<byte> old_path, Buffer<byte> new_path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.RenameDirectory [6]".Debug(Log);
		public override DirectoryEntryType GetEntryType(Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.Span).Split('\0', 2)[0];
			Log($"Attempting to get entry type for '{fn}'");
			if(Backing.GetEntryType(out var het, fn.ToU8Span()).IsFailure()) {
				Log($"Path doesn't exist! '{fn}'");
				throw new IpcException(2U | (1U << 9));
			}
			return het switch {
				LibHac.Fs.DirectoryEntryType.Directory => DirectoryEntryType.Directory, 
				LibHac.Fs.DirectoryEntryType.File => DirectoryEntryType.File,
				_ => throw new NotSupportedException()
			};
		}

		public override IFile OpenFile(uint mode, Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.Span).Split('\0', 2)[0];
			Log($"Attempting to open '{fn}'");
			if(!Backing.FileExists(fn))
				throw new IpcException(2U | (1U << 9)); // FS.PathDoesNotExist
			Backing.OpenFile(out var hif, fn.ToU8Span(), (OpenMode) mode).ThrowIfFailure();
			return new IFile(hif);
		}
		
		public override IDirectory OpenDirectory(uint filter_flags, Buffer<byte> path) => throw new NotImplementedException();
		public override void Commit() => "Stub hit for Nn.Fssrv.Sf.IFileSystem.Commit [10]".Debug(Log);
		public override ulong GetFreeSpaceSize(Buffer<byte> path) => throw new NotImplementedException();
		public override ulong GetTotalSpaceSize(Buffer<byte> path) => throw new NotImplementedException();
		public override void CleanDirectoryRecursively(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.CleanDirectoryRecursively [13]".Debug(Log);
		public override void GetFileTimeStampRaw(Buffer<byte> path, out byte[] timestamp) => throw new NotImplementedException();
		public override void QueryEntry(uint _0, Buffer<byte> path, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
	}
}