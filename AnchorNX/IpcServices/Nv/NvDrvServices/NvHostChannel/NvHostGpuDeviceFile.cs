using System;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel.Types;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel {
	class NvHostGpuDeviceFile : NvHostChannelDeviceFile {
		/*readonly KEvent _errorNotifierEvent;
		readonly KEvent _smExceptionBptIntReportEvent;
		readonly KEvent _smExceptionBptPauseReportEvent;*/

		public NvHostGpuDeviceFile(IVirtualMemoryManager memory, long owner) : base(memory, owner) {
			/*_smExceptionBptIntReportEvent = new KEvent(context.Device.System.KernelContext);
			_smExceptionBptPauseReportEvent = new KEvent(context.Device.System.KernelContext);
			_errorNotifierEvent = new KEvent(context.Device.System.KernelContext);*/
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

		public override NvInternalResult QueryEvent(out int eventHandle, uint eventId) {
			// TODO: accurately represent and implement those events.
			/*KEvent targetEvent = null;

			switch(eventId) {
				case 0x1:
					targetEvent = _smExceptionBptIntReportEvent;
					break;
				case 0x2:
					targetEvent = _smExceptionBptPauseReportEvent;
					break;
				case 0x3:
					targetEvent = _errorNotifierEvent;
					break;
			}

			if(targetEvent != null) {
				if(Context.Process.HandleTable.GenerateHandle(targetEvent.ReadableEvent, out eventHandle) !=
				   KernelResult.Success) throw new InvalidOperationException("Out of handles!");
			} else {
				eventHandle = 0;

				return NvInternalResult.InvalidInput;
			}*/
			throw new NotImplementedException();

			return NvInternalResult.Success;
		}

		NvInternalResult SubmitGpfifoEx(ref SubmitGpfifoArguments arguments, Span<ulong> inlineData) {
			return SubmitGpfifo(ref arguments, inlineData);
		}
	}
}