using System.Collections.Generic;
using Ryujinx.Graphics.Gpu.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu.Types {
	class AddressSpaceContext {
		readonly SortedList<ulong, Range> _maps;
		readonly SortedList<ulong, Range> _reservations;

		public AddressSpaceContext(MemoryManager gmm) {
			Gmm = gmm;

			_maps = new SortedList<ulong, Range>();
			_reservations = new SortedList<ulong, Range>();
		}

		public MemoryManager Gmm { get; }

		public bool ValidateFixedBuffer(ulong address, ulong size, ulong alignment) {
			var mapEnd = address + size;

			// Check if size is valid (0 is also not allowed).
			if(mapEnd <= address) return false;

			// Check if address is aligned.
			if((address & (alignment - 1)) != 0) return false;

			// Check if region is reserved.
			if(BinarySearch(_reservations, address) == null) return false;

			// Check for overlap with already mapped buffers.
			var map = BinarySearchLt(_maps, mapEnd);

			if(map != null && map.End > address) return false;

			return true;
		}

		public void AddMap(ulong gpuVa, ulong size, ulong physicalAddress, bool vaAllocated) {
			_maps.Add(gpuVa, new MappedMemory(gpuVa, size, physicalAddress, vaAllocated));
		}

		public bool RemoveMap(ulong gpuVa, out ulong size) {
			size = 0;

			if(_maps.Remove(gpuVa, out var value)) {
				var map = (MappedMemory) value;

				if(map.VaAllocated) size = map.End - map.Start;

				return true;
			}

			return false;
		}

		public bool TryGetMapPhysicalAddress(ulong gpuVa, out ulong physicalAddress) {
			var map = BinarySearch(_maps, gpuVa);

			if(map != null) {
				physicalAddress = ((MappedMemory) map).PhysicalAddress;
				return true;
			}

			physicalAddress = 0;
			return false;
		}

		public void AddReservation(ulong gpuVa, ulong size) {
			_reservations.Add(gpuVa, new Range(gpuVa, size));
		}

		public bool RemoveReservation(ulong gpuVa) {
			return _reservations.Remove(gpuVa);
		}

		static Range BinarySearch(SortedList<ulong, Range> list, ulong address) {
			var left = 0;
			var right = list.Count - 1;

			while(left <= right) {
				var size = right - left;

				var middle = left + (size >> 1);

				var rg = list.Values[middle];

				if(address >= rg.Start && address < rg.End) return rg;

				if(address < rg.Start)
					right = middle - 1;
				else
					left = middle + 1;
			}

			return null;
		}

		static Range BinarySearchLt(SortedList<ulong, Range> list, ulong address) {
			Range ltRg = null;

			var left = 0;
			var right = list.Count - 1;

			while(left <= right) {
				var size = right - left;

				var middle = left + (size >> 1);

				var rg = list.Values[middle];

				if(address < rg.Start)
					right = middle - 1;
				else {
					left = middle + 1;

					if(address > rg.Start) ltRg = rg;
				}
			}

			return ltRg;
		}

		class Range {
			public Range(ulong address, ulong size) {
				Start = address;
				End = size + Start;
			}

			public ulong Start { get; }
			public ulong End { get; }
		}

		class MappedMemory : Range {
			public MappedMemory(ulong address, ulong size, ulong physicalAddress, bool vaAllocated) : base(address,
				size) {
				PhysicalAddress = physicalAddress;
				VaAllocated = vaAllocated;
			}

			public ulong PhysicalAddress { get; }
			public bool VaAllocated { get; }
		}
	}
}