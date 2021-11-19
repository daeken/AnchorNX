namespace AnchorNX.IpcServices.Nn.Gpio {
	public partial class IManager {
		public override IPadSession GetPadSession(uint pin) => new IPadSession(pin);
	}
}