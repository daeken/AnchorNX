#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Usb.Hs {
	public unsafe partial class IClientRootSession : _Base_IClientRootSession {}
	public class _Base_IClientRootSession : IpcInterface {
		static readonly Logger Logger = new("IClientRootSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // BindClientProcess
					BindClientProcess(im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					var ret = Unknown4(null);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 5: { // Unknown5
					Unknown5(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // GetInterfaceStateChangeEvent
					var ret = GetInterfaceStateChangeEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 7: { // GetClientIfSession
					GetClientIfSession(null, im.GetBuffer<byte>(0x6, 0), im.GetBuffer<byte>(0x6, 1), out var _2);
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(_2));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IClientRootSession: {im.CommandId}");
			}
		}
		
		public virtual void BindClientProcess(uint _0) => "Stub hit for Nn.Usb.Hs.IClientRootSession.BindClientProcess [0]".Debug(Log);
		public virtual void Unknown1(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown2(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown3(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual uint Unknown4(object _0) => throw new NotImplementedException();
		public virtual void Unknown5(object _0) => "Stub hit for Nn.Usb.Hs.IClientRootSession.Unknown5 [5]".Debug(Log);
		public virtual uint GetInterfaceStateChangeEvent() => throw new NotImplementedException();
		public virtual void GetClientIfSession(object _0, Buffer<byte> _1, Buffer<byte> _2, out Nn.Usb.Hs.IClientIfSession _3) => throw new NotImplementedException();
	}
	
	public unsafe partial class IClientEpSession : _Base_IClientEpSession {}
	public class _Base_IClientEpSession : IpcInterface {
		static readonly Logger Logger = new("IClientEpSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					var ret = Unknown2();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 3: { // Unknown3
					Unknown3();
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // PostBufferAsync
					var ret = PostBufferAsync(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					Unknown5(null, out var _0, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Unknown6
					var ret = Unknown6(null, im.GetBuffer<byte>(0x21, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // Unknown7
					Unknown7(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // Unknown8
					Unknown8(null, im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IClientEpSession: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0() => "Stub hit for Nn.Usb.Hs.IClientEpSession.Unknown0 [0]".Debug(Log);
		public virtual void Unknown1() => "Stub hit for Nn.Usb.Hs.IClientEpSession.Unknown1 [1]".Debug(Log);
		public virtual uint Unknown2() => throw new NotImplementedException();
		public virtual void Unknown3() => "Stub hit for Nn.Usb.Hs.IClientEpSession.Unknown3 [3]".Debug(Log);
		public virtual object PostBufferAsync(object _0) => throw new NotImplementedException();
		public virtual void Unknown5(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown6(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown7(object _0) => "Stub hit for Nn.Usb.Hs.IClientEpSession.Unknown7 [7]".Debug(Log);
		public virtual void Unknown8(object _0, uint _1) => "Stub hit for Nn.Usb.Hs.IClientEpSession.Unknown8 [8]".Debug(Log);
	}
	
	public unsafe partial class IClientIfSession : _Base_IClientIfSession {}
	public class _Base_IClientIfSession : IpcInterface {
		static readonly Logger Logger = new("IClientIfSession");
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
					Unknown1(null, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(null, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					var ret = Unknown4();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // CtrlXferAsync
					CtrlXferAsync(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Unknown6
					var ret = Unknown6();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 7: { // GetCtrlXferReport
					GetCtrlXferReport(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // Unknown8
					Unknown8();
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // GetClientEpSession
					GetClientEpSession(null, out var _0, out var _1);
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(_1));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IClientIfSession: {im.CommandId}");
			}
		}
		
		public virtual uint Unknown0() => throw new NotImplementedException();
		public virtual void Unknown1(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown2(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void Unknown3(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual object Unknown4() => throw new NotImplementedException();
		public virtual void CtrlXferAsync(object _0) => "Stub hit for Nn.Usb.Hs.IClientIfSession.CtrlXferAsync [5]".Debug(Log);
		public virtual uint Unknown6() => throw new NotImplementedException();
		public virtual void GetCtrlXferReport(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void Unknown8() => "Stub hit for Nn.Usb.Hs.IClientIfSession.Unknown8 [8]".Debug(Log);
		public virtual void GetClientEpSession(object _0, out object _1, out Nn.Usb.Hs.IClientEpSession _2) => throw new NotImplementedException();
	}
}
