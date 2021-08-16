#pragma warning disable 169, 465
using System;
using System.Threading.Tasks;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public enum FileSystemType : uint {
		Invalid = 0x0, 
		Invalid2 = 0x1, 
		Logo = 0x2, 
		ContentControl = 0x3, 
		ContentManual = 0x4, 
		ContentMeta = 0x5, 
		ContentData = 0x6, 
		ApplicationPackage = 0x7, 
	}
	
	public enum DirectoryEntryType : byte {
		Directory = 0x0, 
		File = 0x1, 
	}
	
	public unsafe struct IDirectoryEntry {
		public fixed byte Path[768];
		public uint Unk1;
		public Nn.Fssrv.Sf.DirectoryEntryType DirectoryEntryType;
		public ulong Filesize;
	}
	
	public enum Partition : uint {
		BootPartition1Root = 0x0, 
		BootPartition2Root = 0xA, 
		UserDataRoot = 0x14, 
		BootConfigAndPackage2Part1 = 0x15, 
		BootConfigAndPackage2Part2 = 0x16, 
		BootConfigAndPackage2Part3 = 0x17, 
		BootConfigAndPackage2Part4 = 0x18, 
		BootConfigAndPackage2Part5 = 0x19, 
		BootConfigAndPackage2Part6 = 0x1A, 
		CalibrationBinary = 0x1B, 
		CalibrationFile = 0x1C, 
		SafeMode = 0x1D, 
		SystemProperEncryption = 0x1E, 
		User = 0x1F, 
	}
	
	public partial class IFileSystemProxy : _Base_IFileSystemProxy {}
	public class _Base_IFileSystemProxy : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenFileSystem
					var ret = OpenFileSystem(im.GetData<Nn.Fssrv.Sf.FileSystemType>(8), im.GetBuffer<byte>(0x19, 0));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 1: { // SetCurrentProcess
					SetCurrentProcess(im.GetData<ulong>(8), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // OpenDataFileSystemByCurrentProcess
					var ret = OpenDataFileSystemByCurrentProcess();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 7: { // OpenFileSystemWithPatch
					var ret = OpenFileSystemWithPatch(im.GetData<Nn.Fssrv.Sf.FileSystemType>(8), im.GetData<ulong>(16));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 8: { // OpenFileSystemWithId
					var ret = OpenFileSystemWithId(im.GetData<Nn.Fssrv.Sf.FileSystemType>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x19, 0));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 9: { // OpenDataFileSystemByApplicationId
					var ret = OpenDataFileSystemByApplicationId(im.GetData<ulong>(8));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 11: { // OpenBisFileSystem
					var ret = OpenBisFileSystem(im.GetData<Nn.Fssrv.Sf.Partition>(8), im.GetBuffer<byte>(0x19, 0));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 12: { // OpenBisStorage
					var ret = OpenBisStorage(im.GetData<Nn.Fssrv.Sf.Partition>(8));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 13: { // InvalidateBisCache
					InvalidateBisCache();
					om.Initialize(0, 0, 0);
					break;
				}
				case 17: { // OpenHostFileSystem
					var ret = OpenHostFileSystem(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 18: { // OpenSdCardFileSystem
					var ret = OpenSdCardFileSystem();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 19: { // FormatSdCardFileSystem
					FormatSdCardFileSystem();
					om.Initialize(0, 0, 0);
					break;
				}
				case 21: { // DeleteSaveDataFileSystem
					DeleteSaveDataFileSystem(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 22: { // CreateSaveDataFileSystem
					CreateSaveDataFileSystem(im.GetBytes(8, 0x40), im.GetBytes(72, 0x40), im.GetBytes(136, 0x10));
					om.Initialize(0, 0, 0);
					break;
				}
				case 23: { // CreateSaveDataFileSystemBySystemSaveDataId
					CreateSaveDataFileSystemBySystemSaveDataId(im.GetBytes(8, 0x40), im.GetBytes(72, 0x40));
					om.Initialize(0, 0, 0);
					break;
				}
				case 24: { // RegisterSaveDataFileSystemAtomicDeletion
					RegisterSaveDataFileSystemAtomicDeletion(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 25: { // DeleteSaveDataFileSystemBySaveDataSpaceId
					DeleteSaveDataFileSystemBySaveDataSpaceId(im.GetData<byte>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 26: { // FormatSdCardDryRun
					FormatSdCardDryRun();
					om.Initialize(0, 0, 0);
					break;
				}
				case 27: { // IsExFatSupported
					var ret = IsExFatSupported();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 28: { // DeleteSaveDataFileSystemBySaveDataAttribute
					DeleteSaveDataFileSystemBySaveDataAttribute(im.GetData<byte>(8), im.GetBytes(9, 0x40));
					om.Initialize(0, 0, 0);
					break;
				}
				case 30: { // OpenGameCardStorage
					var ret = OpenGameCardStorage(im.GetData<uint>(8), im.GetData<uint>(12));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 31: { // OpenGameCardFileSystem
					var ret = OpenGameCardFileSystem(im.GetData<uint>(8), im.GetData<uint>(12));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 32: { // ExtendSaveDataFileSystem
					ExtendSaveDataFileSystem(im.GetData<byte>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), im.GetData<ulong>(32));
					om.Initialize(0, 0, 0);
					break;
				}
				case 33: { // DeleteCacheStorage
					var ret = DeleteCacheStorage(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 34: { // GetCacheStorageSize
					var ret = GetCacheStorageSize(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 51: { // OpenSaveDataFileSystem
					var ret = OpenSaveDataFileSystem(im.GetData<byte>(8), im.GetBytes(16, 0x40));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 52: { // OpenSaveDataFileSystemBySystemSaveDataId
					var ret = OpenSaveDataFileSystemBySystemSaveDataId(im.GetData<byte>(8), im.GetBytes(16, 0x40));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 53: { // OpenReadOnlySaveDataFileSystem
					var ret = OpenReadOnlySaveDataFileSystem(im.GetData<byte>(8), im.GetBytes(16, 0x40));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 57: { // ReadSaveDataFileSystemExtraDataBySaveDataSpaceId
					ReadSaveDataFileSystemExtraDataBySaveDataSpaceId(im.GetData<byte>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 58: { // ReadSaveDataFileSystemExtraData
					ReadSaveDataFileSystemExtraData(im.GetData<ulong>(8), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 59: { // WriteSaveDataFileSystemExtraData
					WriteSaveDataFileSystemExtraData(im.GetData<byte>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 60: { // OpenSaveDataInfoReader
					var ret = OpenSaveDataInfoReader();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 61: { // OpenSaveDataInfoReaderBySaveDataSpaceId
					var ret = OpenSaveDataInfoReaderBySaveDataSpaceId(im.GetData<byte>(8));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 62: { // OpenCacheStorageList
					var ret = OpenCacheStorageList(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 64: { // OpenSaveDataInternalStorageFileSystem
					var ret = OpenSaveDataInternalStorageFileSystem(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 65: { // UpdateSaveDataMacForDebug
					var ret = UpdateSaveDataMacForDebug(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 66: { // WriteSaveDataFileSystemExtraData2
					var ret = WriteSaveDataFileSystemExtraData2(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 80: { // OpenSaveDataMetaFile
					var ret = OpenSaveDataMetaFile(im.GetData<byte>(8), im.GetData<uint>(12), im.GetBytes(16, 0x40));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 81: { // OpenSaveDataTransferManager
					var ret = OpenSaveDataTransferManager();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 82: { // OpenSaveDataTransferManagerVersion2
					var ret = OpenSaveDataTransferManagerVersion2(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 100: { // OpenImageDirectoryFileSystem
					var ret = OpenImageDirectoryFileSystem(im.GetData<uint>(8));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 110: { // OpenContentStorageFileSystem
					var ret = OpenContentStorageFileSystem(im.GetData<uint>(8));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 200: { // OpenDataStorageByCurrentProcess
					var ret = OpenDataStorageByCurrentProcess();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 201: { // OpenDataStorageByProgramId
					var ret = OpenDataStorageByProgramId(im.GetData<ulong>(8));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 202: { // OpenDataStorageByDataId
					var ret = await OpenDataStorageByDataId(im.GetData<byte>(8), im.GetData<ulong>(16));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 203: { // OpenPatchDataStorageByCurrentProcess
					var ret = OpenPatchDataStorageByCurrentProcess();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 400: { // OpenDeviceOperator
					var ret = OpenDeviceOperator();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 500: { // OpenSdCardDetectionEventNotifier
					var ret = OpenSdCardDetectionEventNotifier();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 501: { // OpenGameCardDetectionEventNotifier
					var ret = OpenGameCardDetectionEventNotifier();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 510: { // OpenSystemDataUpdateEventNotifier
					var ret = OpenSystemDataUpdateEventNotifier(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 511: { // NotifySystemDataUpdateEvent
					var ret = NotifySystemDataUpdateEvent(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 600: { // SetCurrentPosixTime
					SetCurrentPosixTime(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 601: { // QuerySaveDataTotalSize
					var ret = QuerySaveDataTotalSize(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 602: { // VerifySaveDataFileSystem
					VerifySaveDataFileSystem(im.GetData<ulong>(8), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 603: { // CorruptSaveDataFileSystem
					CorruptSaveDataFileSystem(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 604: { // CreatePaddingFile
					CreatePaddingFile(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 605: { // DeleteAllPaddingFiles
					DeleteAllPaddingFiles();
					om.Initialize(0, 0, 0);
					break;
				}
				case 606: { // GetRightsId
					GetRightsId(im.GetData<byte>(8), im.GetData<ulong>(16), out var _0);
					om.Initialize(0, 0, 16);
					om.SetBytes(8, _0);
					break;
				}
				case 607: { // RegisterExternalKey
					RegisterExternalKey(im.GetBytes(8, 0x10), im.GetBytes(24, 0x10));
					om.Initialize(0, 0, 0);
					break;
				}
				case 608: { // UnregisterAllExternalKey
					UnregisterAllExternalKey();
					om.Initialize(0, 0, 0);
					break;
				}
				case 609: { // GetRightsIdByPath
					GetRightsIdByPath(im.GetBuffer<byte>(0x19, 0), out var _0);
					om.Initialize(0, 0, 16);
					om.SetBytes(8, _0);
					break;
				}
				case 610: { // GetRightsIdAndKeyGenerationByPath
					GetRightsIdAndKeyGenerationByPath(im.GetBuffer<byte>(0x19, 0), out var _0, out var _1);
					om.Initialize(0, 0, 17);
					om.SetData(8, _0);
					om.SetBytes(9, _1);
					break;
				}
				case 611: { // SetCurrentPosixTimeWithTimeDifference
					SetCurrentPosixTimeWithTimeDifference(im.GetData<uint>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 612: { // GetFreeSpaceSizeForSaveData
					var ret = GetFreeSpaceSizeForSaveData(im.GetData<byte>(8));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 613: { // VerifySaveDataFileSystemBySaveDataSpaceId
					VerifySaveDataFileSystemBySaveDataSpaceId(im.GetData<byte>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 614: { // CorruptSaveDataFileSystemBySaveDataSpaceId
					CorruptSaveDataFileSystemBySaveDataSpaceId(im.GetData<byte>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 615: { // QuerySaveDataInternalStorageTotalSize
					var ret = QuerySaveDataInternalStorageTotalSize(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 620: { // SetSdCardEncryptionSeed
					SetSdCardEncryptionSeed(im.GetBytes(8, 0x10));
					om.Initialize(0, 0, 0);
					break;
				}
				case 630: { // SetSdCardAccessibility
					SetSdCardAccessibility(im.GetData<byte>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 631: { // IsSdCardAccessible
					var ret = IsSdCardAccessible();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 640: { // IsSignedSystemPartitionOnSdCardValid
					var ret = IsSignedSystemPartitionOnSdCardValid();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 700: { // OpenAccessFailureResolver
					var ret = OpenAccessFailureResolver(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 701: { // GetAccessFailureDetectionEvent
					var ret = GetAccessFailureDetectionEvent(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 702: { // IsAccessFailureDetected
					var ret = IsAccessFailureDetected(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 710: { // ResolveAccessFailure
					var ret = ResolveAccessFailure(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 720: { // AbandonAccessFailure
					var ret = AbandonAccessFailure(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 800: { // GetAndClearFileSystemProxyErrorInfo
					GetAndClearFileSystemProxyErrorInfo(out var _0);
					om.Initialize(0, 0, 128);
					om.SetBytes(8, _0);
					break;
				}
				case 1000: { // SetBisRootForHost
					SetBisRootForHost(im.GetData<uint>(8), im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1001: { // SetSaveDataSize
					SetSaveDataSize(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1002: { // SetSaveDataRootPath
					SetSaveDataRootPath(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1003: { // DisableAutoSaveDataCreation
					DisableAutoSaveDataCreation();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1004: { // SetGlobalAccessLogMode
					SetGlobalAccessLogMode(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1005: { // GetGlobalAccessLogMode
					var ret = GetGlobalAccessLogMode();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 1006: { // OutputAccessLogToSdCard
					OutputAccessLogToSdCard(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1007: { // RegisterUpdatePartition
					RegisterUpdatePartition();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1008: { // OpenRegisteredUpdatePartition
					var ret = OpenRegisteredUpdatePartition();
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 1009: { // GetAndClearMemoryReportInfo
					GetAndClearMemoryReportInfo(out var _0);
					om.Initialize(0, 0, 128);
					om.SetBytes(8, _0);
					break;
				}
				case 1010: { // Unknown1010
					var ret = Unknown1010(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1100: { // OverrideSaveDataTransferTokenSignVerificationKey
					OverrideSaveDataTransferTokenSignVerificationKey(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IFileSystemProxy: {im.CommandId}");
			}
		}
		
		public virtual Nn.Fssrv.Sf.IFileSystem OpenFileSystem(Nn.Fssrv.Sf.FileSystemType filesystem_type, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void SetCurrentProcess(ulong _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetCurrentProcess [1]".Debug();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenDataFileSystemByCurrentProcess() => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenFileSystemWithPatch(Nn.Fssrv.Sf.FileSystemType filesystem_type, ulong id) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenFileSystemWithId(Nn.Fssrv.Sf.FileSystemType filesystem_type, ulong tid, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenDataFileSystemByApplicationId(ulong u64) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenBisFileSystem(Nn.Fssrv.Sf.Partition partitionId, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IStorage OpenBisStorage(Nn.Fssrv.Sf.Partition partitionId) => throw new NotImplementedException();
		public virtual void InvalidateBisCache() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.InvalidateBisCache [13]".Debug();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenHostFileSystem(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenSdCardFileSystem() => throw new NotImplementedException();
		public virtual void FormatSdCardFileSystem() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.FormatSdCardFileSystem [19]".Debug();
		public virtual void DeleteSaveDataFileSystem(ulong tid) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.DeleteSaveDataFileSystem [21]".Debug();
		public virtual void CreateSaveDataFileSystem(byte[] save_struct, byte[] ave_create_struct, byte[] _2) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CreateSaveDataFileSystem [22]".Debug();
		public virtual void CreateSaveDataFileSystemBySystemSaveDataId(byte[] _0, byte[] _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CreateSaveDataFileSystemBySystemSaveDataId [23]".Debug();
		public virtual void RegisterSaveDataFileSystemAtomicDeletion(Buffer<byte> _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.RegisterSaveDataFileSystemAtomicDeletion [24]".Debug();
		public virtual void DeleteSaveDataFileSystemBySaveDataSpaceId(byte _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.DeleteSaveDataFileSystemBySaveDataSpaceId [25]".Debug();
		public virtual void FormatSdCardDryRun() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.FormatSdCardDryRun [26]".Debug();
		public virtual byte IsExFatSupported() => throw new NotImplementedException();
		public virtual void DeleteSaveDataFileSystemBySaveDataAttribute(byte _0, byte[] _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.DeleteSaveDataFileSystemBySaveDataAttribute [28]".Debug();
		public virtual Nn.Fssrv.Sf.IStorage OpenGameCardStorage(uint _0, uint _1) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenGameCardFileSystem(uint _0, uint _1) => throw new NotImplementedException();
		public virtual void ExtendSaveDataFileSystem(byte _0, ulong _1, ulong _2, ulong _3) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.ExtendSaveDataFileSystem [32]".Debug();
		public virtual object DeleteCacheStorage(object _0) => throw new NotImplementedException();
		public virtual object GetCacheStorageSize(object _0) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenSaveDataFileSystem(byte save_data_space_id, byte[] save_struct) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenSaveDataFileSystemBySystemSaveDataId(byte save_data_space_id, byte[] save_struct) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenReadOnlySaveDataFileSystem(byte save_data_space_id, byte[] save_struct) => throw new NotImplementedException();
		public virtual void ReadSaveDataFileSystemExtraDataBySaveDataSpaceId(byte _0, ulong _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void ReadSaveDataFileSystemExtraData(ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void WriteSaveDataFileSystemExtraData(byte _0, ulong _1, Buffer<byte> _2) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.WriteSaveDataFileSystemExtraData [59]".Debug();
		public virtual Nn.Fssrv.Sf.ISaveDataInfoReader OpenSaveDataInfoReader() => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.ISaveDataInfoReader OpenSaveDataInfoReaderBySaveDataSpaceId(byte _0) => throw new NotImplementedException();
		public virtual object OpenCacheStorageList(object _0) => throw new NotImplementedException();
		public virtual object OpenSaveDataInternalStorageFileSystem(object _0) => throw new NotImplementedException();
		public virtual object UpdateSaveDataMacForDebug(object _0) => throw new NotImplementedException();
		public virtual object WriteSaveDataFileSystemExtraData2(object _0) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFile OpenSaveDataMetaFile(byte _0, uint _1, byte[] _2) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.ISaveDataTransferManager OpenSaveDataTransferManager() => throw new NotImplementedException();
		public virtual object OpenSaveDataTransferManagerVersion2(object _0) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenImageDirectoryFileSystem(uint _0) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenContentStorageFileSystem(uint content_storage_id) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IStorage OpenDataStorageByCurrentProcess() => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IStorage OpenDataStorageByProgramId(ulong tid) => throw new NotImplementedException();
		public virtual async Task<Nn.Fssrv.Sf.IStorage> OpenDataStorageByDataId(byte storage_id, ulong tid) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IStorage OpenPatchDataStorageByCurrentProcess() => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IDeviceOperator OpenDeviceOperator() => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IEventNotifier OpenSdCardDetectionEventNotifier() => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IEventNotifier OpenGameCardDetectionEventNotifier() => throw new NotImplementedException();
		public virtual object OpenSystemDataUpdateEventNotifier(object _0) => throw new NotImplementedException();
		public virtual object NotifySystemDataUpdateEvent(object _0) => throw new NotImplementedException();
		public virtual void SetCurrentPosixTime(ulong time) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetCurrentPosixTime [600]".Debug();
		public virtual ulong QuerySaveDataTotalSize(ulong _0, ulong _1) => throw new NotImplementedException();
		public virtual void VerifySaveDataFileSystem(ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void CorruptSaveDataFileSystem(ulong _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CorruptSaveDataFileSystem [603]".Debug();
		public virtual void CreatePaddingFile(ulong _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CreatePaddingFile [604]".Debug();
		public virtual void DeleteAllPaddingFiles() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.DeleteAllPaddingFiles [605]".Debug();
		public virtual void GetRightsId(byte _0, ulong _1, out byte[] rights) => throw new NotImplementedException();
		public virtual void RegisterExternalKey(byte[] _0, byte[] _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.RegisterExternalKey [607]".Debug();
		public virtual void UnregisterAllExternalKey() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.UnregisterAllExternalKey [608]".Debug();
		public virtual void GetRightsIdByPath(Buffer<byte> _0, out byte[] rights) => throw new NotImplementedException();
		public virtual void GetRightsIdAndKeyGenerationByPath(Buffer<byte> _0, out byte _1, out byte[] rights) => throw new NotImplementedException();
		public virtual void SetCurrentPosixTimeWithTimeDifference(uint _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetCurrentPosixTimeWithTimeDifference [611]".Debug();
		public virtual ulong GetFreeSpaceSizeForSaveData(byte _0) => throw new NotImplementedException();
		public virtual void VerifySaveDataFileSystemBySaveDataSpaceId(byte _0, ulong _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void CorruptSaveDataFileSystemBySaveDataSpaceId(byte _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CorruptSaveDataFileSystemBySaveDataSpaceId [614]".Debug();
		public virtual object QuerySaveDataInternalStorageTotalSize(object _0) => throw new NotImplementedException();
		public virtual void SetSdCardEncryptionSeed(byte[] _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetSdCardEncryptionSeed [620]".Debug();
		public virtual void SetSdCardAccessibility(byte _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetSdCardAccessibility [630]".Debug();
		public virtual byte IsSdCardAccessible() => throw new NotImplementedException();
		public virtual byte IsSignedSystemPartitionOnSdCardValid() => throw new NotImplementedException();
		public virtual object OpenAccessFailureResolver(object _0) => throw new NotImplementedException();
		public virtual object GetAccessFailureDetectionEvent(object _0) => throw new NotImplementedException();
		public virtual object IsAccessFailureDetected(object _0) => throw new NotImplementedException();
		public virtual object ResolveAccessFailure(object _0) => throw new NotImplementedException();
		public virtual object AbandonAccessFailure(object _0) => throw new NotImplementedException();
		public virtual void GetAndClearFileSystemProxyErrorInfo(out byte[] error_info) => throw new NotImplementedException();
		public virtual void SetBisRootForHost(uint _0, Buffer<byte> _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetBisRootForHost [1000]".Debug();
		public virtual void SetSaveDataSize(ulong _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetSaveDataSize [1001]".Debug();
		public virtual void SetSaveDataRootPath(Buffer<byte> _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetSaveDataRootPath [1002]".Debug();
		public virtual void DisableAutoSaveDataCreation() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.DisableAutoSaveDataCreation [1003]".Debug();
		public virtual void SetGlobalAccessLogMode(uint mode) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetGlobalAccessLogMode [1004]".Debug();
		public virtual uint GetGlobalAccessLogMode() => throw new NotImplementedException();
		public virtual void OutputAccessLogToSdCard(Buffer<byte> log_text) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.OutputAccessLogToSdCard [1006]".Debug();
		public virtual void RegisterUpdatePartition() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.RegisterUpdatePartition [1007]".Debug();
		public virtual Nn.Fssrv.Sf.IFileSystem OpenRegisteredUpdatePartition() => throw new NotImplementedException();
		public virtual void GetAndClearMemoryReportInfo(out byte[] _0) => throw new NotImplementedException();
		public virtual object Unknown1010(object _0) => throw new NotImplementedException();
		public virtual void OverrideSaveDataTransferTokenSignVerificationKey(Buffer<byte> _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.OverrideSaveDataTransferTokenSignVerificationKey [1100]".Debug();
	}
	
	public unsafe partial class IFileSystemProxyForLoader : _Base_IFileSystemProxyForLoader {}
	public unsafe class _Base_IFileSystemProxyForLoader : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenCodeFileSystem
					var (tid, content_path) = (im.GetData<ulong>(8), im.GetBuffer<byte>(0x19, 0));
					om.Initialize(1, 0, 0, 0x124);
					var ret = OpenCodeFileSystem(tid, content_path, om.GetXBuffer<byte>(0));
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 1: { // IsArchivedProgram
					var ret = IsArchivedProgram(im.GetData<ulong>(8));
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 2: { // SetCurrentProcess
					SetCurrentProcess(im.GetData<ulong>(8), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IFileSystemProxyForLoader: {im.CommandId}");
			}
		}
		
		public virtual Nn.Fssrv.Sf.IFileSystem OpenCodeFileSystem(ulong Tid, Buffer<byte> content_path, Buffer<byte> code_info) => throw new NotImplementedException();
		public virtual byte IsArchivedProgram(ulong _0) => throw new NotImplementedException();
		public virtual void SetCurrentProcess(ulong _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxyForLoader.SetCurrentProcess [2]".Debug();
	}
	
	public unsafe partial class IProgramRegistry : _Base_IProgramRegistry {}
	public unsafe class _Base_IProgramRegistry : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RegisterProgram
					RegisterProgram(im.GetData<byte>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), im.GetData<ulong>(32), im.GetData<ulong>(40), im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x5, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // UnregisterProgram
					UnregisterProgram(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // SetCurrentProcess
					SetCurrentProcess(im.GetData<ulong>(8), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 256: { // SetEnabledProgramVerification
					SetEnabledProgramVerification(im.GetData<byte>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IProgramRegistry: {im.CommandId}");
			}
		}
		
		public virtual void RegisterProgram(byte _0, ulong _1, ulong _2, ulong _3, ulong _4, Buffer<byte> _5, Buffer<byte> _6) => "Stub hit for Nn.Fssrv.Sf.IProgramRegistry.RegisterProgram [0]".Debug();
		public virtual void UnregisterProgram(ulong _0) => "Stub hit for Nn.Fssrv.Sf.IProgramRegistry.UnregisterProgram [1]".Debug();
		public virtual void SetCurrentProcess(ulong _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IProgramRegistry.SetCurrentProcess [2]".Debug();
		public virtual void SetEnabledProgramVerification(byte _0) => "Stub hit for Nn.Fssrv.Sf.IProgramRegistry.SetEnabledProgramVerification [256]".Debug();
	}
	
	public unsafe partial class IDeviceOperator : _Base_IDeviceOperator {}
	public unsafe class _Base_IDeviceOperator : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // IsSdCardInserted
					var ret = IsSdCardInserted();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 1: { // GetSdCardSpeedMode
					var ret = GetSdCardSpeedMode();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 2: { // GetSdCardCid
					GetSdCardCid(im.GetData<ulong>(8), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // GetSdCardUserAreaSize
					var ret = GetSdCardUserAreaSize();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 4: { // GetSdCardProtectedAreaSize
					var ret = GetSdCardProtectedAreaSize();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 5: { // GetAndClearSdCardErrorInfo
					GetAndClearSdCardErrorInfo(im.GetData<ulong>(8), out var _0, out var _1, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 24);
					om.SetBytes(8, _0);
					om.SetData(24, _1);
					break;
				}
				case 100: { // GetMmcCid
					GetMmcCid(im.GetData<ulong>(8), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 101: { // GetMmcSpeedMode
					var ret = GetMmcSpeedMode();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 110: { // EraseMmc
					EraseMmc(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 111: { // GetMmcPartitionSize
					var ret = GetMmcPartitionSize(im.GetData<uint>(8));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 112: { // GetMmcPatrolCount
					var ret = GetMmcPatrolCount();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 113: { // GetAndClearMmcErrorInfo
					GetAndClearMmcErrorInfo(im.GetData<ulong>(8), out var _0, out var _1, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 24);
					om.SetBytes(8, _0);
					om.SetData(24, _1);
					break;
				}
				case 114: { // GetMmcExtendedCsd
					GetMmcExtendedCsd(im.GetData<ulong>(8), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 115: { // SuspendMmcPatrol
					SuspendMmcPatrol();
					om.Initialize(0, 0, 0);
					break;
				}
				case 116: { // ResumeMmcPatrol
					ResumeMmcPatrol();
					om.Initialize(0, 0, 0);
					break;
				}
				case 200: { // IsGameCardInserted
					var ret = IsGameCardInserted();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 201: { // EraseGameCard
					EraseGameCard(im.GetData<uint>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 202: { // GetGameCardHandle
					var ret = GetGameCardHandle();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 203: { // GetGameCardUpdatePartitionInfo
					GetGameCardUpdatePartitionInfo(im.GetData<uint>(8), out var _0, out var _1);
					om.Initialize(0, 0, 16);
					om.SetData(8, _0);
					om.SetData(16, _1);
					break;
				}
				case 204: { // FinalizeGameCardDriver
					FinalizeGameCardDriver();
					om.Initialize(0, 0, 0);
					break;
				}
				case 205: { // GetGameCardAttribute
					var ret = GetGameCardAttribute(im.GetData<uint>(8));
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 206: { // GetGameCardDeviceCertificate
					GetGameCardDeviceCertificate(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 207: { // GetGameCardAsicInfo
					GetGameCardAsicInfo(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 208: { // GetGameCardIdSet
					GetGameCardIdSet(im.GetData<ulong>(8), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 209: { // WriteToGameCard
					WriteToGameCard(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 210: { // SetVerifyWriteEnalbleFlag
					SetVerifyWriteEnalbleFlag(im.GetData<byte>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 211: { // GetGameCardImageHash
					GetGameCardImageHash(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 212: { // GetGameCardErrorInfo
					GetGameCardErrorInfo(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 213: { // EraseAndWriteParamDirectly
					EraseAndWriteParamDirectly(im.GetData<ulong>(8), im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 214: { // ReadParamDirectly
					ReadParamDirectly(im.GetData<ulong>(8), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 215: { // ForceEraseGameCard
					ForceEraseGameCard();
					om.Initialize(0, 0, 0);
					break;
				}
				case 216: { // GetGameCardErrorInfo2
					GetGameCardErrorInfo2(out var _0);
					om.Initialize(0, 0, 16);
					om.SetBytes(8, _0);
					break;
				}
				case 217: { // GetGameCardErrorReportInfo
					GetGameCardErrorReportInfo(out var _0);
					om.Initialize(0, 0, 64);
					om.SetBytes(8, _0);
					break;
				}
				case 218: { // GetGameCardDeviceId
					GetGameCardDeviceId(im.GetData<ulong>(8), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 300: { // SetSpeedEmulationMode
					SetSpeedEmulationMode(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 301: { // GetSpeedEmulationMode
					var ret = GetSpeedEmulationMode();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 400: { // SuspendSdmmcControl
					var ret = SuspendSdmmcControl(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 401: { // ResumeSdmmcControl
					var ret = ResumeSdmmcControl(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IDeviceOperator: {im.CommandId}");
			}
		}
		
		public virtual byte IsSdCardInserted() => throw new NotImplementedException();
		public virtual ulong GetSdCardSpeedMode() => throw new NotImplementedException();
		public virtual void GetSdCardCid(ulong _0, Buffer<byte> cid) => throw new NotImplementedException();
		public virtual ulong GetSdCardUserAreaSize() => throw new NotImplementedException();
		public virtual ulong GetSdCardProtectedAreaSize() => throw new NotImplementedException();
		public virtual void GetAndClearSdCardErrorInfo(ulong _0, out byte[] _1, out ulong _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void GetMmcCid(ulong _0, Buffer<byte> cid) => throw new NotImplementedException();
		public virtual ulong GetMmcSpeedMode() => throw new NotImplementedException();
		public virtual void EraseMmc(uint _0) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.EraseMmc [110]".Debug();
		public virtual ulong GetMmcPartitionSize(uint _0) => throw new NotImplementedException();
		public virtual uint GetMmcPatrolCount() => throw new NotImplementedException();
		public virtual void GetAndClearMmcErrorInfo(ulong _0, out byte[] _1, out ulong _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void GetMmcExtendedCsd(ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void SuspendMmcPatrol() => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.SuspendMmcPatrol [115]".Debug();
		public virtual void ResumeMmcPatrol() => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.ResumeMmcPatrol [116]".Debug();
		public virtual byte IsGameCardInserted() => throw new NotImplementedException();
		public virtual void EraseGameCard(uint _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.EraseGameCard [201]".Debug();
		public virtual uint GetGameCardHandle() => throw new NotImplementedException();
		public virtual void GetGameCardUpdatePartitionInfo(uint _0, out uint version, out ulong tid) => throw new NotImplementedException();
		public virtual void FinalizeGameCardDriver() => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.FinalizeGameCardDriver [204]".Debug();
		public virtual byte GetGameCardAttribute(uint _0) => throw new NotImplementedException();
		public virtual void GetGameCardDeviceCertificate(uint _0, ulong _1, Buffer<byte> certificate) => throw new NotImplementedException();
		public virtual void GetGameCardAsicInfo(ulong _0, ulong _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void GetGameCardIdSet(ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void WriteToGameCard(ulong _0, ulong _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void SetVerifyWriteEnalbleFlag(byte flag) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.SetVerifyWriteEnalbleFlag [210]".Debug();
		public virtual void GetGameCardImageHash(uint _0, ulong _1, Buffer<byte> image_hash) => throw new NotImplementedException();
		public virtual void GetGameCardErrorInfo(ulong _0, ulong _1, Buffer<byte> _2, Buffer<byte> error_info) => throw new NotImplementedException();
		public virtual void EraseAndWriteParamDirectly(ulong _0, Buffer<byte> _1) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.EraseAndWriteParamDirectly [213]".Debug();
		public virtual void ReadParamDirectly(ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void ForceEraseGameCard() => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.ForceEraseGameCard [215]".Debug();
		public virtual void GetGameCardErrorInfo2(out byte[] error_info) => throw new NotImplementedException();
		public virtual void GetGameCardErrorReportInfo(out byte[] error_report_info) => throw new NotImplementedException();
		public virtual void GetGameCardDeviceId(ulong _0, Buffer<byte> device_id) => throw new NotImplementedException();
		public virtual void SetSpeedEmulationMode(uint emu_mode) => "Stub hit for Nn.Fssrv.Sf.IDeviceOperator.SetSpeedEmulationMode [300]".Debug();
		public virtual uint GetSpeedEmulationMode() => throw new NotImplementedException();
		public virtual object SuspendSdmmcControl(object _0) => throw new NotImplementedException();
		public virtual object ResumeSdmmcControl(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IDirectory : _Base_IDirectory {}
	public unsafe class _Base_IDirectory : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Read
					Read(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					break;
				}
				case 1: { // GetEntryCount
					var ret = GetEntryCount();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IDirectory: {im.CommandId}");
			}
		}
		
		public virtual void Read(out ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual ulong GetEntryCount() => throw new NotImplementedException();
	}
	
	public unsafe partial class IEventNotifier : _Base_IEventNotifier {}
	public unsafe class _Base_IEventNotifier : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetEventHandle
					var ret = GetEventHandle();
					om.Initialize(0, 1, 0);
					om.Copy(0, CreateHandle(ret, copy: true));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IEventNotifier: {im.CommandId}");
			}
		}
		
		public virtual object GetEventHandle() => throw new NotImplementedException();
	}
	
	public unsafe partial class IFile : _Base_IFile {}
	public unsafe class _Base_IFile : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Read
					Read(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), out var _0, im.GetBuffer<byte>(0x46, 0));
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					break;
				}
				case 1: { // Write
					Write(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), im.GetBuffer<byte>(0x45, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Flush
					Flush();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // SetSize
					SetSize(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // GetSize
					var ret = GetSize();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 5: { // OperateRange
					OperateRange(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), out var _0);
					om.Initialize(0, 0, 64);
					om.SetBytes(8, _0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IFile: {im.CommandId}");
			}
		}
		
		public virtual void Read(uint _0, ulong offset, ulong size, out ulong out_size, Buffer<byte> out_buf) => throw new NotImplementedException();
		public virtual void Write(uint _0, ulong offset, ulong size, Buffer<byte> in_buf) => "Stub hit for Nn.Fssrv.Sf.IFile.Write [1]".Debug();
		public virtual void Flush() => "Stub hit for Nn.Fssrv.Sf.IFile.Flush [2]".Debug();
		public virtual void SetSize(ulong size) => "Stub hit for Nn.Fssrv.Sf.IFile.SetSize [3]".Debug();
		public virtual ulong GetSize() => throw new NotImplementedException();
		public virtual void OperateRange(uint _0, ulong _1, ulong _2, out byte[] _3) => throw new NotImplementedException();
	}
	
	public unsafe partial class IFileSystem : _Base_IFileSystem {}
	public unsafe class _Base_IFileSystem : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // CreateFile
					CreateFile(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // DeleteFile
					DeleteFile(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // CreateDirectory
					CreateDirectory(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // DeleteDirectory
					DeleteDirectory(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // DeleteDirectoryRecursively
					DeleteDirectoryRecursively(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // RenameFile
					RenameFile(im.GetBuffer<byte>(0x19, 0), im.GetBuffer<byte>(0x19, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // RenameDirectory
					RenameDirectory(im.GetBuffer<byte>(0x19, 0), im.GetBuffer<byte>(0x19, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // GetEntryType
					var ret = GetEntryType(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 8: { // OpenFile
					var ret = OpenFile(im.GetData<uint>(8), im.GetBuffer<byte>(0x19, 0));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 9: { // OpenDirectory
					var ret = OpenDirectory(im.GetData<uint>(8), im.GetBuffer<byte>(0x19, 0));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 10: { // Commit
					Commit();
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // GetFreeSpaceSize
					var ret = GetFreeSpaceSize(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 12: { // GetTotalSpaceSize
					var ret = GetTotalSpaceSize(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 13: { // CleanDirectoryRecursively
					CleanDirectoryRecursively(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 14: { // GetFileTimeStampRaw
					GetFileTimeStampRaw(im.GetBuffer<byte>(0x19, 0), out var _0);
					om.Initialize(0, 0, 32);
					om.SetBytes(8, _0);
					break;
				}
				case 15: { // QueryEntry
					QueryEntry(im.GetData<uint>(8), im.GetBuffer<byte>(0x19, 0), im.GetBuffer<byte>(0x45, 0), im.GetBuffer<byte>(0x46, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IFileSystem: {im.CommandId}");
			}
		}
		
		public virtual void CreateFile(uint mode, ulong size, Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.CreateFile [0]".Debug();
		public virtual void DeleteFile(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.DeleteFile [1]".Debug();
		public virtual void CreateDirectory(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.CreateDirectory [2]".Debug();
		public virtual void DeleteDirectory(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.DeleteDirectory [3]".Debug();
		public virtual void DeleteDirectoryRecursively(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.DeleteDirectoryRecursively [4]".Debug();
		public virtual void RenameFile(Buffer<byte> old_path, Buffer<byte> new_path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.RenameFile [5]".Debug();
		public virtual void RenameDirectory(Buffer<byte> old_path, Buffer<byte> new_path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.RenameDirectory [6]".Debug();
		public virtual Nn.Fssrv.Sf.DirectoryEntryType GetEntryType(Buffer<byte> path) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IFile OpenFile(uint mode, Buffer<byte> path) => throw new NotImplementedException();
		public virtual Nn.Fssrv.Sf.IDirectory OpenDirectory(uint filter_flags, Buffer<byte> path) => throw new NotImplementedException();
		public virtual void Commit() => "Stub hit for Nn.Fssrv.Sf.IFileSystem.Commit [10]".Debug();
		public virtual ulong GetFreeSpaceSize(Buffer<byte> path) => throw new NotImplementedException();
		public virtual ulong GetTotalSpaceSize(Buffer<byte> path) => throw new NotImplementedException();
		public virtual void CleanDirectoryRecursively(Buffer<byte> path) => "Stub hit for Nn.Fssrv.Sf.IFileSystem.CleanDirectoryRecursively [13]".Debug();
		public virtual void GetFileTimeStampRaw(Buffer<byte> path, out byte[] timestamp) => throw new NotImplementedException();
		public virtual void QueryEntry(uint _0, Buffer<byte> path, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
	}
	
	public unsafe partial class ISaveDataExporter : _Base_ISaveDataExporter {}
	public unsafe class _Base_ISaveDataExporter : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(im.GetBuffer<byte>(0x1A, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					var ret = Unknown1();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 16: { // Unknown16
					Unknown16(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					break;
				}
				case 17: { // Unknown17
					Unknown17(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISaveDataExporter: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual ulong Unknown1() => throw new NotImplementedException();
		public virtual void Unknown16(out ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown17(Buffer<byte> _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class ISaveDataImporter : _Base_ISaveDataImporter {}
	public unsafe class _Base_ISaveDataImporter : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(im.GetBuffer<byte>(0x1A, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					var ret = Unknown1();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 16: { // Unknown16
					Unknown16(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 17: { // Unknown17
					Unknown17();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISaveDataImporter: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual ulong Unknown1() => throw new NotImplementedException();
		public virtual void Unknown16(Buffer<byte> _0) => "Stub hit for Nn.Fssrv.Sf.ISaveDataImporter.Unknown16 [16]".Debug();
		public virtual void Unknown17() => "Stub hit for Nn.Fssrv.Sf.ISaveDataImporter.Unknown17 [17]".Debug();
	}
	
	public unsafe partial class ISaveDataInfoReader : _Base_ISaveDataInfoReader {}
	public unsafe class _Base_ISaveDataInfoReader : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // ReadSaveDataInfo
					ReadSaveDataInfo(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISaveDataInfoReader: {im.CommandId}");
			}
		}
		
		public virtual void ReadSaveDataInfo(out ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
	}
	
	public unsafe partial class ISaveDataTransferManager : _Base_ISaveDataTransferManager {}
	public unsafe class _Base_ISaveDataTransferManager : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 16: { // Unknown16
					Unknown16(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 32: { // Unknown32
					var ret = Unknown32(im.GetData<byte>(8), im.GetData<ulong>(16));
					om.Initialize(1, 0, 0);
					om.Move(0, CreateHandle(ret));
					break;
				}
				case 64: { // Unknown64
					Unknown64(im.GetData<byte>(8), im.GetBytes(9, 0x10), im.GetBuffer<byte>(0x5, 0), out var _0, out var _1);
					om.Initialize(1, 0, 8);
					om.SetData(8, _0);
					om.Move(0, CreateHandle(_1));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISaveDataTransferManager: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void Unknown16(Buffer<byte> _0) => "Stub hit for Nn.Fssrv.Sf.ISaveDataTransferManager.Unknown16 [16]".Debug();
		public virtual Nn.Fssrv.Sf.ISaveDataExporter Unknown32(byte _0, ulong _1) => throw new NotImplementedException();
		public virtual void Unknown64(byte _0, byte[] _1, Buffer<byte> _2, out ulong _3, out Nn.Fssrv.Sf.ISaveDataImporter _4) => throw new NotImplementedException();
	}
	
	public unsafe partial class IStorage : _Base_IStorage {}
	public unsafe class _Base_IStorage : IpcInterface {
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Read
					Read(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x46, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Write
					Write(im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetBuffer<byte>(0x45, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Flush
					Flush();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // SetSize
					SetSize(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // GetSize
					var ret = GetSize();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 5: { // OperateRange
					OperateRange(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), out var _0);
					om.Initialize(0, 0, 64);
					om.SetBytes(8, _0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IStorage: {im.CommandId}");
			}
		}
		
		public virtual void Read(ulong offset, ulong length, Buffer<byte> data) => throw new NotImplementedException();
		public virtual void Write(ulong offset, ulong length, Buffer<byte> data) => "Stub hit for Nn.Fssrv.Sf.IStorage.Write [1]".Debug();
		public virtual void Flush() => "Stub hit for Nn.Fssrv.Sf.IStorage.Flush [2]".Debug();
		public virtual void SetSize(ulong size) => "Stub hit for Nn.Fssrv.Sf.IStorage.SetSize [3]".Debug();
		public virtual ulong GetSize() => throw new NotImplementedException();
		public virtual void OperateRange(uint _0, ulong _1, ulong _2, out byte[] _3) => throw new NotImplementedException();
	}
}
