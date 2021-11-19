﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AnchorNX.IpcServices.Nns.Hosbinder.Types;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	abstract class IGraphicBufferProducer : IBinder {
		static readonly Logger Logger = new("IGraphicBufferProducer");
		static readonly Action<string> Log = Logger.Log;

		public string InterfaceToken => "android.gui.IGraphicBufferProducer";

		public uint AdjustRefcount(int addVal, int type) {
			// TODO?
			return 0;
		}

		public void GetNativeHandle(uint typeId, out uint readableEvent) {
			if(typeId == 0xF)
				readableEvent = GetWaitBufferFreeEvent();
			else
				throw new NotImplementedException($"Unimplemented native event type {typeId}!");
		}

		public void OnTransact(uint code, uint flags, Parcel inputParcel, Parcel outputParcel) {
			var status = Status.Success;
			int slot;
			AndroidFence fence;
			QueueBufferInput queueInput;
			QueueBufferOutput queueOutput;
			NativeWindowApi api;

			AndroidStrongPointer<GraphicBuffer> graphicBuffer;
			AndroidStrongPointer<AndroidFence> strongFence;

			switch((TransactionCode) code) {
				case TransactionCode.RequestBuffer:
					slot = inputParcel.ReadInt32();

					status = RequestBuffer(slot, out graphicBuffer);

					outputParcel.WriteStrongPointer(ref graphicBuffer);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.SetBufferCount:
					var bufferCount = inputParcel.ReadInt32();

					status = SetBufferCount(bufferCount);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.DequeueBuffer:
					var async = inputParcel.ReadBoolean();
					var width = inputParcel.ReadUInt32();
					var height = inputParcel.ReadUInt32();
					var format = inputParcel.ReadUnmanagedType<PixelFormat>();
					var usage = inputParcel.ReadUInt32();

					status = DequeueBuffer(out var dequeueSlot, out fence, async, width, height, format, usage);
					strongFence = new AndroidStrongPointer<AndroidFence>(fence);

					outputParcel.WriteInt32(dequeueSlot);
					outputParcel.WriteStrongPointer(ref strongFence);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.DetachBuffer:
					slot = inputParcel.ReadInt32();

					status = DetachBuffer(slot);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.DetachNextBuffer:
					status = DetachNextBuffer(out graphicBuffer, out fence);
					strongFence = new AndroidStrongPointer<AndroidFence>(fence);

					outputParcel.WriteStrongPointer(ref graphicBuffer);
					outputParcel.WriteStrongPointer(ref strongFence);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.AttachBuffer:
					graphicBuffer = inputParcel.ReadStrongPointer<GraphicBuffer>();

					status = AttachBuffer(out slot, graphicBuffer);

					outputParcel.WriteInt32(slot);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.QueueBuffer:
					slot = inputParcel.ReadInt32();
					queueInput = inputParcel.ReadFlattenable<QueueBufferInput>();

					status = QueueBuffer(slot, ref queueInput, out queueOutput);

					queueOutput.WriteToParcel(outputParcel);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.CancelBuffer:
					slot = inputParcel.ReadInt32();
					fence = inputParcel.ReadFlattenable<AndroidFence>();

					CancelBuffer(slot, ref fence);

					outputParcel.WriteStatus(Status.Success);

					break;
				case TransactionCode.Query:
					var what = inputParcel.ReadUnmanagedType<NativeWindowAttribute>();

					status = Query(what, out var outValue);

					outputParcel.WriteInt32(outValue);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.Connect:
					var hasListener = inputParcel.ReadBoolean();

					IProducerListener listener = null;

					if(hasListener)
						throw new NotImplementedException("Connect with a strong binder listener isn't implemented");

					api = inputParcel.ReadUnmanagedType<NativeWindowApi>();

					var producerControlledByApp = inputParcel.ReadBoolean();

					status = Connect(listener, api, producerControlledByApp, out queueOutput);

					queueOutput.WriteToParcel(outputParcel);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.Disconnect:
					api = inputParcel.ReadUnmanagedType<NativeWindowApi>();

					status = Disconnect(api);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.SetPreallocatedBuffer:
					slot = inputParcel.ReadInt32();

					graphicBuffer = inputParcel.ReadStrongPointer<GraphicBuffer>();

					status = SetPreallocatedBuffer(slot, graphicBuffer);

					outputParcel.WriteStatus(status);

					break;
				case TransactionCode.GetBufferHistory:
					var bufferHistoryCount = inputParcel.ReadInt32();

					status = GetBufferHistory(bufferHistoryCount, out var bufferInfos);

					outputParcel.WriteStatus(status);

					outputParcel.WriteInt32(bufferInfos.Length);

					outputParcel.WriteUnmanagedSpan<BufferInfo>(bufferInfos);

					break;
				default:
					throw new NotImplementedException($"Transaction {(TransactionCode) code} not implemented");
			}

			if(status != Status.Success) Log($"Error returned by transaction {(TransactionCode) code}: {status}");
		}

		protected abstract uint GetWaitBufferFreeEvent();

		public abstract Status RequestBuffer(int slot, out AndroidStrongPointer<GraphicBuffer> graphicBuffer);

		public abstract Status SetBufferCount(int bufferCount);

		public abstract Status DequeueBuffer(out int slot, out AndroidFence fence, bool async, uint width, uint height,
			PixelFormat format, uint usage
		);

		public abstract Status DetachBuffer(int slot);

		public abstract Status DetachNextBuffer(out AndroidStrongPointer<GraphicBuffer> graphicBuffer,
			out AndroidFence fence
		);

		public abstract Status AttachBuffer(out int slot, AndroidStrongPointer<GraphicBuffer> graphicBuffer);

		public abstract Status QueueBuffer(int slot, ref QueueBufferInput input, out QueueBufferOutput output);

		public abstract void CancelBuffer(int slot, ref AndroidFence fence);

		public abstract Status Query(NativeWindowAttribute what, out int outValue);

		public abstract Status Connect(IProducerListener listener, NativeWindowApi api, bool producerControlledByApp,
			out QueueBufferOutput output
		);

		public abstract Status Disconnect(NativeWindowApi api);

		public abstract Status SetPreallocatedBuffer(int slot, AndroidStrongPointer<GraphicBuffer> graphicBuffer);

		public abstract Status GetBufferHistory(int bufferHistoryCount, out Span<BufferInfo> bufferInfos);

		enum TransactionCode : uint {
			RequestBuffer = 1,
			SetBufferCount,
			DequeueBuffer,
			DetachBuffer,
			DetachNextBuffer,
			AttachBuffer,
			QueueBuffer,
			CancelBuffer,
			Query,
			Connect,
			Disconnect,
			SetSidebandStream,
			AllocateBuffers,
			SetPreallocatedBuffer,
			Reserved15,
			GetBufferInfo,
			GetBufferHistory
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x54)]
		public struct QueueBufferInput : IFlattenable {
			public long Timestamp;
			public int IsAutoTimestamp;
			public Rect Crop;
			public NativeWindowScalingMode ScalingMode;
			public NativeWindowTransform Transform;
			public uint StickyTransform;
			public int Async;
			public int SwapInterval;
			public AndroidFence Fence;

			public void Flatten(Parcel parcel) {
				parcel.WriteUnmanagedType(ref this);
			}

			public uint GetFdCount() {
				return 0;
			}

			public uint GetFlattenedSize() {
				return (uint) Unsafe.SizeOf<QueueBufferInput>();
			}

			public void Unflatten(Parcel parcel) {
				this = parcel.ReadUnmanagedType<QueueBufferInput>();
			}
		}

		public struct QueueBufferOutput {
			public uint Width;
			public uint Height;
			public NativeWindowTransform TransformHint;
			public uint NumPendingBuffers;
			public ulong FrameNumber;

			public void WriteToParcel(Parcel parcel) {
				parcel.WriteUInt32(Width);
				parcel.WriteUInt32(Height);
				parcel.WriteUnmanagedType(ref TransformHint);
				parcel.WriteUInt32(NumPendingBuffers);

				if(TransformHint.HasFlag(NativeWindowTransform.ReturnFrameNumber)) parcel.WriteUInt64(FrameNumber);
			}
		}
	}
}