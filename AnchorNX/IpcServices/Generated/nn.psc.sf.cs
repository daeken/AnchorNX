#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Psc.Sf {
	public unsafe partial class IPmControl : _Base_IPmControl {}
	public class _Base_IPmControl : IpcInterface {
		static readonly Logger Logger = new("IPmControl");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 1: { // Unknown1
					Unknown1(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					var ret = Unknown3();
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					Unknown4();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					Unknown5();
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Unknown6
					Unknown6(out var _0, im.GetBuffer<byte>(0x6, 0), im.GetBuffer<byte>(0x6, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IPmControl: {im.CommandId}");
			}
		}
		
		public virtual uint Unknown0() => throw new NotImplementedException();
		public virtual void Unknown1(object _0) => "Stub hit for Nn.Psc.Sf.IPmControl.Unknown1 [1]".Debug(Log);
		public virtual void Unknown2() => "Stub hit for Nn.Psc.Sf.IPmControl.Unknown2 [2]".Debug(Log);
		public virtual object Unknown3() => throw new NotImplementedException();
		public virtual void Unknown4() => "Stub hit for Nn.Psc.Sf.IPmControl.Unknown4 [4]".Debug(Log);
		public virtual void Unknown5() => "Stub hit for Nn.Psc.Sf.IPmControl.Unknown5 [5]".Debug(Log);
		public virtual void Unknown6(out object _0, Buffer<byte> _1, Buffer<byte> _2) => throw new NotImplementedException();
	}
	
	public unsafe partial class IPmService : _Base_IPmService {}
	public class _Base_IPmService : IpcInterface {
		static readonly Logger Logger = new("IPmService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetPmModule
					var ret = GetPmModule();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IPmService: {im.CommandId}");
			}
		}
		
		public virtual Nn.Psc.Sf.IPmModule GetPmModule() => throw new NotImplementedException();
	}
	
	public unsafe partial class IPmModule : _Base_IPmModule {}
	public class _Base_IPmModule : IpcInterface {
		static readonly Logger Logger = new("IPmModule");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Initialize
					var ret = Initialize(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 1: { // GetRequest
					var ret = GetRequest();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Acknowledge
					Acknowledge();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IPmModule: {im.CommandId}");
			}
		}
		
		public virtual uint Initialize(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual object GetRequest() => throw new NotImplementedException();
		public virtual void Acknowledge() => "Stub hit for Nn.Psc.Sf.IPmModule.Acknowledge [2]".Debug(Log);
		public virtual void Unknown3() => "Stub hit for Nn.Psc.Sf.IPmModule.Unknown3 [3]".Debug(Log);
	}
}
