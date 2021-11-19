using System;

namespace AnchorNX.IpcServices.Nn.Eth.Sf {
	public partial class IEthInterfaceGroup {
		public static HosEvent Event;

		public override uint GetReadableHandle() {
			Event ??= Box.EventManager.GetEvent();
			return Event.Reader;
		}
		
		public override void Cancel() => "Stub hit for Nn.Eth.Sf.IEthInterfaceGroup.Cancel [1]".Debug(Log);
		public override void GetResult() => "Stub hit for Nn.Eth.Sf.IEthInterfaceGroup.GetResult [2]".Debug(Log);
		public override void GetInterfaceList(Buffer<byte> _0) => throw new NotImplementedException();
		public override uint GetInterfaceCount() => 0;
	}
}