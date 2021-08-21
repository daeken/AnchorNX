using System;
using Ryujinx.Graphics.Gpu;
using Ryujinx.Graphics.Gpu.Memory;
using Ryujinx.Graphics.Host1x;
using Ryujinx.Graphics.Nvdec;
using Ryujinx.Graphics.Vic;
using ClassId = Ryujinx.Graphics.Host1x.ClassId;

namespace AnchorNX.IpcServices.Nns.Nvdrv {
	class Host1xContext : IDisposable {
		public Host1xContext(GpuContext gpu, long pid) {
			MemoryAllocator = new NvMemoryAllocator();
			Host1x = new Host1xDevice(gpu.Synchronization);
			Smmu = gpu.CreateMemoryManager(pid);
			var nvdec = new NvdecDevice(Smmu);
			var vic = new VicDevice(Smmu);
			Host1x.RegisterDevice(ClassId.Nvdec, nvdec);
			Host1x.RegisterDevice(ClassId.Vic, vic);

			nvdec.FrameDecoded += e => {
				// FIXME:
				// Figure out what is causing frame ordering issues on H264.
				// For now this is needed as workaround.
				if(e.CodecId == CodecId.H264)
					vic.SetSurfaceOverride(e.LumaOffset, e.ChromaOffset, 0);
				else
					vic.DisableSurfaceOverride();
			};
		}

		public MemoryManager Smmu { get; }
		public NvMemoryAllocator MemoryAllocator { get; }
		public Host1xDevice Host1x { get; }

		public void Dispose() {
			Host1x.Dispose();
		}
	}
}