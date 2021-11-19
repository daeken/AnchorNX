using System;
using System.Collections.Generic;
using Ryujinx.Cpu;
using Ryujinx.Cpu.Tracking;
using Ryujinx.Memory;
using Ryujinx.Memory.Range;
using Ryujinx.Memory.Tracking;

namespace AnchorNX.IpcServices.Nns.Nvdrv {
	public class NvVmm : IVirtualMemoryManagerTracked {
		readonly ulong Ttbr0;

		public NvVmm(ulong ttbr0) => Ttbr0 = ttbr0;
		
		public void Map(ulong va, nuint hostAddress, ulong size) {
			throw new NotImplementedException();
		}

		public void Unmap(ulong va, ulong size) {
			throw new NotImplementedException();
		}

		public T Read<T>(ulong va) where T : unmanaged {
			throw new NotImplementedException();
		}

		public void Read(ulong va, Span<byte> data) {
			throw new NotImplementedException();
		}

		public void Write<T>(ulong va, T value) where T : unmanaged {
			throw new NotImplementedException();
		}

		public void Write(ulong va, ReadOnlySpan<byte> data) {
			throw new NotImplementedException();
		}

		public ReadOnlySpan<byte> GetSpan(ulong va, int size, bool tracked = false) {
			throw new NotImplementedException();
		}

		public WritableRegion GetWritableRegion(ulong va, int size, bool tracked = false) {
			throw new NotImplementedException();
		}

		public ref T GetRef<T>(ulong va) where T : unmanaged {
			throw new NotImplementedException();
		}

		public IEnumerable<HostMemoryRange> GetPhysicalRegions(ulong va, ulong size) {
			throw new NotImplementedException();
		}

		public bool IsMapped(ulong va) {
			throw new NotImplementedException();
		}

		public bool IsRangeMapped(ulong va, ulong size) {
			throw new NotImplementedException();
		}

		public void SignalMemoryTracking(ulong va, ulong size, bool write) {
			throw new NotImplementedException();
		}

		public void TrackingReprotect(ulong va, ulong size, MemoryPermission protection) {
			throw new NotImplementedException();
		}

		public void WriteUntracked(ulong va, ReadOnlySpan<byte> data) {
			throw new NotImplementedException();
		}

		public CpuRegionHandle BeginTracking(ulong address, ulong size) {
			throw new NotImplementedException();
		}

		public CpuMultiRegionHandle BeginGranularTracking(ulong address, ulong size, IEnumerable<IRegionHandle> handles, ulong granularity) {
			throw new NotImplementedException();
		}

		public CpuSmartMultiRegionHandle BeginSmartGranularTracking(ulong address, ulong size, ulong granularity) {
			throw new NotImplementedException();
		}
	}
}