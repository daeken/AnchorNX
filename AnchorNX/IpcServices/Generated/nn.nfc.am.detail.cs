#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Nfc.Am.Detail {
	public unsafe partial class IAmManager : _Base_IAmManager {}
	public class _Base_IAmManager : IpcInterface {
		static readonly Logger Logger = new("IAmManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // CreateAmInterface
					var ret = CreateAmInterface();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAmManager: {im.CommandId}");
			}
		}
		
		public virtual Nn.Nfc.Am.Detail.IAm CreateAmInterface() => throw new NotImplementedException();
	}
	
	public unsafe partial class IAm : _Base_IAm {}
	public class _Base_IAm : IpcInterface {
		static readonly Logger Logger = new("IAm");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Initialize
					Initialize();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Finalize
					Finalize();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // NotifyForegroundApplet
					NotifyForegroundApplet(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAm: {im.CommandId}");
			}
		}
		
		public virtual void Initialize() => "Stub hit for Nn.Nfc.Am.Detail.IAm.Initialize [0]".Debug(Log);
		public virtual void Finalize() => "Stub hit for Nn.Nfc.Am.Detail.IAm.Finalize [1]".Debug(Log);
		public virtual void NotifyForegroundApplet(object _0) => "Stub hit for Nn.Nfc.Am.Detail.IAm.NotifyForegroundApplet [2]".Debug(Log);
	}
}
