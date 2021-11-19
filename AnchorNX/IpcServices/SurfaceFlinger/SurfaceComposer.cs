using System;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	public class SurfaceComposer : ISurfaceComposer {
		static readonly Logger Logger = new("SurfaceComposer");
		static Action<string> Log = Logger.Log;

		public override ISurfaceFlingerClient CreateConnection() => new SurfaceFlingerClient();
		public override IBinder GetBuiltInDisplay(int displayId) {
			Log($"GetBuiltInDisplay? {displayId}");
			return new SurfaceComposer();
		}

		public override Status CaptureScreen(int displayId, int reqWidth, int reqHeight, int minLayerZ, int maxLayerZ, out IMemoryHeap heap,
			out int width, out int height, out uint pixelFormat
		) {
			Log($"CaptureScreen? {displayId} {reqWidth} {reqHeight} {minLayerZ} {maxLayerZ}");
			throw new System.NotImplementedException();
		}
	}
}