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
		public ulong BaseAddr, IpcBufAddr;
		public uint WakeInterruptEvent, EventInterruptEvent;
		public readonly Dictionary<uint, string> ServiceHandles = new();
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

		public void OnWake() {
			if(!Initialized) {
				WakeInterruptEvent = (uint) this[0];
				EventInterruptEvent = (uint) this[1];
				BaseAddr = this[2];
				IpcBufAddr = this[3];
				var iaddr = VirtMem.Translate(IpcBufAddr, Core.Current.Cpu);
				var (imem, ioff) = PhysMem.GetMemory(iaddr);
				IpcMemory = imem.Memory[ioff..(ioff + 0x100)];
				Console.WriteLine("HvcProxy initialized on HV side; starting processor state machine");
				Initialized = true;
				Processor.MoveNext();
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

			var sessionHandles = new Dictionary<uint, IpcInterface>();
			while(true) {
				var handles = new List<uint> { EventInterruptEvent };
				handles.AddRange(ServiceHandles.Keys);
				handles.AddRange(sessionHandles.Keys);
				var (res, index) = await WaitSynchronization(handles);
				Console.WriteLine($"HvcProxy: WaitSynchronization {res:X} {index}");
				if(res == 0xe401) { // TODO: Unhack.
					Console.WriteLine($"HvcProxy: Bad handle to waitsync...");
					sessionHandles.Clear();
					continue;
				}
				Debug.Assert(res == 0);
				var handle = handles[index];
				if(index == 0) // EventInterrupt
					throw new NotImplementedException();

				if(ServiceHandles.TryGetValue(handle, out var serviceName)) {
					Console.WriteLine($"HvcProxy: Pending connection to '{serviceName}'");
					var (rc, session) = await AcceptSession(handle);
					Debug.Assert(rc == 0);
					Console.WriteLine($"HvcProxy: Got connection! {session:X}");
					var iface = sessionHandles[session] = ServiceMapping[serviceName]();
					iface.Handle = session;
				} else {
					var iface = sessionHandles[handle];
					Console.WriteLine($"HvcProxy: Message for session {handle:X} ({iface})");
					var rc = await Receive(handle);
					Console.WriteLine($"HvcProxy: Got message? {rc:X} ({iface})");
					iface.SyncMessage(IpcMemory, out var closeHandle);
					rc = await Reply(handle);
					Console.WriteLine($"HvcProxy: Reply sent: {rc:X} (closehandle {closeHandle})");
					Debug.Assert(rc == 0xEA01);
				}
			}
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
	}
}