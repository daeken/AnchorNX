using System;

namespace AnchorNX {
	public class Logger {
		static readonly object Lock = "LogLock";

		readonly string Module;
		readonly bool Disabled;

		public Logger(string module, bool disabled = false) {
			Module = module;
			Disabled = disabled;
		}

		public void WithLock(Action func) {
			lock(Lock)
				func();
		}

		public void Log(string message) {
			if(Disabled) return;
			lock(Lock)
				foreach(var line in message.Split('\n'))
					Console.WriteLine($"[{Module}] {line}");
		}
	}
}