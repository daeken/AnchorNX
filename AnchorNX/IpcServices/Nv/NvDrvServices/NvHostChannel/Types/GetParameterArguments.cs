using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostChannel.Types {
	[StructLayout(LayoutKind.Sequential)]
	struct GetParameterArguments {
		public uint Parameter;
		public uint Value;
	}
}