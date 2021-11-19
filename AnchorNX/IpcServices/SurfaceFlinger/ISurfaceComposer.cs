using System;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	public abstract class ISurfaceComposer : IBinder {
		public string InterfaceToken => "android.ui.ISurfaceComposer";
		
		public uint AdjustRefcount(int addVal, int type) {
			// TODO?
			return 0;
		}

		public void GetNativeHandle(uint typeId, out uint readableEvent) {
			throw new System.NotImplementedException();
		}

		public void OnTransact(uint code, uint flags, Parcel inputParcel, Parcel outputParcel) {
			switch((TransactionCode) code) {
				case TransactionCode.CreateConnection:
					outputParcel.WriteObject(CreateConnection(), "abcdefgh");
					break;
				case TransactionCode.CaptureScreen: {
					var res = CaptureScreen(inputParcel.ReadInt32(), inputParcel.ReadInt32(), inputParcel.ReadInt32(),
						inputParcel.ReadInt32(), inputParcel.ReadInt32(), out var heap, out var width, out var height,
						out var pf);
					outputParcel.WriteObject(heap, "heapabcd");
					outputParcel.WriteInt32(width);
					outputParcel.WriteInt32(height);
					outputParcel.WriteUInt32(pf);
					break;
				}
				case TransactionCode.GetBuiltInDisplay:
					outputParcel.WriteObject(GetBuiltInDisplay(inputParcel.ReadInt32()), "gbdpefgh");
					break;
				case var x:
					throw new NotImplementedException($"Unimplemented transaction for ISurfaceComposer: {x}");
			}
		}
		
		enum TransactionCode : uint {
			BootFinished = 1,
			CreateConnection, 
			CreateGraphicBufferAlloc,
			CreateDisplayEventConnection, 
			CreateDisplay, 
			DestroyDisplay, 
			GetBuiltInDisplay, 
			SetTransactionState, 
			AuthenticateSurface, 
			Blank, 
			Unblank, 
			GetDisplayInfo, 
			ConnectDisplay, 
			CaptureScreen, 
		}

		public abstract ISurfaceFlingerClient CreateConnection();
		public abstract IBinder GetBuiltInDisplay(int displayId);
		public abstract Status CaptureScreen(int displayId, int reqWidth, int reqHeight, int minLayerZ, int maxLayerZ, out IMemoryHeap heap, out int width, out int height, out uint pixelFormat);
	}
}