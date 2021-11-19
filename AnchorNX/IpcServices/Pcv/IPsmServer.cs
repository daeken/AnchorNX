using System;

namespace AnchorNX.IpcServices.Nn.Psm {
	public partial class IPsmServer {
		public override object GetBatteryChargePercentage() => throw new NotImplementedException();
		public override object GetChargerType() => throw new NotImplementedException();
		public override void EnableBatteryCharging() => "Stub hit for Nn.Psm.IPsmServer.EnableBatteryCharging [2]".Debug(Log);
		public override void DisableBatteryCharging() => "Stub hit for Nn.Psm.IPsmServer.DisableBatteryCharging [3]".Debug(Log);
		public override object IsBatteryChargingEnabled() => throw new NotImplementedException();
		public override void AcquireControllerPowerSupply() => "Stub hit for Nn.Psm.IPsmServer.AcquireControllerPowerSupply [5]".Debug(Log);
		public override void ReleaseControllerPowerSupply() => "Stub hit for Nn.Psm.IPsmServer.ReleaseControllerPowerSupply [6]".Debug(Log);
		public override Nn.Psm.IPsmSession OpenSession() => throw new NotImplementedException();
		public override void EnableEnoughPowerChargeEmulation() => "Stub hit for Nn.Psm.IPsmServer.EnableEnoughPowerChargeEmulation [8]".Debug(Log);
		public override void DisableEnoughPowerChargeEmulation() => "Stub hit for Nn.Psm.IPsmServer.DisableEnoughPowerChargeEmulation [9]".Debug(Log);
		public override void EnableFastBatteryCharging() => "Stub hit for Nn.Psm.IPsmServer.EnableFastBatteryCharging [10]".Debug(Log);
		public override void DisableFastBatteryCharging() => "Stub hit for Nn.Psm.IPsmServer.DisableFastBatteryCharging [11]".Debug(Log);
		public override uint GetBatteryVoltageState() => 3;
		public override object GetRawBatteryChargePercentage() => throw new NotImplementedException();
		public override object IsEnoughPowerSupplied() => throw new NotImplementedException();
		public override object GetBatteryAgePercentage() => throw new NotImplementedException();
		public override uint GetBatteryChargeInfoEvent() => Box.EventManager.GetEvent().Reader;
		public override object GetBatteryChargeInfoFields() => throw new NotImplementedException();
		public override uint GetBatteryChargeCalibratedEvent() => Box.EventManager.GetEvent().Reader;
	}
}