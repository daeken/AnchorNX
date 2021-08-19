#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Timesrv.Detail.Service {
	public unsafe partial class IStaticService : _Base_IStaticService {}
	public class _Base_IStaticService : IpcInterface {
		static readonly Logger Logger = new("IStaticService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetStandardUserSystemClock
					var ret = GetStandardUserSystemClock();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 1: { // GetStandardNetworkSystemClock
					var ret = GetStandardNetworkSystemClock();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 2: { // GetStandardSteadyClock
					var ret = GetStandardSteadyClock();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 3: { // GetTimeZoneService
					var ret = GetTimeZoneService();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 4: { // GetStandardLocalSystemClock
					var ret = GetStandardLocalSystemClock();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 5: { // GetEphemeralNetworkSystemClock
					var ret = GetEphemeralNetworkSystemClock();
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 20: { // GetSharedMemoryNativeHandle
					var ret = GetSharedMemoryNativeHandle();
					om.Initialize(0, 1, 0);
					om.Copy(0, ret);
					break;
				}
				case 50: { // SetStandardSteadyClockInternalOffset
					SetStandardSteadyClockInternalOffset(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 100: { // IsStandardUserSystemClockAutomaticCorrectionEnabled
					var ret = IsStandardUserSystemClockAutomaticCorrectionEnabled();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 101: { // SetStandardUserSystemClockAutomaticCorrectionEnabled
					SetStandardUserSystemClockAutomaticCorrectionEnabled(im.GetData<byte>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 102: { // GetStandardUserSystemClockInitialYear
					var ret = GetStandardUserSystemClockInitialYear(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 200: { // IsStandardNetworkSystemClockAccuracySufficient
					var ret = IsStandardNetworkSystemClockAccuracySufficient();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 300: { // CalculateMonotonicSystemClockBaseTimePoint
					var ret = CalculateMonotonicSystemClockBaseTimePoint(im.GetBytes(8, 0x20));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 400: { // GetClockSnapshot
					GetClockSnapshot(im.GetData<byte>(8), im.GetBuffer<byte>(0x1A, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 401: { // GetClockSnapshotFromSystemClockContext
					GetClockSnapshotFromSystemClockContext(im.GetData<byte>(8), im.GetBytes(9, 0x20), im.GetBytes(41, 0x20), im.GetBuffer<byte>(0x1A, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 500: { // CalculateStandardUserSystemClockDifferenceByUser
					var ret = CalculateStandardUserSystemClockDifferenceByUser(im.GetBuffer<byte>(0x19, 0), im.GetBuffer<byte>(0x19, 1));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 501: { // CalculateSpanBetween
					var ret = CalculateSpanBetween(im.GetBuffer<byte>(0x19, 0), im.GetBuffer<byte>(0x19, 1));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IStaticService: {im.CommandId}");
			}
		}
		
		public virtual Nn.Timesrv.Detail.Service.ISystemClock GetStandardUserSystemClock() => throw new NotImplementedException();
		public virtual Nn.Timesrv.Detail.Service.ISystemClock GetStandardNetworkSystemClock() => throw new NotImplementedException();
		public virtual Nn.Timesrv.Detail.Service.ISteadyClock GetStandardSteadyClock() => throw new NotImplementedException();
		public virtual Nn.Timesrv.Detail.Service.ITimeZoneService GetTimeZoneService() => throw new NotImplementedException();
		public virtual Nn.Timesrv.Detail.Service.ISystemClock GetStandardLocalSystemClock() => throw new NotImplementedException();
		public virtual Nn.Timesrv.Detail.Service.ISystemClock GetEphemeralNetworkSystemClock() => throw new NotImplementedException();
		public virtual void SetStandardSteadyClockInternalOffset(ulong _0) => "Stub hit for Nn.Timesrv.Detail.Service.IStaticService.SetStandardSteadyClockInternalOffset [50]".Debug(Log);
		public virtual byte IsStandardUserSystemClockAutomaticCorrectionEnabled() => throw new NotImplementedException();
		public virtual void SetStandardUserSystemClockAutomaticCorrectionEnabled(byte _0) => "Stub hit for Nn.Timesrv.Detail.Service.IStaticService.SetStandardUserSystemClockAutomaticCorrectionEnabled [101]".Debug(Log);
		public virtual object GetStandardUserSystemClockInitialYear(object _0) => throw new NotImplementedException();
		public virtual byte IsStandardNetworkSystemClockAccuracySufficient() => throw new NotImplementedException();
		public virtual ulong CalculateMonotonicSystemClockBaseTimePoint(byte[] _0) => throw new NotImplementedException();
		public virtual void GetClockSnapshot(byte _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void GetClockSnapshotFromSystemClockContext(byte _0, byte[] _1, byte[] _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual ulong CalculateStandardUserSystemClockDifferenceByUser(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual ulong CalculateSpanBetween(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual uint GetSharedMemoryNativeHandle() => throw new NotImplementedException();
	}
	
	public unsafe partial class ISteadyClock : _Base_ISteadyClock {}
	public class _Base_ISteadyClock : IpcInterface {
		static readonly Logger Logger = new("ISteadyClock");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetCurrentTimePoint
					GetCurrentTimePoint(out var _0);
					om.Initialize(0, 0, 24);
					om.SetBytes(8, _0);
					break;
				}
				case 2: { // GetTestOffset
					var ret = GetTestOffset();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 3: { // SetTestOffset
					SetTestOffset(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 100: { // GetRtcValue
					var ret = GetRtcValue();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 101: { // IsRtcResetDetected
					var ret = IsRtcResetDetected();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 102: { // GetSetupResultValue
					var ret = GetSetupResultValue();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 200: { // GetInternalOffset
					var ret = GetInternalOffset();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 201: { // SetInternalOffset
					SetInternalOffset(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISteadyClock: {im.CommandId}");
			}
		}
		
		public virtual void GetCurrentTimePoint(out byte[] _0) => throw new NotImplementedException();
		public virtual ulong GetTestOffset() => throw new NotImplementedException();
		public virtual void SetTestOffset(ulong _0) => "Stub hit for Nn.Timesrv.Detail.Service.ISteadyClock.SetTestOffset [3]".Debug(Log);
		public virtual ulong GetRtcValue() => throw new NotImplementedException();
		public virtual byte IsRtcResetDetected() => throw new NotImplementedException();
		public virtual uint GetSetupResultValue() => throw new NotImplementedException();
		public virtual ulong GetInternalOffset() => throw new NotImplementedException();
		public virtual void SetInternalOffset(ulong _0) => "Stub hit for Nn.Timesrv.Detail.Service.ISteadyClock.SetInternalOffset [201]".Debug(Log);
	}
	
	public unsafe partial class ISystemClock : _Base_ISystemClock {}
	public class _Base_ISystemClock : IpcInterface {
		static readonly Logger Logger = new("ISystemClock");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetCurrentTime
					var ret = GetCurrentTime();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 1: { // SetCurrentTime
					SetCurrentTime(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetSystemClockContext
					GetSystemClockContext(out var _0);
					om.Initialize(0, 0, 32);
					om.SetBytes(8, _0);
					break;
				}
				case 3: { // SetSystemClockContext
					SetSystemClockContext(im.GetBytes(8, 0x20));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISystemClock: {im.CommandId}");
			}
		}
		
		public virtual ulong GetCurrentTime() => throw new NotImplementedException();
		public virtual void SetCurrentTime(ulong _0) => "Stub hit for Nn.Timesrv.Detail.Service.ISystemClock.SetCurrentTime [1]".Debug(Log);
		public virtual void GetSystemClockContext(out byte[] _0) => throw new NotImplementedException();
		public virtual void SetSystemClockContext(byte[] _0) => "Stub hit for Nn.Timesrv.Detail.Service.ISystemClock.SetSystemClockContext [3]".Debug(Log);
	}
	
	public unsafe partial class ITimeZoneService : _Base_ITimeZoneService {}
	public class _Base_ITimeZoneService : IpcInterface {
		static readonly Logger Logger = new("ITimeZoneService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetDeviceLocationName
					GetDeviceLocationName(out var _0);
					om.Initialize(0, 0, 36);
					om.SetBytes(8, _0);
					break;
				}
				case 1: { // SetDeviceLocationName
					SetDeviceLocationName(im.GetBytes(8, 0x24));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetTotalLocationNameCount
					var ret = GetTotalLocationNameCount();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 3: { // LoadLocationNameList
					LoadLocationNameList(im.GetData<uint>(8), out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 4: { // LoadTimeZoneRule
					LoadTimeZoneRule(im.GetBytes(8, 0x24), im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // GetTimeZoneRuleVersion
					GetTimeZoneRuleVersion(out var _0);
					om.Initialize(0, 0, 16);
					om.SetBytes(8, _0);
					break;
				}
				case 100: { // ToCalendarTime
					ToCalendarTime(im.GetData<ulong>(8), im.GetBuffer<byte>(0x15, 0), om.GetDataSpan<Nn.Time.CalendarTime>(8), om.GetDataSpan<Nn.Time.Sf.CalendarAdditionalInfo>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 101: { // ToCalendarTimeWithMyRule
					ToCalendarTimeWithMyRule(im.GetData<ulong>(8), om.GetDataSpan<Nn.Time.CalendarTime>(8), om.GetDataSpan<Nn.Time.Sf.CalendarAdditionalInfo>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 201: { // ToPosixTime
					ToPosixTime(im.GetDataSpan<Nn.Time.CalendarTime>(8), im.GetBuffer<byte>(0x15, 0), out var _0, im.GetBuffer<ulong>(0xA, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 202: { // ToPosixTimeWithMyRule
					ToPosixTimeWithMyRule(im.GetDataSpan<Nn.Time.CalendarTime>(8), out var _0, im.GetBuffer<ulong>(0xA, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ITimeZoneService: {im.CommandId}");
			}
		}
		
		public virtual void GetDeviceLocationName(out byte[] _0) => throw new NotImplementedException();
		public virtual void SetDeviceLocationName(byte[] _0) => "Stub hit for Nn.Timesrv.Detail.Service.ITimeZoneService.SetDeviceLocationName [1]".Debug(Log);
		public virtual uint GetTotalLocationNameCount() => throw new NotImplementedException();
		public virtual void LoadLocationNameList(uint _0, out uint _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void LoadTimeZoneRule(byte[] _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void GetTimeZoneRuleVersion(out byte[] _0) => throw new NotImplementedException();
		public virtual void ToCalendarTime(ulong _0, Buffer<byte> _1, Span< Nn.Time.CalendarTime> _2, Span< Nn.Time.Sf.CalendarAdditionalInfo> _3) => throw new NotImplementedException();
		public virtual void ToCalendarTimeWithMyRule(ulong _0, Span< Nn.Time.CalendarTime> _1, Span< Nn.Time.Sf.CalendarAdditionalInfo> _2) => throw new NotImplementedException();
		public virtual void ToPosixTime(Span<Nn.Time.CalendarTime> _0, Buffer<byte> _1, out uint _2, Buffer<ulong> _3) => throw new NotImplementedException();
		public virtual void ToPosixTimeWithMyRule(Span<Nn.Time.CalendarTime> _0, out uint _1, Buffer<ulong> _2) => throw new NotImplementedException();
	}
}
