using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnchorNX.IpcServices.Nn.Audioctrl.Detail;
using AnchorNX.IpcServices.Nn.Bluetooth;
using AnchorNX.IpcServices.Nn.Cec;
using AnchorNX.IpcServices.Nn.Eth.Sf;
using AnchorNX.IpcServices.Nn.Fssrv.Sf;
using AnchorNX.IpcServices.Nn.Gpio;
using AnchorNX.IpcServices.Nn.Psc.Sf;
using AnchorNX.IpcServices.Nn.Psm;
using AnchorNX.IpcServices.Nn.Sm.Detail;
using AnchorNX.IpcServices.Nn.Socket.Sf;
using AnchorNX.IpcServices.Nn.Ssl.Sf;
using AnchorNX.IpcServices.Nn.Timesrv.Detail.Service;
using AnchorNX.IpcServices.Nn.Usb;
using AnchorNX.IpcServices.Nn.Visrv.Sf;
using AnchorNX.IpcServices.Nn.Wlan.Detail;
using AnchorNX.IpcServices.Nns.Hosbinder;
using AnchorNX.IpcServices.Nns.Nvdrv;
using AnchorNX.IpcServices.Pcv;
using AnchorNX.IpcServices.Time;
using IronVisor;
using MoreLinq.Extensions;

namespace AnchorNX {
	public class WaiterManager {
		static readonly Logger Logger = new("WaiterManager");
		static Action<string> Log = Logger.Log;
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		unsafe struct Waiter {
			public ulong LastResult, LastIndex, HandleCount;
			public uint NotificationEvent, StartEvent;
			public uint NotificationEventWrite, StartEventWrite;
			public fixed uint WaitList[0x40];
		}
		
		public readonly Dictionary<uint, (ulong Address, uint[] WaitList)> Waiters = new();
		readonly Queue<uint> FreeList = new();
		public readonly Dictionary<uint, uint> StartHandles = new();

		public async Task AddHandle(uint handle) {
			Log($"Adding handle to WaiterManager: 0x{handle:X}");
			var fresh = false;
			if(FreeList.Count == 0) {
				var nwaiter = await Box.HvcProxy.CreateWaiterThread();
				var nwhandle = GetNotifyHandle(nwaiter);
				Waiters[nwhandle] = (nwaiter, new uint[0x40]);
				StartHandles[nwhandle] = GetStartHandle(nwaiter);
				for(var i = 0; i < 0x40; ++i)
					FreeList.Enqueue(nwhandle);
				fresh = true;
			}

			var whandle = FreeList.Dequeue();
			var waiter = Waiters[whandle];

			AddHandle(waiter, handle);
			if(fresh)
				await TriggerStart(whandle);
		}

		public void RemoveHandle(uint handle) {
			foreach(var (whandle, (addr, waitlist)) in Waiters) {
				var index = Array.FindIndex(waitlist, elem => elem == handle);
				if(index == -1) continue;
				RemoveHandle((addr, waitlist), index);
			}
		}

		public (uint Result, uint Handle) GetState(uint whandle) {
			var (addr, waitlist) = Waiters[whandle];
			var waiter = VirtMem.GetSpan<Waiter>(addr, Core.Current.Cpu)[0];
			var li = (int) (uint) waiter.LastIndex;
			return ((uint) waiter.LastResult, li < 0x40 ? waitlist[li] : 1);
		}

		public unsafe void DumpState(uint whandle) {
			Log($"Waiter handle 0x{whandle:X}");
			var (addr, waitlist) = Waiters[whandle];
			var waiter = VirtMem.GetSpan<Waiter>(addr, Core.Current.Cpu)[0];
			Log($"Last result 0x{waiter.LastResult:X} Last index 0x{waiter.LastIndex:X}");
			Log($"Number of handles: 0x{waiter.HandleCount:X}");
			for(var i = 0; i < (int) waiter.HandleCount; ++i)
				Log($"Handle #0x{i:X} -- 0x{waitlist[i]:X} -- 0x{waiter.WaitList[i]:X}");
		}

		uint GetNotifyHandle(ulong addr) {
			var waiter = VirtMem.GetSpan<Waiter>(addr, Core.Current.Cpu);
			return waiter[0].NotificationEvent;
		}

