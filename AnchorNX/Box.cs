using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AnchorNX.Devices;
using AnchorNX.IpcServices.Nns.Hosbinder;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrl;
using IronVisor;
using LibHac.Common.Keys;
using Ryujinx.Graphics.Gpu;

namespace AnchorNX {
	public static class Box {
		public static readonly Stopwatch Stopwatch = Stopwatch.StartNew();
		public static Vm Vm;
		public static readonly KeySet KeySet = ExternalKeyReader.ReadKeyFile(
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".switch/prod.keys"));
		public static readonly Core[] Cores = new Core[4];
		public static readonly SecMon.SecMon SecMon = new();
		public static readonly HvcDevice HvcDevice = new();
		public static HvcProxy HvcProxy;
		public static readonly EventManager EventManager = new();
		public static readonly InterruptController InterruptController = new();
		public static readonly InterruptDistributor InterruptDistributor = new();
		public static readonly MemoryController MemoryController = new();
		public static readonly PowerManagementController PowerManagementController = new();
		public static readonly Uart Uart = new();
		public static readonly ulong[] PagetableForProcess = new ulong[1024];

		public static GpuContext Gpu;
		public static readonly NvHostSyncpt HostSyncpoint = new();

		public static readonly HashSet<ulong> DisabledTitles = new();

		public static readonly Dictionary<ulong, string> PidNames = new() {
			[0xDEADBEEF] = "<<internal>>"
		};

		public static bool EnableDeviceVsync = true;
		internal static readonly SurfaceFlinger SurfaceFlinger = new();

		public static string PidName(ulong pid) => PidNames.TryGetValue(pid, out var name)
			? $"{pid} ({name})"
			: pid.ToString();
	}
}