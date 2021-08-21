using System.Threading;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap {
	class NvMapHandle {
		long _dupes;
		public ulong Address;
		public int Align;
		public bool Allocated;
		public ulong DmaMapAddress;
		public int Kind;
		public int Size;

		public NvMapHandle() => _dupes = 1;

		public NvMapHandle(int size) : this() => Size = size;

		public void IncrementRefCount() {
			Interlocked.Increment(ref _dupes);
		}

		public long DecrementRefCount() {
			return Interlocked.Decrement(ref _dupes);
		}
#pragma warning disable CS0649
		public int Handle;
		public int Id;
#pragma warning restore CS0649
	}
}