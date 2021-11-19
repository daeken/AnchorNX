using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibHac.Svc;

namespace AnchorNX.IpcServices.Nn.Sm.Detail {
	public partial class IUserInterface {
		static readonly Logger Logger = new("IUserInterface");
		public static Action<string> Log = Logger.Log;

		ulong Pid;

		static readonly Dictionary<string, List<(ulong Pid, Func<uint, Task> Reply)>> Waiters = new();
		static readonly Dictionary<string, uint> ServicePorts = new();
		public static readonly Dictionary<string, Func<IpcInterface>> HleServices = new();
		static readonly Dictionary<uint, Queue<Func<Task>>> OpenWaiters = new();
		
		protected override async Task Dispatch(Memory<byte> ipcBuf, IncomingMessage im, OutgoingMessage om, Func<bool, Task> reply) {
			//Log($"Got message for sm:! {im.IsTipc} {im.CommandId:X}");
			try {
				switch(im.CommandId) {
					case 0: { // Initialize
						Log($"Foo? {im.Pid}");
						Initialize(im.Pid, im.GetData<ulong>(8));
						om.Initialize(0, 0, 0);
						break;
					}
					case 1: { // GetService
						try {
							await GetService(im.GetBytes(8, 0x8), async handle => {
								om.Initialize(1, 0, 0);
								om.Move(0, await CreateHandle(handle));
								om.Bake();
								ipcBuf.Span.Hexdump(Logger);
								await reply(false);
							});
						} catch(IpcException ie) {
							om.Initialize(1, 0, 0);
							om.Move(0, 0);
							om.ErrCode = ie.Code;
							om.Bake();
							ipcBuf.Span.Hexdump(Logger);
							await reply(false);
						}
						DumpWaiters();
						return;
					}
					case 2: { // RegisterService
						async Task Reply(uint handle) {
							om.Initialize(1, 0, 0);
							om.Move(0, await CreateHandle(handle));
							om.Bake();
							ipcBuf.Span.Hexdump(Logger);
							await reply(false);
						}
						if(im.IsTipc)
							await RegisterService(im.GetBytes(8, 0x8), im.GetData<byte>(20), im.GetData<uint>(16), Reply);
						else
							await RegisterService(im.GetBytes(8, 0x8), im.GetData<byte>(16), im.GetData<uint>(20), Reply);
						DumpWaiters();
						return;
					}
					case 3: { // UnregisterService
						UnregisterService(im.GetBytes(8, 0x8));
						om.Initialize(0, 0, 0);
						break;
					}
					default:
						throw new NotImplementedException($"Unhandled command ID to IUserInterface: {im.CommandId}");
				}
			} catch(IpcException ie) {
				om.Initialize(0, 0, 0);
				om.ErrCode = ie.Code;
			}
			om.Bake();
			ipcBuf.Span.Hexdump(Logger);
			await reply(false);
			DumpWaiters();
		}

		public override void Initialize(ulong pid, ulong reserved) => Pid = pid;

		async Task RegisterService(byte[] _name, byte isLight, uint maxHandles, Func<uint, Task> rreply) {
			var name = Encoding.ASCII.GetString(_name).TrimEnd('\0');
			Log($"Registering service '{name}' with {maxHandles} max handles -- pid {Box.PidName(Pid)}");
			var (rc, serverPort, clientPort) = await Box.HvcProxy.CreatePort((int) maxHandles, isLight != 0, name);
			Log($"Registered? 0x{rc:X}");
			Debug.Assert(rc == 0);
			ServicePorts[name] = clientPort;
			await rreply(serverPort);
			if(Waiters.TryGetValue(name, out var list)) {
				foreach(var (pid, reply) in list) {
					var (res, port) = await Box.HvcProxy.ConnectToPort(clientPort);
					Log($"Satisfied '{name}' waiter on pid {Box.PidName(pid)}? 0x{res:X} 0x{port:X}");
					Debug.Assert(res == 0);
					await reply(port);
				}
				Waiters.Remove(name);
			}
		}

		public async Task<uint> RegisterService(string name, uint maxHandles) {
			uint handle = 0;
			await RegisterService(Encoding.ASCII.GetBytes(name), 0, maxHandles, async x => handle = x);
			return handle;
		}

		async Task GetService(byte[] _name, Func<uint, Task> reply) {
			var name = Encoding.ASCII.GetString(_name).TrimEnd('\0');
			Log($"Getting service '{name}' from pid {Box.PidName(Pid)}");
			if(name == "srepo:u" && Pid == 109) {
				Log("BCAT trying to get srepo:u! Erroring.");
				throw new IpcException(21 | (8 << 9));
			}
			if(name == "bcat:u" && Pid == 107) {
				Log("NGCT trying to get bcat:u! Erroring.");
				throw new IpcException(21 | (8 << 9));
			}
			if(HleServices.TryGetValue(name, out var gen)) {
				var obj = gen();
				Log($"HLE service found for '{name}': {obj}");
				obj.OwningPid = Pid;
				await reply(await CreateHandle(obj));
			} else if(ServicePorts.TryGetValue(name, out var sport)) {
				Log($"Real service found for '{name}': 0x{sport:X}");
				var (rc, port) = await Box.HvcProxy.ConnectToPort(sport);
				Log($"Connected to port for '{name}'? 0x{rc:X}");
				if(rc == 0xE01) { // Too many connections
					Log($"Too many connections for '{name}'");
					if(!OpenWaiters.TryGetValue(sport, out var queue)) {
						queue = OpenWaiters[sport] = new();
						Box.HvcProxy.TriggerHandles[sport] = async () => {
							await queue.Dequeue()(); 
							if(queue.Count == 0) {
								Box.HvcProxy.Waiters.RemoveHandle(sport);
								Box.HvcProxy.TriggerHandles.Remove(sport);
							}
						};
						await Box.HvcProxy.Waiters.AddHandle(sport);
					}
					queue.Enqueue(async () => {
						Log($"Deferred connection for '{name}' -- firing on pid {Box.PidName(Pid)}");
						var (rc, port) = await Box.HvcProxy.ConnectToPort(sport);
						Log($"Connected to port for '{name}'? 0x{rc:X}");
						Debug.Assert(rc == 0);
						await reply(port);
					});
				} else {
					Debug.Assert(rc == 0);
					await reply(port);
				}
			} else {
				Log($"No service registered for '{name}' -- adding waiter");
				AddWaiter(name, reply, Pid);
			}
		}

		public static async Task GetService(string name, Func<uint, Task> reply) {
			Log($"Getting service '{name}'");
			if(HleServices.TryGetValue(name, out var gen))
				throw new NotSupportedException();
			if(ServicePorts.TryGetValue(name, out var sport)) {
				Log($"Real service found for '{name}': 0x{sport:X}");
				var (rc, port) = await Box.HvcProxy.ConnectToPort(sport);
				Log($"Connected to port for '{name}'? 0x{rc:X}");
				Debug.Assert(rc == 0);
				await reply(port);
			} else {
				Log($"No service registered for '{name}' -- adding waiter");
				AddWaiter(name, reply);
			}
		}

		public static void AddWaiter(string name, Func<uint, Task> reply, ulong pid = 0xDEADBEEF) {
			if(!Waiters.TryGetValue(name, out var list))
				list = Waiters[name] = new();
			list.Add((pid, reply));
		}

		public static void DumpWaiters() {
			Log("Processes waiting for services:");
			foreach(var (name, pids) in Waiters)
				Log($"- '{name}' -- {string.Join(", ", pids.Select(x => Box.PidName(x.Pid)))}");
		}
	}
}