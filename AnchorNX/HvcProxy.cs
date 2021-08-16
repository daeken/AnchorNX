using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnchorNX.IpcServices.Nn.Fssrv.Sf;
using IronVisor;

namespace AnchorNX {
	public class HvcProxy {
		public readonly Memory<byte> SharedMemory;
		public readonly Memory<uint> SharedMemory32;
		public readonly Memory<ulong> SharedMemory64;
		public Memory<byte> IpcMemory;
		bool Initialized;
		public ulong BaseAddr, IpcBufAddr, XBufAddr, RecvBufAddr;
		public uint WakeInterruptEvent, EventInterruptEvent;
		public readonly Dictionary<uint, string> ServiceHandles = new();
		public readonly Dictionary<uint, IpcInterface> SessionHandles = new();
		readonly AsyncAutoResetEvent Awoken = new();
		bool WaitingForWake;
		readonly IAsyncStateMachine Processor;
		readonly Func<Task> TaskGetter;
		Task ProcessorTask => TaskGetter();

		readonly Dictionary<string, Func<IpcInterface>> ServiceMapping = new() {
			["fsp-ldr"] = () => new IFileSystemProxyForLoader(), 
			["fsp-pr"] = () => new IProgramRegistry(), 
			["fsp-srv"] = () => new IFileSystemProxy(), 
		};

		public HvcProxy() {
			SharedMemory = PhysMem.GetMemory(0x5701_0000).Memory.Memory;
			SharedMemory32 = SharedMemory.Cast<byte, uint>();
			SharedMemory64 = SharedMemory.Cast<byte, ulong>();

			var found = false;
			foreach(var cls in GetType().GetNestedTypes(BindingFlags.NonPublic)) {
				if(!cls.Name.Contains("<Process>") || cls.GetInterface("IAsyncStateMachine") == null)
					continue;
				found = true;
				var sm = (IAsyncStateMachine) Activator.CreateInstance(cls);
				foreach(var fi in cls.GetFields(BindingFlags.Public | BindingFlags.Instance)) {
					if(fi.Name.EndsWith("__state"))
						fi.SetValue(sm, -1);
					else if(fi.Name.EndsWith("__builder")) {
						fi.SetValue(sm, AsyncTaskMethodBuilder.Create());
						TaskGetter = () => ((AsyncTaskMethodBuilder) fi.GetValue(sm)).Task;
					} else if(fi.Name.EndsWith("__this"))
						fi.SetValue(sm, this);
					else
						throw new NotImplementedException($"Unknown public field: '{fi.Name}'");
				}
				Processor = sm;
			}
			Debug.Assert(found);
		}

		public ulong this[int index] {
			get => SharedMemory64.Span[index];
			set => SharedMemory64.Span[index] = value;
		}

		public T Get<T>(int offset) where T : unmanaged =>
			SharedMemory[offset..].Cast<byte, T>().Span[0];
		public void Set<T>(int offset, T value) where T : unmanaged =>
			SharedMemory[offset..].Cast<byte, T>().Span[0] = value;
		
		public void SendInterrupt() =>
			Box.InterruptDistributor.SendInterrupt(3, 0x2E);

		public void SendEvent() =>
			Box.InterruptDistributor.SendInterrupt(3, 0x2F);

		readonly Stopwatch Stopwatch = Stopwatch.StartNew();
		long LastTime;

		public void OnWake() {
			if(!Initialized) {
				WakeInterruptEvent = (uint) this[0];
				EventInterruptEvent = (uint) this[1];
				BaseAddr = this[2];
				IpcBufAddr = this[3];
				XBufAddr = this[4];
				Console.WriteLine($"XBuf addr: {XBufAddr:X}");
				RecvBufAddr = BaseAddr + 0x11000;
				var iaddr = VirtMem.Translate(IpcBufAddr, Core.Current.Cpu);
				var (imem, ioff) = PhysMem.GetMemory(iaddr);
				IpcMemory = imem.Memory[ioff..(ioff + 0x100)];
				Console.WriteLine("HvcProxy initialized on HV side; starting processor state machine");
				Initialized = true;
				Processor.MoveNext();

				new Thread(() => {
					while(true)
						if(Stopwatch.ElapsedMilliseconds - LastTime > 1000) {
							Console.WriteLine("Time to initialize boot!");
							Thread.Sleep(1000);
							Console.WriteLine("Starting for real");
							SendEvent();
							break;
						}
				}).Start();
			} else {
				Console.WriteLine("HvcProxy: Awoken; triggering next cycle");
				Console.WriteLine($"HvcProxy: {ProcessorTask.Status}");
				Awoken.Set();
			}

			while(!WaitingForWake && !ProcessorTask.IsCompleted)
				Processor.MoveNext();

			if(ProcessorTask.IsCompleted)
				throw ProcessorTask.Exception;
		}

