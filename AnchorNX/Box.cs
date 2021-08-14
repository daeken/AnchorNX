using System.Threading;
using AnchorNX.Devices;
using IronVisor;

namespace AnchorNX {
	public static class Box {
		public static Vm Vm;
		public static readonly Core[] Cores = new Core[4];
		public static readonly SecMon.SecMon SecMon = new();
		public static readonly HvcDevice HvcDevice = new();
		public static HvcProxy HvcProxy;
		public static readonly InterruptController InterruptController = new();
		public static readonly InterruptDistributor InterruptDistributor = new();
		public static readonly MemoryController MemoryController = new();
		public static readonly PowerManagementController PowerManagementController = new();
		public static readonly Uart Uart = new();
	}
}