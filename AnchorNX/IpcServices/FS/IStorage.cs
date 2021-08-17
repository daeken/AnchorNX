using System;
using System.Diagnostics;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IStorage {
		static readonly Logger Logger = new("IFile");
		new static Action<string> Log = Logger.Log;
		
		readonly LibHac.Fs.IStorage Backing;
		readonly long Length;

		public IStorage(LibHac.Fs.IStorage backing) {
			Backing = backing;
			backing.GetSize(out Length).ThrowIfFailure();
		}

		public override void Read(ulong offset, ulong length, Buffer<byte> data) {
			Backing.Read((long) offset, data).ThrowIfFailure();
			if((int) length > data.Length) {
				Log("OVERLENGTH BUFFER!");
				length = (ulong) data.Length;
			}
			
			Debug.Assert(length == (ulong) data.Length);

			for(var i = 0UL; i < length; ) {
				var pageOff = 0x1000 - ((data.Address + i) & 0xFFF);
				var tr = (int) Math.Min((ulong) Length - offset, Math.Min(pageOff, length - i));
				var cs = data.SpanFrom((int) i)[..tr];
				Log($"Buffer for offset 0x{offset+i:X}");
				Backing.Read((long) (offset + i), cs).ThrowIfFailure();
				cs.Hexdump(Logger);
				i += (ulong) Math.Min(tr, cs.Length);
			}

			Log($"Attempted read into 0x{data.Address:X}, offset 0x{offset:X} size 0x{length:X}");
		}

		public override void Write(ulong offset, ulong length, Buffer<byte> data) => "Stub hit for Nn.Fssrv.Sf.IStorage.Write [1]".Debug(Log);
		public override void Flush() => Backing.Flush().ThrowIfFailure();
		public override void SetSize(ulong size) => "Stub hit for Nn.Fssrv.Sf.IStorage.SetSize [3]".Debug(Log);
		public override ulong GetSize() {
			Log($"GetSize: 0x{Length:X}");
			return (ulong) Length;
		}

		public override void OperateRange(uint _0, ulong _1, ulong _2, out byte[] _3) => throw new NotImplementedException();
	}
}