#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Ts.Server {
	public unsafe partial class IMeasurementServer : _Base_IMeasurementServer {}
	public class _Base_IMeasurementServer : IpcInterface {
		static readonly Logger Logger = new("IMeasurementServer");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					var ret = Unknown1(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					var ret = Unknown3(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IMeasurementServer: {im.CommandId}");
			}
		}
		
		public virtual object Unknown0(object _0) => throw new NotImplementedException();
		public virtual object Unknown1(object _0) => throw new NotImplementedException();
		public virtual void Unknown2(object _0) => "Stub hit for Nn.Ts.Server.IMeasurementServer.Unknown2 [2]".Debug(Log);
		public virtual object Unknown3(object _0) => throw new NotImplementedException();
	}
}
