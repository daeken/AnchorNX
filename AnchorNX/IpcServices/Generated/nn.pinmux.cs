#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Pinmux {
	public unsafe partial class IManager : _Base_IManager {}
	public class _Base_IManager : IpcInterface {
		static readonly Logger Logger = new("IManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenSession
					var ret = OpenSession(null);
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IManager: {im.CommandId}");
			}
		}
		
		public virtual Nn.Pinmux.ISession OpenSession(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class ISession : _Base_ISession {}
	public class _Base_ISession : IpcInterface {
		static readonly Logger Logger = new("ISession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // SetPinAssignment
					SetPinAssignment(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // GetPinAssignment
					var ret = GetPinAssignment();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // SetPinAssignmentForHardwareTest
					SetPinAssignmentForHardwareTest(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISession: {im.CommandId}");
			}
		}
		
		public virtual void SetPinAssignment(object _0) => "Stub hit for Nn.Pinmux.ISession.SetPinAssignment [0]".Debug(Log);
		public virtual object GetPinAssignment() => throw new NotImplementedException();
		public virtual void SetPinAssignmentForHardwareTest(object _0) => "Stub hit for Nn.Pinmux.ISession.SetPinAssignmentForHardwareTest [2]".Debug(Log);
	}
}
