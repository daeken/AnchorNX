using System;
using System.Threading.Tasks;

namespace AnchorNX.IpcServices.Time {
	public class IRtc : IpcInterface {
		static readonly Logger Logger = new("IAdministrator");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetRtcTime
					var ret = GetRtcTime(im.GetData<uint>(8));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 3: { // GetRtcResetDetected
					var ret = GetRtcResetDetected(im.GetData<uint>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IRtc: {im.CommandId}");
			}
		}

		public virtual ulong GetRtcTime(uint deviceCode) {
			Log($"GetRtcTime stub, deviceCode 0x{deviceCode:X}");
			return 0xDEADBEEF;
		}

		public virtual bool GetRtcResetDetected(uint deviceCode) {
			Log($"GetRtcResetDetected stub, deviceCode 0x{deviceCode:X}");
			return false;
		}
	}
}