		uint GetStartHandle(ulong addr) {
			var waiter = VirtMem.GetSpan<Waiter>(addr, Core.Current.Cpu);
			return waiter[0].StartEventWrite;
		}

		unsafe void AddHandle((ulong Address, uint[] WaitList) twaiter, uint handle) {
			var (addr, waitlist) = twaiter;
			var waiter = VirtMem.GetSpan<Waiter>(addr, Core.Current.Cpu);
			var hc = waiter[0].HandleCount;
			waiter[0].WaitList[hc] = handle;
			waiter[0].HandleCount++;
			waitlist[hc] = handle;
		}

		unsafe void RemoveHandle((ulong Address, uint[] WaitList) twaiter, int index) {
			var (addr, waitlist) = twaiter;
			var waiter = VirtMem.GetSpan<Waiter>(addr, Core.Current.Cpu);
			waiter[0].HandleCount--;
			for(var i = index; i < 0x3F; ++i) {
				waiter[0].WaitList[i] = waiter[0].WaitList[i + 1];
				waitlist[i] = waitlist[i + 1];
			}
		}

		public async Task TriggerStart(uint handle) =>
			await Box.HvcProxy.SignalEvent(StartHandles[handle]);
	}
	
	public class HvcProxy {
		static readonly Logger Logger = new("HvcProxy");
		static Action<string> Log = Logger.Log;
		
		public readonly Memory<byte> SharedMemory;
		public readonly Memory<uint> SharedMemory32;
		public readonly Memory<ulong> SharedMemory64;
		public Memory<byte> IpcMemory;
		bool Initialized;
		public ulong BaseAddr, IpcBufAddr, XBufAddr, RecvBufAddr;
		public uint WakeInterruptEvent, EventInterruptEvent;
		public readonly Dictionary<uint, string> ServiceHandles = new();
		public readonly Dictionary<uint, IpcInterface> SessionHandles = new();
		public readonly Dictionary<uint, Func<Task>> TriggerHandles = new();
		Dictionary<uint, ulong> PscModuleHandles;
		readonly AsyncAutoResetEvent Awoken = new();
		readonly Dictionary<byte, ulong> LrResolvers = new();
		bool WaitingForWake;
		readonly IAsyncStateMachine Processor;
		readonly Func<Task> TaskGetter;
		public readonly WaiterManager Waiters = new();
		Task ProcessorTask => TaskGetter();

		readonly Dictionary<string, Func<IpcInterface>> ServiceMapping = new() {
			["fsp-ldr"] = () => new IFileSystemProxyForLoader(), 
			["fsp-pr"] = () => new IProgramRegistry(), 
			["fsp-srv"] = () => new IFileSystemProxy(), 
			["gpio"] = () => new IManager(),
			/*["time:u"] = () => new IStaticService(),
			["time:a"] = () => new IStaticService(),
			["time:s"] = () => new IStaticService(),*/
			["bsd:u"] = () => new IClient(), 
			["bsd:s"] = () => new IClient(), 
			["nsd:u"] = () => new AnchorNX.IpcServices.Nn.Nsd.Detail.IManager(), 
			["nsd:a"] = () => new AnchorNX.IpcServices.Nn.Nsd.Detail.IManager(), 
			["ethc:i"] = () => new IEthInterfaceGroup(),
			["wlan:inf"] = () => new IInfraManager(), 
			["nvdrv"] = () => new INvDrvServices(), 
			["nvdrv:a"] = () => new INvDrvServices(), 
			["nvdrv:s"] = () => new INvDrvServices(), 
			["nvdrv:t"] = () => new INvDrvServices(), 
			["rtc"] = () => new IRtc(), 
			["ssl"] = () => new ISslService(), 
			["sm:"] = () => new IUserInterface(), 
			["sm:m"] = () => new IManagerInterface(), 
			//["psc:c"] = () => new IPmControl(),
			["usb:obsv"] = () => new IObserve(),
			["psm"] = () => new IPsmServer(), 
			["dispdrv"] = () => new IHOSBinderDriver(), 
			/*["vi:u"] = () => new IApplicationRootService(), 
			["vi:s"] = () => new ISystemRootService(), 
			["vi:m"] = () => new IManagerRootService(), 
			["cec-mgr"] = () => new ICecManager(), */
			["clkrst"] = () => new IClkrstManager(), 
			["audctl"] = () => new IAudioController(), 
		};

