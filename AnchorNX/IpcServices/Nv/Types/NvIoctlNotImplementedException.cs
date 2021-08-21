using System;
using System.Text;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.Types {
	class NvIoctlNotImplementedException : Exception {
		public NvIoctlNotImplementedException(NvDeviceFile deviceFile, NvIoctl command)
			: this(deviceFile, command, "The ioctl is not implemented.") { }

		public NvIoctlNotImplementedException(NvDeviceFile deviceFile, NvIoctl command,
			string message
		)
			: base(message) {
			DeviceFile = deviceFile;
			Command = command;
		}

		public NvDeviceFile DeviceFile { get; }
		public NvIoctl Command { get; }

		public override string Message =>
			base.Message +
			Environment.NewLine +
			Environment.NewLine +
			BuildMessage();

		string BuildMessage() {
			var sb = new StringBuilder();

			sb.AppendLine($"Device File: {DeviceFile.GetType().Name}");
			sb.AppendLine();

			sb.AppendLine($"Ioctl (0x{Command.RawValue:x8})");
			sb.AppendLine($"\tNumber: 0x{Command.Number:x8}");
			sb.AppendLine($"\tType: 0x{Command.Type:x8}");
			sb.AppendLine($"\tSize: 0x{Command.Size:x8}");
			sb.AppendLine($"\tDirection: {Command.DirectionValue}");

			return sb.ToString();
		}
	}
}