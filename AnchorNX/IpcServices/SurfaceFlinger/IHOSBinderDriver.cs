using System;
using System.Collections.Generic;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	public partial class IHOSBinderDriver {
		static readonly Logger Logger = new("IHOSBinderDriver");
		static Action<string> Log = Logger.Log;

		static readonly Dictionary<int, IBinder> _registeredBinderObjects = new();
		static int _lastBinderId;
		static readonly object _lock = new();

		public static void Initialize() {
			RegisterBinderObject(new SurfaceComposer());
		}

		public override void TransactParcel(int id, uint code, Buffer<byte> parcel_data, uint flags,
			Buffer<byte> parcel_reply
		) {
			var reply = new Span<byte>(new byte[parcel_reply.Length]);
			var rc = OnTransact(id, code, flags, parcel_data.SafeSpan, reply);
			if(rc != 0)
				throw new IpcException(rc);
			parcel_reply.CopyFrom(reply);
		}

		public override uint GetNativeHandle(int id, uint _1) {
			GetNativeHandle(id, _1, out var nh);
			return nh;
		}

		public override void TransactParcelAuto(int id, uint code, Buffer<byte> parcel_data, uint flags,
			Buffer<byte> parcel_reply
		) {
			var reply = new Span<byte>(new byte[parcel_reply?.Length ?? 0]);
			Log($"TransactParsalAuto reply size 0x{parcel_reply?.Length ?? 0:X}");
			var rc = OnTransact(id, code, flags, parcel_data.SafeSpan, reply);
			if(rc != 0) {
				Log($"Error for TransactParcelAuto? 0x{rc:X}");
				throw new IpcException(rc);
			}
			parcel_reply?.CopyFrom(reply);
		}

		public static int RegisterBinderObject(IBinder binder) {
			lock(_lock) {
				_registeredBinderObjects.Add(_lastBinderId, binder);
				return _lastBinderId++;
			}
		}

		public static void UnregisterBinderObject(int binderId) {
			lock(_lock) _registeredBinderObjects.Remove(binderId);
		}

		public static int GetBinderId(IBinder binder) {
			lock(_lock) {
				foreach(var pair in _registeredBinderObjects)
					if(ReferenceEquals(binder, pair.Value))
						return pair.Key;

				return -1;
			}
		}

		static IBinder GetBinderObjectById(int binderId) {
			lock(_lock) {
				if(_registeredBinderObjects.TryGetValue(binderId, out var binder)) return binder;

				return null;
			}
		}

		public override void AdjustRefcount(int binderId, int addVal, int type) {
			var binder = GetBinderObjectById(binderId);

			if(binder == null)
				Log($"Invalid binder id {binderId}");
			else
				binder.AdjustRefcount(addVal, type);
		}

		void GetNativeHandle(int binderId, uint typeId, out uint readableEvent) {
			var binder = GetBinderObjectById(binderId);

			if(binder == null) {
				readableEvent = 0;

				Log($"Invalid binder id {binderId}");

				return;
			}

			binder.GetNativeHandle(typeId, out readableEvent);
		}

		protected uint OnTransact(int binderId, uint code, uint flags, ReadOnlySpan<byte> inputParcel,
			Span<byte> outputParcel
		) {
			var binder = GetBinderObjectById(binderId);

			if(binder == null) {
				Log($"Invalid binder id {binderId}");

				return 0;
			}

			return binder.OnTransact(code, flags, inputParcel, outputParcel);
		}
	}
}