		public HvcProxy() {
			var fakes = new[] {
				"i2c", "i2c:pcv", "uart", "pwm", "pinmux", "sasbus", "led", // bus
				"usb:ds", "usb:hs", "usb:hs:a", "usb:pd", "usb:pd:c", "usb:pd:m", "usb:pm", "usb:qdb", // usb
				"pcie", "pcie:log", // pcie
				// pcv
				"bpc", "bpc:r", "bpc:c", "bpc:b", "bpc:w", "pcv", "pcv:arb", "pcv:imm", "clkrst", "clkrst:i", "clkrst:a",
				"rgltr", 
				"nvmemp", "nvdrvdbg", "nvgem:c", "nvgem:cd", "nvdbg:d", // NV
				// ptm
				"fan", "tc", "ts", "pcm", "apm:am", "apm:sys", 
				"fgm", "fgm:0", "fgm:1", "fgm:2", "fgm:3", "fgm:4", "fgm:5", "fgm:6", "fgm:7", "fgm:8", "fgm:9", 
				"fgm:dbg", "lbl", 
				// hid
				"hid", "hid:dbg", "hid:sys", "hid:tmp", "irs", "irs:sys", "ahid:cd", "ahid:hdr", "xcd:sys", "hidbus", 
				// audio
				"audout:u", "audin:u", "audrec:u", "auddev", "audren:u", "audout:a", "audin:a", "audrec:a", "audren:a", 
				"audout:d", "audin:d", "audrec:d", "audren:d", "codecctl", "hwopus", "auddebug", 
				"aud:a", "aud:d", 
				"wlan:lcl", "wlan:lg", "wlan:lga", "wlan:sg", "wlan:soc", "wlan:dtc", // wlan
				"btdrv", "bt", // bt
				"btm", "btm:dbg", "btm:sys", "btm:u", // btm
				"nfc:am", "nfc:mf:u", "nfc:user", "nfc:sys", "nfp:user", "nfp:dbg", "nfp:sys", // nfc
				"bsdcfg", "ethc:c", "sfdnsres", // bsdsockets
				//"caps:sc", "caps:ss", "caps:su", "mm:u", "lbl", // vi
				//"fatal:u", 
			};
			fakes.ForEach(name => ServiceMapping[name] = () => new IFake(name));
			
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
				RecvBufAddr = BaseAddr + 0x11000;
				var iaddr = VirtMem.Translate(IpcBufAddr, Core.Current.Cpu);
				var (imem, ioff) = PhysMem.GetMemory(iaddr);
				IpcMemory = imem.Memory[ioff..(ioff + 0x100)];
				
				var vbar = Core.Current.Cpu[SysReg.VBAR_EL1];
				var hea = vbar - 0x60800 + 0xA1320;
				/*var syn = VirtMem.GetSpan<uint>(hea, Core.Current.Cpu);
				if(syn[0] == 0xD10243FF) {
					syn[0] = 0xD4000022;
					Log("Patched usermode exception handler to break to HVC!");
				}*/

				Initialized = true;
				Log("HvcProxy initialized on HV side; starting processor state machine");
				Processor.MoveNext();

				new Thread(() => {
					var booted = false;
					while(true) {
						var time = Stopwatch.ElapsedMilliseconds - LastTime;
						if(!booted && time > 1000) {
							Log("Time to initialize boot!");
							Thread.Sleep(1000);
							Log("Starting for real");
							SendEvent();
							booted = true;
						} else if(booted && time > 5000) {
							SendEvent();
							break;
						}
					}
				}).Start();
			} else {
				Log("Awoken; triggering next cycle");
				Awoken.Set();
			}

			while(!WaitingForWake && !ProcessorTask.IsCompleted)
				Processor.MoveNext();

			if(ProcessorTask.IsCompleted)
				throw ProcessorTask.Exception;
		}

