using System;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvDisp {
	class NvDispDeviceFile : NvDeviceFile {
		static readonly Logger Logger = new("NvDispDeviceFile");
		static Action<string> Log = Logger.Log;
        
		public NvDispDeviceFile(IVirtualMemoryManager memory, long owner) : base(owner) {
		}

		public override NvInternalResult Ioctl(NvIoctl command, Span<byte> arguments) {
			var result = NvInternalResult.NotImplemented;
			Log($"Ioctl to NvDispDeviceFile! Command 0x{command.Number:X} Type 0x{command.Type:X}");

			if(command.Type == NvIoctl.NvDispCtrlMagic)
				switch(command.Number) {
					case 0x01:
						result = CallIoctlMethod<uint>(GetWindow, arguments);
						break;
					case 0x03:
						Log("Flipping!");
						Box.Gpu.Window.SignalFrameReady();
						result = NvInternalResult.Success;
						break;
					case 0x0E:
						Log("Ignoring SetCmu");
						result = NvInternalResult.Success;
						break;
					case 0x0F:
						Log("Ignoring DPMS");
						arguments.Hexdump(Logger);
						arguments[0] = 0;
						result = NvInternalResult.Success;
						break;
					case 0x1B:
						Log("Ignoring NVDISP_GET_MODE2");
						result = NvInternalResult.Success;
						break;
					case 0x1D:
						Log("Ignoring NVDISP_VALIDATE_MODE2");
						result = NvInternalResult.Success;
						break;
					case 0x1E:
						Log("Ignoring NVDISP_GET_MODE_DB2");
						arguments[0x2F1C] = 1;
						arguments[0x2F1D] = 0;
						arguments[0x2F1E] = 0;
						arguments[0x2F1F] = 0;
						result = NvInternalResult.Success;
						break;
					default:
						Log($"Unsupported ioctl to NvDispDeviceFile: 0x{command.Number:X}");
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

		// TODO: Figure out: This should be an in-only... Why is it get?!
		NvInternalResult GetWindow(ref uint window) {
			Log($"'GetWindow' 0x{window:X}");
			window = 0;
			return NvInternalResult.Success;
		}
		
		public override void Close() { }
	}
}