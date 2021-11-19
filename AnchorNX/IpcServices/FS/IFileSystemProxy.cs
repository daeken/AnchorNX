using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using LibHac.Fs;
using LibHac.FsSystem;
using LibHac.FsSystem.NcaUtils;
using LibHac.FsSystem.Save;

namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IFileSystemProxy {
		public override IFileSystem OpenFileSystem(FileSystemType filesystem_type, Buffer<byte> _1) => throw new NotImplementedException();
		public override void SetCurrentProcess(ulong _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetCurrentProcess [1]".Debug(Log);
		public override IFileSystem OpenDataFileSystemByCurrentProcess() => throw new NotImplementedException();
		public override IFileSystem OpenFileSystemWithPatch(FileSystemType filesystem_type, ulong id) => throw new NotImplementedException();
		public override IFileSystem OpenFileSystemWithId(FileSystemType filesystem_type, ulong tid, Buffer<byte> _2) => throw new NotImplementedException();
		public override IFileSystem OpenDataFileSystemByApplicationId(ulong u64) => throw new NotImplementedException();
		public override IFileSystem OpenBisFileSystem(Partition partitionId, Buffer<byte> _1) => throw new NotImplementedException();
		public override IStorage OpenBisStorage(Partition partitionId) {
			Console.WriteLine($"Attempting to open BIS storage for {partitionId}");
			return new(new LocalStorage(partitionId switch {
				Partition.CalibrationBinary => "switchroot/raw/PRODINFO.bin", 
				_ => throw new NotImplementedException()
			}, FileAccess.Read));
		}

		public override void InvalidateBisCache() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.InvalidateBisCache [13]".Debug(Log);
		public override IFileSystem OpenHostFileSystem(Buffer<byte> _0) => throw new NotImplementedException();
		public override IFileSystem OpenSdCardFileSystem() => throw new NotImplementedException();
		public override void FormatSdCardFileSystem() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.FormatSdCardFileSystem [19]".Debug(Log);
		public override void DeleteSaveDataFileSystem(ulong tid) {
			Log($"Attempting to delete save data filesystem for {tid:X16}");
			var fn = $"switchroot/system/save/{tid:X16}";
			if(Directory.Exists(fn)) {
				Directory.Delete(Path.Join(fn, "0"), true);
				Directory.Delete(Path.Join(fn, "1"), true);
				Directory.CreateDirectory(Path.Join(fn, "0"));
				Directory.CreateDirectory(Path.Join(fn, "1"));
			}
		}

		public override void CreateSaveDataFileSystem(byte[] save_struct, byte[] ave_create_struct, byte[] _2) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CreateSaveDataFileSystem [22]".Debug(Log);
		public override void CreateSaveDataFileSystemBySystemSaveDataId(byte[] _0, byte[] _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CreateSaveDataFileSystemBySystemSaveDataId [23]".Debug(Log);
		public override void RegisterSaveDataFileSystemAtomicDeletion(Buffer<byte> _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.RegisterSaveDataFileSystemAtomicDeletion [24]".Debug(Log);
		public override void DeleteSaveDataFileSystemBySaveDataSpaceId(byte _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.DeleteSaveDataFileSystemBySaveDataSpaceId [25]".Debug(Log);
		public override void FormatSdCardDryRun() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.FormatSdCardDryRun [26]".Debug(Log);
		public override byte IsExFatSupported() => throw new NotImplementedException();
		public override void DeleteSaveDataFileSystemBySaveDataAttribute(byte _0, byte[] _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.DeleteSaveDataFileSystemBySaveDataAttribute [28]".Debug(Log);
		public override IStorage OpenGameCardStorage(uint _0, uint _1) => throw new NotImplementedException();
		public override IFileSystem OpenGameCardFileSystem(uint _0, uint _1) => throw new NotImplementedException();
		public override void ExtendSaveDataFileSystem(byte _0, ulong _1, ulong _2, ulong _3) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.ExtendSaveDataFileSystem [32]".Debug(Log);
		public override object DeleteCacheStorage(object _0) => throw new NotImplementedException();
		public override object GetCacheStorageSize(object _0) => throw new NotImplementedException();

		public override IFileSystem OpenSaveDataFileSystem(byte save_data_space_id, byte[] save_struct) => throw new NotImplementedException();

		public unsafe override IFileSystem OpenSaveDataFileSystemBySystemSaveDataId(byte save_data_space_id, byte[] save_struct) {
			var saveData = ((Span<byte>) save_struct).As<byte, SaveDataAttribute>()[0];
			Log($"Save data id 0x{save_data_space_id:X}");
			Log($"Save app id 0x{saveData.ProgramId:X}");
			Log($"Save user id 0x{saveData.UserId[0]:X} 0x{saveData.UserId[1]:X}");
			Log($"Save sys id 0x{saveData.StaticSaveDataId:X}");

			try {
				return new($"System save {saveData.StaticSaveDataId:X16}", new DirectorySaveDataFileSystem(new LocalFileSystem($"switchroot/system/save/{saveData.StaticSaveDataId:X16}/")));
			} catch(Exception) {
				Log("Could not find save");
				throw new IpcException(0x7D402);
			}
		}
		
		public override IFileSystem OpenReadOnlySaveDataFileSystem(byte save_data_space_id, byte[] save_struct) => throw new NotImplementedException();
		public override void ReadSaveDataFileSystemExtraDataBySaveDataSpaceId(byte spaceId, ulong saveId, Buffer<byte> extraDataBuf) {
			Log($"ReadSaveDataFileSystemExtraDataBySaveDataSpaceId for space ID 0x{spaceId:X} and save ID 0x{saveId:X}");
			extraDataBuf.CopyFrom(ReadSaveExtraData(saveId));
		}

		public override void ReadSaveDataFileSystemExtraData(ulong saveId, Buffer<byte> extraDataBuf) {
			Log($"ReadSaveDataFileSystemExtraData for 0x{saveId:X}");
			extraDataBuf.CopyFrom(ReadSaveExtraData(saveId));
		}

		Span<byte> ReadSaveExtraData(ulong saveId) {
			var sd = new DirectorySaveDataFileSystem(new LocalFileSystem($"switchroot/system/save/{saveId:X16}/"));
			var rc = sd.ReadExtraData(out var ed);
			//if(rc.IsFailure()) throw new IpcException(rc.Value);
			if(rc.IsFailure()) ed = new SaveDataExtraData();
			return MemoryMarshal.Cast<SaveDataExtraData, byte>(new[] { ed });
		}

		void WriteSaveExtraData(ulong saveId, ReadOnlySpan<byte> data) {
			var sd = new DirectorySaveDataFileSystem(new LocalFileSystem($"switchroot/system/save/{saveId:X16}/"));
			var rc = sd.WriteExtraData(MemoryMarshal.Cast<byte, SaveDataExtraData>(data)[0]);
			if(rc.IsFailure()) throw new IpcException(rc.Value);
		}

		public override void WriteSaveDataFileSystemExtraData(byte spaceId, ulong saveId, Buffer<byte> _2) =>
			WriteSaveExtraData(saveId, _2.SafeSpan);
		public override ISaveDataInfoReader OpenSaveDataInfoReader() => new();
		public override ISaveDataInfoReader OpenSaveDataInfoReaderBySaveDataSpaceId(byte _0) => new();
		public override object OpenCacheStorageList(object _0) => throw new NotImplementedException();
		public override object OpenSaveDataInternalStorageFileSystem(object _0) => throw new NotImplementedException();
		public override object UpdateSaveDataMacForDebug(object _0) => throw new NotImplementedException();
		public override object WriteSaveDataFileSystemExtraData2(object _0) => throw new NotImplementedException();

		public override int FindSaveDataWithFilter(ulong _0, SaveDataFilter filter, Buffer<byte> data) {
			Log("FindSaveDataWithFilter stub");
			return 0;
		}

		public override IFile OpenSaveDataMetaFile(byte _0, uint _1, byte[] _2) => throw new NotImplementedException();
		public override ISaveDataTransferManager OpenSaveDataTransferManager() => throw new NotImplementedException();
		public override object OpenSaveDataTransferManagerVersion2(object _0) => throw new NotImplementedException();

		public override IFileSystem OpenImageDirectoryFileSystem(uint _0) =>
			new("Album", new LocalFileSystem("switchroot/user/Album"));

		public override IFileSystem OpenContentStorageFileSystem(uint content_storage_id) => new("system contents", 
			content_storage_id switch {
				0 => new LocalFileSystem("switchroot/system/Contents"), 
				1 => new LocalFileSystem("switchroot/user/Contents"), 
				_ => throw new NotImplementedException($"Unknown content id {content_storage_id}")
			});
		
		public override IStorage OpenDataStorageByCurrentProcess() => throw new NotImplementedException();
		
		public override IStorage OpenDataStorageByProgramId(ulong tid) => throw new NotImplementedException();

		public override async Task<IStorage> OpenDataStorageByDataId(byte storage_id, ulong tid) {
			Log($"Attempting to OpenDataStorageByDataId: {storage_id:X} -- {tid:X}");
			Debug.Assert(storage_id == 3); // System
			
			var fn = await MapTidToNca(storage_id, tid);
			if(fn == null) {
				Log($"Could not find NCA for title id {tid:X}");
				throw new IpcException(0x7D402);
			}

			Debug.Assert(fn.StartsWith("@SystemContent://"));
			var ncaFn = $"switchroot/system/Contents/{fn.Split('/', 2)[1]}";
			var nca = new Nca(Box.KeySet, new LocalStorage(ncaFn, FileAccess.Read));
			return new IStorage(nca.OpenStorage(NcaSectionType.Data, IntegrityCheckLevel.ErrorOnInvalid));
		}

		async Task<string> MapTidToNca(byte storage_id, ulong tid) {
			var (rc, resolver) = await Box.HvcProxy.OpenLocationResolver(storage_id);
			Log($"HvcProxy: OpenLocationResolver? {rc:X} {resolver:X}");
			var (res, path) = await Box.HvcProxy.ResolveDataPath(resolver, tid);
			Log($"HvcProxy: ResolveDataPath? {res:X} '{path}'");
			return path;
		}
		
		public override IStorage OpenPatchDataStorageByCurrentProcess() => throw new NotImplementedException();
		public override IDeviceOperator OpenDeviceOperator() => new();
		public override IEventNotifier OpenSdCardDetectionEventNotifier() => new();
		public override IEventNotifier OpenGameCardDetectionEventNotifier() => new();
		public override IEventNotifier OpenSystemDataUpdateEventNotifier(object _0) => new();
		public override object NotifySystemDataUpdateEvent(object _0) => throw new NotImplementedException();
		public override void SetCurrentPosixTime(ulong time) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetCurrentPosixTime [600]".Debug(Log);
		public override ulong QuerySaveDataTotalSize(ulong _0, ulong _1) => throw new NotImplementedException();
		public override void VerifySaveDataFileSystem(ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override void CorruptSaveDataFileSystem(ulong _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CorruptSaveDataFileSystem [603]".Debug(Log);
		public override void CreatePaddingFile(ulong _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CreatePaddingFile [604]".Debug(Log);
		public override void DeleteAllPaddingFiles() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.DeleteAllPaddingFiles [605]".Debug(Log);
		public override void GetRightsId(byte _0, ulong _1, out byte[] rights) => throw new NotImplementedException();
		public override void RegisterExternalKey(byte[] _0, byte[] _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.RegisterExternalKey [607]".Debug(Log);
		public override void UnregisterAllExternalKey() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.UnregisterAllExternalKey [608]".Debug(Log);
		public override void GetRightsIdByPath(Buffer<byte> _0, out byte[] rights) => throw new NotImplementedException();
		public override void GetRightsIdAndKeyGenerationByPath(Buffer<byte> _0, out byte _1, out byte[] rights) => throw new NotImplementedException();
		public override void SetCurrentPosixTimeWithTimeDifference(uint _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetCurrentPosixTimeWithTimeDifference [611]".Debug(Log);
		public override ulong GetFreeSpaceSizeForSaveData(byte _0) => throw new NotImplementedException();
		public override void VerifySaveDataFileSystemBySaveDataSpaceId(byte _0, ulong _1, Buffer<byte> _2) => throw new NotImplementedException();
		public override void CorruptSaveDataFileSystemBySaveDataSpaceId(byte _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.CorruptSaveDataFileSystemBySaveDataSpaceId [614]".Debug(Log);
		public override object QuerySaveDataInternalStorageTotalSize(object _0) => throw new NotImplementedException();
		public override void SetSdCardEncryptionSeed(byte[] _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetSdCardEncryptionSeed [620]".Debug(Log);
		public override void SetSdCardAccessibility(byte _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetSdCardAccessibility [630]".Debug(Log);
		public override byte IsSdCardAccessible() => throw new NotImplementedException();
		public override byte IsSignedSystemPartitionOnSdCardValid() => throw new NotImplementedException();
		public override object OpenAccessFailureResolver(object _0) => throw new NotImplementedException();
		public override object GetAccessFailureDetectionEvent(object _0) => throw new NotImplementedException();
		public override object IsAccessFailureDetected(object _0) => throw new NotImplementedException();
		public override object ResolveAccessFailure(object _0) => throw new NotImplementedException();
		public override object AbandonAccessFailure(object _0) => throw new NotImplementedException();
		public override void GetAndClearFileSystemProxyErrorInfo(out byte[] error_info) => error_info = Array.Empty<byte>();
		public override void SetBisRootForHost(uint _0, Buffer<byte> _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetBisRootForHost [1000]".Debug(Log);
		public override void SetSaveDataSize(ulong _0, ulong _1) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetSaveDataSize [1001]".Debug(Log);
		public override void SetSaveDataRootPath(Buffer<byte> _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetSaveDataRootPath [1002]".Debug(Log);
		public override void DisableAutoSaveDataCreation() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.DisableAutoSaveDataCreation [1003]".Debug(Log);
		public override void SetGlobalAccessLogMode(uint mode) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.SetGlobalAccessLogMode [1004]".Debug(Log);
		public override uint GetGlobalAccessLogMode() => 0;
		public override void OutputAccessLogToSdCard(Buffer<byte> log_text) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.OutputAccessLogToSdCard [1006]".Debug(Log);
		public override void RegisterUpdatePartition() => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.RegisterUpdatePartition [1007]".Debug(Log);
		public override IFileSystem OpenRegisteredUpdatePartition() => throw new NotImplementedException();
		public override void GetAndClearMemoryReportInfo(out byte[] report_info) => report_info = Array.Empty<byte>();
		public override object Unknown1010(object _0) => throw new NotImplementedException();
		public override void OverrideSaveDataTransferTokenSignVerificationKey(Buffer<byte> _0) => "Stub hit for Nn.Fssrv.Sf.IFileSystemProxy.OverrideSaveDataTransferTokenSignVerificationKey [1100]".Debug(Log);
	}
}