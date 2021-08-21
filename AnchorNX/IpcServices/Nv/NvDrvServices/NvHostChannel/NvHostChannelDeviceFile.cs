using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel.Types;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrl;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap;
using AnchorNX.IpcServices.Nns.Nvdrv.Types;
using Ryujinx.Common.Logging;
using Ryujinx.Graphics.Gpu;
using Ryujinx.Graphics.Gpu.Memory;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel {
	class NvHostChannelDeviceFile : NvDeviceFile {
		static readonly Logger Logger = new("NvHostChannelDeviceFile");
		static Action<string> Log = Logger.Log;
		
		public enum ResourcePolicy {
			Device,
			Channel
		}

		const uint MaxModuleSyncpoint = 16;
		static readonly ConcurrentDictionary<long, Host1xContext> _host1xContextRegistry = new();

		protected static uint[] DeviceSyncpoints = new uint[MaxModuleSyncpoint];

		protected static ResourcePolicy ChannelResourcePolicy = ResourcePolicy.Device;

		readonly Host1xContext _host1xContext;

		readonly IVirtualMemoryManager _memory;

		NvFence _channelSyncpoint;
		uint _submitTimeout;

		uint _timeout;
		uint _timeslice;

		protected uint[] ChannelSyncpoints;

		public NvHostChannelDeviceFile(IVirtualMemoryManager memory, long owner) : base(owner) {
			_memory = memory;
			_timeout = 3000;
			_submitTimeout = 0;
			_timeslice = 0;
			_host1xContext = GetHost1XContext(Box.Gpu, owner);
			Channel = Box.Gpu.CreateChannel();

			ChannelInitialization.InitializeState(Channel);

			ChannelSyncpoints = new uint[MaxModuleSyncpoint];

			// TODO!
			//_channelSyncpoint.Id = _device.System.HostSyncpoint.AllocateSyncpoint(false);
			//_channelSyncpoint.UpdateValue(_device.System.HostSyncpoint);
		}

		public GpuChannel Channel { get; }

		public override NvInternalResult Ioctl(NvIoctl command, Span<byte> arguments) {
			var result = NvInternalResult.NotImplemented;

			if(command.Type == NvIoctl.NvHostCustomMagic)
				switch(command.Number) {
					case 0x01:
						result = Submit(arguments);
						break;
					case 0x02:
						result = CallIoctlMethod<GetParameterArguments>(GetSyncpoint, arguments);
						break;
					case 0x03:
						result = CallIoctlMethod<GetParameterArguments>(GetWaitBase, arguments);
						break;
					case 0x07:
						result = CallIoctlMethod<uint>(SetSubmitTimeout, arguments);
						break;
					case 0x09:
						result = MapCommandBuffer(arguments);
						break;
					case 0x0a:
						result = UnmapCommandBuffer(arguments);
						break;
				}
			else if(command.Type == NvIoctl.NvHostMagic)
				switch(command.Number) {
					case 0x01:
						result = CallIoctlMethod<int>(SetNvMapFd, arguments);
						break;
					case 0x03:
						result = CallIoctlMethod<uint>(SetTimeout, arguments);
						break;
					case 0x08:
						result = SubmitGpfifo(arguments);
						break;
					case 0x09:
						result = CallIoctlMethod<AllocObjCtxArguments>(AllocObjCtx, arguments);
						break;
					case 0x0b:
						result = CallIoctlMethod<ZcullBindArguments>(ZcullBind, arguments);
						break;
					case 0x0c:
						result = CallIoctlMethod<SetErrorNotifierArguments>(SetErrorNotifier, arguments);
						break;
					case 0x0d:
						result = CallIoctlMethod<NvChannelPriority>(SetPriority, arguments);
						break;
					case 0x18:
						result = CallIoctlMethod<AllocGpfifoExArguments>(AllocGpfifoEx, arguments);
						break;
					case 0x1a:
						result = CallIoctlMethod<AllocGpfifoExArguments>(AllocGpfifoEx2, arguments);
						break;
					case 0x1d:
						result = CallIoctlMethod<uint>(SetTimeslice, arguments);
						break;
				}
			else if(command.Type == NvIoctl.NvGpuMagic)
				switch(command.Number) {
					case 0x14:
						result = CallIoctlMethod<ulong>(SetUserData, arguments);
						break;
				}

			return result;
		}

		NvInternalResult Submit(Span<byte> arguments) {
			var submitHeader = GetSpanAndSkip<SubmitArguments>(ref arguments, 1)[0];
			var commandBuffers = GetSpanAndSkip<CommandBuffer>(ref arguments, submitHeader.CmdBufsCount);
			var relocs = GetSpanAndSkip<Reloc>(ref arguments, submitHeader.RelocsCount);
			var relocShifts = GetSpanAndSkip<uint>(ref arguments, submitHeader.RelocsCount);
			var syncptIncrs = GetSpanAndSkip<SyncptIncr>(ref arguments, submitHeader.SyncptIncrsCount);
			var waitChecks = GetSpanAndSkip<SyncptIncr>(ref arguments, submitHeader.SyncptIncrsCount); // ?
			var fences = GetSpanAndSkip<Fence>(ref arguments, submitHeader.FencesCount);

			lock(Box.Gpu) {
				for(var i = 0; i < syncptIncrs.Length; i++) {
					var syncptIncr = syncptIncrs[i];

					var id = syncptIncr.Id;

					fences[i].Id = id;
					// TODO!
					//fences[i].Thresh = Context.Device.System.HostSyncpoint.IncrementSyncpointMax(id, syncptIncr.Incrs);
				}

				foreach(var commandBuffer in commandBuffers) {
					var map = NvMapDeviceFile.GetMapFromHandle(Owner, commandBuffer.Mem);

					var data = _memory.GetSpan(map.Address + commandBuffer.Offset, commandBuffer.WordsCount * 4);

					_host1xContext.Host1x.Submit(MemoryMarshal.Cast<byte, int>(data));
				}
			}

			// TODO!
			//fences[0].Thresh = Context.Device.System.HostSyncpoint.IncrementSyncpointMax(fences[0].Id, 1);

			Span<int> tmpCmdBuff = stackalloc int[1];

			tmpCmdBuff[0] = (4 << 28) | (int) fences[0].Id;

			_host1xContext.Host1x.Submit(tmpCmdBuff);

			return NvInternalResult.Success;
		}

		Span<T> GetSpanAndSkip<T>(ref Span<byte> arguments, int count) where T : unmanaged {
			var output = MemoryMarshal.Cast<byte, T>(arguments).Slice(0, count);

			arguments = arguments.Slice(Unsafe.SizeOf<T>() * count);

			return output;
		}

		NvInternalResult GetSyncpoint(ref GetParameterArguments arguments) {
			if(arguments.Parameter >= MaxModuleSyncpoint) return NvInternalResult.InvalidInput;

			if(ChannelResourcePolicy == ResourcePolicy.Device)
				throw new NotImplementedException(); //arguments.Value = GetSyncpointDevice(_device.System.HostSyncpoint, arguments.Parameter, false);
			else
				arguments.Value = GetSyncpointChannel(arguments.Parameter, false);

			if(arguments.Value == 0) return NvInternalResult.TryAgain;

			return NvInternalResult.Success;
		}

		NvInternalResult GetWaitBase(ref GetParameterArguments arguments) {
			arguments.Value = 0;

			Log("Stub GetWaitBase");

			return NvInternalResult.Success;
		}

		NvInternalResult SetSubmitTimeout(ref uint submitTimeout) {
			_submitTimeout = submitTimeout;

			Log("Stub SetSubmitTimeout");

			return NvInternalResult.Success;
		}

		NvInternalResult MapCommandBuffer(Span<byte> arguments) {
			var headerSize = Unsafe.SizeOf<MapCommandBufferArguments>();
			var commandBufferHeader = MemoryMarshal.Cast<byte, MapCommandBufferArguments>(arguments)[0];
			var commandBufferEntries = MemoryMarshal.Cast<byte, CommandBufferHandle>(arguments.Slice(headerSize))
				.Slice(0, commandBufferHeader.NumEntries);

			foreach(ref var commandBufferEntry in commandBufferEntries) {
				var map = NvMapDeviceFile.GetMapFromHandle(Owner, commandBufferEntry.MapHandle);

				if(map == null) {
					Log($"Invalid handle 0x{commandBufferEntry.MapHandle:x8}!");

					return NvInternalResult.InvalidInput;
				}

				lock(map) {
					if(map.DmaMapAddress == 0) {
						var va = _host1xContext.MemoryAllocator.GetFreeAddress((ulong) map.Size,
							out var freeAddressStartPosition, 1, MemoryManager.PageSize);

						if(va != NvMemoryAllocator.PteUnmapped && va <= uint.MaxValue &&
						   va + (uint) map.Size <= uint.MaxValue) {
							_host1xContext.MemoryAllocator.AllocateRange(va, (uint) map.Size, freeAddressStartPosition);
							_host1xContext.Smmu.Map(map.Address, va, (uint) map.Size);
							map.DmaMapAddress = va;
						} else
							map.DmaMapAddress = NvMemoryAllocator.PteUnmapped;
					}

					commandBufferEntry.MapAddress = (int) map.DmaMapAddress;
				}
			}

			return NvInternalResult.Success;
		}

		NvInternalResult UnmapCommandBuffer(Span<byte> arguments) {
			var headerSize = Unsafe.SizeOf<MapCommandBufferArguments>();
			var commandBufferHeader = MemoryMarshal.Cast<byte, MapCommandBufferArguments>(arguments)[0];
			var commandBufferEntries = MemoryMarshal.Cast<byte, CommandBufferHandle>(arguments.Slice(headerSize))
				.Slice(0, commandBufferHeader.NumEntries);

			foreach(ref var commandBufferEntry in commandBufferEntries) {
				var map = NvMapDeviceFile.GetMapFromHandle(Owner, commandBufferEntry.MapHandle);

				if(map == null) {
					Log($"Invalid handle 0x{commandBufferEntry.MapHandle:x8}!");

					return NvInternalResult.InvalidInput;
				}

				lock(map)
					if(map.DmaMapAddress != 0) {
						// FIXME:
						// To make unmapping work, we need separate address space per channel.
						// Right now NVDEC and VIC share the GPU address space which is not correct at all.

						// _host1xContext.MemoryAllocator.Free((ulong)map.DmaMapAddress, (uint)map.Size);

						// map.DmaMapAddress = 0;
					}
			}

			return NvInternalResult.Success;
		}

		NvInternalResult SetNvMapFd(ref int nvMapFd) {
			Log("Stub SetNvMapFd");

			return NvInternalResult.Success;
		}

		NvInternalResult SetTimeout(ref uint timeout) {
			_timeout = timeout;

			Log("Stub SetTimeout");

			return NvInternalResult.Success;
		}

		NvInternalResult SubmitGpfifo(Span<byte> arguments) {
			var headerSize = Unsafe.SizeOf<SubmitGpfifoArguments>();
			var gpfifoSubmissionHeader = MemoryMarshal.Cast<byte, SubmitGpfifoArguments>(arguments)[0];
			var gpfifoEntries = MemoryMarshal.Cast<byte, ulong>(arguments.Slice(headerSize))
				.Slice(0, gpfifoSubmissionHeader.NumEntries);

			return SubmitGpfifo(ref gpfifoSubmissionHeader, gpfifoEntries);
		}

		NvInternalResult AllocObjCtx(ref AllocObjCtxArguments arguments) {
			Log("Stub AllocObjCtx");

			return NvInternalResult.Success;
		}

		NvInternalResult ZcullBind(ref ZcullBindArguments arguments) {
			Log("Stub ZcullBind");

			return NvInternalResult.Success;
		}

		NvInternalResult SetErrorNotifier(ref SetErrorNotifierArguments arguments) {
			Log("Stub SetErrorNotifier");

			return NvInternalResult.Success;
		}

		NvInternalResult SetPriority(ref NvChannelPriority priority) {
			switch(priority) {
				case NvChannelPriority.Low:
					_timeslice = 1300; // Timeslice low priority in micro-seconds
					break;
				case NvChannelPriority.Medium:
					_timeslice = 2600; // Timeslice medium priority in micro-seconds
					break;
				case NvChannelPriority.High:
					_timeslice = 5200; // Timeslice high priority in micro-seconds
					break;
				default:
					return NvInternalResult.InvalidInput;
			}

			Log("Stub SetPriority");

			// TODO: disable and preempt channel when GPU scheduler will be implemented.

			return NvInternalResult.Success;
		}

		NvInternalResult AllocGpfifoEx(ref AllocGpfifoExArguments arguments) {
			// TODO! //_channelSyncpoint.UpdateValue(_device.System.HostSyncpoint);

			arguments.Fence = _channelSyncpoint;

			Log("Stub AllocGpfifoEx");

			return NvInternalResult.Success;
		}

		NvInternalResult AllocGpfifoEx2(ref AllocGpfifoExArguments arguments) {
			// TODO! //_channelSyncpoint.UpdateValue(_device.System.HostSyncpoint);

			arguments.Fence = _channelSyncpoint;

			Log("Stub AllocGpfifoEx2");

			return NvInternalResult.Success;
		}

		NvInternalResult SetTimeslice(ref uint timeslice) {
			if(timeslice < 1000 || timeslice > 50000) return NvInternalResult.InvalidInput;

			_timeslice = timeslice; // in micro-seconds

			Log("Stub SetTimeslice");

			// TODO: disable and preempt channel when GPU scheduler will be implemented.

			return NvInternalResult.Success;
		}

		NvInternalResult SetUserData(ref ulong userData) {
			Log("Stub SetUserData");

			return NvInternalResult.Success;
		}

		protected NvInternalResult SubmitGpfifo(ref SubmitGpfifoArguments header, Span<ulong> entries) {
			if(header.Flags.HasFlag(SubmitGpfifoFlags.FenceWait) &&
			   header.Flags.HasFlag(SubmitGpfifoFlags.IncrementWithValue)) return NvInternalResult.InvalidInput;

			// TODO!
			/*if(header.Flags.HasFlag(SubmitGpfifoFlags.FenceWait) &&
			   !_device.System.HostSyncpoint.IsSyncpointExpired(header.Fence.Id, header.Fence.Value))
				Channel.PushHostCommandBuffer(CreateWaitCommandBuffer(header.Fence));*/

			Channel.PushEntries(entries);

			header.Fence.Id = _channelSyncpoint.Id;

			if(header.Flags.HasFlag(SubmitGpfifoFlags.FenceIncrement) ||
			   header.Flags.HasFlag(SubmitGpfifoFlags.IncrementWithValue)) {
				var incrementCount = header.Flags.HasFlag(SubmitGpfifoFlags.FenceIncrement) ? 2u : 0u;

				if(header.Flags.HasFlag(SubmitGpfifoFlags.IncrementWithValue)) incrementCount += header.Fence.Value;

				// TODO!
				//header.Fence.Value =
				//	_device.System.HostSyncpoint.IncrementSyncpointMaxExt(header.Fence.Id, (int) incrementCount);
			} else
				throw new NotImplementedException();//header.Fence.Value = _device.System.HostSyncpoint.ReadSyncpointMaxValue(header.Fence.Id);

			if(header.Flags.HasFlag(SubmitGpfifoFlags.FenceIncrement))
				Channel.PushHostCommandBuffer(CreateIncrementCommandBuffer(ref header.Fence, header.Flags));

			header.Flags = SubmitGpfifoFlags.None;

			Box.Gpu.GPFifo.SignalNewEntries();

			return NvInternalResult.Success;
		}

		public uint GetSyncpointChannel(uint index, bool isClientManaged) {
			if(ChannelSyncpoints[index] != 0) return ChannelSyncpoints[index];

			// TODO!
			//ChannelSyncpoints[index] = _device.System.HostSyncpoint.AllocateSyncpoint(isClientManaged);

			return ChannelSyncpoints[index];
		}

		public static uint GetSyncpointDevice(NvHostSyncpt syncpointManager, uint index, bool isClientManaged) {
			if(DeviceSyncpoints[index] != 0) return DeviceSyncpoints[index];

			DeviceSyncpoints[index] = syncpointManager.AllocateSyncpoint(isClientManaged);

			return DeviceSyncpoints[index];
		}

		static int[] CreateWaitCommandBuffer(NvFence fence) {
			var commandBuffer = new int[4];

			// SyncpointValue = fence.Value;
			commandBuffer[0] = 0x2001001C;
			commandBuffer[1] = (int) fence.Value;

			// SyncpointAction(fence.id, increment: false, switch_en: true);
			commandBuffer[2] = 0x2001001D;
			commandBuffer[3] = ((int) fence.Id << 8) | (0 << 0) | (1 << 4);

			return commandBuffer;
		}

		int[] CreateIncrementCommandBuffer(ref NvFence fence, SubmitGpfifoFlags flags) {
			var hasWfi = !flags.HasFlag(SubmitGpfifoFlags.SuppressWfi);

			int[] commandBuffer;

			var offset = 0;

			if(hasWfi) {
				commandBuffer = new int[8];

				// WaitForInterrupt(handle)
				commandBuffer[offset++] = 0x2001001E;
				commandBuffer[offset++] = 0x0;
			} else
				commandBuffer = new int[6];

			// SyncpointValue = 0x0;
			commandBuffer[offset++] = 0x2001001C;
			commandBuffer[offset++] = 0x0;

			// Increment the syncpoint 2 times. (mitigate a hardware bug)

			// SyncpointAction(fence.id, increment: true, switch_en: false);
			commandBuffer[offset++] = 0x2001001D;
			commandBuffer[offset++] = ((int) fence.Id << 8) | (1 << 0) | (0 << 4);

			// SyncpointAction(fence.id, increment: true, switch_en: false);
			commandBuffer[offset++] = 0x2001001D;
			commandBuffer[offset++] = ((int) fence.Id << 8) | (1 << 0) | (0 << 4);

			return commandBuffer;
		}

		public override void Close() {
			Channel.Dispose();
		}

		static Host1xContext GetHost1XContext(GpuContext gpu, long pid) {
			return _host1xContextRegistry.GetOrAdd(pid, key => new Host1xContext(gpu, key));
		}

		public static void Destroy() {
			foreach(var host1xContext in _host1xContextRegistry.Values) host1xContext.Dispose();

			_host1xContextRegistry.Clear();
		}
	}
}