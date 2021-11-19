using System;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHdcpUpCtrl {
	class NvHdcpUpCtrlDeviceFile : NvDeviceFile {
		static readonly Logger Logger = new("NvHdcpUpCtrlDeviceFile");
		static Action<string> Log = Logger.Log;
        
		public NvHdcpUpCtrlDeviceFile(IVirtualMemoryManager memory, long owner) : base(owner) {
		}

		public override NvInternalResult Ioctl(NvIoctl command, Span<byte> arguments) {
			var result = NvInternalResult.NotImplemented;
			Log($"Ioctl to NvHdcpUpCtrlDeviceFile! Command 0x{command.Number:X} Type 0x{command.Type:X}");

			if(command.Type == NvIoctl.NvHdcpUpMagic)
				switch(command.Number) {
					default:
						Log($"Unsupported ioctl to NvHdcpUpCtrlDeviceFile: 0x{command.Number:X}");
						result = NvInternalResult.Success;
						break;
				}

			return result;
		}

		public override NvInternalResult Ioctl3(NvIoctl command, Span<byte> arguments, Span<byte> inlineOutBuffer) {
			var result = NvInternalResult.NotImplemented;

			if(command.Type == NvIoctl.NvDispCtrlMagic)
				switch(command.Number) {
					default:
						Log($"Unsupported ioctl3 to NvDispDeviceFile: 0x{command.Number:X}");
						break;
				}

			return result;
		}

		public override void Close() { }
	}
}