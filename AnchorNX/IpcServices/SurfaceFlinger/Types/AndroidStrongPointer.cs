namespace AnchorNX.IpcServices.Nns.Hosbinder.Types {
	public class AndroidStrongPointer<T> where T : unmanaged, IFlattenable {
		bool _hasObject;
		public T Object;

		public AndroidStrongPointer() => _hasObject = false;

		public AndroidStrongPointer(T obj) {
			Set(obj);
		}

		public bool IsNull => !_hasObject;

		public void Set(AndroidStrongPointer<T> other) {
			Object = other.Object;
			_hasObject = other._hasObject;
		}

		public void Set(T obj) {
			Object = obj;
			_hasObject = true;
		}

		public void Reset() {
			_hasObject = false;
		}
	}
}