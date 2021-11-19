using System;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvSchedCtrl {
	class NvSchedCtrlDeviceFile : NvDeviceFile {
		static readonly Logger Logger = new("NvSchedCtrlDeviceFile");
		static Action<string> Log = Logger.Log;
        
		public NvSchedCtrlDeviceFile(IVirtualMemoryManager memory, long owner) : base(owner) {
		}

		public override NvInternalResult Ioctl(NvIoctl command, Span<byte> arguments) {
			var result = NvInternalResult.NotImplemented;
			Log($"Ioctl to NvSchedCtrlDeviceFile! Command 0x{command.Number:X} Type 0x{command.Type:X}");

			if(command.Type == NvIoctl.NvSchedCtrlMagic)
				switch(command.Number) {
					default:
						Log($"Unsupported ioctl to NvSchedCtrlDeviceFile: 0x{command.Number:X}");
						break;
				}

			return result;
		}

		public override NvInternalResult Ioctl3(NvIoctl command, Span<byte> arguments, Span<byte> inlineOutBuffer) {
			var result = NvInternalResult.NotImplemented;

			if(command.Type == NvIoctl.NvSchedCtrlMagic)
				switch(command.Number) {
					default:
						Log($"Unsupported ioctl3 to NvSchedCtrlDeviceFile: 0x{command.Number:X}");
						break;
				}

			return result;
		}
        
		public override void Close() { }
	}
}