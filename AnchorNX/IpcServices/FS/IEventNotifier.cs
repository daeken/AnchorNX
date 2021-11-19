namespace AnchorNX.IpcServices.Nn.Fssrv.Sf {
	public partial class IEventNotifier {
		public override uint GetEventHandle() => Box.EventManager.GetEvent().Reader;
	}
}