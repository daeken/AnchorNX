#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Apm {
	public unsafe partial class IManager : _Base_IManager {}
	public class _Base_IManager : IpcInterface {
		static readonly Logger Logger = new("IManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenSession
					var ret = OpenSession();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 1: { // GetPerformanceMode
					var ret = GetPerformanceMode();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IManager: {im.CommandId}");
			}
		}
		
		public virtual Nn.Apm.ISession OpenSession() => throw new NotImplementedException();
		public virtual uint GetPerformanceMode() => throw new NotImplementedException();
	}
	
	public unsafe partial class IManagerPrivileged : _Base_IManagerPrivileged {}
	public class _Base_IManagerPrivileged : IpcInterface {
		static readonly Logger Logger = new("IManagerPrivileged");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenSession
					var ret = OpenSession();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IManagerPrivileged: {im.CommandId}");
			}
		}
		
		public virtual Nn.Apm.ISession OpenSession() => throw new NotImplementedException();
	}
	
	public unsafe partial class ISystemManager : _Base_ISystemManager {}
	public class _Base_ISystemManager : IpcInterface {
		static readonly Logger Logger = new("ISystemManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestPerformanceMode
					RequestPerformanceMode(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // GetPerformanceEvent
					var ret = GetPerformanceEvent(im.GetData<uint>(8));
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 2: { // GetThrottlingState
					GetThrottlingState(out var _0);
					om.Initialize(0, 0, 40);
					om.SetBytes(8, _0);
					break;
				}
				case 3: { // GetLastThrottlingState
					GetLastThrottlingState(out var _0);
					om.Initialize(0, 0, 40);
					om.SetBytes(8, _0);
					break;
				}
				case 4: { // ClearLastThrottlingState
					ClearLastThrottlingState();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // LoadAndApplySettings
					var ret = LoadAndApplySettings(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISystemManager: {im.CommandId}");
			}
		}
		
		public virtual void RequestPerformanceMode(uint _0) => "Stub hit for Nn.Apm.ISystemManager.RequestPerformanceMode [0]".Debug(Log);
		public virtual uint GetPerformanceEvent(uint _0) => throw new NotImplementedException();
		public virtual void GetThrottlingState(out byte[] _0) => throw new NotImplementedException();
		public virtual void GetLastThrottlingState(out byte[] _0) => throw new NotImplementedException();
		public virtual void ClearLastThrottlingState() => "Stub hit for Nn.Apm.ISystemManager.ClearLastThrottlingState [4]".Debug(Log);
		public virtual object LoadAndApplySettings(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IDebugManager : _Base_IDebugManager {}
	public class _Base_IDebugManager : IpcInterface {
		static readonly Logger Logger = new("IDebugManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetThrottlingState
					var ret = GetThrottlingState();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // GetLastThrottlingState
					var ret = GetLastThrottlingState();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // ClearLastThrottlingState
					ClearLastThrottlingState();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IDebugManager: {im.CommandId}");
			}
		}
		
		public virtual object GetThrottlingState() => throw new NotImplementedException();
		public virtual object GetLastThrottlingState() => throw new NotImplementedException();
		public virtual void ClearLastThrottlingState() => "Stub hit for Nn.Apm.IDebugManager.ClearLastThrottlingState [2]".Debug(Log);
	}
	
	public unsafe partial class ISession : _Base_ISession {}
	public class _Base_ISession : IpcInterface {
		static readonly Logger Logger = new("ISession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // SetPerformanceConfiguration
					SetPerformanceConfiguration(im.GetData<uint>(8), im.GetData<uint>(12));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // GetPerformanceConfiguration
					var ret = GetPerformanceConfiguration(im.GetData<uint>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISession: {im.CommandId}");
			}
		}
		
		public virtual void SetPerformanceConfiguration(uint _0, uint _1) => "Stub hit for Nn.Apm.ISession.SetPerformanceConfiguration [0]".Debug(Log);
		public virtual uint GetPerformanceConfiguration(uint _0) => throw new NotImplementedException();
	}
}
