using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AnchorNX.IpcServices.Nns.Nvdrv.Nv;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvDisp;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvDispCtrl;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHdcpUpCtrl;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostAsGpu;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrl;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrlGpu;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvSchedCtrl;
using Ryujinx.Common.Logging;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv {
	public partial class INvDrvServices {
		static readonly Logger Logger = new("INvDrvServices");
		static Action<string> Log = Logger.Log;

		static Dictionary<string, Type> DeviceFileRegistry = new() {
			{ "/dev/nvmap", typeof(NvMapDeviceFile) },
			{ "/dev/nvhost-ctrl", typeof(NvHostCtrlDeviceFile) },
			{ "/dev/nvhost-ctrl-gpu", typeof(NvHostCtrlGpuDeviceFile) },
			{ "/dev/nvhost-as-gpu", typeof(NvHostAsGpuDeviceFile) },
			{ "/dev/nvhost-gpu", typeof(NvHostGpuDeviceFile) },
			//{ "/dev/nvhost-msenc",    typeof(NvHostChannelDeviceFile) },
			{ "/dev/nvhost-nvdec", typeof(NvHostChannelDeviceFile) },
			//{ "/dev/nvhost-nvjpg",    typeof(NvHostChannelDeviceFile) },
			{ "/dev/nvhost-vic", typeof(NvHostChannelDeviceFile) }, 
			//{ "/dev/nvhost-display",  typeof(NvHostChannelDeviceFile) },
			{ "/dev/nvdisp-ctrl", typeof(NvDispCtrlDeviceFile) }, 
			{ "/dev/nvdisp-disp0", typeof(NvDispDeviceFile) }, 
			{ "/dev/nvdisp-disp1", typeof(NvDispDeviceFile) }, 
			{ "/dev/nvsched-ctrl", typeof(NvSchedCtrlDeviceFile) }, 
			{ "/dev/nvhdcp_up-ctrl", typeof(NvHdcpUpCtrlDeviceFile) }, 
		};

		public static IdDictionary DeviceFileIdRegistry = new();

		IVirtualMemoryManager ClientMemory;
		long Owner;

		bool TransferMemInitialized = false;

		public override void Open(Buffer<byte> pathBytes, out uint fd, out uint error_code) {
			var path = Encoding.ASCII.GetString(pathBytes.Span);
			Log($"Attempting to open nvdrv device '{path}'");
			if(DeviceFileRegistry.TryGetValue(path, out Type deviceFileClass)) {
				var constructor = deviceFileClass.GetConstructor(new Type[] { typeof(IVirtualMemoryManager), typeof(long) });

				var deviceFile = (NvDeviceFile) constructor.Invoke(new object[] { ClientMemory, Owner });
				deviceFile.Path = path;

				fd = (uint) DeviceFileIdRegistry.Add(deviceFile);
				error_code = 0;
			} else {
				Log($"Cannot find file device \"{path}\"!");
				throw new NotImplementedException($"Cannot find file device \"{path}\"!");
				fd = 0xFFFFFFFF;
				error_code = (uint) NvResult.FileOperationFailed;
			}
		}

		public override void Ioctl(IncomingMessage im, OutgoingMessage om) {
			var (fd, rq_id) = (im.GetData<uint>(8), im.GetData<uint>(12));
			var ibuf = im.GetBuffer<byte>(0x21, 0);
			var obuf = im.GetBuffer<byte>(0x22, 0);
			var ioctlCommand = new NvIoctl(rq_id);
			if((ioctlCommand.DirectionValue & NvIoctl.Direction.Write) != 0 && obuf == null) {
				om.Initialize(0, 0, 4, (int) ioctlCommand.Size);
				obuf = om.GetXBuffer<byte>(0);
			} else
				om.Initialize(0, 0, 4);

			var errorCode = GetIoctlArgument(ioctlCommand, ibuf, obuf, out var arguments);
			if(errorCode == NvResult.Success) {
				errorCode = GetDeviceFileFromFd(fd, out var deviceFile);
				if(errorCode == NvResult.Success) {
					NvInternalResult internalResult = deviceFile.Ioctl(ioctlCommand, arguments);

					if (internalResult == NvInternalResult.NotImplemented)
						throw new NotImplementedException($"Unimplemented ioctl 0x{ioctlCommand.Number:X} (0x{rq_id:X}) for {deviceFile}");

					errorCode = ConvertInternalErrorCode(internalResult);
				}
			}
			om.SetData(8, (uint) errorCode);
		}

		static NvResult ConvertInternalErrorCode(NvInternalResult errorCode) =>
			errorCode switch {
				NvInternalResult.Success => NvResult.Success,
				NvInternalResult.Unknown0x72 => NvResult.AlreadyAllocated,
				NvInternalResult.TimedOut => NvResult.Timeout,
				NvInternalResult.TryAgain => NvResult.Timeout,
				NvInternalResult.Interrupted => NvResult.Timeout,
				NvInternalResult.InvalidAddress => NvResult.InvalidAddress,
				NvInternalResult.NotSupported => NvResult.NotSupported,
				NvInternalResult.Unknown0x18 => NvResult.NotSupported,
				NvInternalResult.InvalidState => NvResult.InvalidState,
				NvInternalResult.ReadOnlyAttribute => NvResult.ReadOnlyAttribute,
				NvInternalResult.NoSpaceLeft => NvResult.InvalidSize,
				NvInternalResult.FileTooBig => NvResult.InvalidSize,
				NvInternalResult.FileTableOverflow => NvResult.FileOperationFailed,
				NvInternalResult.BadFileNumber => NvResult.FileOperationFailed,
				NvInternalResult.InvalidInput => NvResult.InvalidValue,
				NvInternalResult.NotADirectory => NvResult.DirectoryOperationFailed,
				NvInternalResult.Busy => NvResult.Busy,
				NvInternalResult.BadAddress => NvResult.InvalidAddress,
				NvInternalResult.AccessDenied => NvResult.AccessDenied,
				NvInternalResult.OperationNotPermitted => NvResult.AccessDenied,
				NvInternalResult.OutOfMemory => NvResult.InsufficientMemory,
				NvInternalResult.DeviceNotFound => NvResult.ModuleNotPresent,
				NvInternalResult.IoError => NvResult.ResourceError,
				_ => NvResult.IoctlFailed
			};

		NvResult GetDeviceFileFromFd(uint fd, out NvDeviceFile deviceFile) {
			deviceFile = null;

			if((int) fd < 0) return NvResult.InvalidParameter;

			deviceFile = DeviceFileIdRegistry.GetData<NvDeviceFile>((int) fd);

			if(deviceFile == null) {
				Log($"Invalid file descriptor {fd}");
				return NvResult.NotImplemented;
			}

			if(deviceFile.Owner != Owner)
				return NvResult.AccessDenied;

			return NvResult.Success;
		}

		NvResult GetIoctlArgument(NvIoctl ioctlCommand, Buffer<byte> ibuf, Buffer<byte> obuf, out Span<byte> arguments) {
			var ioctlDirection = ioctlCommand.DirectionValue;
			var ioctlSize = ioctlCommand.Size;

			var isRead  = (ioctlDirection & NvIoctl.Direction.Read)  != 0;
			var isWrite = (ioctlDirection & NvIoctl.Direction.Write) != 0;

			Log($"Ioctl size 0x{ioctlSize:X} obuf size 0x{obuf?.Length:X} ibuf size 0x{ibuf?.Size} -- {isWrite} {isRead}");

			if ((isWrite && ioctlSize > obuf.Length) || (isRead && ioctlSize > ibuf.Length)) {
				arguments = null;
				Log("Ioctl size inconsistency found!");
				return NvResult.InvalidSize;
			}

			if (isRead && isWrite) {
				if(obuf.Length < ibuf.Length) {
					arguments = null;
					Log("Ioctl size inconsistency found!");
					return NvResult.InvalidSize;
				}

				obuf.CopyFrom(ibuf.SafeSpan);

				arguments = obuf.Span;
			}
			else if(isWrite)
				arguments = obuf.Span;
			else
				arguments = ibuf.Span;

			return NvResult.Success;
		}

		public override uint _Close(uint fd) {
			var errorCode = GetDeviceFileFromFd(fd, out NvDeviceFile deviceFile);

			if(errorCode == NvResult.Success) {
				deviceFile.Close();
				DeviceFileIdRegistry.Delete((int) fd);
			}

			return (uint) errorCode;
		}

		public override async Task<uint> Initialize(uint transfer_memory_size, uint current_process, uint transfer_memory) {
			var (r, pid) = await Box.HvcProxy.GetProcessId(current_process);
			Owner = (long) pid;
			Log($"Initializing NV for process 0x{pid:X} -- TTBR0 0x{Box.PagetableForProcess[pid]:X}");
			Box.Gpu.RegisterProcess((long) pid, new NvVmm(Box.PagetableForProcess[pid]));
			return 0;
		}

		public override void QueryEvent(uint fd, uint event_id, out uint error_code, out uint event_handle) {
			var errorCode = GetDeviceFileFromFd(fd, out var deviceFile);
			event_handle = 0;
			if(errorCode == NvResult.Success) {
				var internalResult = deviceFile.QueryEvent(out var evt, event_id);
				if(internalResult == NvInternalResult.NotImplemented)
					throw new NotImplementedException($"QueryEvent (0x{event_id:X}) not implemented on {deviceFile}");
				errorCode = ConvertInternalErrorCode(internalResult);

				if(errorCode == NvResult.Success)
					event_handle = evt.Reader;
			}
			error_code = (uint) errorCode;
		}

		public override uint MapSharedMem(uint fd, uint nvmap_handle, uint _2) {
			throw new NotImplementedException();
		}

		public override object GetStatus() {
			throw new NotImplementedException();
		}

		public override uint ForceSetClientPID(ulong pid) {
			throw new NotImplementedException();
		}

		public override uint SetClientPID(ulong _0, ulong _1) {
			throw new NotImplementedException();
		}

		public override void DumpGraphicsMemoryInfo() {
			"Stub hit for Nns.Nvdrv.INvDrvServices.DumpGraphicsMemoryInfo [9]".Debug(Log);
		}

		public override uint Unknown10(uint _0, uint _1) {
			throw new NotImplementedException();
		}

		public override void Ioctl2(uint _0, uint _1, Buffer<byte> _2, Buffer<byte> _3, out uint _4, Buffer<byte> _5) {
			throw new NotImplementedException();
		}

		public override void Ioctl3(uint _0, uint _1, Buffer<byte> _2, out uint _3, Buffer<byte> _4, Buffer<byte> _5) {
			throw new NotImplementedException();
		}

		public override void Unknown13(object _0) {
			"Stub hit for Nns.Nvdrv.INvDrvServices.Unknown13 [13]".Debug(Log);
		}
	}
}