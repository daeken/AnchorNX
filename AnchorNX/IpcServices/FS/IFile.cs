using System;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IFile {
		readonly LibHac.Fs.Fsa.IFile Backing;
		readonly long Length;
		
		public IFile(LibHac.Fs.Fsa.IFile backing) {
			Backing = backing;
			Backing.GetSize(out Length).ThrowIfFailure();
		}

		public override void Read(uint readOption, ulong offset, ulong size, out ulong out_size, Buffer<byte> out_buf) {
			out_size = 0;
			for(var i = 0UL; i < size;) {
				var pageOff = 0x1000 - ((out_buf.Address + i) & 0xFFF);
				var tr = (int) Math.Min((ulong) Length - offset, Math.Min(pageOff, size - i));
				var cs = out_buf.SpanFrom((int) i)[..tr];
				Console.WriteLine($"Buffer for offset 0x{offset+i:X}");
				Backing.Read(out var read, (long) (offset + i), cs, new((int) readOption))
					.ThrowIfFailure();
				cs.Hexdump();
				out_size += (ulong) read;
				i += (ulong) read;
			}

			Console.WriteLine($"Attempted read into 0x{out_buf.Address:X}, readoption {readOption} offset 0x{offset:X} size 0x{size:X} -- actually read 0x{out_size:X}");
		}

		public override void Write(uint writeOption, ulong offset, ulong size, Buffer<byte> in_buf) =>
			Backing.Write((long) offset, in_buf.Span, new((int) writeOption)).ThrowIfFailure();

		public override void Flush() => Backing.Flush();
		
		public override void SetSize(ulong size) => Backing.SetSize((long) size);

		public override ulong GetSize() {
			Console.WriteLine($"Size! {Length}");
			return (ulong) Length;
		}

		public override void OperateRange(uint _0, ulong _1, ulong _2, out byte[] _3) => throw new NotImplementedException();
	}
}