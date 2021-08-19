#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Ntc.Detail.Service {
	public unsafe partial class IStaticService : _Base_IStaticService {}
	public class _Base_IStaticService : IpcInterface {
		static readonly Logger Logger = new("IStaticService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenEnsureNetworkClockAvailabilityService
					var ret = OpenEnsureNetworkClockAvailabilityService(im.GetData<uint>(8), im.GetData<uint>(12));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 100: { // SuspendAutonomicTimeCorrection
					SuspendAutonomicTimeCorrection();
					om.Initialize(0, 0, 0);
					break;
				}
				case 101: { // ResumeAutonomicTimeCorrection
					ResumeAutonomicTimeCorrection();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IStaticService: {im.CommandId}");
			}
		}
		
		public virtual Nn.Ntc.Detail.Service.IEnsureNetworkClockAvailabilityService OpenEnsureNetworkClockAvailabilityService(uint _0, uint _1) => throw new NotImplementedException();
		public virtual void SuspendAutonomicTimeCorrection() => "Stub hit for Nn.Ntc.Detail.Service.IStaticService.SuspendAutonomicTimeCorrection [100]".Debug(Log);
		public virtual void ResumeAutonomicTimeCorrection() => "Stub hit for Nn.Ntc.Detail.Service.IStaticService.ResumeAutonomicTimeCorrection [101]".Debug(Log);
	}
	
	public unsafe partial class IEnsureNetworkClockAvailabilityService : _Base_IEnsureNetworkClockAvailabilityService {}
	public class _Base_IEnsureNetworkClockAvailabilityService : IpcInterface {
		static readonly Logger Logger = new("IEnsureNetworkClockAvailabilityService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // StartTask
					StartTask();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // GetFinishNotificationEvent
					var ret = GetFinishNotificationEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 2: { // GetResult
					GetResult();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Cancel
					Cancel();
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // IsProcessing
					var ret = IsProcessing();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 5: { // GetServerTime
					var ret = GetServerTime();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IEnsureNetworkClockAvailabilityService: {im.CommandId}");
			}
		}
		
		public virtual void StartTask() => "Stub hit for Nn.Ntc.Detail.Service.IEnsureNetworkClockAvailabilityService.StartTask [0]".Debug(Log);
		public virtual uint GetFinishNotificationEvent() => throw new NotImplementedException();
		public virtual void GetResult() => "Stub hit for Nn.Ntc.Detail.Service.IEnsureNetworkClockAvailabilityService.GetResult [2]".Debug(Log);
		public virtual void Cancel() => "Stub hit for Nn.Ntc.Detail.Service.IEnsureNetworkClockAvailabilityService.Cancel [3]".Debug(Log);
		public virtual byte IsProcessing() => throw new NotImplementedException();
		public virtual ulong GetServerTime() => throw new NotImplementedException();
	}
}
