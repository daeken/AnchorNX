using System;
using System.Text;
using LibHac.Common;
using LibHac.Fs;
using LibHac.Fs.Fsa;
using LibHac.FsSystem;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IFileSystem {
		static readonly Logger Logger = new("IFileSystem");
		new static Action<string> Log = Logger.Log;
		
		readonly string RootPath;
		readonly LibHac.Fs.Fsa.IFileSystem Backing;

		public IFileSystem(string rootPath, LibHac.Fs.Fsa.IFileSystem backing) {
			RootPath = rootPath;
			Backing = backing;
		}

		public override void CreateFile(uint mode, ulong size, Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.SafeSpan).Split('\0', 2)[0];
			Log($"[{RootPath}] Attempting to create file '{fn}'");
			if(Backing.FileExists(fn)) {
				Log($"[{RootPath}] File already exists! '{fn}'");
				throw new IpcException(2U | (2U << 9));
			}
			Backing.CreateFile(fn.ToU8Span(), (long) size, (CreateFileOptions) mode).ThrowIfFailure();
		}

		public override void DeleteFile(Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.SafeSpan).Split('\0', 2)[0];
			Log($"[{RootPath}] Attempting to delete file '{fn}'");
			var res = Backing.DeleteFile(fn.ToU8Span());
			Log($"[{RootPath}] Res? 0x{res.Value:X} {res}");
			if(res.IsFailure())
				throw new IpcException(res.Value);
		}

		public override void CreateDirectory(Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.SafeSpan).Split('\0', 2)[0];
			Log($"[{RootPath}] Attempting to create directory '{fn}'");
			if(Backing.DirectoryExists(fn)) {
				Log($"[{RootPath}] Directory already exists! '{fn}'");
				throw new IpcException(2U | (2U << 9));
			}
			Backing.CreateDirectory(fn.ToU8Span()).ThrowIfFailure();
		}

		public override void DeleteDirectory(Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.SafeSpan).Split('\0', 2)[0];
			Log($"[{RootPath}] Attempting to delete directory '{fn}'");
			var res = Backing.DeleteDirectory(fn.ToU8Span());
			Log($"[{RootPath}] Deleted '{fn}'? 0x{res.Value:X} {res}");
			if(res.IsFailure())
				throw new IpcException(res.Value);
		}

		public override void DeleteDirectoryRecursively(Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.SafeSpan).Split('\0', 2)[0];
			Log($"[{RootPath}] Attempting to recursively delete directory '{fn}'");
			var res = Backing.DeleteDirectoryRecursively(fn.ToU8Span());
			Log($"[{RootPath}] Deleted '{fn}'? 0x{res.Value:X} {res}");
			if(res.IsFailure())
				throw new IpcException(res.Value);
		}

		public override void RenameFile(Buffer<byte> old_path, Buffer<byte> new_path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.RenameFile [5]".Debug(Log);
		public override void RenameDirectory(Buffer<byte> old_path, Buffer<byte> new_path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.RenameDirectory [6]".Debug(Log);
		public override DirectoryEntryType GetEntryType(Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.SafeSpan).Split('\0', 2)[0];
			Log($"[{RootPath}] Attempting to get entry type for '{fn}'");
			if(Backing.GetEntryType(out var het, fn.ToU8Span()).IsFailure()) {
				Log($"[{RootPath}] Path doesn't exist! '{fn}'");
				throw new IpcException(2U | (1U << 9));
			}
			return het switch {
				LibHac.Fs.DirectoryEntryType.Directory => DirectoryEntryType.Directory, 
				LibHac.Fs.DirectoryEntryType.File => DirectoryEntryType.File,
				_ => throw new NotSupportedException()
			};
		}

		public override IFile OpenFile(uint mode, Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.SafeSpan).Split('\0', 2)[0];
			Log($"[{RootPath}] Attempting to open '{fn}'");
			try {
				if(!Backing.FileExists(fn))
					throw new Exception();
			} catch(Exception) {
				Log($"[{RootPath}] Could not open '{fn}'");
				throw new IpcException(2U | (1U << 9)); // FS.PathDoesNotExist
			}

			Backing.OpenFile(out var hif, fn.ToU8Span(), (OpenMode) mode).ThrowIfFailure();
			return new IFile($"{fn} in {RootPath}", hif);
		}
		
		public override IDirectory OpenDirectory(uint filter_flags, Buffer<byte> path) {
			var fn = Encoding.ASCII.GetString(path.SafeSpan).Split('\0', 2)[0];
			Log($"[{RootPath}] Attempting to open directory '{fn}'");
			if(!Backing.DirectoryExists(fn))
				throw new IpcException(2U | (1U << 9)); // FS.PathDoesNotExist
			Backing.OpenDirectory(out var dir, fn.ToU8Span(), (OpenDirectoryMode) filter_flags).ThrowIfFailure();
			return new IDirectory(dir);
		}

		public override void Commit() => "Stub hit for Nn.Fssrv.Sf.IFileSystem.Commit [10]".Debug(Log);
		public override ulong GetFreeSpaceSize(Buffer<byte> path) => throw new NotImplementedException();
		public override ulong GetTotalSpaceSize(Buffer<byte> path) => throw new NotImplementedException();
		public override void CleanDirectoryRecursively(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.CleanDirectoryRecursively [13]".Debug(Log);
		public override void GetFileTimeStampRaw(Buffer<byte> path, out byte[] timestamp) => throw new NotImplementedException();
		public override void QueryEntry(uint _0, Buffer<byte> path, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
	}
}