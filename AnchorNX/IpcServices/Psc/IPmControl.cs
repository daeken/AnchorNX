using System;

namespace AnchorNX.IpcServices.Nn.Psc.Sf {
	public partial class IPmControl {
		readonly HosEvent Event;

		public IPmControl() =>
			Event = Box.EventManager.GetEvent();

		public override uint Initialize() => Event.Reader;
		public override void Unknown1(object _0) => "Stub hit for Nn.Psc.Sf.IPmControl.Unknown1 [1]".Debug(Log);
		public override void Unknown2() => "Stub hit for Nn.Psc.Sf.IPmControl.Unknown2 [2]".Debug(Log);
		public override object Unknown3() => throw new NotImplementedException();
		public override void Unknown4() => "Stub hit for Nn.Psc.Sf.IPmControl.Unknown4 [4]".Debug(Log);
		public override void Unknown5() => "Stub hit for Nn.Psc.Sf.IPmControl.Unknown5 [5]".Debug(Log);
		public override void Unknown6(out object _0, Buffer<byte> _1, Buffer<byte> _2) => throw new NotImplementedException();
	}
}