#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Pm.Detail {
	public unsafe partial class IBootModeInterface : _Base_IBootModeInterface {}
	public class _Base_IBootModeInterface : IpcInterface {
		static readonly Logger Logger = new("IBootModeInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetBootMode
					var ret = GetBootMode();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // SetMaintenanceBoot
					SetMaintenanceBoot();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IBootModeInterface: {im.CommandId}");
			}
		}
		
		public virtual object GetBootMode() => throw new NotImplementedException();
		public virtual void SetMaintenanceBoot() => "Stub hit for Nn.Pm.Detail.IBootModeInterface.SetMaintenanceBoot [1]".Debug(Log);
	}
	
	public unsafe partial class IDebugMonitorInterface : _Base_IDebugMonitorInterface {}
	public class _Base_IDebugMonitorInterface : IpcInterface {
		static readonly Logger Logger = new("IDebugMonitorInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetDebugProcesses
					GetDebugProcesses(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // StartDebugProcess
					StartDebugProcess(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetTitlePid
					GetTitlePid(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // EnableDebugForTitleId
					var ret = EnableDebugForTitleId(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // GetApplicationPid
					var ret = GetApplicationPid(null);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 5: { // EnableDebugForApplication
					var ret = EnableDebugForApplication();
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // DisableDebug
					var ret = DisableDebug();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IDebugMonitorInterface: {im.CommandId}");
			}
		}
		
		public virtual void GetDebugProcesses(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void StartDebugProcess(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void GetTitlePid(object _0) => "Stub hit for Nn.Pm.Detail.IDebugMonitorInterface.GetTitlePid [2]".Debug(Log);
		public virtual object EnableDebugForTitleId(object _0) => throw new NotImplementedException();
		public virtual uint GetApplicationPid(object _0) => throw new NotImplementedException();
		public virtual object EnableDebugForApplication() => throw new NotImplementedException();
		public virtual uint DisableDebug() => throw new NotImplementedException();
	}
	
	public unsafe partial class IInformationInterface : _Base_IInformationInterface {}
	public class _Base_IInformationInterface : IpcInterface {
		static readonly Logger Logger = new("IInformationInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetTitleId
					var ret = GetTitleId(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IInformationInterface: {im.CommandId}");
			}
		}
		
		public virtual object GetTitleId(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IShellInterface : _Base_IShellInterface {}
	public class _Base_IShellInterface : IpcInterface {
		static readonly Logger Logger = new("IShellInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // LaunchProcess
					var ret = LaunchProcess(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // TerminateProcessByPid
					TerminateProcessByPid(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // TerminateProcessByTitleId
					TerminateProcessByTitleId(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // GetProcessEventWaiter
					var ret = GetProcessEventWaiter();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 4: { // GetProcessEventType
					var ret = GetProcessEventType();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // NotifyBootFinished
					NotifyBootFinished(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // GetApplicationPid
					GetApplicationPid(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // BoostSystemMemoryResourceLimit
					BoostSystemMemoryResourceLimit();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IShellInterface: {im.CommandId}");
			}
		}
		
		public virtual object LaunchProcess(object _0) => throw new NotImplementedException();
		public virtual void TerminateProcessByPid(object _0) => "Stub hit for Nn.Pm.Detail.IShellInterface.TerminateProcessByPid [1]".Debug(Log);
		public virtual void TerminateProcessByTitleId(object _0) => "Stub hit for Nn.Pm.Detail.IShellInterface.TerminateProcessByTitleId [2]".Debug(Log);
		public virtual uint GetProcessEventWaiter() => throw new NotImplementedException();
		public virtual object GetProcessEventType() => throw new NotImplementedException();
		public virtual void NotifyBootFinished(object _0) => "Stub hit for Nn.Pm.Detail.IShellInterface.NotifyBootFinished [5]".Debug(Log);
		public virtual void GetApplicationPid(object _0) => "Stub hit for Nn.Pm.Detail.IShellInterface.GetApplicationPid [6]".Debug(Log);
		public virtual void BoostSystemMemoryResourceLimit() => "Stub hit for Nn.Pm.Detail.IShellInterface.BoostSystemMemoryResourceLimit [7]".Debug(Log);
	}
}
