using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using AnchorNX.IpcServices.Nns.Nvdrv.Types;
using Ryujinx.Graphics.Gpu;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x24)]
	struct AndroidFence : IFlattenable {
		static readonly Logger Logger = new("AndroidFence");
		static readonly Action<string> Log = Logger.Log;

		public int FenceCount;

		byte _fenceStorageStart;

		Span<byte> _storage => MemoryMarshal.CreateSpan(ref _fenceStorageStart, Unsafe.SizeOf<NvFence>() * 4);

		public Span<NvFence> NvFences => MemoryMarshal.Cast<byte, NvFence>(_storage);

		public static AndroidFence NoFence {
			get {
				var fence = new AndroidFence {
					FenceCount = 0
				};

				fence.NvFences[0].Id = NvFence.InvalidSyncPointId;

				return fence;
			}
		}

		public void AddFence(NvFence fence) {
			NvFences[FenceCount++] = fence;
		}

		public void WaitForever(GpuContext gpuContext) {
			var hasTimeout = Wait(gpuContext, TimeSpan.FromMilliseconds(3000));

			if(hasTimeout) {
				Log("Android fence didn't signal in 3000 ms");
				Wait(gpuContext, Timeout.InfiniteTimeSpan);
			}
		}

		public bool Wait(GpuContext gpuContext, TimeSpan timeout) {
			for(var i = 0; i < FenceCount; i++) {
				var hasTimeout = NvFences[i].Wait(gpuContext, timeout);

				if(hasTimeout) return true;
			}

			return false;
		}

		public void RegisterCallback(GpuContext gpuContext, Action callback) {
			ref var fence = ref NvFences[FenceCount - 1];

			if(fence.IsValid())
				gpuContext.Synchronization.RegisterCallbackOnSyncpoint(fence.Id, fence.Value, callback);
			else
				callback();
		}

		public uint GetFlattenedSize() {
			return (uint) Unsafe.SizeOf<AndroidFence>();
		}

		public uint GetFdCount() {
			return 0;
		}

		public void Flatten(Parcel parcel) {
			parcel.WriteUnmanagedType(ref this);
		}

		public void Unflatten(Parcel parcel) {
			this = parcel.ReadUnmanagedType<AndroidFence>();
		}
	}
}