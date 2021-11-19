using System;

namespace AnchorNX.IpcServices.Nn.Cec {
	public partial class ICecManager {
		public override void RegisterCallback(out ulong _0, out uint _1) {
			_0 = 0;
			_1 = Box.EventManager.GetEvent().Reader;
		}

		public override object Unknown1(object _0) => throw new NotImplementedException();
		public override void Unknown2(object _0) => "Stub hit for Nn.Cec.ICecManager.Unknown2 [2]".Debug(Log);
		public override object Unknown3(object _0) => throw new NotImplementedException();
		public override object Unknown4(object _0) => throw new NotImplementedException();
		public override object Unknown5() => throw new NotImplementedException();
		public override object Unknown6() => throw new NotImplementedException();
		public override IHdcpController GetHdcpServiceObject() => new();
	}
}