using System;

namespace AnchorNX.IpcServices.Nn.Visrv.Sf {
	public partial class IManagerDisplayService {
		static readonly Logger Logger = new("IManagerDisplayService");
		public static Action<string> Log = Logger.Log;

		public override ulong AllocateProcessHeapBlock(ulong _0) => throw new NotImplementedException();
		public override void FreeProcessHeapBlock(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.FreeProcessHeapBlock [201]".Debug(Log);
		public override void GetDisplayResolution(ulong _0, out ulong _1, out ulong _2) => throw new NotImplementedException();
		public override ulong CreateManagedLayer(uint _0, ulong _1, ulong _2) {
			Box.SurfaceFlinger.CreateLayer((long) OwningPid, out var layerId);
			Box.SurfaceFlinger.SetRenderLayer(layerId);
			return (ulong) layerId;
		}

		public override void DestroyManagedLayer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.DestroyManagedLayer [2011]".Debug(Log);
		public override ulong CreateIndirectLayer() => throw new NotImplementedException();
		public override void DestroyIndirectLayer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.DestroyIndirectLayer [2051]".Debug(Log);
		public override ulong CreateIndirectProducerEndPoint(ulong _0, ulong _1) => throw new NotImplementedException();
		public override void DestroyIndirectProducerEndPoint(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.DestroyIndirectProducerEndPoint [2053]".Debug(Log);
		public override ulong CreateIndirectConsumerEndPoint(ulong _0, ulong _1) => throw new NotImplementedException();
		public override void DestroyIndirectConsumerEndPoint(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.DestroyIndirectConsumerEndPoint [2055]".Debug(Log);
		public override uint AcquireLayerTexturePresentingEvent(ulong _0) => throw new NotImplementedException();
		public override void ReleaseLayerTexturePresentingEvent(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.ReleaseLayerTexturePresentingEvent [2301]".Debug(Log);
		public override uint GetDisplayHotplugEvent(ulong _0) => Box.EventManager.GetEvent().Reader;
		public override uint GetDisplayModeChangedEvent(ulong _0) => Box.EventManager.GetEvent().Reader;
		public override uint GetDisplayHotplugState(ulong _0) => throw new NotImplementedException();
		public override void GetCompositorErrorInfo(ulong _0, ulong _1, out uint _2, Buffer<byte> _3) => throw new NotImplementedException();
		public override uint GetDisplayErrorEvent(ulong _0) => Box.EventManager.GetEvent().Reader;

		public override void SetDisplayAlpha(float _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetDisplayAlpha [4201]".Debug(Log);
		public override void SetDisplayLayerStack(uint _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetDisplayLayerStack [4203]".Debug(Log);
		public override void SetDisplayPowerState(uint _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetDisplayPowerState [4205]".Debug(Log);
		public override void SetDefaultDisplay(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetDefaultDisplay [4206]".Debug(Log);
		public override void AddToLayerStack(uint _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.AddToLayerStack [6000]".Debug(Log);
		public override void RemoveFromLayerStack(uint _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.RemoveFromLayerStack [6001]".Debug(Log);
		public override void SetLayerVisibility(byte _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetLayerVisibility [6002]".Debug(Log);
		public override ulong SetLayerConfig(ulong _0) {
			Log($"SetLayerConfig 0x{_0:X}");
			return 0;
		}

		public override object AttachLayerPresentationTracer(object _0) => throw new NotImplementedException();
		public override object DetachLayerPresentationTracer(object _0) => throw new NotImplementedException();
		public override object StartLayerPresentationRecording(object _0) => throw new NotImplementedException();
		public override object StopLayerPresentationRecording(object _0) => throw new NotImplementedException();
		public override object StartLayerPresentationFenceWait(object _0) => throw new NotImplementedException();
		public override object StopLayerPresentationFenceWait(object _0) => throw new NotImplementedException();
		public override object GetLayerPresentationAllFencesExpiredEvent(object _0) => throw new NotImplementedException();
		public override void SetContentVisibility(byte _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetContentVisibility [7000]".Debug(Log);
		public override void SetConductorLayer(byte _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetConductorLayer [8000]".Debug(Log);
		public override void SetIndirectProducerFlipOffset(ulong _0, ulong _1, ulong _2) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetIndirectProducerFlipOffset [8100]".Debug(Log);
		public override ulong CreateSharedBufferStaticStorage(ulong _0, Buffer<byte> _1) {
			Log($"CreateSharedBufferStaticStorage? 0x{_0:X} 0x{_1.Length:X}");
			_1.SafeSpan.Hexdump(Logger);
			return 0;
		}

		public override ulong CreateSharedBufferTransferMemory(ulong _0, uint _1, Buffer<byte> _2) => throw new NotImplementedException();
		public override void DestroySharedBuffer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.DestroySharedBuffer [8202]".Debug(Log);
		public override void BindSharedLowLevelLayerToManagedLayer(byte[] _0, ulong _1, ulong _2, ulong _3) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.BindSharedLowLevelLayerToManagedLayer [8203]".Debug(Log);
		public override void BindSharedLowLevelLayerToIndirectLayer(ulong _0, ulong _1, ulong _2) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.BindSharedLowLevelLayerToIndirectLayer [8204]".Debug(Log);
		public override void UnbindSharedLowLevelLayer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.UnbindSharedLowLevelLayer [8207]".Debug(Log);
		public override void ConnectSharedLowLevelLayerToSharedBuffer(ulong _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.ConnectSharedLowLevelLayerToSharedBuffer [8208]".Debug(Log);
		public override void DisconnectSharedLowLevelLayerFromSharedBuffer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.DisconnectSharedLowLevelLayerFromSharedBuffer [8209]".Debug(Log);
		public override ulong CreateSharedLayer(ulong _0) => throw new NotImplementedException();
		public override void DestroySharedLayer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.DestroySharedLayer [8211]".Debug(Log);
		public override void AttachSharedLayerToLowLevelLayer(byte[] _0, ulong _1, ulong _2) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.AttachSharedLayerToLowLevelLayer [8216]".Debug(Log);
		public override void ForceDetachSharedLayerFromLowLevelLayer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.ForceDetachSharedLayerFromLowLevelLayer [8217]".Debug(Log);
		public override void StartDetachSharedLayerFromLowLevelLayer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.StartDetachSharedLayerFromLowLevelLayer [8218]".Debug(Log);
		public override void FinishDetachSharedLayerFromLowLevelLayer(ulong _0) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.FinishDetachSharedLayerFromLowLevelLayer [8219]".Debug(Log);
		public override uint GetSharedLayerDetachReadyEvent(ulong _0) => throw new NotImplementedException();
		public override uint GetSharedLowLevelLayerSynchronizedEvent(ulong _0) {
			Log($"GetSharedLowLevelLayerSynchronizedEvent 0x{_0:X}");
			return Box.EventManager.GetEvent().Reader;
		}

		public override ulong CheckSharedLowLevelLayerSynchronized(ulong _0) => throw new NotImplementedException();
		public override void RegisterSharedBufferImporterAruid(ulong _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.RegisterSharedBufferImporterAruid [8223]".Debug(Log);
		public override void UnregisterSharedBufferImporterAruid(ulong _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.UnregisterSharedBufferImporterAruid [8224]".Debug(Log);
		public override ulong CreateSharedBufferProcessHeap(ulong _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override uint GetSharedLayerLayerStacks(ulong _0) => throw new NotImplementedException();
		public override void SetSharedLayerLayerStacks(uint _0, ulong _1) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetSharedLayerLayerStacks [8229]".Debug(Log);
		public override void PresentDetachedSharedFrameBufferToLowLevelLayer(ulong _0, ulong _1, ulong _2) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.PresentDetachedSharedFrameBufferToLowLevelLayer [8291]".Debug(Log);
		public override void FillDetachedSharedFrameBufferColor(uint _0, uint _1, uint _2, uint _3, uint _4, ulong _5, ulong _6) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.FillDetachedSharedFrameBufferColor [8292]".Debug(Log);
		public override void GetDetachedSharedFrameBufferImage(ulong _0, ulong _1, out ulong _2, Buffer<byte> _3) => throw new NotImplementedException();
		public override void SetDetachedSharedFrameBufferImage(uint _0, ulong _1, ulong _2, Buffer<byte> _3) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetDetachedSharedFrameBufferImage [8294]".Debug(Log);
		public override void CopyDetachedSharedFrameBufferImage(uint _0, uint _1, ulong _2, ulong _3, ulong _4) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.CopyDetachedSharedFrameBufferImage [8295]".Debug(Log);
		public override void SetDetachedSharedFrameBufferSubImage(uint _0, uint _1, uint _2, uint _3, uint _4, uint _5, ulong _6, ulong _7, Buffer<byte> _8) => "Stub hit for Nn.Visrv.Sf.IManagerDisplayService.SetDetachedSharedFrameBufferSubImage [8296]".Debug(Log);
		public override void GetSharedFrameBufferContentParameter(ulong _0, ulong _1, out uint _2, out byte[] _3, out uint _4, out uint _5, out uint _6) => throw new NotImplementedException();
		public override object ExpandStartupLogoOnSharedFrameBuffer(object _0) => null;
	}
}