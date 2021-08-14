using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnchorNX {
	public class AsyncAutoResetEvent {
		static readonly Task Completed = Task.FromResult(true);
		readonly Queue<TaskCompletionSource<bool>> Waits = new();
		bool Signaled;

		public Task WaitAsync() {
			lock(Waits)
				if(Signaled) {
					Signaled = false;
					return Completed;
				} else {
					var tcs = new TaskCompletionSource<bool>();
					Waits.Enqueue(tcs);
					return tcs.Task;
				}
		}

		public void Set() {
			TaskCompletionSource<bool> toRelease = null;

			lock(Waits)
				if(Waits.Count > 0)
					toRelease = Waits.Dequeue();
				else if(!Signaled)
					Signaled = true;

			toRelease?.SetResult(true);
		}
	}
}