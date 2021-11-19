using System;
using System.Threading.Tasks;

namespace AnchorNX.IpcServices.Nn.Sm.Detail {
	public partial class IManagerInterface {
		static readonly Logger Logger = new("IManagerInterface");
		public static Action<string> Log = Logger.Log;

		protected override async Task Dispatch(Memory<byte> ipcBuf, IncomingMessage im, OutgoingMessage om, Func<bool, Task> reply) {
			Log($"Got message for IManagerInterface; just returning an empty message");
			om.Initialize(0, 0, 0);
			om.Bake();
			ipcBuf.Span.Hexdump(Logger);
			await reply(false);
		}
	}
}