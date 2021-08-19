#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Cec {
	public unsafe partial class ICecManager : _Base_ICecManager {}
	public class _Base_ICecManager : IpcInterface {
		static readonly Logger Logger = new("ICecManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(out var _0, out var _1);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(_1, copy: true));
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
				default:
					throw new NotImplementedException($"Unhandled command ID to ICecManager: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(out object _0, out uint _1) => throw new NotImplementedException();
		public virtual object Unknown1(object _0) => throw new NotImplementedException();
		public virtual void Unknown2(object _0) => "Stub hit for Nn.Cec.ICecManager.Unknown2 [2]".Debug(Log);
		public virtual object Unknown3(object _0) => throw new NotImplementedException();
		public virtual object Unknown4(object _0) => throw new NotImplementedException();
		public virtual object Unknown5() => throw new NotImplementedException();
		public virtual object Unknown6() => throw new NotImplementedException();
	}
}
