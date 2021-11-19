#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Sm.Detail {
	public unsafe partial class IManagerInterface : _Base_IManagerInterface {}
	public class _Base_IManagerInterface : IpcInterface {
		static readonly Logger Logger = new("IManagerInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RegisterProcess
					RegisterProcess(im.GetData<ulong>(8), im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x5, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // UnregisterProcess
					UnregisterProcess(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IManagerInterface: {im.CommandId}");
			}
		}
		
		public virtual void RegisterProcess(ulong _0, Buffer<byte> _1, Buffer<byte> _2) => "Stub hit for Nn.Sm.Detail.IManagerInterface.RegisterProcess [0]".Debug(Log);
		public virtual void UnregisterProcess(ulong _0) => "Stub hit for Nn.Sm.Detail.IManagerInterface.UnregisterProcess [1]".Debug(Log);
	}
	
	public unsafe partial class IUserInterface : _Base_IUserInterface {}
	public class _Base_IUserInterface : IpcInterface {
		static readonly Logger Logger = new("IUserInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) => throw new NotImplementedException();

		public virtual void Initialize(ulong _0, ulong reserved) => "Stub hit for Nn.Sm.Detail.IUserInterface.Initialize [0]".Debug(Log);
		public virtual void UnregisterService(byte[] name) => "Stub hit for Nn.Sm.Detail.IUserInterface.UnregisterService [3]".Debug(Log);
	}
}
