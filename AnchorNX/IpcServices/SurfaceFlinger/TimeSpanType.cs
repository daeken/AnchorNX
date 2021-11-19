using System;
using System.Runtime.InteropServices;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	[StructLayout(LayoutKind.Sequential)]
	struct TimeSpanType {
		const long NanoSecondsPerSecond = 1000000000;

		public static readonly TimeSpanType Zero = new(0);

		static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public long NanoSeconds;

		public TimeSpanType(long nanoSeconds) => NanoSeconds = nanoSeconds;

		public long ToSeconds() {
			return NanoSeconds / NanoSecondsPerSecond;
		}

		public TimeSpanType AddSeconds(long seconds) {
			return new TimeSpanType(NanoSeconds + seconds * NanoSecondsPerSecond);
		}

		public bool IsDaylightSavingTime() {
			return UnixEpoch.AddSeconds(ToSeconds()).ToLocalTime().IsDaylightSavingTime();
		}

		public static TimeSpanType FromSeconds(long seconds) {
			return new TimeSpanType(seconds * NanoSecondsPerSecond);
		}

		public static TimeSpanType FromTimeSpan(TimeSpan timeSpan) {
			return new TimeSpanType((long) (timeSpan.TotalMilliseconds * 1000000));
		}

		public static TimeSpanType FromTicks(ulong ticks, ulong frequency) {
			return FromSeconds((long) ticks / (long) frequency);
		}
	}
}