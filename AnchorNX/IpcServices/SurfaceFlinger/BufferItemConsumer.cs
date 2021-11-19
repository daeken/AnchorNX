﻿using System;
using Ryujinx.Graphics.Gpu;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	class BufferItemConsumer : ConsumerBase {
		readonly GpuContext _gpuContext;

		public BufferItemConsumer(BufferQueueConsumer consumer,
			uint consumerUsage,
			int bufferCount,
			bool controlledByApp,
			IConsumerListener listener = null
		) : base(consumer, controlledByApp, listener) {
			_gpuContext = Box.Gpu;

			var status = Consumer.SetConsumerUsageBits(consumerUsage);

			if(status != Status.Success) throw new InvalidOperationException();

			if(bufferCount != -1) {
				status = Consumer.SetMaxAcquiredBufferCount(bufferCount);

				if(status != Status.Success) throw new InvalidOperationException();
			}
		}

		public Status AcquireBuffer(out BufferItem bufferItem, ulong expectedPresent, bool waitForFence = false) {
			lock(Lock) {
				var status = AcquireBufferLocked(out var tmp, expectedPresent);

				if(status != Status.Success) {
					bufferItem = null;

					return status;
				}

				// Make sure to clone the object to not temper the real instance.
				bufferItem = (BufferItem) tmp.Clone();

				if(waitForFence) bufferItem.Fence.WaitForever(_gpuContext);

				bufferItem.GraphicBuffer.Set(Slots[bufferItem.Slot].GraphicBuffer);

				return Status.Success;
			}
		}

		public Status ReleaseBuffer(BufferItem bufferItem, ref AndroidFence fence) {
			lock(Lock) {
				var result = AddReleaseFenceLocked(bufferItem.Slot, ref bufferItem.GraphicBuffer, ref fence);

				if(result == Status.Success)
					result = ReleaseBufferLocked(bufferItem.Slot, ref bufferItem.GraphicBuffer);

				return result;
			}
		}

		public Status SetDefaultBufferSize(uint width, uint height) {
			lock(Lock) return Consumer.SetDefaultBufferSize(width, height);
		}

		public Status SetDefaultBufferFormat(PixelFormat defaultFormat) {
			lock(Lock) return Consumer.SetDefaultBufferFormat(defaultFormat);
		}
	}
}