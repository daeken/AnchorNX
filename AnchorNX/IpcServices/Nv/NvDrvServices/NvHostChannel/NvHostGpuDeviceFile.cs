using System;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel.Types;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel {
	class NvHostGpuDeviceFile : NvHostChannelDeviceFile {
		readonly HosEvent _errorNotifierEvent;
		readonly HosEvent _smExceptionBptIntReportEvent;
		readonly HosEvent _smExceptionBptPauseReportEvent;

		public NvHostGpuDeviceFile(IVirtualMemoryManager memory, long owner) : base(memory, owner) {
			_smExceptionBptIntReportEvent = Box.EventManager.GetEvent();
			_smExceptionBptPauseReportEvent = Box.EventManager.GetEvent();
			_errorNotifierEvent = Box.EventManager.GetEvent();
		}

		public override NvInternalResult Ioctl2(NvIoctl command, Span<byte> arguments, Span<byte> inlineInBuffer) {
			var result = NvInternalResult.NotImplemented;

			if(command.Type == NvIoctl.NvHostMagic)
				switch(command.Number) {
					case 0x1b:
						result = CallIoctlMethod<SubmitGpfifoArguments, ulong>(SubmitGpfifoEx, arguments,
							inlineInBuffer);
						break;
				}

			return result;
		}

		public override NvInternalResult QueryEvent(out HosEvent eventHandle, uint eventId) {
			// TODO: accurately represent and implement those events.
			switch(eventId) {
				case 0x1:
					eventHandle = _smExceptionBptIntReportEvent;
					break;
				case 0x2:
					eventHandle = _smExceptionBptPauseReportEvent;
					break;
				case 0x3:
					eventHandle = _errorNotifierEvent;
					break;
				default:
					eventHandle = null;
					return NvInternalResult.InvalidInput;
			}

			return NvInternalResult.Success;
		}

		NvInternalResult SubmitGpfifoEx(ref SubmitGpfifoArguments arguments, Span<ulong> inlineData) {
			return SubmitGpfifo(ref arguments, inlineData);
		}
	}
}