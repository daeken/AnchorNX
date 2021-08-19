#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nv.Gemcontrol {
	public unsafe partial class INvGemControl : _Base_INvGemControl {}
	public class _Base_INvGemControl : IpcInterface {
		static readonly Logger Logger = new("INvGemControl");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(out var _0, out var _1);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(_1, copy: true));
					break;
				}
				case 2: { // Unknown2
					var ret = Unknown2(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					var ret = Unknown3(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					var ret = Unknown4(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					var ret = Unknown5();
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Unknown6
					var ret = Unknown6();
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // Unknown7
					var ret = Unknown7();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to INvGemControl: {im.CommandId}");
			}
		}
		
		public virtual object Unknown0() => throw new NotImplementedException();
		public virtual void Unknown1(out object _0, out uint _1) => throw new NotImplementedException();
		public virtual object Unknown2(object _0) => throw new NotImplementedException();
		public virtual object Unknown3(object _0) => throw new NotImplementedException();
		public virtual object Unknown4(object _0) => throw new NotImplementedException();
		public virtual object Unknown5() => throw new NotImplementedException();
		public virtual object Unknown6() => throw new NotImplementedException();
		public virtual object Unknown7() => throw new NotImplementedException();
	}
}
