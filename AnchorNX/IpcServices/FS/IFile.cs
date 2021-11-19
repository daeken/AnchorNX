using System;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IFile {
		static readonly Logger Logger = new("IFile");
		new static Action<string> Log = Logger.Log;

		readonly string Path;
		readonly LibHac.Fs.Fsa.IFile Backing;
		
		public IFile(string path, LibHac.Fs.Fsa.IFile backing) {
			Path = path;
			Backing = backing;
		}

		public override void Read(uint readOption, ulong offset, ulong size, out ulong out_size, Buffer<byte> out_buf) {
			var temp = new byte[Math.Min((int) size, out_buf.Length)];
			var res = Backing.Read(out var read, (long) offset, temp, new((int) readOption));
			Log($"[{Path}] Attempted read into 0x{out_buf.Address:X}, readoption {readOption} offset 0x{offset:X} size 0x{size:X} -- actually read 0x{read:X}");
			if(res.IsFailure()) {
				Log($"[{Path}] Read failed! {res}");
				throw new IpcException(res.Value);
			}

			out_buf.CopyFrom(temp[..(int) read]);
			//out_buf.SafeSpan.Hexdump(Logger);
			out_size = (ulong) read;
		}

		public override void Write(uint writeOption, ulong offset, ulong size, Buffer<byte> in_buf) {
			var temp = new byte[(int) size];
			in_buf.CopyTo(temp);
			Log($"[{Path}] Attempted write from 0x{in_buf.Address:X}, writeoption {writeOption} offset 0x{offset:X} size 0x{size:X} -- buffer size 0x{in_buf.Length:X}");
			//temp.Hexdump(Logger);
			Backing.Write((long) offset, temp, new((int) writeOption)).ThrowIfFailure();
		}

		public override void Flush() => Backing.Flush();
		
		public override void SetSize(ulong size) {
			Log($"[{Path}] Setting size 0x{size:X}");
			Backing.SetSize((long) size);
		}

		public override ulong GetSize() {
			Backing.GetSize(out var size).ThrowIfFailure();
			Log($"[{Path}] Got size 0x{size:X}");
			return (ulong) size;
		}

		public override void OperateRange(uint _0, ulong _1, ulong _2, out byte[] _3) => throw new NotImplementedException();

		public override void Close() {
			Backing.Flush();
			Backing.Dispose();
		}
	}
}