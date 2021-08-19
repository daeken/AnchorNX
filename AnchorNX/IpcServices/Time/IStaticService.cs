using System;

namespace AnchorNX.IpcServices.Nn.Timesrv.Detail.Service {
	public partial class IStaticService {
		public override ISystemClock GetStandardUserSystemClock() => new();
		public override ISystemClock GetStandardNetworkSystemClock() => new();
		public override ISteadyClock GetStandardSteadyClock() => new();
		public override ITimeZoneService GetTimeZoneService() => new();
		public override ISystemClock GetStandardLocalSystemClock() => new();
		public override ISystemClock GetEphemeralNetworkSystemClock() => new();
		public override void SetStandardSteadyClockInternalOffset(ulong _0) => "Stub hit for Nn.Timesrv.Detail.Service.IStaticService.SetStandardSteadyClockInternalOffset [50]".Debug(Log);
		public override byte IsStandardUserSystemClockAutomaticCorrectionEnabled() => throw new NotImplementedException();
		public override void SetStandardUserSystemClockAutomaticCorrectionEnabled(byte _0) => "Stub hit for Nn.Timesrv.Detail.Service.IStaticService.SetStandardUserSystemClockAutomaticCorrectionEnabled [101]".Debug(Log);
		public override object GetStandardUserSystemClockInitialYear(object _0) => throw new NotImplementedException();
		public override byte IsStandardNetworkSystemClockAccuracySufficient() => throw new NotImplementedException();
		public override ulong CalculateMonotonicSystemClockBaseTimePoint(byte[] _0) => throw new NotImplementedException();
		public override void GetClockSnapshot(byte _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override void GetClockSnapshotFromSystemClockContext(byte _0, byte[] _1, byte[] _2, Buffer<byte> _3) => throw new NotImplementedException();
		public override ulong CalculateStandardUserSystemClockDifferenceByUser(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override ulong CalculateSpanBetween(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override uint GetSharedMemoryNativeHandle() => 0;
	}
}