#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nv.Gemcoredump {
	public unsafe partial class INvGemCoreDump : _Base_INvGemCoreDump {}
	public class _Base_INvGemCoreDump : IpcInterface {
		static readonly Logger Logger = new("INvGemCoreDump");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					var ret = Unknown1();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(out var _0, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to INvGemCoreDump: {im.CommandId}");
			}
		}
		
		public virtual object Unknown0() => throw new NotImplementedException();
		public virtual object Unknown1() => throw new NotImplementedException();
		public virtual void Unknown2(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
	}
}
