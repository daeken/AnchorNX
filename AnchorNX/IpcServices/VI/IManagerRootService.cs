using System;

namespace AnchorNX.IpcServices.Nn.Visrv.Sf {
	public partial class IManagerRootService {
		public override Nn.Visrv.Sf.IApplicationDisplayService GetDisplayService(uint _0) => new();
		public override Nn.Visrv.Sf.IApplicationDisplayService GetDisplayServiceWithProxyNameExchange(byte[] _0, uint _1) => new();
	}
}