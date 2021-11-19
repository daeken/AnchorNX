using AnchorNX.IpcServices.Nns.Hosbinder.Types;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	class BufferSlot {
		public bool AcquireCalled;
		public bool AttachedByConsumer;
		public BufferState BufferState;
		public AndroidFence Fence;
		public ulong FrameNumber;
		public AndroidStrongPointer<GraphicBuffer> GraphicBuffer;
		public bool IsPreallocated;
		public bool NeedsCleanupOnRelease;
		public TimeSpanType PresentationTime;
		public TimeSpanType QueueTime;
		public bool RequestBufferCalled;

		public BufferSlot() {
			GraphicBuffer = new AndroidStrongPointer<GraphicBuffer>();
			BufferState = BufferState.Free;
			QueueTime = TimeSpanType.Zero;
			PresentationTime = TimeSpanType.Zero;
			IsPreallocated = false;
		}
	}
}