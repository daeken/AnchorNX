#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Xcd.Detail {
	public unsafe partial class ISystemServer : _Base_ISystemServer {}
	public class _Base_ISystemServer : IpcInterface {
		static readonly Logger Logger = new("ISystemServer");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetDataFormat
					var ret = GetDataFormat(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // SetDataFormat
					SetDataFormat(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetMcuState
					var ret = GetMcuState(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // SetMcuState
					SetMcuState(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // GetMcuVersionForNfc
					var ret = GetMcuVersionForNfc(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // CheckNfcDevicePower
					CheckNfcDevicePower(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // SetNfcEvent
					SetNfcEvent(null, out var _0, out var _1);
					om.Initialize(0, 2, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Copy(1, await CreateHandle(_1, copy: true));
					break;
				}
				case 11: { // GetNfcInfo
					GetNfcInfo(null, im.GetBuffer<byte>(0x1A, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // StartNfcDiscovery
					StartNfcDiscovery(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 13: { // StopNfcDiscovery
					StopNfcDiscovery(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 14: { // StartNtagRead
					StartNtagRead(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 15: { // StartNtagWrite
					StartNtagWrite(null, im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 16: { // SendNfcRawData
					SendNfcRawData(null, im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 17: { // RegisterMifareKey
					RegisterMifareKey(null, im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 18: { // ClearMifareKey
					ClearMifareKey(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 19: { // StartMifareRead
					StartMifareRead(null, im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 20: { // StartMifareWrite
					StartMifareWrite(null, im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 101: { // GetAwakeTriggerReasonForLeftRail
					var ret = GetAwakeTriggerReasonForLeftRail();
					om.Initialize(0, 0, 0);
					break;
				}
				case 102: { // GetAwakeTriggerReasonForRightRail
					var ret = GetAwakeTriggerReasonForRightRail();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISystemServer: {im.CommandId}");
			}
		}
		
		public virtual object GetDataFormat(object _0) => throw new NotImplementedException();
		public virtual void SetDataFormat(object _0) => "Stub hit for Nn.Xcd.Detail.ISystemServer.SetDataFormat [1]".Debug(Log);
		public virtual object GetMcuState(object _0) => throw new NotImplementedException();
		public virtual void SetMcuState(object _0) => "Stub hit for Nn.Xcd.Detail.ISystemServer.SetMcuState [3]".Debug(Log);
		public virtual object GetMcuVersionForNfc(object _0) => throw new NotImplementedException();
		public virtual void CheckNfcDevicePower(object _0) => "Stub hit for Nn.Xcd.Detail.ISystemServer.CheckNfcDevicePower [5]".Debug(Log);
		public virtual void SetNfcEvent(object _0, out uint _1, out uint _2) => throw new NotImplementedException();
		public virtual void GetNfcInfo(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void StartNfcDiscovery(object _0) => "Stub hit for Nn.Xcd.Detail.ISystemServer.StartNfcDiscovery [12]".Debug(Log);
		public virtual void StopNfcDiscovery(object _0) => "Stub hit for Nn.Xcd.Detail.ISystemServer.StopNfcDiscovery [13]".Debug(Log);
		public virtual void StartNtagRead(object _0) => "Stub hit for Nn.Xcd.Detail.ISystemServer.StartNtagRead [14]".Debug(Log);
		public virtual void StartNtagWrite(object _0, Buffer<byte> _1) => "Stub hit for Nn.Xcd.Detail.ISystemServer.StartNtagWrite [15]".Debug(Log);
		public virtual void SendNfcRawData(object _0, Buffer<byte> _1) => "Stub hit for Nn.Xcd.Detail.ISystemServer.SendNfcRawData [16]".Debug(Log);
		public virtual void RegisterMifareKey(object _0, Buffer<byte> _1) => "Stub hit for Nn.Xcd.Detail.ISystemServer.RegisterMifareKey [17]".Debug(Log);
		public virtual void ClearMifareKey(object _0) => "Stub hit for Nn.Xcd.Detail.ISystemServer.ClearMifareKey [18]".Debug(Log);
		public virtual void StartMifareRead(object _0, Buffer<byte> _1) => "Stub hit for Nn.Xcd.Detail.ISystemServer.StartMifareRead [19]".Debug(Log);
		public virtual void StartMifareWrite(object _0, Buffer<byte> _1) => "Stub hit for Nn.Xcd.Detail.ISystemServer.StartMifareWrite [20]".Debug(Log);
		public virtual object GetAwakeTriggerReasonForLeftRail() => throw new NotImplementedException();
		public virtual object GetAwakeTriggerReasonForRightRail() => throw new NotImplementedException();
	}
}
