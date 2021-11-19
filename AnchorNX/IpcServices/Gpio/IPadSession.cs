namespace AnchorNX.IpcServices.Nn.Gpio {
	public partial class IPadSession {
		public readonly uint Pin;
		public IPadSession(uint pin) => Pin = pin;
		
		public override uint GetValue() {
			Log($"Getting value for GPIO {Pin}");
			return Pin is 26 or 25 ? 1U : 0;
		}
	}
}