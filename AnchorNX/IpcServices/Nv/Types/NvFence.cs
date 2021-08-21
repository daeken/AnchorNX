using System;
using System.Runtime.InteropServices;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrl;
using Ryujinx.Graphics.Gpu;

namespace AnchorNX.IpcServices.Nns.Nvdrv.Types {
	[StructLayout(LayoutKind.Sequential, Size = 0x8)]
	struct NvFence {
		public const uint InvalidSyncPointId = uint.MaxValue;

		public uint Id;
		public uint Value;

		public bool IsValid() {
			return Id != InvalidSyncPointId;
		}

		public void UpdateValue(NvHostSyncpt hostSyncpt) {
			Value = hostSyncpt.ReadSyncpointValue(Id);
		}

		public void Increment(GpuContext gpuContext) {
			Value = gpuContext.Synchronization.IncrementSyncpoint(Id);
		}

		public bool Wait(GpuContext gpuContext, TimeSpan timeout) {
			if(IsValid()) {
				// return gpuContext.Synchronization.WaitOnSyncpoint(Id, Value, timeout);
			}

			return false;
		}
	}
}