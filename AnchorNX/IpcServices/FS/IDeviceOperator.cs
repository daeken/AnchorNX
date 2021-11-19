using System;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IDeviceOperator {
		public override byte IsSdCardInserted() => 0;
		public override ulong GetSdCardSpeedMode() => 0;
		public override void GetSdCardCid(ulong _0, Buffer<byte> cid) => cid.CopyFrom(new byte[cid.Length]);
		public override ulong GetSdCardUserAreaSize() => 0;
		public override ulong GetSdCardProtectedAreaSize() => 0;
		public override void GetAndClearSdCardErrorInfo(ulong _0, out byte[] _1, out ulong _2, Buffer<byte> ei) {
			_1 = Array.Empty<byte>();
			_2 = 0;
			ei.CopyFrom(new byte[ei.Length]);
		}

		public override void GetMmcCid(ulong _0, Buffer<byte> cid) => cid.CopyFrom(new byte[cid.Length]);

		public override ulong GetMmcSpeedMode() => 0;
		public override void EraseMmc(uint _0) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.EraseMmc [110]".Debug(Log);
		public override ulong GetMmcPartitionSize(uint _0) => throw new NotImplementedException();
		public override uint GetMmcPatrolCount() => 0;
		public override void GetAndClearMmcErrorInfo(ulong _0, out byte[] _1, out ulong _2, Buffer<byte> ei) {
			_1 = Array.Empty<byte>();
			_2 = 0;
			ei.CopyFrom(new byte[ei.Length]);
		}

		public override void GetMmcExtendedCsd(ulong _0, Buffer<byte> csd) => csd.CopyFrom(new byte[csd.Length]);
		public override void SuspendMmcPatrol() => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.SuspendMmcPatrol [115]".Debug(Log);
		public override void ResumeMmcPatrol() => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.ResumeMmcPatrol [116]".Debug(Log);
		public override byte IsGameCardInserted() => 0;
		public override void EraseGameCard(uint _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.EraseGameCard [201]".Debug(Log);
		public override uint GetGameCardHandle() => throw new NotImplementedException();
		public override void GetGameCardUpdatePartitionInfo(uint _0, out uint version, out ulong tid) => throw new NotImplementedException();
		public override void FinalizeGameCardDriver() => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.FinalizeGameCardDriver [204]".Debug(Log);
		public override byte GetGameCardAttribute(uint _0) => throw new NotImplementedException();
		public override void GetGameCardDeviceCertificate(uint _0, ulong _1, Buffer<byte> certificate) => throw new NotImplementedException();
		public override void GetGameCardAsicInfo(ulong _0, ulong _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public override void GetGameCardIdSet(ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override void WriteToGameCard(ulong _0, ulong _1, Buffer<byte> _2) => throw new NotImplementedException();
		public override void SetVerifyWriteEnalbleFlag(byte flag) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.SetVerifyWriteEnalbleFlag [210]".Debug(Log);
		public override void GetGameCardImageHash(uint _0, ulong _1, Buffer<byte> image_hash) => throw new NotImplementedException();
		public override void GetGameCardErrorInfo(ulong _0, ulong _1, Buffer<byte> _2, Buffer<byte> error_info) => throw new NotImplementedException();
		public override void EraseAndWriteParamDirectly(ulong _0, Buffer<byte> _1) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.EraseAndWriteParamDirectly [213]".Debug(Log);
		public override void ReadParamDirectly(ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override void ForceEraseGameCard() => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.ForceEraseGameCard [215]".Debug(Log);
		public override void GetGameCardErrorInfo2(out byte[] error_info) => throw new NotImplementedException();
		public override void GetGameCardErrorReportInfo(out byte[] error_report_info) => error_report_info = Array.Empty<byte>();
		public override void GetGameCardDeviceId(ulong _0, Buffer<byte> device_id) => throw new NotImplementedException();
		public override void SetSpeedEmulationMode(uint emu_mode) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.SetSpeedEmulationMode [300]".Debug(Log);
		public override uint GetSpeedEmulationMode() => throw new NotImplementedException();
		public override object SuspendSdmmcControl(object _0) => throw new NotImplementedException();
		public override object ResumeSdmmcControl(object _0) => throw new NotImplementedException();
	}
}