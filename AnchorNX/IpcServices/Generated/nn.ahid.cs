#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Ahid {
	public unsafe partial class IServerSession : _Base_IServerSession {}
	public class _Base_IServerSession : IpcInterface {
		static readonly Logger Logger = new("IServerSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					var ret = Unknown2(null);
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 3: { // Unknown3
					var ret = Unknown3(null);
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IServerSession: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(object _0) => "Stub hit for Nn.Ahid.IServerSession.Unknown0 [0]".Debug(Log);
		public virtual void Unknown1(object _0) => "Stub hit for Nn.Ahid.IServerSession.Unknown1 [1]".Debug(Log);
		public virtual Nn.Ahid.ICtrlSession Unknown2(object _0) => throw new NotImplementedException();
		public virtual Nn.Ahid.IReadSession Unknown3(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class ICtrlSession : _Base_ICtrlSession {}
	public class _Base_ICtrlSession : IpcInterface {
		static readonly Logger Logger = new("ICtrlSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(null, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					Unknown4(null, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					Unknown5(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Unknown6
					Unknown6(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // Unknown7
					Unknown7(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // Unknown8
					Unknown8(null, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // Unknown9
					Unknown9(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // Unknown10
					var ret = Unknown10();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 11: { // Unknown11
					Unknown11();
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // Unknown12
					var ret = Unknown12(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ICtrlSession: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown1(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void Unknown2(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown3(object _0, Buffer<byte> _1) => "Stub hit for Nn.Ahid.ICtrlSession.Unknown3 [3]".Debug(Log);
		public virtual void Unknown4(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown5(object _0) => "Stub hit for Nn.Ahid.ICtrlSession.Unknown5 [5]".Debug(Log);
		public virtual void Unknown6(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void Unknown7(object _0) => "Stub hit for Nn.Ahid.ICtrlSession.Unknown7 [7]".Debug(Log);
		public virtual void Unknown8(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown9(object _0, Buffer<byte> _1) => "Stub hit for Nn.Ahid.ICtrlSession.Unknown9 [9]".Debug(Log);
		public virtual uint Unknown10() => throw new NotImplementedException();
		public virtual void Unknown11() => "Stub hit for Nn.Ahid.ICtrlSession.Unknown11 [11]".Debug(Log);
		public virtual object Unknown12(object _0, Buffer<byte> _1) => throw new NotImplementedException();
	}
	
	public unsafe partial class IReadSession : _Base_IReadSession {}
	public class _Base_IReadSession : IpcInterface {
		static readonly Logger Logger = new("IReadSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IReadSession: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
	}
	
	public unsafe partial class IWriteSession : _Base_IWriteSession {}
	public class _Base_IWriteSession : IpcInterface {
		static readonly Logger Logger = new("IWriteSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IWriteSession: {im.CommandId}");
			}
		}
		
		public virtual object Unknown0(Buffer<byte> _0) => throw new NotImplementedException();
	}
}
