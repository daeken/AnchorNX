#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Ahid.Hdr {
	public unsafe partial class ISession : _Base_ISession {}
	public class _Base_ISession : IpcInterface {
		static readonly Logger Logger = new("ISession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(null, im.GetBuffer<byte>(0x5, 0), out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					var ret = Unknown3(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					Unknown4(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISession: {im.CommandId}");
			}
		}
		
		public virtual object Unknown0() => throw new NotImplementedException();
		public virtual void Unknown1(object _0, Buffer<byte> _1, out object _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void Unknown2(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual object Unknown3(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void Unknown4(object _0) => "Stub hit for Nn.Ahid.Hdr.ISession.Unknown4 [4]".Debug(Log);
	}
}
