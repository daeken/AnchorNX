namespace AnchorNX.IpcServices.Nns.Hosbinder {
	public abstract class ISurfaceFlingerClient : IBinder {
		public string InterfaceToken => "android.ui.ISurfaceFlingerClient";
		
		public uint AdjustRefcount(int addVal, int type) {
			throw new System.NotImplementedException();
		}

		public void GetNativeHandle(uint typeId, out uint readableEvent) {
			throw new System.NotImplementedException();
		}

		public void OnTransact(uint code, uint flags, Parcel inputParcel, Parcel outputParcel) {
			throw new System.NotImplementedException();
		}
	}
}