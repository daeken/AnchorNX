using System;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu.Types;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap;
using Ryujinx.Common.Logging;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu {
	class NvHostAsGpuDeviceFile : NvDeviceFile {
		static readonly Logger Logger = new("NvHostAsGpuDeviceFile");
		static Action<string> Log = Logger.Log;
		
		readonly AddressSpaceContext _asContext;
		readonly NvMemoryAllocator _memoryAllocator;

		public NvHostAsGpuDeviceFile(IVirtualMemoryManager memory, long owner) : base(owner) {
			_asContext = new AddressSpaceContext(Box.Gpu.CreateMemoryManager(owner));
			_memoryAllocator = new NvMemoryAllocator();
		}

		public override NvInternalResult Ioctl(NvIoctl command, Span<byte> arguments) {
			var result = NvInternalResult.NotImplemented;

			if(command.Type == NvIoctl.NvGpuAsMagic)
				switch(command.Number) {
					case 0x01:
						result = CallIoctlMethod<BindChannelArguments>(BindChannel, arguments);
						break;
					case 0x02:
						result = CallIoctlMethod<AllocSpaceArguments>(AllocSpace, arguments);
						break;
					case 0x03:
						result = CallIoctlMethod<FreeSpaceArguments>(FreeSpace, arguments);
						break;
					case 0x05:
						result = CallIoctlMethod<UnmapBufferArguments>(UnmapBuffer, arguments);
						break;
					case 0x06:
						result = CallIoctlMethod<MapBufferExArguments>(MapBufferEx, arguments);
						break;
					case 0x08:
						result = CallIoctlMethod<GetVaRegionsArguments>(GetVaRegions, arguments);
						break;
					case 0x09:
						result = CallIoctlMethod<InitializeExArguments>(InitializeEx, arguments);
						break;
					case 0x14:
						result = CallIoctlMethod<RemapArguments>(Remap, arguments);
						break;
				}

			return result;
		}

		public override NvInternalResult Ioctl3(NvIoctl command, Span<byte> arguments, Span<byte> inlineOutBuffer) {
			var result = NvInternalResult.NotImplemented;

			if(command.Type == NvIoctl.NvGpuAsMagic)
				switch(command.Number) {
					case 0x08:
						// This is the same as the one in ioctl as inlineOutBuffer is empty.
						result = CallIoctlMethod<GetVaRegionsArguments>(GetVaRegions, arguments);
						break;
				}

			return result;
		}

		NvInternalResult BindChannel(ref BindChannelArguments arguments) {
			var channelDeviceFile = INvDrvServices.DeviceFileIdRegistry.GetData<NvHostChannelDeviceFile>(arguments.Fd);
			if(channelDeviceFile == null) {
				// TODO: Return invalid Fd error.
			}

			channelDeviceFile.Channel.BindMemory(_asContext.Gmm);

			return NvInternalResult.Success;
		}

		NvInternalResult AllocSpace(ref AllocSpaceArguments arguments) {
			var size = arguments.Pages * (ulong) arguments.PageSize;

			var result = NvInternalResult.Success;

			lock(_asContext) {
				// Note: When the fixed offset flag is not set,
				// the Offset field holds the alignment size instead.
				if((arguments.Flags & AddressSpaceFlags.FixedOffset) != 0) {
					var regionInUse =
						_memoryAllocator.IsRegionInUse(arguments.Offset, size, out var freeAddressStartPosition);
					ulong address;

					if(!regionInUse) {
						_memoryAllocator.AllocateRange(arguments.Offset, size, freeAddressStartPosition);
						address = freeAddressStartPosition;
					} else
						address = NvMemoryAllocator.PteUnmapped;

					arguments.Offset = address;
				} else {
					var address =
						_memoryAllocator.GetFreeAddress(size, out var freeAddressStartPosition, arguments.Offset);
					if(address != NvMemoryAllocator.PteUnmapped)
						_memoryAllocator.AllocateRange(address, size, freeAddressStartPosition);

					arguments.Offset = address;
				}

				if(arguments.Offset == NvMemoryAllocator.PteUnmapped) {
					arguments.Offset = 0;

					Log($"Failed to allocate size {size:x16}!");

					result = NvInternalResult.OutOfMemory;
				} else
					_asContext.AddReservation(arguments.Offset, size);
			}

			return result;
		}

		NvInternalResult FreeSpace(ref FreeSpaceArguments arguments) {
			var size = arguments.Pages * (ulong) arguments.PageSize;

			var result = NvInternalResult.Success;

			lock(_asContext)
				if(_asContext.RemoveReservation(arguments.Offset)) {
					_memoryAllocator.DeallocateRange(arguments.Offset, size);
					_asContext.Gmm.Unmap(arguments.Offset, size);
				} else {
					Log($"Failed to free offset 0x{arguments.Offset:x16} size 0x{size:x16}!");

					result = NvInternalResult.InvalidInput;
				}

			return result;
		}

		NvInternalResult UnmapBuffer(ref UnmapBufferArguments arguments) {
			lock(_asContext)
				if(_asContext.RemoveMap(arguments.Offset, out var size)) {
					if(size != 0) {
						_memoryAllocator.DeallocateRange(arguments.Offset, size);
						_asContext.Gmm.Unmap(arguments.Offset, size);
					}
				} else
					Log($"Invalid buffer offset {arguments.Offset:x16}!");

			return NvInternalResult.Success;
		}

		NvInternalResult MapBufferEx(ref MapBufferExArguments arguments) {
			const string MapErrorMsg =
				"Failed to map fixed buffer with offset 0x{0:x16}, size 0x{1:x16} and alignment 0x{2:x16}!";

			ulong physicalAddress;

			if((arguments.Flags & AddressSpaceFlags.RemapSubRange) != 0)
				lock(_asContext)
					if(_asContext.TryGetMapPhysicalAddress(arguments.Offset, out physicalAddress)) {
						var virtualAddress = arguments.Offset + arguments.BufferOffset;

						physicalAddress += arguments.BufferOffset;
						_asContext.Gmm.Map(physicalAddress, virtualAddress, arguments.MappingSize);

						return NvInternalResult.Success;
					} else {
						Log($"Address 0x{arguments.Offset:x16} not mapped!");

						return NvInternalResult.InvalidInput;
					}

			var map = NvMapDeviceFile.GetMapFromHandle(Owner, arguments.NvMapHandle);

			if(map == null) {
				Log($"Invalid NvMap handle 0x{arguments.NvMapHandle:x8}!");

				return NvInternalResult.InvalidInput;
			}

			var pageSize = (ulong) arguments.PageSize;

			if(pageSize == 0) pageSize = (ulong) map.Align;

			physicalAddress = map.Address + arguments.BufferOffset;

			var size = arguments.MappingSize;

			if(size == 0) size = (uint) map.Size;

			var result = NvInternalResult.Success;

			lock(_asContext) {
				// Note: When the fixed offset flag is not set,
				// the Offset field holds the alignment size instead.
				var virtualAddressAllocated = (arguments.Flags & AddressSpaceFlags.FixedOffset) == 0;

				if(!virtualAddressAllocated) {
					if(_asContext.ValidateFixedBuffer(arguments.Offset, size, pageSize))
						_asContext.Gmm.Map(physicalAddress, arguments.Offset, size);
					else {
						var message = string.Format(MapErrorMsg, arguments.Offset, size, pageSize);
						Log(message);

						result = NvInternalResult.InvalidInput;
					}
				} else {
					var va = _memoryAllocator.GetFreeAddress(size, out var freeAddressStartPosition, pageSize);
					if(va != NvMemoryAllocator.PteUnmapped)
						_memoryAllocator.AllocateRange(va, size, freeAddressStartPosition);

					_asContext.Gmm.Map(physicalAddress, va, size);
					arguments.Offset = va;
				}

				if(arguments.Offset == NvMemoryAllocator.PteUnmapped) {
					arguments.Offset = 0;

					Log($"Failed to map size 0x{size:x16}!");

					result = NvInternalResult.InvalidInput;
				} else
					_asContext.AddMap(arguments.Offset, size, physicalAddress, virtualAddressAllocated);
			}

			return result;
		}

		NvInternalResult GetVaRegions(ref GetVaRegionsArguments arguments) {
			Log($"Stub GetVaRegions");

			return NvInternalResult.Success;
		}

		NvInternalResult InitializeEx(ref InitializeExArguments arguments) {
			Log($"Stub InitializeEx");

			return NvInternalResult.Success;
		}

		NvInternalResult Remap(Span<RemapArguments> arguments) {
			var gmm = _asContext.Gmm;

			for(var index = 0; index < arguments.Length; index++) {
				var mapOffs = (ulong) arguments[index].MapOffset << 16;
				var gpuVa = (ulong) arguments[index].GpuOffset << 16;
				var size = (ulong) arguments[index].Pages << 16;

				if(arguments[index].NvMapHandle == 0)
					gmm.Unmap(gpuVa, size);
				else {
					var map = NvMapDeviceFile.GetMapFromHandle(Owner, arguments[index].NvMapHandle);

					if(map == null) {
						Log($"Invalid NvMap handle 0x{arguments[index].NvMapHandle:x8}!");

						return NvInternalResult.InvalidInput;
					}

					gmm.Map(mapOffs + map.Address, gpuVa, size);
				}
			}

			return NvInternalResult.Success;
		}

		public override void Close() { }
	}
}