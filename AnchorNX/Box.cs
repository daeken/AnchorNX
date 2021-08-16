using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AnchorNX.Devices;
using IronVisor;
using LibHac.Common.Keys;

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
		public static readonly InterruptController InterruptController = new();
		public static readonly InterruptDistributor InterruptDistributor = new();
		public static readonly MemoryController MemoryController = new();
		public static readonly PowerManagementController PowerManagementController = new();
		public static readonly Uart Uart = new();

		public static readonly HashSet<ulong> DisabledTitles = new();
	}
}