		async Task SendCommand() {
			SendInterrupt();
			WaitingForWake = true;
			await Awoken.WaitAsync();
			WaitingForWake = false;
		}

		// ReSharper disable once UnusedMember.Local
		async Task Process() {
			await Waiters.AddHandle(EventInterruptEvent);
			
			var (src, smPort) = await ManageNamedPort("sm:", 0x100);
			Log($"Set up sm: port? 0x{src:X} 0x{smPort:X}");
			Debug.Assert(src == 0);
			ServiceHandles[smPort] = "sm:";
			await Waiters.AddHandle(smPort);

			IUserInterface.AddWaiter("lr", InitializeLr);

			foreach(var (name, gen) in ServiceMapping) {
				Log($"Registering service '{name}' -- HLE");
				IUserInterface.HleServices[name] = gen;
			}

			var pscStarted = false;
			var startedBoot2 = false;
			var sfInitialized = false;

			while(true) {
				await Box.EventManager.Refresh();

				if(!sfInitialized) {
					sfInitialized = true;
					//IHOSBinderDriver.Initialize();
				}
				
				var handles = Waiters.Waiters.Keys.ToList();
				var (res, index) = await WaitSynchronization(handles);
				Log($"WaitSynchronization {res:X} {index}");
				Debug.Assert(res == 0);
				var whandle = handles[index];
				await ResetSignal(whandle);
				Console.WriteLine($"Waiter {index} ({whandle:X}) triggered");
				var (nres, handle) = Waiters.GetState(whandle);
				Console.WriteLine($"Waiter res {nres:X} -- {handle:X}");
				if(nres != 0) {
					Console.WriteLine("Bad handle in waitlist?");
					Waiters.DumpState(whandle);
					await DumpHandleTable();
					Environment.Exit(0);
				}
				if(handle == EventInterruptEvent) {
					Log("Clearing interrupt");
					await ResetSignal(EventInterruptEvent);
					if(startedBoot2) {
						/*if(!pscStarted) {
							pscStarted = true;
							PscModuleHandles = await RegisterPmModules(
								PmModuleId.Audio, 
								PmModuleId.Bluetooth, 
								PmModuleId.Btm, 
								PmModuleId.Ethernet, 
								PmModuleId.Fs, 
								PmModuleId.Gpio, 
								PmModuleId.Hid, 
								PmModuleId.I2c, 
								PmModuleId.Nfc, 
								PmModuleId.Nvservices, 
								PmModuleId.Pcie, 
								PmModuleId.Pinmux, 
								PmModuleId.Psm, 
								PmModuleId.Pwm, 
								PmModuleId.Sasbus, 
								PmModuleId.Spi, 
								PmModuleId.Uart, 
								PmModuleId.Usb, 
								PmModuleId.GpioLow, 
								PmModuleId.I2cPcv, 
								PmModuleId.PcvClock, 
								PmModuleId.PcvVoltage, 
								PmModuleId.PsmLow, 
								PmModuleId.WlanSockets
							);
							foreach(var evt in PscModuleHandles.Keys)
								await Waiters.AddHandle(evt);
						} else*/
							await DumpHandleTable();
						goto end;
					}
					
					Log("Woken up to start boot2!");
					startedBoot2 = true;
					await StartBoot2();
				} else if(ServiceHandles.TryGetValue(handle, out var serviceName)) {
					Log($"Pending connection to '{serviceName}'");
					var (rc, session) = await AcceptSession(handle);
					Debug.Assert(rc == 0);
					Log($"Got connection! {session:X}");
					var iface = SessionHandles[session] = ServiceMapping[serviceName]();
					await Waiters.AddHandle(session);
					iface.Handle = session;
					Log("Added handle!");
				} else if(PscModuleHandles != null && PscModuleHandles.TryGetValue(handle, out var mod)) {
					Log($"PSC state change for 0x{handle:X} - 0x{mod:X}");
					var state = await GetAndAcknowledgeRequest(mod);
					Log($"State changed to 0x{state:X}");
				} else if(TriggerHandles.TryGetValue(handle, out var func))
					await func();
				else {
					var iface = SessionHandles[handle];
					Log($"Message for session {handle:X} ({iface})");
					uint rc;
					while(true) {
						SetupIpcReceive();
						rc = await Receive(handle);
						Log($"Got message? {rc:X} ({iface})");
						switch(rc) {
							case 0:
								goto done;
							case 0xF601:
								Log($"Session closed: 0x{handle} ({iface})");
								SessionHandles.Remove(handle);
								Waiters.RemoveHandle(handle);
								await CloseHandle(handle);
								goto done;
							default:
								throw new NotImplementedException($"Unhandled response for receive: 0x{rc:X}");
						}
					}
					done:
					if(rc == 0) {
						try {
							await iface.SyncMessage(IpcMemory, XBufAddr, async closeHandle => {
								rc = await Reply(handle);
								Log($"Reply sent: {rc:X} (closehandle {closeHandle})");
								Debug.Assert(rc == 0xEA01);
								if(closeHandle) {
									Log($"Closing session 0x{handle:X}");
									iface.Close();
									SessionHandles.Remove(handle);
									Waiters.RemoveHandle(handle);
									await CloseHandle(handle);
								}
							});
						} catch(IpcIgnoreException) {
							Log($"This message should be ignored; just moving on.");
						}
					}
				}

				end:
				await Waiters.TriggerStart(whandle);
				LastTime = Stopwatch.ElapsedMilliseconds;
			}
		}

