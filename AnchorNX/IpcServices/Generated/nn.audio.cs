#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Audio {
	public unsafe struct AudioInBuffer {
		public ulong Next;
		public ulong Samples;
		public ulong Capacity;
		public ulong Size;
		public ulong Offset;
	}
	
	public unsafe struct AudioOutBuffer {
		public ulong Next;
		public ulong Samples;
		public ulong Capacity;
		public ulong Size;
		public ulong Offset;
	}
}
