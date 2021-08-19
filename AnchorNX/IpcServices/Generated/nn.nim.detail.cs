#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Nim.Detail {
	public unsafe partial class INetworkInstallManager : _Base_INetworkInstallManager {}
	public class _Base_INetworkInstallManager : IpcInterface {
		static readonly Logger Logger = new("INetworkInstallManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // CreateSystemUpdateTask
					var ret = CreateSystemUpdateTask(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // DestroySystemUpdateTask
					DestroySystemUpdateTask(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // ListSystemUpdateTask
					ListSystemUpdateTask(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // RequestSystemUpdateTaskRun
					RequestSystemUpdateTaskRun(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 4: { // GetSystemUpdateTaskInfo
					var ret = GetSystemUpdateTaskInfo(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // CommitSystemUpdateTask
					CommitSystemUpdateTask(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // CreateNetworkInstallTask
					var ret = CreateNetworkInstallTask(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // DestroyNetworkInstallTask
					DestroyNetworkInstallTask(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // ListNetworkInstallTask
					ListNetworkInstallTask(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // RequestNetworkInstallTaskRun
					RequestNetworkInstallTaskRun(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 10: { // GetNetworkInstallTaskInfo
					var ret = GetNetworkInstallTaskInfo(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // CommitNetworkInstallTask
					CommitNetworkInstallTask(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // RequestLatestSystemUpdateMeta
					RequestLatestSystemUpdateMeta(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 14: { // ListApplicationNetworkInstallTask
					ListApplicationNetworkInstallTask(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 15: { // ListNetworkInstallTaskContentMeta
					ListNetworkInstallTaskContentMeta(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 16: { // RequestLatestVersion
					RequestLatestVersion(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 17: { // SetNetworkInstallTaskAttribute
					SetNetworkInstallTaskAttribute(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 18: { // AddNetworkInstallTaskContentMeta
					AddNetworkInstallTaskContentMeta(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 19: { // GetDownloadedSystemDataPath
					GetDownloadedSystemDataPath(null, im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 20: { // CalculateNetworkInstallTaskRequiredSize
					var ret = CalculateNetworkInstallTaskRequiredSize(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 21: { // IsExFatDriverIncluded
					var ret = IsExFatDriverIncluded(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 22: { // GetBackgroundDownloadStressTaskInfo
					var ret = GetBackgroundDownloadStressTaskInfo();
					om.Initialize(0, 0, 0);
					break;
				}
				case 23: { // RequestDeviceAuthenticationToken
					RequestDeviceAuthenticationToken(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 24: { // RequestGameCardRegistrationStatus
					RequestGameCardRegistrationStatus(null, im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x5, 1), out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 25: { // RequestRegisterGameCard
					RequestRegisterGameCard(null, im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x5, 1), out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 26: { // RequestRegisterNotificationToken
					RequestRegisterNotificationToken(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 27: { // RequestDownloadTaskList
					RequestDownloadTaskList(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 28: { // RequestApplicationControl
					RequestApplicationControl(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 29: { // RequestLatestApplicationControl
					RequestLatestApplicationControl(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 30: { // RequestVersionList
					RequestVersionList(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 31: { // CreateApplyDeltaTask
					var ret = CreateApplyDeltaTask(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 32: { // DestroyApplyDeltaTask
					DestroyApplyDeltaTask(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 33: { // ListApplicationApplyDeltaTask
					ListApplicationApplyDeltaTask(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 34: { // RequestApplyDeltaTaskRun
					RequestApplyDeltaTaskRun(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 35: { // GetApplyDeltaTaskInfo
					var ret = GetApplyDeltaTaskInfo(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 36: { // ListApplyDeltaTask
					ListApplyDeltaTask(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 37: { // CommitApplyDeltaTask
					CommitApplyDeltaTask(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 38: { // CalculateApplyDeltaTaskRequiredSize
					var ret = CalculateApplyDeltaTaskRequiredSize(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 39: { // PrepareShutdown
					PrepareShutdown();
					om.Initialize(0, 0, 0);
					break;
				}
				case 40: { // ListApplyDeltaTask2
					ListApplyDeltaTask2(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 41: { // ClearNotEnoughSpaceStateOfApplyDeltaTask
					ClearNotEnoughSpaceStateOfApplyDeltaTask(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 42: { // Unknown42
					var ret = Unknown42(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 43: { // Unknown43
					var ret = Unknown43();
					om.Initialize(0, 0, 0);
					break;
				}
				case 44: { // Unknown44
					var ret = Unknown44(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 45: { // Unknown45
					var ret = Unknown45(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 46: { // Unknown46
					Unknown46();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to INetworkInstallManager: {im.CommandId}");
			}
		}
		
		public virtual object CreateSystemUpdateTask(object _0) => throw new NotImplementedException();
		public virtual void DestroySystemUpdateTask(object _0) => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.DestroySystemUpdateTask [1]".Debug(Log);
		public virtual void ListSystemUpdateTask(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void RequestSystemUpdateTaskRun(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual object GetSystemUpdateTaskInfo(object _0) => throw new NotImplementedException();
		public virtual void CommitSystemUpdateTask(object _0) => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.CommitSystemUpdateTask [5]".Debug(Log);
		public virtual object CreateNetworkInstallTask(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void DestroyNetworkInstallTask(object _0) => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.DestroyNetworkInstallTask [7]".Debug(Log);
		public virtual void ListNetworkInstallTask(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void RequestNetworkInstallTaskRun(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual object GetNetworkInstallTaskInfo(object _0) => throw new NotImplementedException();
		public virtual void CommitNetworkInstallTask(object _0) => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.CommitNetworkInstallTask [11]".Debug(Log);
		public virtual void RequestLatestSystemUpdateMeta(out uint _0, out Nn.Nim.Detail.IAsyncValue _1) => throw new NotImplementedException();
		public virtual void ListApplicationNetworkInstallTask(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void ListNetworkInstallTaskContentMeta(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void RequestLatestVersion(object _0, out uint _1, out Nn.Nim.Detail.IAsyncValue _2) => throw new NotImplementedException();
		public virtual void SetNetworkInstallTaskAttribute(object _0) => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.SetNetworkInstallTaskAttribute [17]".Debug(Log);
		public virtual void AddNetworkInstallTaskContentMeta(object _0, Buffer<byte> _1) => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.AddNetworkInstallTaskContentMeta [18]".Debug(Log);
		public virtual void GetDownloadedSystemDataPath(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual object CalculateNetworkInstallTaskRequiredSize(object _0) => throw new NotImplementedException();
		public virtual object IsExFatDriverIncluded(object _0) => throw new NotImplementedException();
		public virtual object GetBackgroundDownloadStressTaskInfo() => throw new NotImplementedException();
		public virtual void RequestDeviceAuthenticationToken(out uint _0, out Nn.Nim.Detail.IAsyncValue _1) => throw new NotImplementedException();
		public virtual void RequestGameCardRegistrationStatus(object _0, Buffer<byte> _1, Buffer<byte> _2, out uint _3, out Nn.Nim.Detail.IAsyncValue _4) => throw new NotImplementedException();
		public virtual void RequestRegisterGameCard(object _0, Buffer<byte> _1, Buffer<byte> _2, out uint _3, out Nn.Nim.Detail.IAsyncResult _4) => throw new NotImplementedException();
		public virtual void RequestRegisterNotificationToken(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual void RequestDownloadTaskList(object _0, out uint _1, out Nn.Nim.Detail.IAsyncData _2) => throw new NotImplementedException();
		public virtual void RequestApplicationControl(object _0, out uint _1, out Nn.Nim.Detail.IAsyncValue _2) => throw new NotImplementedException();
		public virtual void RequestLatestApplicationControl(object _0, out uint _1, out Nn.Nim.Detail.IAsyncValue _2) => throw new NotImplementedException();
		public virtual void RequestVersionList(object _0, out uint _1, out Nn.Nim.Detail.IAsyncData _2) => throw new NotImplementedException();
		public virtual object CreateApplyDeltaTask(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void DestroyApplyDeltaTask(object _0) => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.DestroyApplyDeltaTask [32]".Debug(Log);
		public virtual void ListApplicationApplyDeltaTask(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void RequestApplyDeltaTaskRun(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual object GetApplyDeltaTaskInfo(object _0) => throw new NotImplementedException();
		public virtual void ListApplyDeltaTask(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void CommitApplyDeltaTask(object _0) => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.CommitApplyDeltaTask [37]".Debug(Log);
		public virtual object CalculateApplyDeltaTaskRequiredSize(object _0) => throw new NotImplementedException();
		public virtual void PrepareShutdown() => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.PrepareShutdown [39]".Debug(Log);
		public virtual void ListApplyDeltaTask2(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void ClearNotEnoughSpaceStateOfApplyDeltaTask(object _0) => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.ClearNotEnoughSpaceStateOfApplyDeltaTask [41]".Debug(Log);
		public virtual object Unknown42(object _0) => throw new NotImplementedException();
		public virtual object Unknown43() => throw new NotImplementedException();
		public virtual object Unknown44(object _0) => throw new NotImplementedException();
		public virtual object Unknown45(object _0) => throw new NotImplementedException();
		public virtual void Unknown46() => "Stub hit for Nn.Nim.Detail.INetworkInstallManager.Unknown46 [46]".Debug(Log);
	}
	
	public unsafe partial class IShopServiceManager : _Base_IShopServiceManager {}
	public class _Base_IShopServiceManager : IpcInterface {
		static readonly Logger Logger = new("IShopServiceManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestDeviceAuthenticationToken
					RequestDeviceAuthenticationToken(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 1: { // RequestCachedDeviceAuthenticationToken
					RequestCachedDeviceAuthenticationToken(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 100: { // RequestRegisterDeviceAccount
					RequestRegisterDeviceAccount(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 101: { // RequestUnregisterDeviceAccount
					RequestUnregisterDeviceAccount(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 102: { // RequestDeviceAccountStatus
					RequestDeviceAccountStatus(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 103: { // GetDeviceAccountInfo
					var ret = GetDeviceAccountInfo();
					om.Initialize(0, 0, 0);
					break;
				}
				case 104: { // RequestDeviceRegistrationInfo
					RequestDeviceRegistrationInfo(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 105: { // RequestTransferDeviceAccount
					RequestTransferDeviceAccount(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 106: { // RequestSyncRegistration
					RequestSyncRegistration(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 107: { // IsOwnDeviceId
					var ret = IsOwnDeviceId(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 200: { // RequestRegisterNotificationToken
					RequestRegisterNotificationToken(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 300: { // RequestUnlinkDevice
					RequestUnlinkDevice(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 301: { // RequestUnlinkDeviceIntegrated
					RequestUnlinkDeviceIntegrated(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 302: { // RequestLinkDevice
					RequestLinkDevice(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 303: { // HasDeviceLink
					var ret = HasDeviceLink(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 304: { // RequestUnlinkDeviceAll
					RequestUnlinkDeviceAll(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 305: { // RequestCreateVirtualAccount
					RequestCreateVirtualAccount(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 306: { // RequestDeviceLinkStatus
					RequestDeviceLinkStatus(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 400: { // GetAccountByVirtualAccount
					var ret = GetAccountByVirtualAccount(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 500: { // RequestSyncTicket
					RequestSyncTicket(out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 501: { // RequestDownloadTicket
					RequestDownloadTicket(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 502: { // RequestDownloadTicketForPrepurchasedContents
					RequestDownloadTicketForPrepurchasedContents(null, out var _0, out var _1);
					om.Initialize(1, 1, 0);
					om.Copy(0, await CreateHandle(_0, copy: true));
					om.Move(0, await CreateHandle(_1));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IShopServiceManager: {im.CommandId}");
			}
		}
		
		public virtual void RequestDeviceAuthenticationToken(out uint _0, out Nn.Nim.Detail.IAsyncValue _1) => throw new NotImplementedException();
		public virtual void RequestCachedDeviceAuthenticationToken(object _0, out uint _1, out Nn.Nim.Detail.IAsyncValue _2) => throw new NotImplementedException();
		public virtual void RequestRegisterDeviceAccount(out uint _0, out Nn.Nim.Detail.IAsyncResult _1) => throw new NotImplementedException();
		public virtual void RequestUnregisterDeviceAccount(out uint _0, out Nn.Nim.Detail.IAsyncResult _1) => throw new NotImplementedException();
		public virtual void RequestDeviceAccountStatus(out uint _0, out Nn.Nim.Detail.IAsyncValue _1) => throw new NotImplementedException();
		public virtual object GetDeviceAccountInfo() => throw new NotImplementedException();
		public virtual void RequestDeviceRegistrationInfo(out uint _0, out Nn.Nim.Detail.IAsyncValue _1) => throw new NotImplementedException();
		public virtual void RequestTransferDeviceAccount(out uint _0, out Nn.Nim.Detail.IAsyncResult _1) => throw new NotImplementedException();
		public virtual void RequestSyncRegistration(out uint _0, out Nn.Nim.Detail.IAsyncResult _1) => throw new NotImplementedException();
		public virtual object IsOwnDeviceId(object _0) => throw new NotImplementedException();
		public virtual void RequestRegisterNotificationToken(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual void RequestUnlinkDevice(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual void RequestUnlinkDeviceIntegrated(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual void RequestLinkDevice(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual object HasDeviceLink(object _0) => throw new NotImplementedException();
		public virtual void RequestUnlinkDeviceAll(out uint _0, out Nn.Nim.Detail.IAsyncResult _1) => throw new NotImplementedException();
		public virtual void RequestCreateVirtualAccount(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual void RequestDeviceLinkStatus(object _0, out uint _1, out Nn.Nim.Detail.IAsyncValue _2) => throw new NotImplementedException();
		public virtual object GetAccountByVirtualAccount(object _0) => throw new NotImplementedException();
		public virtual void RequestSyncTicket(out uint _0, out Nn.Nim.Detail.IAsyncProgressResult _1) => throw new NotImplementedException();
		public virtual void RequestDownloadTicket(object _0, out uint _1, out Nn.Nim.Detail.IAsyncResult _2) => throw new NotImplementedException();
		public virtual void RequestDownloadTicketForPrepurchasedContents(object _0, out uint _1, out Nn.Nim.Detail.IAsyncValue _2) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAsyncData : _Base_IAsyncData {}
	public class _Base_IAsyncData : IpcInterface {
		static readonly Logger Logger = new("IAsyncData");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					var ret = Unknown2();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					var ret = Unknown4();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					Unknown5(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAsyncData: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0() => "Stub hit for Nn.Nim.Detail.IAsyncData.Unknown0 [0]".Debug(Log);
		public virtual void Unknown1() => "Stub hit for Nn.Nim.Detail.IAsyncData.Unknown1 [1]".Debug(Log);
		public virtual object Unknown2() => throw new NotImplementedException();
		public virtual void Unknown3(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown4() => throw new NotImplementedException();
		public virtual void Unknown5(Buffer<byte> _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAsyncProgressResult : _Base_IAsyncProgressResult {}
	public class _Base_IAsyncProgressResult : IpcInterface {
		static readonly Logger Logger = new("IAsyncProgressResult");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					var ret = Unknown2();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAsyncProgressResult: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0() => "Stub hit for Nn.Nim.Detail.IAsyncProgressResult.Unknown0 [0]".Debug(Log);
		public virtual void Unknown1() => "Stub hit for Nn.Nim.Detail.IAsyncProgressResult.Unknown1 [1]".Debug(Log);
		public virtual object Unknown2() => throw new NotImplementedException();
		public virtual void Unknown3(Buffer<byte> _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAsyncResult : _Base_IAsyncResult {}
	public class _Base_IAsyncResult : IpcInterface {
		static readonly Logger Logger = new("IAsyncResult");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAsyncResult: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0() => "Stub hit for Nn.Nim.Detail.IAsyncResult.Unknown0 [0]".Debug(Log);
		public virtual void Unknown1() => "Stub hit for Nn.Nim.Detail.IAsyncResult.Unknown1 [1]".Debug(Log);
		public virtual void Unknown2(Buffer<byte> _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAsyncValue : _Base_IAsyncValue {}
	public class _Base_IAsyncValue : IpcInterface {
		static readonly Logger Logger = new("IAsyncValue");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAsyncValue: {im.CommandId}");
			}
		}
		
		public virtual object Unknown0() => throw new NotImplementedException();
		public virtual void Unknown1(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void Unknown2() => "Stub hit for Nn.Nim.Detail.IAsyncValue.Unknown2 [2]".Debug(Log);
		public virtual void Unknown3(Buffer<byte> _0) => throw new NotImplementedException();
	}
}
