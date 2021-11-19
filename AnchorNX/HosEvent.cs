namespace AnchorNX {
	public class HosEvent {
		public readonly uint Writer, Reader;
		public bool NeedSignal, NeedClear;

		public HosEvent(uint writer, uint reader) {
			Writer = writer;
			Reader = reader;
		}

		public void Signal() {
			lock(this) {
				var prior = NeedClear || NeedSignal;
				NeedClear = false;
				NeedSignal = true;
				if(!prior) Box.EventManager.ToUpdate.Enqueue(this);
			}
		}

		public void Clear() {
			lock(this) {
				var prior = NeedClear || NeedSignal;
				NeedClear = true;
				NeedSignal = false;
				if(!prior) Box.EventManager.ToUpdate.Enqueue(this);
			}
		}

		public void Close() => Box.EventManager.ToClose.Enqueue(this);
	}
}