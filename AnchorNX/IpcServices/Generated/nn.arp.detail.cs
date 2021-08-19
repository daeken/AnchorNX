#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Arp.Detail {
	public unsafe partial class IReader : _Base_IReader {}
	public class _Base_IReader : IpcInterface {
		static readonly Logger Logger = new("IReader");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetApplicationLaunchProperty
					var ret = GetApplicationLaunchProperty(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // GetApplicationLaunchPropertyWithApplicationId
					var ret = GetApplicationLaunchPropertyWithApplicationId(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetApplicationControlProperty
					GetApplicationControlProperty(null, im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // GetApplicationControlPropertyWithApplicationId
					GetApplicationControlPropertyWithApplicationId(null, im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IReader: {im.CommandId}");
			}
		}
		
		public virtual object GetApplicationLaunchProperty(object _0) => throw new NotImplementedException();
		public virtual object GetApplicationLaunchPropertyWithApplicationId(object _0) => throw new NotImplementedException();
		public virtual void GetApplicationControlProperty(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void GetApplicationControlPropertyWithApplicationId(object _0, Buffer<byte> _1) => throw new NotImplementedException();
	}
	
	public unsafe partial class IWriter : _Base_IWriter {}
	public class _Base_IWriter : IpcInterface {
		static readonly Logger Logger = new("IWriter");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // AcquireRegistrar
					var ret = AcquireRegistrar();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 1: { // DeleteProperties
					DeleteProperties(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IWriter: {im.CommandId}");
			}
		}
		
		public virtual Nn.Arp.Detail.IRegistrar AcquireRegistrar() => throw new NotImplementedException();
		public virtual void DeleteProperties(object _0) => "Stub hit for Nn.Arp.Detail.IWriter.DeleteProperties [1]".Debug(Log);
	}
	
	public unsafe partial class IRegistrar : _Base_IRegistrar {}
	public class _Base_IRegistrar : IpcInterface {
		static readonly Logger Logger = new("IRegistrar");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Issue
					Issue(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // SetApplicationLaunchProperty
					SetApplicationLaunchProperty(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // SetApplicationControlProperty
					SetApplicationControlProperty(im.GetBuffer<byte>(0x15, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IRegistrar: {im.CommandId}");
			}
		}
		
		public virtual void Issue(object _0) => "Stub hit for Nn.Arp.Detail.IRegistrar.Issue [0]".Debug(Log);
		public virtual void SetApplicationLaunchProperty(object _0) => "Stub hit for Nn.Arp.Detail.IRegistrar.SetApplicationLaunchProperty [1]".Debug(Log);
		public virtual void SetApplicationControlProperty(Buffer<byte> _0) => "Stub hit for Nn.Arp.Detail.IRegistrar.SetApplicationControlProperty [2]".Debug(Log);
	}
}
