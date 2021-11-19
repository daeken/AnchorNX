using System;
using System.Collections.Generic;
using System.Threading;
using AnchorNX.IpcServices.Nns.Hosbinder.Types;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	class BufferQueueCore {
		public const int BufferHistoryArraySize = 8;
		static readonly Logger Logger = new("BufferQueueCore");
		static readonly Action<string> Log = Logger.Log;

		public readonly object Lock = new();
		readonly HosEvent _frameAvailableEvent;

		readonly HosEvent _waitBufferFreeEvent;
		public bool BufferHasBeenQueued;
		public BufferInfo[] BufferHistory;
		public uint BufferHistoryPosition;
		public NativeWindowApi ConnectedApi;
		public bool ConsumerControlledByApp;
		public IConsumerListener ConsumerListener;
		public uint ConsumerUsageBits;
		public PixelFormat DefaultBufferFormat;
		public int DefaultHeight;
		public int DefaultMaxBufferCount;
		public int DefaultWidth;
		public bool DequeueBufferCannotBlock;
		public bool EnableExternalEvent;
		public ulong FrameCounter;
		public bool IsAbandoned;
		public bool IsAllocating;
		public int MaxAcquiredBufferCount;
		public int MaxBufferCountCached;
		public int OverrideMaxBufferCount;
		public IProducerListener ProducerListener;
		public List<BufferItem> Queue;

		public BufferSlotArray Slots;
		public NativeWindowTransform TransformHint;
		public bool UseAsyncBuffer;

		public BufferQueueCore(long pid) {
			Slots = new BufferSlotArray();
			IsAbandoned = false;
			OverrideMaxBufferCount = 0;
			DequeueBufferCannotBlock = false;
			UseAsyncBuffer = false;
			DefaultWidth = 1;
			DefaultHeight = 1;
			DefaultMaxBufferCount = 2;
			MaxAcquiredBufferCount = 1;
			FrameCounter = 0;
			TransformHint = 0;
			DefaultBufferFormat = PixelFormat.Rgba8888;
			IsAllocating = false;
			ProducerListener = null;
			ConsumerListener = null;
			ConsumerUsageBits = 0;

			Queue = new List<BufferItem>();

			// TODO: CreateGraphicBufferAlloc?

			_waitBufferFreeEvent = Box.EventManager.GetEvent();
			_frameAvailableEvent = Box.EventManager.GetEvent();

			Owner = pid;

			Active = true;

			BufferHistory = new BufferInfo[BufferHistoryArraySize];
			EnableExternalEvent = true;
			MaxBufferCountCached = 0;
		}

		public long Owner { get; }

		public bool Active { get; private set; }

		public event Action BufferQueued;

		public int GetMinUndequeuedBufferCountLocked(bool async) {
			if(!UseAsyncBuffer) return 0;

			if(DequeueBufferCannotBlock || async) return MaxAcquiredBufferCount + 1;

			return MaxAcquiredBufferCount;
		}

		public int GetMinMaxBufferCountLocked(bool async) {
			return GetMinUndequeuedBufferCountLocked(async);
		}

		public void UpdateMaxBufferCountCachedLocked(int slot) {
			if(MaxBufferCountCached <= slot) MaxBufferCountCached = slot + 1;
		}

		public int GetMaxBufferCountLocked(bool async) {
			var minMaxBufferCount = GetMinMaxBufferCountLocked(async);

			var maxBufferCount = Math.Max(DefaultMaxBufferCount, minMaxBufferCount);

			if(OverrideMaxBufferCount != 0) return OverrideMaxBufferCount;

			// Preserve all buffers already in control of the producer and the consumer.
			for(var slot = maxBufferCount; slot < Slots.Length; slot++) {
				var state = Slots[slot].BufferState;

				if(state == BufferState.Queued || state == BufferState.Dequeued) maxBufferCount = slot + 1;
			}

			return maxBufferCount;
		}

		public Status SetDefaultMaxBufferCountLocked(int count) {
			var minBufferCount = UseAsyncBuffer ? 2 : 1;

			if(count < minBufferCount || count > Slots.Length) return Status.BadValue;

			DefaultMaxBufferCount = count;

			SignalDequeueEvent();

			return Status.Success;
		}

		public void SignalWaitBufferFreeEvent() {
			if(EnableExternalEvent) _waitBufferFreeEvent.Signal();
		}

		public void SignalFrameAvailableEvent() {
			if(EnableExternalEvent) _frameAvailableEvent.Signal();
		}

		public void PrepareForExit() {
			lock(Lock) {
				Active = false;

				Monitor.PulseAll(Lock);
			}
		}

		// TODO: Find an accurate way to handle a regular condvar here as this will wake up unwanted threads in some edge cases.
		public void SignalDequeueEvent() {
			Monitor.PulseAll(Lock);
		}

		public void WaitDequeueEvent() {
			WaitForLock();
		}

		public void SignalIsAllocatingEvent() {
			Monitor.PulseAll(Lock);
		}

		public void WaitIsAllocatingEvent() {
			WaitForLock();
		}

		public void SignalQueueEvent() {
			BufferQueued?.Invoke();
		}

		void WaitForLock() {
			if(Active) Monitor.Wait(Lock);
		}

		public void FreeBufferLocked(int slot) {
			Slots[slot].GraphicBuffer.Reset();

			if(Slots[slot].BufferState == BufferState.Acquired) Slots[slot].NeedsCleanupOnRelease = true;

			Slots[slot].BufferState = BufferState.Free;
			Slots[slot].FrameNumber = uint.MaxValue;
			Slots[slot].AcquireCalled = false;
			Slots[slot].Fence.FenceCount = 0;
		}

		public void FreeAllBuffersLocked() {
			BufferHasBeenQueued = false;

			for(var slot = 0; slot < Slots.Length; slot++) FreeBufferLocked(slot);
		}

		public bool StillTracking(ref BufferItem item) {
			var slot = Slots[item.Slot];

			// TODO: Check this. On Android, this checks the "handle". I assume NvMapHandle is the handle, but it might not be. 
			return !slot.GraphicBuffer.IsNull && slot.GraphicBuffer.Object.Buffer.Surfaces[0].NvMapHandle ==
				item.GraphicBuffer.Object.Buffer.Surfaces[0].NvMapHandle;
		}

		public void WaitWhileAllocatingLocked() {
			while(IsAllocating) WaitIsAllocatingEvent();
		}

		public void CheckSystemEventsLocked(int maxBufferCount) {
			if(!EnableExternalEvent) return;

			var needBufferReleaseSignal = false;
			var needFrameAvailableSignal = false;

			if(maxBufferCount > 1)
				for(var i = 0; i < maxBufferCount; i++)
					if(Slots[i].BufferState == BufferState.Queued)
						needFrameAvailableSignal = true;
					else if(Slots[i].BufferState == BufferState.Free) needBufferReleaseSignal = true;

			if(needBufferReleaseSignal)
				SignalWaitBufferFreeEvent();
			else
				_waitBufferFreeEvent.Clear();

			if(needFrameAvailableSignal)
				SignalFrameAvailableEvent();
			else
				_frameAvailableEvent.Clear();
		}

		public bool IsProducerConnectedLocked() {
			return ConnectedApi != NativeWindowApi.NoApi;
		}

		public bool IsConsumerConnectedLocked() {
			return ConsumerListener != null;
		}

		public uint GetWaitBufferFreeEvent() {
			lock(Lock) return _waitBufferFreeEvent.Reader;
		}

		public bool IsOwnedByConsumerLocked(int slot) {
			if(Slots[slot].BufferState != BufferState.Acquired) {
				Log($"Slot {slot} is not owned by the consumer (state = {Slots[slot].BufferState})");

				return false;
			}

			return true;
		}

		public bool IsOwnedByProducerLocked(int slot) {
			if(Slots[slot].BufferState != BufferState.Dequeued) {
				Log($"Slot {slot} is not owned by the producer (state = {Slots[slot].BufferState})");

				return false;
			}

			return true;
		}
	}
}