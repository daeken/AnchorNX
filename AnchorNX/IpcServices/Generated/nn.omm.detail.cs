#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Omm.Detail {
	public unsafe partial class IOperationModeManager : _Base_IOperationModeManager {}
	public class _Base_IOperationModeManager : IpcInterface {
		static readonly Logger Logger = new("IOperationModeManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetOperationMode
					var ret = GetOperationMode();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // GetOperationModeChangeEvent
					var ret = GetOperationModeChangeEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 2: { // EnableAudioVisual
					EnableAudioVisual();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // DisableAudioVisual
					DisableAudioVisual();
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // EnterSleepAndWait
					EnterSleepAndWait(im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // GetCradleStatus
					var ret = GetCradleStatus();
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // FadeInDisplay
					FadeInDisplay();
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // FadeOutDisplay
					FadeOutDisplay();
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // Unknown8
					var ret = Unknown8();
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // Unknown9
					Unknown9();
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // Unknown10
					Unknown10(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // Unknown11
					var ret = Unknown11();
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // Unknown12
					var ret = Unknown12();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 13: { // Unknown13
					Unknown13();
					om.Initialize(0, 0, 0);
					break;
				}
				case 14: { // Unknown14
					var ret = Unknown14();
					om.Initialize(0, 0, 0);
					break;
				}
				case 15: { // Unknown15
					Unknown15();
					om.Initialize(0, 0, 0);
					break;
				}
				case 16: { // Unknown16
					Unknown16();
					om.Initialize(0, 0, 0);
					break;
				}
				case 17: { // Unknown17
					Unknown17();
					om.Initialize(0, 0, 0);
					break;
				}
				case 18: { // Unknown18
					Unknown18();
					om.Initialize(0, 0, 0);
					break;
				}
				case 19: { // Unknown19
					var ret = Unknown19();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 20: { // Unknown20
					var ret = Unknown20();
					om.Initialize(0, 0, 0);
					break;
				}
				case 21: { // Unknown21
					Unknown21(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 22: { // Unknown22
					var ret = Unknown22();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 23: { // Unknown23
					var ret = Unknown23();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IOperationModeManager: {im.CommandId}");
			}
		}
		
		public virtual object GetOperationMode() => throw new NotImplementedException();
		public virtual uint GetOperationModeChangeEvent() => throw new NotImplementedException();
		public virtual void EnableAudioVisual() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.EnableAudioVisual [2]".Debug(Log);
		public virtual void DisableAudioVisual() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.DisableAudioVisual [3]".Debug(Log);
		public virtual void EnterSleepAndWait(uint _0) => "Stub hit for Nn.Omm.Detail.IOperationModeManager.EnterSleepAndWait [4]".Debug(Log);
		public virtual object GetCradleStatus() => throw new NotImplementedException();
		public virtual void FadeInDisplay() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.FadeInDisplay [6]".Debug(Log);
		public virtual void FadeOutDisplay() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.FadeOutDisplay [7]".Debug(Log);
		public virtual object Unknown8() => throw new NotImplementedException();
		public virtual void Unknown9() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.Unknown9 [9]".Debug(Log);
		public virtual void Unknown10(object _0) => "Stub hit for Nn.Omm.Detail.IOperationModeManager.Unknown10 [10]".Debug(Log);
		public virtual object Unknown11() => throw new NotImplementedException();
		public virtual uint Unknown12() => throw new NotImplementedException();
		public virtual void Unknown13() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.Unknown13 [13]".Debug(Log);
		public virtual object Unknown14() => throw new NotImplementedException();
		public virtual void Unknown15() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.Unknown15 [15]".Debug(Log);
		public virtual void Unknown16() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.Unknown16 [16]".Debug(Log);
		public virtual void Unknown17() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.Unknown17 [17]".Debug(Log);
		public virtual void Unknown18() => "Stub hit for Nn.Omm.Detail.IOperationModeManager.Unknown18 [18]".Debug(Log);
		public virtual uint Unknown19() => throw new NotImplementedException();
		public virtual object Unknown20() => throw new NotImplementedException();
		public virtual void Unknown21(object _0) => "Stub hit for Nn.Omm.Detail.IOperationModeManager.Unknown21 [21]".Debug(Log);
		public virtual uint Unknown22() => throw new NotImplementedException();
		public virtual object Unknown23() => throw new NotImplementedException();
	}
}
