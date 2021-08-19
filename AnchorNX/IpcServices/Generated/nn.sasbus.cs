#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Sasbus {
	public unsafe partial class IManager : _Base_IManager {}
	public class _Base_IManager : IpcInterface {
		static readonly Logger Logger = new("IManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenSession
					var ret = OpenSession(null);
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IManager: {im.CommandId}");
			}
		}
		
		public virtual Nn.Sasbus.ISession OpenSession(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class ISession : _Base_ISession {}
	public class _Base_ISession : IpcInterface {
		static readonly Logger Logger = new("ISession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(null, im.GetBuffer<byte>(0x21, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(null, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null, im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISession: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(object _0, Buffer<byte> _1) => "Stub hit for Nn.Sasbus.ISession.Unknown0 [0]".Debug(Log);
		public virtual void Unknown1(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown2(object _0, uint _1) => "Stub hit for Nn.Sasbus.ISession.Unknown2 [2]".Debug(Log);
		public virtual void Unknown3() => "Stub hit for Nn.Sasbus.ISession.Unknown3 [3]".Debug(Log);
	}
}
