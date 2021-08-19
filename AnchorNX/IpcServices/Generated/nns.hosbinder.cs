#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nns.Hosbinder {
	public unsafe partial class IHOSBinderDriver : _Base_IHOSBinderDriver {}
	public class _Base_IHOSBinderDriver : IpcInterface {
		static readonly Logger Logger = new("IHOSBinderDriver");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // TransactParcel
					TransactParcel(im.GetData<int>(8), im.GetData<uint>(12), im.GetBuffer<byte>(0x5, 0), im.GetData<uint>(16), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // AdjustRefcount
					AdjustRefcount(im.GetData<int>(8), im.GetData<int>(12), im.GetData<int>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetNativeHandle
					var ret = GetNativeHandle(im.GetData<int>(8), im.GetData<uint>(12));
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 3: { // TransactParcelAuto
					TransactParcelAuto(im.GetData<int>(8), im.GetData<uint>(12), im.GetBuffer<byte>(0x21, 0), im.GetData<uint>(16), im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IHOSBinderDriver: {im.CommandId}");
			}
		}
		
		public virtual void TransactParcel(int id, uint code, Buffer<byte> parcel_data, uint flags, Buffer<byte> parcel_reply) => throw new NotImplementedException();
		public virtual void AdjustRefcount(int id, int addVal, int type) => "Stub hit for Nns.Hosbinder.IHOSBinderDriver.AdjustRefcount [1]".Debug(Log);
		public virtual uint GetNativeHandle(int id, uint _1) => throw new NotImplementedException();
		public virtual void TransactParcelAuto(int id, uint code, Buffer<byte> parcel_data, uint flags, Buffer<byte> parcel_reply) => throw new NotImplementedException();
	}
}