		async Task StartBoot2() {
			Log("Getting pm:shell");
			await IUserInterface.GetService("pm:shell", async session => {
				Log($"Got pm:shell? {session:X}");
				SetupNotifyBootFinished();
				var rc = await SendRequest(session);
				Log($"NotifyBootFinished? {rc:X}");
				Debug.Assert(rc == 0);
				await IUserInterface.GetService("set", async ss => {
					Log("Completed boot2 startup!");
					await CloseHandle(ss);
					await CloseHandle(session);
				});
			});
		}

		public void RegisteredService(string name) {
			if(name == "psc:m")
				SendEvent();
		}

		void SetupIpcReceive() {
			IpcMemory.Span.Clear();
			var ispan = IpcMemory.Span.As<byte, uint>();
			ispan[0] = 0;
			ispan[1] = 2U << 10; // HIPC_AUTO_RECV_STATIC
			ispan[2] = (uint) RecvBufAddr;
			ispan[3] = (uint) (RecvBufAddr >> 32) | (0x4000U << 16);
		}

		void DumpIpcBuffer() {
			var msg = "";
			var buf = IpcMemory.Span;
			for(var i = 0; i < 0x100; i += 16) {
				msg += $"{i:X4} | ";
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
			Log(msg);
		}

		public async Task<(uint Result, int Index)> WaitSynchronization(List<uint> handles, long timeout = -1) {
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
			if(LrResolvers.TryGetValue(storageId, out var res))
				return (0, res);
			this[0] = 9;
			this[1] = storageId;
			await SendCommand();
			LrResolvers[storageId] = this[1];
			return ((uint) this[0], this[1]);
		}

		public async Task<(uint Result, string Path)> ResolveDataPath(ulong resolver, ulong tid) {
			this[0] = 10;
			this[1] = resolver;
			this[2] = tid;
			await SendCommand();
			return ((uint) this[0], Encoding.ASCII.GetString(SharedMemory.Span[32..(256 + 32)]).Split('\0', 2)[0]);
		}

		public async Task<ulong> CreateWaiterThread() {
			this[0] = 11;
			await SendCommand();
			return this[0];
		}

		public async Task<uint> SignalEvent(uint handle) {
			this[0] = 12;
			this[1] = handle;
			await SendCommand();
			return (uint) this[0];
		}

		public async Task AddSession(uint handle, IpcInterface session) {
			SessionHandles[handle] = session;
			await Waiters.AddHandle(handle);
		}
		
		async Task DumpHandleTable() {
			this[0] = 13;
			await SendCommand();
		}

		public async Task<(uint Result, uint WriterHandle, uint ReaderHandle)> CreateEvent() {
			this[0] = 14;
			this[1] = 0xDEADBEEF;
			this[2] = 0xCAFEBABE;
			await SendCommand();
			return ((uint) this[0], (uint) this[1], (uint) this[2]);
		}

		public async Task<uint> CloseHandle(uint handle) {
			this[0] = 15;
			this[1] = handle;
			await SendCommand();
			return (uint) this[0];
		}

		public async Task<(uint Result, ulong Pid)> GetProcessId(uint handle) {
			this[0] = 16;
			this[1] = handle;
			await SendCommand();
			return ((uint) this[0], this[1]);
		}

		public async Task<(uint Result, uint Handle)> ManageNamedPort(string name, int maxSessions) {
			this[0] = 17;
			this[1] = (uint) maxSessions;
			var nameBytes = Encoding.ASCII.GetBytes(name).Concat(new byte[8]).ToArray();
			this[2] = BitConverter.ToUInt64(nameBytes);
			await SendCommand();
			return ((uint) this[0], (uint) this[1]);
		}
		
		public async Task<(uint Result, uint Handle)> ConnectToPort(uint handle) {
			this[0] = 18;
			this[1] = handle;
			await SendCommand();
			return ((uint) this[0], (uint) this[1]);
		}

		public enum PmModuleId : uint {
			Usb           = 4,
			Ethernet      = 5,
			Fgm           = 6,
			PcvClock      = 7,
			PcvVoltage    = 8,
			Gpio          = 9,
			Pinmux        = 10,
			Uart          = 11,
			I2c           = 12,
			I2cPcv        = 13,
			Spi           = 14,
			Pwm           = 15,
			Psm           = 16,
			Tc            = 17,
			Omm           = 18,
			Pcie          = 19,
			Lbl           = 20,
			Display       = 21,
			Hid           = 24,
			WlanSockets   = 25,
			Fs            = 27,
			Audio         = 28,
			TmaHostIo     = 30,
			Bluetooth     = 31,
			Bpc           = 32,
			Fan           = 33,
			Pcm           = 34,
			Nfc           = 35,
			Apm           = 36,
			Btm           = 37,
			Nifm          = 38,
			GpioLow       = 39,
			Npns          = 40,
			Lm            = 41,
			Bcat          = 42,
			Time          = 43,
			Pctl          = 44,
			Erpt          = 45,
			Eupld         = 46,
			Friends       = 47,
			Bgtc          = 48,
			Account       = 49,
			Sasbus        = 50,
			Ntc           = 51,
			Idle          = 52,
			Tcap          = 53,
			PsmLow        = 54,
			Ndd           = 55,
			Olsc          = 56,
			Ns            = 61,
			Nvservices    = 101,
			Spsm          = 127,
		}

		public async Task<(uint Result, ulong Mod, uint Event)>  InitPmModule(PmModuleId moduleId) {
			this[0] = 19;
			this[1] = (uint) moduleId;
			await SendCommand();
			return ((uint) this[0], this[1], (uint) this[2]);
		}

		async Task<Dictionary<uint, ulong>> RegisterPmModules(params PmModuleId[] moduleIds) {
			var ret = new Dictionary<uint, ulong>();
			foreach(var id in moduleIds) {
				var (res, mod, evt) = await InitPmModule(id);
				Log($"Registered PM module {id}? 0x{res:X} 0x{mod:X} 0x{evt:X}");
				Debug.Assert(res == 0);
				ret[evt] = mod;
			}
			return ret;
		}

		public async Task<uint> GetAndAcknowledgeRequest(ulong mod) {
			this[0] = 20;
			this[1] = mod;
			await SendCommand();
			return (uint) this[0];
		}

		public async Task<(uint Result, uint ServerPort, uint ClientPort)> CreatePort(int maxSessions, bool isLight, string name) {
			this[0] = 21;
			this[1] = (ulong) maxSessions;
			this[2] = isLight ? 1UL : 0;
			this[3] = BitConverter.ToUInt64(Encoding.ASCII.GetBytes(name).Concat(new byte[8]).ToArray());
			await SendCommand();
			return ((uint) this[0], (uint) this[1], (uint) this[2]);
		}
		
		public async Task InitializeLr(uint handle) {
			Log("Initializing LR in libnx");
			this[0] = 22;
			this[1] = handle;
			await SendCommand();
			Log("LR initialized!");
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