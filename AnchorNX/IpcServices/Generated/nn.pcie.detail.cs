#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Pcie.Detail {
	public unsafe partial class IManager : _Base_IManager {}
	public class _Base_IManager : IpcInterface {
		static readonly Logger Logger = new("IManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RegisterClassDriver
					RegisterClassDriver(null, im.GetCopy(0), out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 1: { // QueryFunctionsUnregistered
					QueryFunctionsUnregistered(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IManager: {im.CommandId}");
			}
		}
		
		public virtual void RegisterClassDriver(object _0, uint _1, out uint _2, out Nn.Pcie.Detail.ISession _3) => throw new NotImplementedException();
		public virtual void QueryFunctionsUnregistered(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
	}
	
	public unsafe partial class ISession : _Base_ISession {}
	public class _Base_ISession : IpcInterface {
		static readonly Logger Logger = new("ISession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // QueryFunctions
					QueryFunctions(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // AcquireFunction
					var ret = AcquireFunction(null);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 2: { // ReleaseFunction
					ReleaseFunction(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // GetFunctionState
					GetFunctionState(null, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // GetBarProfile
					var ret = GetBarProfile(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // ReadConfig
					var ret = ReadConfig(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // WriteConfig
					WriteConfig(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // ReadBarRegion
					ReadBarRegion(null, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // WriteBarRegion
					WriteBarRegion(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // FindCapability
					var ret = FindCapability(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // FindExtendedCapability
					var ret = FindExtendedCapability(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // MapDma
					var ret = MapDma(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // UnmapDma
					UnmapDma(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 13: { // UnmapDmaBusAddress
					UnmapDmaBusAddress(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 14: { // GetDmaBusAddress
					var ret = GetDmaBusAddress(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 15: { // GetDmaBusAddressRange
					var ret = GetDmaBusAddressRange(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 16: { // SetDmaEnable
					SetDmaEnable(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 17: { // AcquireIrq
					var ret = AcquireIrq(null);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 18: { // ReleaseIrq
					ReleaseIrq(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 19: { // SetIrqEnable
					SetIrqEnable(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 20: { // SetAspmEnable
					SetAspmEnable(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISession: {im.CommandId}");
			}
		}
		
		public virtual void QueryFunctions(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual uint AcquireFunction(object _0) => throw new NotImplementedException();
		public virtual void ReleaseFunction(object _0) => "Stub hit for Nn.Pcie.Detail.ISession.ReleaseFunction [2]".Debug(Log);
		public virtual void GetFunctionState(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual object GetBarProfile(object _0) => throw new NotImplementedException();
		public virtual object ReadConfig(object _0) => throw new NotImplementedException();
		public virtual void WriteConfig(object _0) => "Stub hit for Nn.Pcie.Detail.ISession.WriteConfig [6]".Debug(Log);
		public virtual void ReadBarRegion(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void WriteBarRegion(object _0, Buffer<byte> _1) => "Stub hit for Nn.Pcie.Detail.ISession.WriteBarRegion [8]".Debug(Log);
		public virtual object FindCapability(object _0) => throw new NotImplementedException();
		public virtual object FindExtendedCapability(object _0) => throw new NotImplementedException();
		public virtual object MapDma(object _0) => throw new NotImplementedException();
		public virtual void UnmapDma(object _0) => "Stub hit for Nn.Pcie.Detail.ISession.UnmapDma [12]".Debug(Log);
		public virtual void UnmapDmaBusAddress(object _0) => "Stub hit for Nn.Pcie.Detail.ISession.UnmapDmaBusAddress [13]".Debug(Log);
		public virtual object GetDmaBusAddress(object _0) => throw new NotImplementedException();
		public virtual object GetDmaBusAddressRange(object _0) => throw new NotImplementedException();
		public virtual void SetDmaEnable(object _0) => "Stub hit for Nn.Pcie.Detail.ISession.SetDmaEnable [16]".Debug(Log);
		public virtual uint AcquireIrq(object _0) => throw new NotImplementedException();
		public virtual void ReleaseIrq(object _0) => "Stub hit for Nn.Pcie.Detail.ISession.ReleaseIrq [18]".Debug(Log);
		public virtual void SetIrqEnable(object _0) => "Stub hit for Nn.Pcie.Detail.ISession.SetIrqEnable [19]".Debug(Log);
		public virtual void SetAspmEnable(object _0) => "Stub hit for Nn.Pcie.Detail.ISession.SetAspmEnable [20]".Debug(Log);
	}
}
