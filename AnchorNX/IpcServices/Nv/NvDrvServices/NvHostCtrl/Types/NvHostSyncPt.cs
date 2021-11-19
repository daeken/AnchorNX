using System;
using System.Threading;
using Ryujinx.Common.Logging;
using Ryujinx.Graphics.Gpu.Synchronization;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvHostCtrl {
	public class NvHostSyncpt {
		static readonly Logger Logger = new("NvHostSyncpt");
		static Action<string> Log = Logger.Log;
		
		public const int VBlank0SyncpointId = 26;
		public const int VBlank1SyncpointId = 27;
		readonly bool[] _assigned;
		readonly bool[] _clientManaged;
		readonly int[] _counterMax;

		readonly int[] _counterMin;

		readonly object _syncpointAllocatorLock = new();

		public NvHostSyncpt() {
			_counterMin = new int[SynchronizationManager.MaxHardwareSyncpoints];
			_counterMax = new int[SynchronizationManager.MaxHardwareSyncpoints];
			_clientManaged = new bool[SynchronizationManager.MaxHardwareSyncpoints];
			_assigned = new bool[SynchronizationManager.MaxHardwareSyncpoints];

			// Reserve VBLANK syncpoints
			ReserveSyncpointLocked(VBlank0SyncpointId, true);
			ReserveSyncpointLocked(VBlank1SyncpointId, true);
		}

		void ReserveSyncpointLocked(uint id, bool isClientManaged) {
			if(id >= SynchronizationManager.MaxHardwareSyncpoints || _assigned[id])
				throw new ArgumentOutOfRangeException(nameof(id));

			_assigned[id] = true;
			_clientManaged[id] = isClientManaged;
		}

		public uint AllocateSyncpoint(bool isClientManaged) {
			lock(_syncpointAllocatorLock)
				for(uint i = 1; i < SynchronizationManager.MaxHardwareSyncpoints; i++)
					if(!_assigned[i]) {
						ReserveSyncpointLocked(i, isClientManaged);
						return i;
					}

			Log("Cannot allocate a new syncpoint!");

			return 0;
		}

		public void ReleaseSyncpoint(uint id) {
			if(id == 0) return;

			lock(_syncpointAllocatorLock) {
				if(id >= SynchronizationManager.MaxHardwareSyncpoints || !_assigned[id])
					throw new ArgumentOutOfRangeException(nameof(id));

				_assigned[id] = false;
				_clientManaged[id] = false;

				SetSyncpointMinEqualSyncpointMax(id);
			}
		}

		public void SetSyncpointMinEqualSyncpointMax(uint id) {
			if(id >= SynchronizationManager.MaxHardwareSyncpoints) throw new ArgumentOutOfRangeException(nameof(id));

			var value = (int) ReadSyncpointValue(id);

			Interlocked.Exchange(ref _counterMax[id], value);
		}

		public uint ReadSyncpointValue(uint id) {
			return UpdateMin(id);
		}

		public uint ReadSyncpointMinValue(uint id) {
			return (uint) _counterMin[id];
		}

		public uint ReadSyncpointMaxValue(uint id) {
			return (uint) _counterMax[id];
		}

		bool IsClientManaged(uint id) {
			if(id >= SynchronizationManager.MaxHardwareSyncpoints) return false;

			return _clientManaged[id];
		}

		public void Increment(uint id) {
			if(IsClientManaged(id)) IncrementSyncpointMax(id);

			IncrementSyncpointGPU(id);
		}

		public uint UpdateMin(uint id) {
			uint newValue = Box.Gpu.Synchronization.GetSyncpointValue(id);

			Interlocked.Exchange(ref _counterMin[id], (int) newValue);

			return newValue;
		}

		void IncrementSyncpointGPU(uint id) {
			Box.Gpu.Synchronization.IncrementSyncpoint(id);
		}

		public void IncrementSyncpointMin(uint id) {
			Interlocked.Increment(ref _counterMin[id]);
		}

		public uint IncrementSyncpointMaxExt(uint id, int count) {
			if(count == 0) return ReadSyncpointMaxValue(id);

			uint result = 0;

			for(var i = 0; i < count; i++) result = IncrementSyncpointMax(id);

			return result;
		}

		uint IncrementSyncpointMax(uint id) {
			return (uint) Interlocked.Increment(ref _counterMax[id]);
		}

		public uint IncrementSyncpointMax(uint id, uint incrs) {
			return (uint) Interlocked.Add(ref _counterMax[id], (int) incrs);
		}

		public bool IsSyncpointExpired(uint id, uint threshold) {
			return MinCompare(id, _counterMin[id], _counterMax[id], (int) threshold);
		}

		bool MinCompare(uint id, int min, int max, int threshold) {
			var minDiff = min - threshold;
			var maxDiff = max - threshold;

			if(IsClientManaged(id))
				return minDiff >= 0;
			return (uint) maxDiff >= (uint) minDiff;
		}
	}
}