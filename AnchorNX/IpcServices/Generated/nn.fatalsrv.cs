#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Fatalsrv {
	public unsafe partial class IPrivateService : _Base_IPrivateService {}
	public class _Base_IPrivateService : IpcInterface {
		static readonly Logger Logger = new("IPrivateService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetFatalEvent
					var ret = GetFatalEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IPrivateService: {im.CommandId}");
			}
		}
		
		public virtual uint GetFatalEvent() => throw new NotImplementedException();
	}
	
	public unsafe partial class IService : _Base_IService {}
	public class _Base_IService : IpcInterface {
		static readonly Logger Logger = new("IService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // ThrowFatal
					ThrowFatal(im.GetData<ulong>(8), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // ThrowFatalWithPolicy
					ThrowFatalWithPolicy(im.GetData<ulong>(8), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // ThrowFatalWithCpuContext
					ThrowFatalWithCpuContext(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x15, 0), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IService: {im.CommandId}");
			}
		}
		
		public virtual void ThrowFatal(ulong _0, ulong _1, ulong _2) => "Stub hit for Nn.Fatalsrv.IService.ThrowFatal [0]".Debug(Log);
		public virtual void ThrowFatalWithPolicy(ulong _0, ulong _1, ulong _2) => "Stub hit for Nn.Fatalsrv.IService.ThrowFatalWithPolicy [1]".Debug(Log);
		public virtual void ThrowFatalWithCpuContext(ulong errorCode, ulong _1, Buffer<byte> errorBuf, ulong _3) => "Stub hit for Nn.Fatalsrv.IService.ThrowFatalWithCpuContext [2]".Debug(Log);
	}
}
