using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AnchorNX {
	public class EventManager {
		static readonly Logger Logger = new("EventManager");
		static Action<string> Log = Logger.Log;
        
		const int MinReady = 5;
		
		public readonly ConcurrentQueue<HosEvent> Ready = new();
		public readonly ConcurrentQueue<HosEvent> ToClose = new();
		public readonly ConcurrentQueue<HosEvent> ToUpdate = new();

		public async Task Refresh() {
			while(ToUpdate.TryDequeue(out var tc)) {
				bool needClear, needSignal;
				lock(tc) {
					needClear = tc.NeedClear;
					needSignal = tc.NeedSignal;
					tc.NeedSignal = tc.NeedClear = false;
				}

				if(needClear)
					await Box.HvcProxy.ResetSignal(tc.Writer);
				if(needSignal)
					await Box.HvcProxy.SignalEvent(tc.Writer);
			}
			
			if(Ready.Count < MinReady)
				for(var i = Ready.Count; i < MinReady; ++i) {
					var (rc, w, r) = await Box.HvcProxy.CreateEvent();
					Log($"Created event? 0x{rc:X} 0x{w:X} 0x{r:X}");
					Debug.Assert(rc == 0);
					Ready.Enqueue(new(w, r));
				}

			while(ToClose.TryDequeue(out var tc)) {
				await Box.HvcProxy.CloseHandle(tc.Reader);
				await Box.HvcProxy.CloseHandle(tc.Writer);
			}
		}

		public HosEvent GetEvent() {
			if(!Ready.TryDequeue(out var evt))
				throw new Exception("No events available!");
			return evt;
		}
	}
}