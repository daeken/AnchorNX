namespace AnchorNX.IpcServices.Nns.Hosbinder {
	static class BufferQueue {
		public static BufferQueueCore CreateBufferQueue(long pid, out BufferQueueProducer producer,
			out BufferQueueConsumer consumer
		) {
			var core = new BufferQueueCore(pid);

			producer = new BufferQueueProducer(core);
			consumer = new BufferQueueConsumer(core);

			return core;
		}
	}
}