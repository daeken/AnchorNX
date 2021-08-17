using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using LibHac.Crypto;
using LibHac.FsSystem;
using LibHac.FsSystem.NcaUtils;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IFileSystemProxyForLoader {
		static readonly Logger Logger = new("IFileSystemProxyForLoader");
		new static Action<string> Log = Logger.Log;
		
		static FieldInfo HeaderField =
			typeof(NcaHeader).GetField("_header", BindingFlags.Instance | BindingFlags.NonPublic);
		
		public override IFileSystem OpenCodeFileSystem(ulong tid, Buffer<byte> content_path, Buffer<byte> code_info) {
			if(Box.DisabledTitles.Contains(tid)) {
				Log($"Attempting to open code file system for TID {tid:X} -- disabled!");
				throw new IpcException(0x7D402);
			}
			var fn = Encoding.ASCII.GetString(content_path.Span).Split('\0', 2)[0];
			Log($"Attempting to open code file system '{fn}' -- TID 0x{tid:X}");
			Debug.Assert(fn.StartsWith("@SystemContent://"));
			code_info.Span.Clear();
			var ncaFn = $"/Volumes/NO NAME/Contents/{fn.Split('/', 2)[1]}/00";
			var nca = new Nca(Box.KeySet, new LocalStorage(ncaFn, FileAccess.Read));
			var header = nca.Header;
			header.Signature2.CopyTo(code_info);
			var hm = (Memory<byte>) HeaderField.GetValue(header);
			Span<byte> hash = stackalloc byte[0x20];
			Sha256.GenerateSha256Hash(hm.Span[0x200..0x400], hash);
			hash.CopyTo(code_info.Span[0x100..]);
			code_info[0x120] = 1;
			code_info.Hexdump(Logger);
			return new IFileSystem(nca.OpenFileSystem(NcaSectionType.Code, IntegrityCheckLevel.ErrorOnInvalid));
		}

		public override byte IsArchivedProgram(ulong _0) => throw new NotImplementedException();
		public override void SetCurrentProcess(ulong _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxyForLoader.SetCurrentProcess [2]".Debug(Log);
	}
}