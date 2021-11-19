using System;

namespace AnchorNX.IpcServices.Nn.Wlan.Detail {
	public partial class IInfraManager {
		public static HosEvent Event;

		public override void Unknown0() => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown0 [0]".Debug(Log);
		public override void Unknown1() => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown1 [1]".Debug(Log);
		public override ulong GetMacAddress() => 0xDEADBEEFCAFE;
		public override void StartScan(Buffer<byte> _0) => "Stub hit for Nn.Wlan.Detail.IInfraManager.StartScan [3]".Debug(Log);
		public override void StopScan() => "Stub hit for Nn.Wlan.Detail.IInfraManager.StopScan [4]".Debug(Log);
		public override void Connect(object _0) => "Stub hit for Nn.Wlan.Detail.IInfraManager.Connect [5]".Debug(Log);
		public override void CancelConnect() => "Stub hit for Nn.Wlan.Detail.IInfraManager.CancelConnect [6]".Debug(Log);
		public override void Disconnect() => "Stub hit for Nn.Wlan.Detail.IInfraManager.Disconnect [7]".Debug(Log);
		public override uint GetConnectionEvent(object _0) {
			Event ??= Box.EventManager.GetEvent();
			return Event.Reader;
		}

		public override object Unknown9() => throw new NotImplementedException();
		public override object GetState() => throw new NotImplementedException();
		public override void GetScanResult(Buffer<byte> _0) => throw new NotImplementedException();
		public override object GetRssi() => throw new NotImplementedException();
		public override void ChangeRxAntenna(object _0) => "Stub hit for Nn.Wlan.Detail.IInfraManager.ChangeRxAntenna [13]".Debug(Log);
		public override void Unknown14(Buffer<byte> _0) => throw new NotImplementedException();
		public override void Unknown15() => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown15 [15]".Debug(Log);
		public override void RequestWakeUp() => "Stub hit for Nn.Wlan.Detail.IInfraManager.RequestWakeUp [16]".Debug(Log);
		public override void RequestIfUpDown(object _0, Buffer<byte> _1) => "Stub hit for Nn.Wlan.Detail.IInfraManager.RequestIfUpDown [17]".Debug(Log);
		public override object Unknown18() => throw new NotImplementedException();
		public override void Unknown19(object _0) => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown19 [19]".Debug(Log);
		public override void Unknown20() => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown20 [20]".Debug(Log);
		public override object Unknown21() => throw new NotImplementedException();
		public override void Unknown22(object _0) => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown22 [22]".Debug(Log);
		public override void Unknown23(object _0) => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown23 [23]".Debug(Log);
		public override object Unknown24() => throw new NotImplementedException();
		public override void Unknown25(object _0) => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown25 [25]".Debug(Log);
		public override void Unknown26() => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown26 [26]".Debug(Log);
		public override void Unknown27() => "Stub hit for Nn.Wlan.Detail.IInfraManager.Unknown27 [27]".Debug(Log);
	}
}