#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Account.Detail {
	public unsafe partial class IAsyncContext : _Base_IAsyncContext {}
	public class _Base_IAsyncContext : IpcInterface {
		static readonly Logger Logger = new("IAsyncContext");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetSystemEvent
					var ret = GetSystemEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 1: { // Cancel
					Cancel();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // HasDone
					var ret = HasDone();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 3: { // GetResult
					GetResult();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAsyncContext: {im.CommandId}");
			}
		}
		
		public virtual uint GetSystemEvent() => throw new NotImplementedException();
		public virtual void Cancel() => "Stub hit for Nn.Account.Detail.IAsyncContext.Cancel [1]".Debug(Log);
		public virtual byte HasDone() => throw new NotImplementedException();
		public virtual void GetResult() => "Stub hit for Nn.Account.Detail.IAsyncContext.GetResult [3]".Debug(Log);
	}
	
	public unsafe partial class INotifier : _Base_INotifier {}
	public class _Base_INotifier : IpcInterface {
		static readonly Logger Logger = new("INotifier");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetSystemEvent
					var ret = GetSystemEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to INotifier: {im.CommandId}");
			}
		}
		
		public virtual uint GetSystemEvent() => throw new NotImplementedException();
	}
	
	public unsafe partial class ISessionObject : _Base_ISessionObject {}
	public class _Base_ISessionObject : IpcInterface {
		static readonly Logger Logger = new("ISessionObject");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 999: { // Dummy
					Dummy();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISessionObject: {im.CommandId}");
			}
		}
		
		public virtual void Dummy() => "Stub hit for Nn.Account.Detail.ISessionObject.Dummy [999]".Debug(Log);
	}
}
