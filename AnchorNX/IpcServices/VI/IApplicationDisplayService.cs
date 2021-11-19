using System;
using System.Runtime.InteropServices;
using System.Text;
using AnchorNX.IpcServices.Nns.Hosbinder;

namespace AnchorNX.IpcServices.Nn.Visrv.Sf {
	public partial class IApplicationDisplayService {
		public override Nns.Hosbinder.IHOSBinderDriver GetRelayService() => new();
		public override Nn.Visrv.Sf.ISystemDisplayService GetSystemDisplayService() => new();
		public override Nn.Visrv.Sf.IManagerDisplayService GetManagerDisplayService() => new();
		public override Nns.Hosbinder.IHOSBinderDriver GetIndirectDisplayTransactionService() => new();
		public override void ListDisplays(out ulong _0, Buffer<byte> _1) {
			_0 = 1;
			var data = (Span<byte>) new byte[0x60];
			var ud = MemoryMarshal.Cast<byte, ulong>(data);
			Encoding.ASCII.GetBytes("Default").CopyTo(data);
			ud[0x40 >> 3] = 1;
			ud[0x48 >> 3] = 1;
			ud[0x50 >> 3] = 1280;
			ud[0x58 >> 3] = 720;
			_1.CopyFrom(data);
		}

		public override ulong OpenDisplay(byte[] dnb) {
			var displayName = Encoding.ASCII.GetString(dnb).Split('\0')[0];
			Log($"Opening display '{displayName}'");
			return 0;
		}

		public override ulong OpenDefaultDisplay() => 0;
		public override void CloseDisplay(ulong _0) => "Stub hit for Nn.Visrv.Sf.IApplicationDisplayService.CloseDisplay [1020]".Debug(Log);
		public override void SetDisplayEnabled(byte _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IApplicationDisplayService.SetDisplayEnabled [1101]".Debug(Log);
		public override void GetDisplayResolution(ulong _0, out ulong _1, out ulong _2) => throw new NotImplementedException();
		public override void OpenLayer(byte[] _0, ulong _1, ulong _2, ulong _3, out ulong _4, Buffer<byte> _5) => throw new NotImplementedException();
		public override void CloseLayer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IApplicationDisplayService.CloseLayer [2021]".Debug(Log);
		public override void CreateStrayLayer(uint _0, ulong _1, out ulong _2, out ulong _3, Buffer<byte> _4) => throw new NotImplementedException();
		public override void DestroyStrayLayer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IApplicationDisplayService.DestroyStrayLayer [2031]".Debug(Log);
		public override void SetLayerScalingMode(uint _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IApplicationDisplayService.SetLayerScalingMode [2101]".Debug(Log);
		public override object ConvertScalingMode(object _0) => throw new NotImplementedException();
		public override void GetIndirectLayerImageMap(ulong _0, ulong _1, ulong _2, ulong _3, ulong _4, out ulong _5, out ulong _6, Buffer<byte> _7) => throw new NotImplementedException();
		public override void GetIndirectLayerImageCropMap(float _0, float _1, float _2, float _3, ulong _4, ulong _5, ulong _6, ulong _7, ulong _8, out ulong _9, out ulong _10, Buffer<byte> _11) => throw new NotImplementedException();
		public override void GetIndirectLayerImageRequiredMemoryInfo(ulong _0, ulong _1, out ulong _2, out ulong _3) => throw new NotImplementedException();
		public override uint GetDisplayVsyncEvent(ulong _0) => throw new NotImplementedException();
		public override uint GetDisplayVsyncEventForDebug(ulong _0) => throw new NotImplementedException();
	}
}