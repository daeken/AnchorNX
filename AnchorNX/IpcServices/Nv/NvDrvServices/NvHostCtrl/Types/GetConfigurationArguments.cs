using System;
using System.Text;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrl.Types {
	class GetConfigurationArguments {
		public byte[] Configuration;
		public string Domain;
		public string Parameter;

		public static GetConfigurationArguments FromSpan(Span<byte> span) {
			var domain = Encoding.ASCII.GetString(span.Slice(0, 0x41));
			var parameter = Encoding.ASCII.GetString(span.Slice(0x41, 0x41));

			var result = new GetConfigurationArguments {
				Domain = domain.Substring(0, domain.IndexOf('\0')),
				Parameter = parameter.Substring(0, parameter.IndexOf('\0')),
				Configuration = span.Slice(0x82, 0x101).ToArray()
			};

			return result;
		}

		public void CopyTo(Span<byte> span) {
			Encoding.ASCII.GetBytes(Domain + '\0').CopyTo(span.Slice(0, 0x41));
			Encoding.ASCII.GetBytes(Parameter + '\0').CopyTo(span.Slice(0x41, 0x41));
			Configuration.CopyTo(span.Slice(0x82, 0x101));
		}
	}
}