#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Pdm.Detail {
	public unsafe partial class INotifyService : _Base_INotifyService {}
	public class _Base_INotifyService : IpcInterface {
		static readonly Logger Logger = new("INotifyService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					Unknown4();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					Unknown5(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Unknown6
					Unknown6(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // Unknown7
					Unknown7(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to INotifyService: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(object _0) => "Stub hit for Nn.Pdm.Detail.INotifyService.Unknown0 [0]".Debug(Log);
		public virtual void Unknown2(object _0) => "Stub hit for Nn.Pdm.Detail.INotifyService.Unknown2 [2]".Debug(Log);
		public virtual void Unknown3(object _0) => "Stub hit for Nn.Pdm.Detail.INotifyService.Unknown3 [3]".Debug(Log);
		public virtual void Unknown4() => "Stub hit for Nn.Pdm.Detail.INotifyService.Unknown4 [4]".Debug(Log);
		public virtual void Unknown5(Buffer<byte> _0) => "Stub hit for Nn.Pdm.Detail.INotifyService.Unknown5 [5]".Debug(Log);
		public virtual void Unknown6(object _0) => "Stub hit for Nn.Pdm.Detail.INotifyService.Unknown6 [6]".Debug(Log);
		public virtual void Unknown7(object _0) => "Stub hit for Nn.Pdm.Detail.INotifyService.Unknown7 [7]".Debug(Log);
	}
	
	public unsafe partial class IQueryService : _Base_IQueryService {}
	public class _Base_IQueryService : IpcInterface {
		static readonly Logger Logger = new("IQueryService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					var ret = Unknown4(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					var ret = Unknown5(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Unknown6
					var ret = Unknown6(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // Unknown7
					Unknown7(im.GetBuffer<byte>(0x5, 0), out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // Unknown8
					Unknown8(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // Unknown9
					var ret = Unknown9();
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // Unknown10
					Unknown10(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // Unknown11
					Unknown11(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // Unknown12
					var ret = Unknown12(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IQueryService: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown1(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown2(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown3(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown4(object _0) => throw new NotImplementedException();
		public virtual object Unknown5(object _0) => throw new NotImplementedException();
		public virtual object Unknown6(object _0) => throw new NotImplementedException();
		public virtual void Unknown7(Buffer<byte> _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown8(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown9() => throw new NotImplementedException();
		public virtual void Unknown10(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown11(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown12(object _0) => throw new NotImplementedException();
	}
}
