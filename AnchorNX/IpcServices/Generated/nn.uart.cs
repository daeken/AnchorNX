#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Uart {
	public unsafe partial class IManager : _Base_IManager {}
	public class _Base_IManager : IpcInterface {
		static readonly Logger Logger = new("IManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // DoesUartExist
					var ret = DoesUartExist(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // DoesUartExistForTest
					var ret = DoesUartExistForTest(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // SetUartBaudrate
					var ret = SetUartBaudrate(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // SetUartBaudrateForTest
					var ret = SetUartBaudrateForTest(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // IsSomethingUartValid
					var ret = IsSomethingUartValid(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // IsSomethingUartValidForTest
					var ret = IsSomethingUartValidForTest(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // GetSession
					var ret = GetSession();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 7: { // IsSomethingUartValid2
					var ret = IsSomethingUartValid2(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // IsSomethingUartValid2ForTest
					var ret = IsSomethingUartValid2ForTest(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IManager: {im.CommandId}");
			}
		}
		
		public virtual object DoesUartExist(object _0) => throw new NotImplementedException();
		public virtual object DoesUartExistForTest(object _0) => throw new NotImplementedException();
		public virtual object SetUartBaudrate(object _0) => throw new NotImplementedException();
		public virtual object SetUartBaudrateForTest(object _0) => throw new NotImplementedException();
		public virtual object IsSomethingUartValid(object _0) => throw new NotImplementedException();
		public virtual object IsSomethingUartValidForTest(object _0) => throw new NotImplementedException();
		public virtual Nn.Uart.IPortSession GetSession() => throw new NotImplementedException();
		public virtual object IsSomethingUartValid2(object _0) => throw new NotImplementedException();
		public virtual object IsSomethingUartValid2ForTest(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IPortSession : _Base_IPortSession {}
	public class _Base_IPortSession : IpcInterface {
		static readonly Logger Logger = new("IPortSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenSession
					var ret = OpenSession(null, im.GetCopy(0), im.GetCopy(1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // OpenSessionForTest
					var ret = OpenSessionForTest(null, im.GetCopy(0), im.GetCopy(1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					var ret = Unknown2();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					var ret = Unknown3(im.GetBuffer<byte>(0x21, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					var ret = Unknown4();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					Unknown5(out var _0, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Unknown6
					Unknown6(null, out var _0, out var _1);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(_1, copy: true));
					break;
				}
				case 7: { // Unknown7
					var ret = Unknown7(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IPortSession: {im.CommandId}");
			}
		}
		
		public virtual object OpenSession(object _0, uint _1, uint _2) => throw new NotImplementedException();
		public virtual object OpenSessionForTest(object _0, uint _1, uint _2) => throw new NotImplementedException();
		public virtual object Unknown2() => throw new NotImplementedException();
		public virtual object Unknown3(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual object Unknown4() => throw new NotImplementedException();
		public virtual void Unknown5(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown6(object _0, out object _1, out uint _2) => throw new NotImplementedException();
		public virtual object Unknown7(object _0) => throw new NotImplementedException();
	}
}