		async Task SendCommand() {
			Console.WriteLine("HvcProxy: Sending command???");
			SendInterrupt();
			WaitingForWake = true;
			await Awoken.WaitAsync();
			WaitingForWake = false;
		}

		// ReSharper disable once UnusedMember.Local
		async Task Process() {
			foreach(var name in ServiceMapping.Keys) {
				Console.WriteLine($"HvcProxy: Attempting to register '{name}'");
				var (res, port) = await RegisterService(name, 1000);
				Console.WriteLine($"HvcProxy: Registering '{name}': {res:X} {port:X}");
				Debug.Assert(res == 0);
				ServiceHandles[port] = name;
			}

			var startedBoot2 = false;

			while(true) {
				var handles = new List<uint> { EventInterruptEvent };
				handles.AddRange(ServiceHandles.Keys);
				handles.AddRange(SessionHandles.Keys);
				var (res, index) = await WaitSynchronization(handles);
				Console.WriteLine($"HvcProxy: WaitSynchronization {res:X} {index}");
				if(res == 0xe401) { // TODO: Unhack.
					Console.WriteLine($"HvcProxy: Bad handle to waitsync...");
					SessionHandles.Clear();
					continue;
				}
				Debug.Assert(res == 0);
				var handle = handles[index];
				if(index == 0) { // EventInterrupt
					Console.WriteLine("HvcProxy: Woken up to start boot2!");
					Debug.Assert(!startedBoot2);
					startedBoot2 = true;
					Console.WriteLine("HvcProxy: Clearing interrupt");
					await ResetSignal(EventInterruptEvent);
					Console.WriteLine("HvcProxy: Getting pm:shell");
					var (rc, session) = await GetService("pm:shell");
					Console.WriteLine($"HvcProxy: Got pm:shell? {rc:X} {session:X}");
					Debug.Assert(rc == 0);
					SetupNotifyBootFinished();
					rc = await SendRequest(session);
					Console.WriteLine($"HvcProxy: NotifyBootFinished? {rc:X}");
					Debug.Assert(rc == 0);
				} else if(ServiceHandles.TryGetValue(handle, out var serviceName)) {
					Console.WriteLine($"HvcProxy: Pending connection to '{serviceName}'");
					var (rc, session) = await AcceptSession(handle);
					Debug.Assert(rc == 0);
					Console.WriteLine($"HvcProxy: Got connection! {session:X}");
					var iface = SessionHandles[session] = ServiceMapping[serviceName]();
					iface.Handle = session;
				} else {
					var iface = SessionHandles[handle];
					Console.WriteLine($"HvcProxy: Message for session {handle:X} ({iface})");
					uint rc;
					while(true) {
						SetupIpcReceive();
						rc = await Receive(handle);
						Console.WriteLine($"HvcProxy: Got message? {rc:X} ({iface})");
						switch(rc) {
							case 0:
								goto done;
							default:
								throw new NotImplementedException($"Unhandled response for receive: 0x{rc:X}");
						}
					}
					done:

					var (_, closeHandle) = await iface.SyncMessage(IpcMemory, XBufAddr);
					rc = await Reply(handle);
					Console.WriteLine($"HvcProxy: Reply sent: {rc:X} (closehandle {closeHandle})");
					Debug.Assert(rc == 0xEA01);
				}

				LastTime = Stopwatch.ElapsedMilliseconds;
			}
		}

		void SetupIpcReceive() {
			IpcMemory.Span.Clear();
			var ispan = IpcMemory.Span.As<byte, uint>();
			ispan[0] = 0;
			ispan[1] = 2U << 10; // HIPC_AUTO_RECV_STATIC
			ispan[2] = (uint) RecvBufAddr;
			ispan[3] = (uint) (RecvBufAddr >> 32) | (0x500U << 16);
		}

