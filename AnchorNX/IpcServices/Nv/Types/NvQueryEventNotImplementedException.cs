using System;
using System.Text;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.Types {
	class NvQueryEventNotImplementedException : Exception {
		public NvQueryEventNotImplementedException(NvDeviceFile deviceFile, uint eventId)
			: this(deviceFile, eventId, "This query event is not implemented.") { }

		public NvQueryEventNotImplementedException(NvDeviceFile deviceFile, uint eventId,
			string message
		)
			: base(message) {
			DeviceFile = deviceFile;
			EventId = eventId;
		}

		public NvDeviceFile DeviceFile { get; }
		public uint EventId { get; }

		public override string Message =>
			base.Message +
			Environment.NewLine +
			Environment.NewLine +
			BuildMessage();

		string BuildMessage() {
			var sb = new StringBuilder();

			sb.AppendLine($"Device File: {DeviceFile.GetType().Name}");
			sb.AppendLine();

			sb.AppendLine($"Event ID: (0x{EventId:x8})");

			return sb.ToString();
		}
	}
}