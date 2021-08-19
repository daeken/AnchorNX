#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Ldr.Detail {
	public unsafe partial class IDebugMonitorInterface : _Base_IDebugMonitorInterface {}
	public class _Base_IDebugMonitorInterface : IpcInterface {
		static readonly Logger Logger = new("IDebugMonitorInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // AddProcessToDebugLaunchQueue
					AddProcessToDebugLaunchQueue(null, im.GetBuffer<byte>(0x9, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // ClearDebugLaunchQueue
					ClearDebugLaunchQueue();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetNsoInfos
					GetNsoInfos(null, out var _0, im.GetBuffer<byte>(0xA, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IDebugMonitorInterface: {im.CommandId}");
			}
		}
		
		public virtual void AddProcessToDebugLaunchQueue(object _0, Buffer<byte> _1) => "Stub hit for Nn.Ldr.Detail.IDebugMonitorInterface.AddProcessToDebugLaunchQueue [0]".Debug(Log);
		public virtual void ClearDebugLaunchQueue() => "Stub hit for Nn.Ldr.Detail.IDebugMonitorInterface.ClearDebugLaunchQueue [1]".Debug(Log);
		public virtual void GetNsoInfos(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
	}
	
	public unsafe partial class IProcessManagerInterface : _Base_IProcessManagerInterface {}
	public class _Base_IProcessManagerInterface : IpcInterface {
		static readonly Logger Logger = new("IProcessManagerInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // CreateProcess
					var ret = CreateProcess(null, im.GetCopy(0));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 1: { // GetProgramInfo
					GetProgramInfo(null, im.GetBuffer<byte>(0x1A, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // RegisterTitle
					var ret = RegisterTitle(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // UnregisterTitle
					UnregisterTitle(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IProcessManagerInterface: {im.CommandId}");
			}
		}
		
		public virtual IpcInterface CreateProcess(object _0, uint _1) => throw new NotImplementedException();
		public virtual void GetProgramInfo(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual object RegisterTitle(object _0) => throw new NotImplementedException();
		public virtual void UnregisterTitle(object _0) => "Stub hit for Nn.Ldr.Detail.IProcessManagerInterface.UnregisterTitle [3]".Debug(Log);
	}
	
	public unsafe partial class IRoInterface : _Base_IRoInterface {}
	public class _Base_IRoInterface : IpcInterface {
		static readonly Logger Logger = new("IRoInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // LoadNro
					var ret = LoadNro(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), im.GetData<ulong>(32), im.GetData<ulong>(40), im.Pid);
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 1: { // UnloadNro
					UnloadNro(im.GetData<ulong>(8), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // LoadNrr
					LoadNrr(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // UnloadNrr
					UnloadNrr(im.GetData<ulong>(8), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Initialize
					Initialize(im.GetData<ulong>(8), im.Pid, im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IRoInterface: {im.CommandId}");
			}
		}
		
		public virtual ulong LoadNro(ulong _0, ulong _1, ulong _2, ulong _3, ulong _4, ulong _5) => throw new NotImplementedException();
		public virtual void UnloadNro(ulong _0, ulong _1, ulong _2) => "Stub hit for Nn.Ldr.Detail.IRoInterface.UnloadNro [1]".Debug(Log);
		public virtual void LoadNrr(ulong _0, ulong _1, ulong _2, ulong _3) => "Stub hit for Nn.Ldr.Detail.IRoInterface.LoadNrr [2]".Debug(Log);
		public virtual void UnloadNrr(ulong _0, ulong _1, ulong _2) => "Stub hit for Nn.Ldr.Detail.IRoInterface.UnloadNrr [3]".Debug(Log);
		public virtual void Initialize(ulong _0, ulong _1, uint _2) => "Stub hit for Nn.Ldr.Detail.IRoInterface.Initialize [4]".Debug(Log);
	}
	
	public unsafe partial class IShellInterface : _Base_IShellInterface {}
	public class _Base_IShellInterface : IpcInterface {
		static readonly Logger Logger = new("IShellInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // AddProcessToLaunchQueue
					AddProcessToLaunchQueue(im.GetBuffer<byte>(0x9, 0), im.GetData<uint>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // ClearLaunchQueue
					ClearLaunchQueue();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IShellInterface: {im.CommandId}");
			}
		}
		
		public virtual void AddProcessToLaunchQueue(Buffer<byte> _0, uint size, ulong appID) => "Stub hit for Nn.Ldr.Detail.IShellInterface.AddProcessToLaunchQueue [0]".Debug(Log);
		public virtual void ClearLaunchQueue() => "Stub hit for Nn.Ldr.Detail.IShellInterface.ClearLaunchQueue [1]".Debug(Log);
	}
}
