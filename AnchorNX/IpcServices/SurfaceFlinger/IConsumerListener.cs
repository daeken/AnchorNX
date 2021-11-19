namespace AnchorNX.IpcServices.Nns.Hosbinder {
	interface IConsumerListener {
		void OnFrameAvailable(ref BufferItem item);
		void OnFrameReplaced(ref BufferItem item);
		void OnBuffersReleased();
	}
}