using System;
using AnchorNX.IpcServices.Nns.Hosbinder.Types;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	class BufferItem : ICloneable {
		public bool AcquireCalled;
		public Rect Crop;
		public AndroidFence Fence;
		public ulong FrameNumber;
		public AndroidStrongPointer<GraphicBuffer> GraphicBuffer;
		public bool IsAutoTimestamp;
		public bool IsDroppable;
		public NativeWindowScalingMode ScalingMode;
		public int Slot;
		public int SwapInterval;
		public long Timestamp;
		public NativeWindowTransform Transform;
		public bool TransformToDisplayInverse;

		public BufferItem() {
			GraphicBuffer = new AndroidStrongPointer<GraphicBuffer>();
			Transform = NativeWindowTransform.None;
			ScalingMode = NativeWindowScalingMode.Freeze;
			Timestamp = 0;
			IsAutoTimestamp = false;
			FrameNumber = 0;
			Slot = BufferSlotArray.InvalidBufferSlot;
			IsDroppable = false;
			AcquireCalled = false;
			TransformToDisplayInverse = false;
			SwapInterval = 1;
			Fence = AndroidFence.NoFence;

			Crop = new Rect();
			Crop.MakeInvalid();
		}

		public object Clone() {
			var item = new BufferItem();

			item.Transform = Transform;
			item.ScalingMode = ScalingMode;
			item.IsAutoTimestamp = IsAutoTimestamp;
			item.FrameNumber = FrameNumber;
			item.Slot = Slot;
			item.IsDroppable = IsDroppable;
			item.AcquireCalled = AcquireCalled;
			item.TransformToDisplayInverse = TransformToDisplayInverse;
			item.SwapInterval = SwapInterval;
			item.Fence = Fence;
			item.Crop = Crop;

			item.GraphicBuffer.Set(GraphicBuffer);

			return item;
		}
	}
}