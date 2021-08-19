#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Jitsrv {
	public unsafe partial class IJitService : _Base_IJitService {}
	public class _Base_IJitService : IpcInterface {
		static readonly Logger Logger = new("IJitService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // CreateJitEnvironment
					var ret = CreateJitEnvironment(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IJitService: {im.CommandId}");
			}
		}
		
		public virtual object CreateJitEnvironment(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IJitEnvironment : _Base_IJitEnvironment {}
	public class _Base_IJitEnvironment : IpcInterface {
		static readonly Logger Logger = new("IJitEnvironment");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Control
					var ret = Control(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // GenerateCode
					var ret = GenerateCode(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1000: { // LoadPlugin
					var ret = LoadPlugin(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1001: { // GetCodeAddress
					var ret = GetCodeAddress(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IJitEnvironment: {im.CommandId}");
			}
		}
		
		public virtual object Control(object _0) => throw new NotImplementedException();
		public virtual object GenerateCode(object _0) => throw new NotImplementedException();
		public virtual object LoadPlugin(object _0) => throw new NotImplementedException();
		public virtual object GetCodeAddress(object _0) => throw new NotImplementedException();
	}
}
