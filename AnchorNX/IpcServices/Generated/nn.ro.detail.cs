#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Ro.Detail {
	public unsafe partial class IDebugMonitorInterface : _Base_IDebugMonitorInterface {}
	public class _Base_IDebugMonitorInterface : IpcInterface {
		static readonly Logger Logger = new("IDebugMonitorInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IDebugMonitorInterface: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
	}
	
	public unsafe partial class IRoInterface : _Base_IRoInterface {}
	public class _Base_IRoInterface : IpcInterface {
		static readonly Logger Logger = new("IRoInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), im.GetData<ulong>(32), im.GetData<ulong>(40), im.Pid);
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 1: { // Unknown1
					Unknown1(im.GetData<ulong>(8), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(im.GetData<ulong>(8), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					Unknown4(im.GetData<ulong>(8), im.Pid, im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IRoInterface: {im.CommandId}");
			}
		}
		
		public virtual ulong Unknown0(ulong _0, ulong _1, ulong _2, ulong _3, ulong _4, ulong _5) => throw new NotImplementedException();
		public virtual void Unknown1(ulong _0, ulong _1, ulong _2) => "Stub hit for Nn.Ro.Detail.IRoInterface.Unknown1 [1]".Debug(Log);
		public virtual void Unknown2(ulong _0, ulong _1, ulong _2, ulong _3) => "Stub hit for Nn.Ro.Detail.IRoInterface.Unknown2 [2]".Debug(Log);
		public virtual void Unknown3(ulong _0, ulong _1, ulong _2) => "Stub hit for Nn.Ro.Detail.IRoInterface.Unknown3 [3]".Debug(Log);
		public virtual void Unknown4(ulong _0, ulong _1, uint _2) => "Stub hit for Nn.Ro.Detail.IRoInterface.Unknown4 [4]".Debug(Log);
	}
}