		void DumpIpcBuffer() {
			var msg = "";
			var buf = IpcMemory.Span;
			for(var i = 0; i < 0x100; i += 16) {
				msg += $"HvcProxy: {i:X4} | ";
				for(var j = 0; j < 16; ++j) {
					msg += $"{buf[i + j]:X2} ";
					if(j == 7)
						msg += " ";
				}
				msg += "| ";
				for(var j = 0; j < 16; ++j) {
					if(buf[i + j] > 0x20 && buf[i + j] < 0x7F)
						msg += (char) buf[i + j];
					else
						msg += ".";
					if(j == 7)
						msg += " ";
				}
				msg += "\n";
			}
			Console.WriteLine(msg);
		}

		async Task<(uint Result, int Index)> WaitSynchronization(List<uint> handles, long timeout = -1) {
			this[0] = 0;
			this[1] = (ulong) handles.Count;
			this[2] = (ulong) timeout;
			var offset = 4 * 8;
			foreach(var handle in handles) {
				Set(offset, handle);
				offset += 4;
			}
			await SendCommand();
			return ((uint) this[0], (int) (uint) this[1]);
		}

		async Task<(uint Result, uint Port)> RegisterService(string name, int maxSessions) {
			var nameBytes = Encoding.ASCII.GetBytes(name).Concat(new byte[8]).ToArray();
			this[0] = 1;
			this[1] = BitConverter.ToUInt64(nameBytes);
			this[2] = (ulong) maxSessions;
			await SendCommand();
			return ((uint) this[0], (uint) this[1]);
		}

		async Task<(uint Result, uint Session)> AcceptSession(uint port) {
			this[0] = 2;
			this[1] = port;
			await SendCommand();
			return ((uint) this[0], (uint) this[1]);
		}

		async Task<uint> Receive(uint handle = 0, long timeout = -1) {
			this[0] = 3;
			this[1] = handle;
			this[2] = (ulong) timeout;
			await SendCommand();
			return (uint) this[0];
		}

		async Task<uint> Reply(uint handle, long timeout = 0) {
			this[0] = 4;
			this[1] = handle;
			this[2] = (ulong) timeout;
			await SendCommand();
			return (uint) this[0];
		}

		public async Task<(uint Result, uint ServerHandle, uint ClientHandle)> CreateSession() {
			this[0] = 5;
			await SendCommand();
			return ((uint) this[0], (uint) this[1], (uint) this[2]);
		}

		public async Task<(uint Result, uint ClientHandle)> GetService(string name) {
			var nameBytes = Encoding.ASCII.GetBytes(name).Concat(new byte[8]).ToArray();
			this[0] = 6;
			this[1] = BitConverter.ToUInt64(nameBytes);
			await SendCommand();
			return ((uint) this[0], (uint) this[1]);
		}
		
		public async Task<uint> SendRequest(uint handle) {
			this[0] = 7;
			this[1] = handle;
			await SendCommand();
			return (uint) this[0];
		}
		
		public async Task<uint> ResetSignal(uint handle) {
			this[0] = 8;
			this[1] = handle;
			await SendCommand();
			return (uint) this[0];
		}

		public async Task<(uint Result, ulong Resolver)> OpenLocationResolver(byte storageId) {
			this[0] = 9;
			this[1] = storageId;
			await SendCommand();
			return ((uint) this[0], this[1]);
		}

		public async Task<(uint Result, string Path)> ResolveDataPath(ulong resolver, ulong tid) {
			this[0] = 10;
			this[1] = resolver;
			this[2] = tid;
			await SendCommand();
			return ((uint) this[0], Encoding.ASCII.GetString(SharedMemory.Span[32..(256 + 32)]).Split('\0', 2)[0]);
		}

		void SetupNotifyBootFinished() {
			var ipcBuf = IpcMemory.Span.As<byte, uint>();
			ipcBuf[0] = 4;
			ipcBuf[1] = 8;
			ipcBuf[2] = 0;
			ipcBuf[3] = 0;
			ipcBuf[4] = 0x49434653; // SFCI
			ipcBuf[5] = 0;
			ipcBuf[6] = 5;
			ipcBuf[7] = 0;
			ipcBuf[8] = 0;
			
			DumpIpcBuffer();
		}
	}
}