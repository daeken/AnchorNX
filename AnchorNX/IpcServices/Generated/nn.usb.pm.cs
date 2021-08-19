#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Usb.Pm {
	public unsafe partial class IPmService : _Base_IPmService {}
	public class _Base_IPmService : IpcInterface {
		static readonly Logger Logger = new("IPmService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 1: { // Unknown1
					Unknown1(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					var ret = Unknown2();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 3: { // Unknown3
					var ret = Unknown3();
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					Unknown4(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					var ret = Unknown5(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IPmService: {im.CommandId}");
			}
		}
		
		public virtual uint Unknown0() => throw new NotImplementedException();
		public virtual void Unknown1(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual uint Unknown2() => throw new NotImplementedException();
		public virtual object Unknown3() => throw new NotImplementedException();
		public virtual void Unknown4(object _0) => "Stub hit for Nn.Usb.Pm.IPmService.Unknown4 [4]".Debug(Log);
		public virtual object Unknown5(object _0) => throw new NotImplementedException();
	}
}
