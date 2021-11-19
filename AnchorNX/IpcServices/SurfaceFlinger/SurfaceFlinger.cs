using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvMap;
using Ryujinx.Common.Configuration;
using Ryujinx.Graphics.GAL;
using Ryujinx.Graphics.Gpu;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	class SurfaceFlinger : IConsumerListener, IDisposable {
		const int TargetFps = 60;
		static readonly Logger Logger = new("SurfaceFlinger");
		static readonly Action<string> Log = Logger.Log;

		readonly object Lock = new();
		readonly long _1msTicks;

		readonly Stopwatch _chrono;

		readonly Thread _composerThread;

		readonly ManualResetEvent _event = new(false);

		bool _isRunning;

		readonly Dictionary<long, Layer> _layers;
		readonly AutoResetEvent _nextFrameEvent = new(true);
		readonly long _spinTicks;

		int _swapInterval;
		long _ticks;
		long _ticksPerFrame;

		public SurfaceFlinger() {
			_layers = new Dictionary<long, Layer>();
			RenderLayerId = 0;

			_composerThread = new Thread(HandleComposition) {
				Name = "SurfaceFlinger.Composer"
			};

			_chrono = new Stopwatch();
			_chrono.Start();

			_ticks = 0;
			_spinTicks = Stopwatch.Frequency / 500;
			_1msTicks = Stopwatch.Frequency / 1000;

			UpdateSwapInterval(1);

			_composerThread.Start();
		}

		public long RenderLayerId { get; private set; }

		public void OnFrameAvailable(ref BufferItem item) {
			//_device.Statistics.RecordGameFrameTime();
		}

		public void OnFrameReplaced(ref BufferItem item) {
			//_device.Statistics.RecordGameFrameTime();
		}

		public void OnBuffersReleased() { }

		public void Dispose() {
			_isRunning = false;

			foreach(var layer in _layers.Values) layer.Core.PrepareForExit();
		}

		void UpdateSwapInterval(int swapInterval) {
			_swapInterval = swapInterval;

			// If the swap interval is 0, Game VSync is disabled.
			if(_swapInterval == 0) {
				_nextFrameEvent.Set();
				_ticksPerFrame = 1;
			} else
				_ticksPerFrame = Stopwatch.Frequency / (TargetFps / _swapInterval);
		}

		public IGraphicBufferProducer OpenLayer(long pid, long layerId) {
			bool needCreate;

			lock(Lock) needCreate = GetLayerByIdLocked(layerId) == null;

			if(needCreate) CreateLayerFromId(pid, layerId);

			return GetProducerByLayerId(layerId);
		}

		public IGraphicBufferProducer CreateLayer(long pid, out long layerId) {
			layerId = 1;

			lock(Lock)
				foreach(var pair in _layers)
					if(pair.Key >= layerId)
						layerId = pair.Key + 1;

			CreateLayerFromId(pid, layerId);

			return GetProducerByLayerId(layerId);
		}

		void CreateLayerFromId(long pid, long layerId) {
			lock(Lock) {
				Log($"Creating layer {layerId}");

				var core = BufferQueue.CreateBufferQueue(pid, out var producer, out var consumer);

				core.BufferQueued += () => { _nextFrameEvent.Set(); };

				_layers.Add(layerId, new Layer {
					ProducerBinderId = IHOSBinderDriver.RegisterBinderObject(producer),
					Producer = producer,
					Consumer = new BufferItemConsumer(consumer, 0, -1, false, this),
					Core = core,
					Owner = pid
				});
			}
		}

		public bool CloseLayer(long layerId) {
			lock(Lock) {
				var layer = GetLayerByIdLocked(layerId);

				if(layer != null) IHOSBinderDriver.UnregisterBinderObject(layer.ProducerBinderId);

				var removed = _layers.Remove(layerId);

				// If the layer was removed and the current in use, we need to change the current layer in use.
				if(removed && RenderLayerId == layerId) {
					// If no layer is availaible, reset to default value.
					if(_layers.Count == 0)
						SetRenderLayer(0);
					else
						SetRenderLayer(_layers.Last().Key);
				}

				return removed;
			}
		}

		public void SetRenderLayer(long layerId) {
			lock(Lock) RenderLayerId = layerId;
		}

		Layer GetLayerByIdLocked(long layerId) {
			foreach(var pair in _layers)
				if(pair.Key == layerId)
					return pair.Value;

			return null;
		}

		public IGraphicBufferProducer GetProducerByLayerId(long layerId) {
			lock(Lock) {
				var layer = GetLayerByIdLocked(layerId);

				if(layer != null) return layer.Producer;
			}

			return null;
		}

		void HandleComposition() {
			_isRunning = true;

			var lastTicks = _chrono.ElapsedTicks;

			while(_isRunning) {
				var ticks = _chrono.ElapsedTicks;

				if(_swapInterval == 0) {
					Compose();

					//_device.System?.SignalVsync();

					_nextFrameEvent.WaitOne(17);
					lastTicks = ticks;
				} else {
					_ticks += ticks - lastTicks;
					lastTicks = ticks;

					if(_ticks >= _ticksPerFrame) {
						Compose();

						//_device.System?.SignalVsync();

						// Apply a maximum bound of 3 frames to the tick remainder, in case some event causes Ryujinx to pause for a long time or messes with the timer.
						_ticks = Math.Min(_ticks - _ticksPerFrame, _ticksPerFrame * 3);
					}

					// Sleep if possible. If the time til the next frame is too low, spin wait instead.
					var diff = _ticksPerFrame - (_ticks + _chrono.ElapsedTicks - ticks);
					if(diff > 0) {
						if(diff < _spinTicks)
							do {
								// SpinWait is a little more HT/SMT friendly than aggressively updating/checking ticks.
								// The value of 5 still gives us quite a bit of precision (~0.0003ms variance at worst) while waiting a reasonable amount of time.
								Thread.SpinWait(5);

								ticks = _chrono.ElapsedTicks;
								_ticks += ticks - lastTicks;
								lastTicks = ticks;
							} while(_ticks < _ticksPerFrame);
						else
							_event.WaitOne((int) (diff / _1msTicks));
					}
				}
			}
		}

		public void Compose() {
			lock(Lock) {
				// TODO: support multilayers (& multidisplay ?)
				if(RenderLayerId == 0) return;

				var layer = GetLayerByIdLocked(RenderLayerId);

				var acquireStatus = layer.Consumer.AcquireBuffer(out var item, 0);

				if(acquireStatus == Status.Success) {
					// If device vsync is disabled, reflect the change.
					if(!Box.EnableDeviceVsync) {
						if(_swapInterval != 0) UpdateSwapInterval(0);
					} else if(item.SwapInterval != _swapInterval) UpdateSwapInterval(item.SwapInterval);

					PostFrameBuffer(layer, item);
				} else if(acquireStatus != Status.NoBufferAvailaible && acquireStatus != Status.InvalidOperation)
					throw new InvalidOperationException();
			}
		}

		void PostFrameBuffer(Layer layer, BufferItem item) {
			var frameBufferWidth = item.GraphicBuffer.Object.Width;
			var frameBufferHeight = item.GraphicBuffer.Object.Height;

			var nvMapHandle = item.GraphicBuffer.Object.Buffer.Surfaces[0].NvMapHandle;

			if(nvMapHandle == 0) nvMapHandle = item.GraphicBuffer.Object.Buffer.NvMapId;

			var bufferOffset = (ulong) item.GraphicBuffer.Object.Buffer.Surfaces[0].Offset;

			var map = NvMapDeviceFile.GetMapFromHandle(layer.Owner, nvMapHandle);

			var frameBufferAddress = map.Address + bufferOffset;

			var format = ConvertColorFormat(item.GraphicBuffer.Object.Buffer.Surfaces[0].ColorFormat);

			var bytesPerPixel =
				format == Format.B5G6R5Unorm ||
				format == Format.R4G4B4A4Unorm
					? 2
					: 4;

			var gobBlocksInY = 1 << item.GraphicBuffer.Object.Buffer.Surfaces[0].BlockHeightLog2;

			// Note: Rotation is being ignored.
			var cropRect = item.Crop;

			var flipX = item.Transform.HasFlag(NativeWindowTransform.FlipX);
			var flipY = item.Transform.HasFlag(NativeWindowTransform.FlipY);

			var aspectRatio = AspectRatio.Fixed16x9; //_device.Configuration.AspectRatio;
			var isStretched = aspectRatio == AspectRatio.Stretched;

			var crop = new ImageCrop(
				cropRect.Left,
				cropRect.Right,
				cropRect.Top,
				cropRect.Bottom,
				flipX,
				flipY,
				isStretched,
				aspectRatio.ToFloatX(),
				aspectRatio.ToFloatY());

			var textureCallbackInformation = new TextureCallbackInformation {
				Layer = layer,
				Item = item
			};

			if(item.Fence.FenceCount == 0) {
				Box.Gpu.Window.SignalFrameReady();
				Box.Gpu.GPFifo.Interrupt();
			} else
				item.Fence.RegisterCallback(Box.Gpu, () => {
					Box.Gpu.Window.SignalFrameReady();
					Box.Gpu.GPFifo.Interrupt();
				});

			Box.Gpu.Window.EnqueueFrameThreadSafe(
				layer.Owner,
				frameBufferAddress,
				frameBufferWidth,
				frameBufferHeight,
				0,
				false,
				gobBlocksInY,
				format,
				bytesPerPixel,
				crop,
				AcquireBuffer,
				ReleaseBuffer,
				textureCallbackInformation);
		}

		void ReleaseBuffer(object obj) {
			ReleaseBuffer((TextureCallbackInformation) obj);
		}

		void ReleaseBuffer(TextureCallbackInformation information) {
			var fence = AndroidFence.NoFence;

			information.Layer.Consumer.ReleaseBuffer(information.Item, ref fence);
		}

		void AcquireBuffer(GpuContext ignored, object obj) {
			AcquireBuffer((TextureCallbackInformation) obj);
		}

		void AcquireBuffer(TextureCallbackInformation information) {
			information.Item.Fence.WaitForever(Box.Gpu);
		}

		public static Format ConvertColorFormat(ColorFormat colorFormat) {
			return colorFormat switch {
				ColorFormat.A8B8G8R8 => Format.R8G8B8A8Unorm,
				ColorFormat.X8B8G8R8 => Format.R8G8B8A8Unorm,
				ColorFormat.R5G6B5 => Format.B5G6R5Unorm,
				ColorFormat.A8R8G8B8 => Format.B8G8R8A8Unorm,
				ColorFormat.A4B4G4R4 => Format.R4G4B4A4Unorm,
				_ => throw new NotImplementedException($"Color Format \"{colorFormat}\" not implemented!")
			};
		}

		class Layer {
			public BufferItemConsumer Consumer;
			public BufferQueueCore Core;
			public long Owner;
			public IGraphicBufferProducer Producer;
			public int ProducerBinderId;
		}

		class TextureCallbackInformation {
			public BufferItem Item;
			public Layer Layer;
		}
	}
}