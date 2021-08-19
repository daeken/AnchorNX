#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Grcsrv {
	public unsafe partial class IGrcService : _Base_IGrcService {}
	public class _Base_IGrcService : IpcInterface {
		static readonly Logger Logger = new("IGrcService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 1: { // OpenContinuousRecorder
					var ret = OpenContinuousRecorder(null, im.GetCopy(0));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 2: { // OpenGameMovieTrimmer
					var ret = OpenGameMovieTrimmer(null, im.GetCopy(0));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IGrcService: {im.CommandId}");
			}
		}
		
		public virtual Nn.Grcsrv.IContinuousRecorder OpenContinuousRecorder(object _0, uint _1) => throw new NotImplementedException();
		public virtual Nn.Grcsrv.IGameMovieTrimmer OpenGameMovieTrimmer(object _0, uint _1) => throw new NotImplementedException();
	}
	
	public unsafe partial class IContinuousRecorder : _Base_IContinuousRecorder {}
	public class _Base_IContinuousRecorder : IpcInterface {
		static readonly Logger Logger = new("IContinuousRecorder");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 1: { // Unknown1
					Unknown1();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2();
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // Unknown10
					var ret = Unknown10();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 11: { // Unknown11
					Unknown11();
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // Unknown12
					Unknown12();
					om.Initialize(0, 0, 0);
					break;
				}
				case 13: { // Unknown13
					Unknown13(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IContinuousRecorder: {im.CommandId}");
			}
		}
		
		public virtual void Unknown1() => "Stub hit for Nn.Grcsrv.IContinuousRecorder.Unknown1 [1]".Debug(Log);
		public virtual void Unknown2() => "Stub hit for Nn.Grcsrv.IContinuousRecorder.Unknown2 [2]".Debug(Log);
		public virtual uint Unknown10() => throw new NotImplementedException();
		public virtual void Unknown11() => "Stub hit for Nn.Grcsrv.IContinuousRecorder.Unknown11 [11]".Debug(Log);
		public virtual void Unknown12() => "Stub hit for Nn.Grcsrv.IContinuousRecorder.Unknown12 [12]".Debug(Log);
		public virtual void Unknown13(object _0) => "Stub hit for Nn.Grcsrv.IContinuousRecorder.Unknown13 [13]".Debug(Log);
	}
	
	public unsafe partial class IGameMovieTrimmer : _Base_IGameMovieTrimmer {}
	public class _Base_IGameMovieTrimmer : IpcInterface {
		static readonly Logger Logger = new("IGameMovieTrimmer");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 1: { // BeginTrim
					BeginTrim(im.GetData<uint>(8), im.GetData<uint>(12), im.GetBytes(16, 0x40));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // EndTrim
					EndTrim(out var _0);
					om.Initialize(0, 0, 64);
					om.SetBytes(8, _0);
					break;
				}
				case 10: { // GetNotTrimmingEvent
					var ret = GetNotTrimmingEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 20: { // SetThumbnailRgba
					SetThumbnailRgba(im.GetData<uint>(8), im.GetData<uint>(12), im.GetBuffer<byte>(0x45, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IGameMovieTrimmer: {im.CommandId}");
			}
		}
		
		public virtual void BeginTrim(uint _0, uint _1, byte[] _2) => "Stub hit for Nn.Grcsrv.IGameMovieTrimmer.BeginTrim [1]".Debug(Log);
		public virtual void EndTrim(out byte[] _0) => throw new NotImplementedException();
		public virtual uint GetNotTrimmingEvent() => throw new NotImplementedException();
		public virtual void SetThumbnailRgba(uint _0, uint _1, Buffer<byte> _2) => "Stub hit for Nn.Grcsrv.IGameMovieTrimmer.SetThumbnailRgba [20]".Debug(Log);
	}
}
