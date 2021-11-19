﻿using System;
using AnchorNX.IpcServices.Nns.Hosbinder.Types;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	class ConsumerBase : IConsumerListener {
		protected readonly object Lock = new();

		readonly IConsumerListener _listener;

		protected BufferQueueConsumer Consumer;

		protected bool IsAbandoned;

		protected Slot[] Slots = new Slot[BufferSlotArray.NumBufferSlots];

		public ConsumerBase(BufferQueueConsumer consumer, bool controlledByApp, IConsumerListener listener) {
			for(var i = 0; i < Slots.Length; i++) Slots[i] = new Slot();

			IsAbandoned = false;
			Consumer = consumer;
			_listener = listener;

			var connectStatus = consumer.Connect(this, controlledByApp);

			if(connectStatus != Status.Success) throw new InvalidOperationException();
		}

		public virtual void OnBuffersReleased() {
			lock(Lock) {
				if(IsAbandoned) return;

				Consumer.GetReleasedBuffers(out var slotMask);

				for(var i = 0; i < Slots.Length; i++)
					if((slotMask & (1UL << i)) != 0)
						FreeBufferLocked(i);
			}
		}

		public virtual void OnFrameAvailable(ref BufferItem item) {
			_listener?.OnFrameAvailable(ref item);
		}

		public virtual void OnFrameReplaced(ref BufferItem item) {
			_listener?.OnFrameReplaced(ref item);
		}

		protected virtual void FreeBufferLocked(int slotIndex) {
			Slots[slotIndex].GraphicBuffer.Reset();

			Slots[slotIndex].Fence = AndroidFence.NoFence;
			Slots[slotIndex].FrameNumber = 0;
		}

		public void Abandon() {
			lock(Lock)
				if(!IsAbandoned) {
					AbandonLocked();

					IsAbandoned = true;
				}
		}

		protected virtual void AbandonLocked() {
			for(var i = 0; i < Slots.Length; i++) FreeBufferLocked(i);

			Consumer.Disconnect();
		}

		protected virtual Status AcquireBufferLocked(out BufferItem bufferItem, ulong expectedPresent) {
			var acquireStatus = Consumer.AcquireBuffer(out bufferItem, expectedPresent);

			if(acquireStatus != Status.Success) return acquireStatus;

			if(!bufferItem.GraphicBuffer.IsNull)
				Slots[bufferItem.Slot].GraphicBuffer.Set(bufferItem.GraphicBuffer.Object);

			Slots[bufferItem.Slot].FrameNumber = bufferItem.FrameNumber;
			Slots[bufferItem.Slot].Fence = bufferItem.Fence;

			return Status.Success;
		}

		protected virtual Status AddReleaseFenceLocked(int slot, ref AndroidStrongPointer<GraphicBuffer> graphicBuffer,
			ref AndroidFence fence
		) {
			if(!StillTracking(slot, ref graphicBuffer)) return Status.Success;

			Slots[slot].Fence = fence;

			return Status.Success;
		}

		protected virtual Status ReleaseBufferLocked(int slot, ref AndroidStrongPointer<GraphicBuffer> graphicBuffer) {
			if(!StillTracking(slot, ref graphicBuffer)) return Status.Success;

			var result = Consumer.ReleaseBuffer(slot, Slots[slot].FrameNumber, ref Slots[slot].Fence);

			if(result == Status.StaleBufferSlot) FreeBufferLocked(slot);

			Slots[slot].Fence = AndroidFence.NoFence;

			return result;
		}

		protected virtual bool StillTracking(int slotIndex, ref AndroidStrongPointer<GraphicBuffer> graphicBuffer) {
			if(slotIndex < 0 || slotIndex >= Slots.Length) return false;

			var slot = Slots[slotIndex];

			// TODO: Check this. On Android, this checks the "handle". I assume NvMapHandle is the handle, but it might not be. 
			return !slot.GraphicBuffer.IsNull && slot.GraphicBuffer.Object.Buffer.Surfaces[0].NvMapHandle ==
				graphicBuffer.Object.Buffer.Surfaces[0].NvMapHandle;
		}

		public class Slot {
			public AndroidFence Fence;
			public ulong FrameNumber;
			public AndroidStrongPointer<GraphicBuffer> GraphicBuffer;

			public Slot() => GraphicBuffer = new AndroidStrongPointer<GraphicBuffer>();
		}
	}
}