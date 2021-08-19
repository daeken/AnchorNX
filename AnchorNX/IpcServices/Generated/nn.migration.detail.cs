#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Migration.Detail {
	public unsafe partial class IAsyncContext : _Base_IAsyncContext {}
	public class _Base_IAsyncContext : IpcInterface {
		static readonly Logger Logger = new("IAsyncContext");
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
					Unknown1();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					var ret = Unknown2();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAsyncContext: {im.CommandId}");
			}
		}
		
		public virtual uint Unknown0() => throw new NotImplementedException();
		public virtual void Unknown1() => "Stub hit for Nn.Migration.Detail.IAsyncContext.Unknown1 [1]".Debug(Log);
		public virtual object Unknown2() => throw new NotImplementedException();
		public virtual void Unknown3() => "Stub hit for Nn.Migration.Detail.IAsyncContext.Unknown3 [3]".Debug(Log);
	}
}
