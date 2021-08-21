using System;
using System.Collections.Concurrent;
using Ryujinx.Common;
using Ryujinx.Common.Logging;
using Ryujinx.Graphics.Gpu.Memory;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap {
	class NvMapDeviceFile : NvDeviceFile {
		static readonly Logger Logger = new("NvMapDeviceFile");
		static Action<string> Log = Logger.Log;
		
		const int FlagNotFreedYet = 1;

		static readonly ConcurrentDictionary<long, IdDictionary> _maps = new();

		public NvMapDeviceFile(IVirtualMemoryManager memory, long owner) : base(owner) {
			var dict = _maps.GetOrAdd(Owner, key => new IdDictionary());

			dict.Add(0, new NvMapHandle());
		}

		public override NvInternalResult Ioctl(NvIoctl command, Span<byte> arguments) {
			var result = NvInternalResult.NotImplemented;

			if(command.Type == NvIoctl.NvMapCustomMagic)
				switch(command.Number) {
					case 0x01:
						result = CallIoctlMethod<NvMapCreate>(Create, arguments);
						break;
					case 0x03:
						result = CallIoctlMethod<NvMapFromId>(FromId, arguments);
						break;
					case 0x04:
						result = CallIoctlMethod<NvMapAlloc>(Alloc, arguments);
						break;
					case 0x05:
						result = CallIoctlMethod<NvMapFree>(Free, arguments);
						break;
					case 0x09:
						result = CallIoctlMethod<NvMapParam>(Param, arguments);
						break;
					case 0x0e:
						result = CallIoctlMethod<NvMapGetId>(GetId, arguments);
						break;
					case 0x02:
					case 0x06:
					case 0x07:
					case 0x08:
					case 0x0a:
					case 0x0c:
					case 0x0d:
					case 0x0f:
					case 0x10:
					case 0x11:
						result = NvInternalResult.NotSupported;
						break;
				}

			return result;
		}

		NvInternalResult Create(ref NvMapCreate arguments) {
			if(arguments.Size == 0) {
				Log($"Invalid size 0x{arguments.Size:x8}!");

				return NvInternalResult.InvalidInput;
			}

			var size = BitUtils.AlignUp(arguments.Size, (int) MemoryManager.PageSize);

			arguments.Handle = CreateHandleFromMap(new NvMapHandle(size));

			Log($"Created map {arguments.Handle} with size 0x{size:x8}!");

			return NvInternalResult.Success;
		}

		NvInternalResult FromId(ref NvMapFromId arguments) {
			var map = GetMapFromHandle(Owner, arguments.Id);

			if(map == null) {
				Log($"Invalid handle 0x{arguments.Handle:x8}!");

				return NvInternalResult.InvalidInput;
			}

			map.IncrementRefCount();

			arguments.Handle = arguments.Id;

			return NvInternalResult.Success;
		}

		NvInternalResult Alloc(ref NvMapAlloc arguments) {
			var map = GetMapFromHandle(Owner, arguments.Handle);

			if(map == null) {
				Log($"Invalid handle 0x{arguments.Handle:x8}!");

				return NvInternalResult.InvalidInput;
			}

			if((arguments.Align & (arguments.Align - 1)) != 0) {
				Log($"Invalid alignment 0x{arguments.Align:x8}!");

				return NvInternalResult.InvalidInput;
			}

			if((uint) arguments.Align < MemoryManager.PageSize) arguments.Align = (int) MemoryManager.PageSize;

			var result = NvInternalResult.Success;

			if(!map.Allocated) {
				map.Allocated = true;

				map.Align = arguments.Align;
				map.Kind = (byte) arguments.Kind;

				var size = BitUtils.AlignUp(map.Size, (int) MemoryManager.PageSize);

				var address = arguments.Address;

				if(address == 0)
					// When the address is zero, we need to allocate
					// our own backing memory for the NvMap.
					// TODO: Is this allocation inside the transfer memory?
					result = NvInternalResult.OutOfMemory;

				if(result == NvInternalResult.Success) {
					map.Size = size;
					map.Address = address;
				}
			}

			return result;
		}

		NvInternalResult Free(ref NvMapFree arguments) {
			var map = GetMapFromHandle(Owner, arguments.Handle);

			if(map == null) {
				Log($"Invalid handle 0x{arguments.Handle:x8}!");

				return NvInternalResult.InvalidInput;
			}

			if(DecrementMapRefCount(Owner, arguments.Handle)) {
				arguments.Address = map.Address;
				arguments.Flags = 0;
			} else {
				arguments.Address = 0;
				arguments.Flags = FlagNotFreedYet;
			}

			arguments.Size = map.Size;

			return NvInternalResult.Success;
		}

		NvInternalResult Param(ref NvMapParam arguments) {
			var map = GetMapFromHandle(Owner, arguments.Handle);

			if(map == null) {
				Log($"Invalid handle 0x{arguments.Handle:x8}!");

				return NvInternalResult.InvalidInput;
			}

			switch(arguments.Param) {
				case NvMapHandleParam.Size:
					arguments.Result = map.Size;
					break;
				case NvMapHandleParam.Align:
					arguments.Result = map.Align;
					break;
				case NvMapHandleParam.Heap:
					arguments.Result = 0x40000000;
					break;
				case NvMapHandleParam.Kind:
					arguments.Result = map.Kind;
					break;
				case NvMapHandleParam.Compr:
					arguments.Result = 0;
					break;

				// Note: Base is not supported and returns an error.
				// Any other value also returns an error.
				default: return NvInternalResult.InvalidInput;
			}

			return NvInternalResult.Success;
		}

		NvInternalResult GetId(ref NvMapGetId arguments) {
			var map = GetMapFromHandle(Owner, arguments.Handle);

			if(map == null) {
				Log($"Invalid handle 0x{arguments.Handle:x8}!");

				return NvInternalResult.InvalidInput;
			}

			arguments.Id = arguments.Handle;

			return NvInternalResult.Success;
		}

		public override void Close() {
			// TODO: refcount NvMapDeviceFile instances and remove when closing
			// _maps.TryRemove(GetOwner(), out _);
		}

		int CreateHandleFromMap(NvMapHandle map) {
			var dict = _maps.GetOrAdd(Owner, key => new IdDictionary());

			return dict.Add(map);
		}

		static bool DeleteMapWithHandle(long pid, int handle) {
			if(_maps.TryGetValue(pid, out var dict)) return dict.Delete(handle) != null;

			return false;
		}

		public static void IncrementMapRefCount(long pid, int handle) {
			GetMapFromHandle(pid, handle)?.IncrementRefCount();
		}

		public static bool DecrementMapRefCount(long pid, int handle) {
			var map = GetMapFromHandle(pid, handle);

			if(map == null) return false;

			if(map.DecrementRefCount() <= 0) {
				DeleteMapWithHandle(pid, handle);

				Log($"Deleted map {handle}!");

				return true;
			}

			return false;
		}

		public static NvMapHandle GetMapFromHandle(long pid, int handle) {
			if(_maps.TryGetValue(pid, out var dict)) return dict.GetData<NvMapHandle>(handle);

			return null;
		}
	}
}