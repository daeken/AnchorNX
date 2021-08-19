#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Codec.Detail {
	public unsafe partial class IHardwareOpusDecoderManager : _Base_IHardwareOpusDecoderManager {}
	public class _Base_IHardwareOpusDecoderManager : IpcInterface {
		static readonly Logger Logger = new("IHardwareOpusDecoderManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Initialize
					var ret = Initialize(im.GetBytes(8, 0x8), im.GetData<uint>(16), im.GetCopy(0));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 1: { // GetWorkBufferSize
					var ret = GetWorkBufferSize(im.GetBytes(8, 0x8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 2: { // InitializeMultiStream
					var ret = InitializeMultiStream(im.GetData<uint>(8), im.GetCopy(0), im.GetBuffer<byte>(0x19, 0));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 3: { // GetWorkBufferSizeMultiStream
					var ret = GetWorkBufferSizeMultiStream(im.GetBuffer<byte>(0x19, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IHardwareOpusDecoderManager: {im.CommandId}");
			}
		}
		
		public virtual Nn.Codec.Detail.IHardwareOpusDecoder Initialize(byte[] _0, uint _1, uint _2) => throw new NotImplementedException();
		public virtual uint GetWorkBufferSize(byte[] _0) => throw new NotImplementedException();
		public virtual Nn.Codec.Detail.IHardwareOpusDecoder InitializeMultiStream(uint _0, uint _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual uint GetWorkBufferSizeMultiStream(Buffer<byte> _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IHardwareOpusDecoder : _Base_IHardwareOpusDecoder {}
	public class _Base_IHardwareOpusDecoder : IpcInterface {
		static readonly Logger Logger = new("IHardwareOpusDecoder");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // DecodeInterleaved
					DecodeInterleaved(im.GetBuffer<byte>(0x5, 0), out var _0, out var _1, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 1: { // SetContext
					SetContext(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(im.GetBuffer<byte>(0x5, 0), out var _0, out var _1, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 3: { // Unknown3
					Unknown3(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					Unknown4(im.GetBuffer<byte>(0x5, 0), out var _0, out var _1, out var _2, im.GetBuffer<byte>(0x46, 0));
					om.Initialize(0, 0, 16);
					om.SetData(8, _0);
					om.SetData(12, _1);
					om.SetData(16, _2);
					break;
				}
				case 5: { // Unknown5
					Unknown5(im.GetBuffer<byte>(0x5, 0), out var _0, out var _1, out var _2, im.GetBuffer<byte>(0x46, 0));
					om.Initialize(0, 0, 16);
					om.SetData(8, _0);
					om.SetData(12, _1);
					om.SetData(16, _2);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IHardwareOpusDecoder: {im.CommandId}");
			}
		}
		
		public virtual void DecodeInterleaved(Buffer<byte> _0, out uint _1, out uint _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void SetContext(Buffer<byte> _0) => "Stub hit for Nn.Codec.Detail.IHardwareOpusDecoder.SetContext [1]".Debug(Log);
		public virtual void Unknown2(Buffer<byte> _0, out uint _1, out uint _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void Unknown3(Buffer<byte> _0) => "Stub hit for Nn.Codec.Detail.IHardwareOpusDecoder.Unknown3 [3]".Debug(Log);
		public virtual void Unknown4(Buffer<byte> _0, out uint _1, out uint _2, out ulong _3, Buffer<byte> _4) => throw new NotImplementedException();
		public virtual void Unknown5(Buffer<byte> _0, out uint _1, out uint _2, out ulong _3, Buffer<byte> _4) => throw new NotImplementedException();
	}
}
