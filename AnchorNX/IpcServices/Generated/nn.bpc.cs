#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Bpc {
	public unsafe partial class IBoardPowerControlManager : _Base_IBoardPowerControlManager {}
	public class _Base_IBoardPowerControlManager : IpcInterface {
		static readonly Logger Logger = new("IBoardPowerControlManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // ShutdownSystem
					ShutdownSystem();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // RebootSystem
					RebootSystem();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetWakeupReason
					var ret = GetWakeupReason();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // GetShutdownReason
					var ret = GetShutdownReason();
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // GetAcOk
					var ret = GetAcOk();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // GetBoardPowerControlEvent
					var ret = GetBoardPowerControlEvent(null);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 6: { // GetSleepButtonState
					var ret = GetSleepButtonState();
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // GetPowerEvent
					var ret = GetPowerEvent(null);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 8: { // Unknown8
					var ret = Unknown8(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // Unknown9
					Unknown9(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // Unknown10
					var ret = Unknown10();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IBoardPowerControlManager: {im.CommandId}");
			}
		}
		
		public virtual void ShutdownSystem() => "Stub hit for Nn.Bpc.IBoardPowerControlManager.ShutdownSystem [0]".Debug(Log);
		public virtual void RebootSystem() => "Stub hit for Nn.Bpc.IBoardPowerControlManager.RebootSystem [1]".Debug(Log);
		public virtual object GetWakeupReason() => throw new NotImplementedException();
		public virtual object GetShutdownReason() => throw new NotImplementedException();
		public virtual object GetAcOk() => throw new NotImplementedException();
		public virtual uint GetBoardPowerControlEvent(object _0) => throw new NotImplementedException();
		public virtual object GetSleepButtonState() => throw new NotImplementedException();
		public virtual uint GetPowerEvent(object _0) => throw new NotImplementedException();
		public virtual object Unknown8(object _0) => throw new NotImplementedException();
		public virtual void Unknown9(object _0) => "Stub hit for Nn.Bpc.IBoardPowerControlManager.Unknown9 [9]".Debug(Log);
		public virtual object Unknown10() => throw new NotImplementedException();
	}
	
	public unsafe partial class IRtcManager : _Base_IRtcManager {}
	public class _Base_IRtcManager : IpcInterface {
		static readonly Logger Logger = new("IRtcManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetExternalRtcValue
					var ret = GetExternalRtcValue();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // SetExternalRtcValue
					SetExternalRtcValue(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // ReadExternalRtcResetFlag
					var ret = ReadExternalRtcResetFlag();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // ClearExternalRtcResetFlag
					ClearExternalRtcResetFlag();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IRtcManager: {im.CommandId}");
			}
		}
		
		public virtual object GetExternalRtcValue() => throw new NotImplementedException();
		public virtual void SetExternalRtcValue(object _0) => "Stub hit for Nn.Bpc.IRtcManager.SetExternalRtcValue [1]".Debug(Log);
		public virtual object ReadExternalRtcResetFlag() => throw new NotImplementedException();
		public virtual void ClearExternalRtcResetFlag() => "Stub hit for Nn.Bpc.IRtcManager.ClearExternalRtcResetFlag [3]".Debug(Log);
	}
	
	public unsafe partial class IPowerButtonManager : _Base_IPowerButtonManager {}
	public class _Base_IPowerButtonManager : IpcInterface {
		static readonly Logger Logger = new("IPowerButtonManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					var ret = Unknown1(null);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IPowerButtonManager: {im.CommandId}");
			}
		}
		
		public virtual object Unknown0() => throw new NotImplementedException();
		public virtual uint Unknown1(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IWakeupConfigManager : _Base_IWakeupConfigManager {}
	public class _Base_IWakeupConfigManager : IpcInterface {
		static readonly Logger Logger = new("IWakeupConfigManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					var ret = Unknown2();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IWakeupConfigManager: {im.CommandId}");
			}
		}
		
		public virtual object Unknown0(object _0) => throw new NotImplementedException();
		public virtual void Unknown1(object _0) => "Stub hit for Nn.Bpc.IWakeupConfigManager.Unknown1 [1]".Debug(Log);
		public virtual object Unknown2() => throw new NotImplementedException();
	}
}
