#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Profiler {
	public unsafe partial class IProfiler : _Base_IProfiler {}
	public class _Base_IProfiler : IpcInterface {
		static readonly Logger Logger = new("IProfiler");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetSystemEvent
					var ret = GetSystemEvent(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // StartSignalingEvent
					var ret = StartSignalingEvent(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // StopSignalingEvent
					var ret = StopSignalingEvent(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IProfiler: {im.CommandId}");
			}
		}
		
		public virtual object GetSystemEvent(object _0) => throw new NotImplementedException();
		public virtual object StartSignalingEvent(object _0) => throw new NotImplementedException();
		public virtual object StopSignalingEvent(object _0) => throw new NotImplementedException();
	}
}
