namespace AnchorNX.IpcServices.Nns.Hosbinder {
	public class IMemoryHeap : IBinder {
		public string InterfaceToken { get; }
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