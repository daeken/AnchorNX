using System;
using System.Threading.Tasks;

namespace AnchorNX.IpcServices.Nn.Usb {
	public class IObserve : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: {
					Log("Stub for IObserve cmd 0");
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: {
					Log("Stub for IObserve cmd 1");
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IObserve: {im.CommandId}");
			}
		}
	}
}