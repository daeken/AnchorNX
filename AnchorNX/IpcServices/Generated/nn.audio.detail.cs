#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Audio.Detail {
	public unsafe struct AudioRendererUpdateDataHeader {
		public int Revision;
		public int BehaviorSize;
		public int MemoryPoolSize;
		public int VoiceSize;
		public int VoiceResourceSize;
		public int EffectSize;
		public int MixSize;
		public int SinkSize;
		public int PerformanceManagerSize;
		public int Unknown24;
		public int Unknown28;
		public int Unknown2C;
		public int Unknown30;
		public int Unknown34;
		public int Unknown38;
		public int TotalSize;
	}
	
	public unsafe struct AudioRendererParameterInternal {
		public int SampleRate;
		public int SampleCount;
		public int Unknown8;
		public int MixCount;
		public int VoiceCount;
		public int SinkCount;
		public int EffectCount;
		public int PerformanceManagerCount;
		public int VoiceDropEnable;
		public int SplitterCount;
		public int SplitterDestinationDataCount;
		public int Unknown2C;
		public int Revision;
	}
	
	public unsafe partial class IAudioDebugManager : _Base_IAudioDebugManager {}
	public class _Base_IAudioDebugManager : IpcInterface {
		static readonly Logger Logger = new("IAudioDebugManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
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
				case 3: { // Unknown3
					Unknown3();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioDebugManager: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(uint _0, ulong _1, uint _2) => "Stub hit for Nn.Audio.Detail.IAudioDebugManager.Unknown0 [0]".Debug(Log);
		public virtual void Unknown1() => "Stub hit for Nn.Audio.Detail.IAudioDebugManager.Unknown1 [1]".Debug(Log);
		public virtual void Unknown2() => "Stub hit for Nn.Audio.Detail.IAudioDebugManager.Unknown2 [2]".Debug(Log);
		public virtual void Unknown3() => "Stub hit for Nn.Audio.Detail.IAudioDebugManager.Unknown3 [3]".Debug(Log);
	}
	
	public unsafe partial class IAudioInManager : _Base_IAudioInManager {}
	public class _Base_IAudioInManager : IpcInterface {
		static readonly Logger Logger = new("IAudioInManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // ListAudioIns
					ListAudioIns(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 1: { // OpenAudioIn
					OpenAudioIn(im.GetData<ulong>(8), im.GetData<ulong>(16), im.Pid, im.GetCopy(0), im.GetBuffer<byte>(0x5, 0), out var _0, out var _1, out var _2, out var _3, out var _4, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(1, 0, 16);
					om.SetData(8, _0);
					om.SetData(12, _1);
					om.SetData(16, _2);
					om.SetData(20, _3);
					om.Move(0, await CreateHandle(_4));
					break;
				}
				case 2: { // Unknown2
					Unknown2(out var _0, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 3: { // OpenAudioInAuto
					OpenAudioInAuto(im.GetData<ulong>(8), im.GetData<ulong>(16), im.Pid, im.GetCopy(0), im.GetBuffer<byte>(0x21, 0), out var _0, out var _1, out var _2, out var _3, out var _4, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(1, 0, 16);
					om.SetData(8, _0);
					om.SetData(12, _1);
					om.SetData(16, _2);
					om.SetData(20, _3);
					om.Move(0, await CreateHandle(_4));
					break;
				}
				case 4: { // ListAudioInsAuto
					ListAudioInsAuto(out var _0, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioInManager: {im.CommandId}");
			}
		}
		
		public virtual void ListAudioIns(out uint count, Buffer<byte> names) => throw new NotImplementedException();
		public virtual void OpenAudioIn(ulong _0, ulong pid_copy, ulong _2, uint _3, Buffer<byte> name0, out uint sample_rate, out uint channel_count, out uint pcm_format, out uint _8, out Nn.Audio.Detail.IAudioIn _9, Buffer<byte> name1) => throw new NotImplementedException();
		public virtual void Unknown2(out uint _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void OpenAudioInAuto(ulong _0, ulong pid_copy, ulong _2, uint _3, Buffer<byte> _4, out uint sample_rate, out uint channel_count, out uint pcm_format, out uint _8, out Nn.Audio.Detail.IAudioIn _9, Buffer<byte> name) => throw new NotImplementedException();
		public virtual void ListAudioInsAuto(out uint count, Buffer<byte> names) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAudioInManagerForApplet : _Base_IAudioInManagerForApplet {}
	public class _Base_IAudioInManagerForApplet : IpcInterface {
		static readonly Logger Logger = new("IAudioInManagerForApplet");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestSuspendAudioIns
					RequestSuspendAudioIns(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // RequestResumeAudioIns
					RequestResumeAudioIns(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetAudioInsProcessMasterVolume
					var ret = GetAudioInsProcessMasterVolume(im.GetData<ulong>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 3: { // SetAudioInsProcessMasterVolume
					SetAudioInsProcessMasterVolume(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetData<ulong>(24));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioInManagerForApplet: {im.CommandId}");
			}
		}
		
		public virtual void RequestSuspendAudioIns(ulong _0, ulong _1) => "Stub hit for Nn.Audio.Detail.IAudioInManagerForApplet.RequestSuspendAudioIns [0]".Debug(Log);
		public virtual void RequestResumeAudioIns(ulong _0, ulong _1) => "Stub hit for Nn.Audio.Detail.IAudioInManagerForApplet.RequestResumeAudioIns [1]".Debug(Log);
		public virtual uint GetAudioInsProcessMasterVolume(ulong _0) => throw new NotImplementedException();
		public virtual void SetAudioInsProcessMasterVolume(uint _0, ulong _1, ulong _2) => "Stub hit for Nn.Audio.Detail.IAudioInManagerForApplet.SetAudioInsProcessMasterVolume [3]".Debug(Log);
	}
	
	public unsafe partial class IAudioInManagerForDebugger : _Base_IAudioInManagerForDebugger {}
	public class _Base_IAudioInManagerForDebugger : IpcInterface {
		static readonly Logger Logger = new("IAudioInManagerForDebugger");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestSuspendAudioInsForDebug
					RequestSuspendAudioInsForDebug(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // RequestResumeAudioInsForDebug
					RequestResumeAudioInsForDebug(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioInManagerForDebugger: {im.CommandId}");
			}
		}
		
		public virtual void RequestSuspendAudioInsForDebug(ulong _0) => "Stub hit for Nn.Audio.Detail.IAudioInManagerForDebugger.RequestSuspendAudioInsForDebug [0]".Debug(Log);
		public virtual void RequestResumeAudioInsForDebug(ulong _0) => "Stub hit for Nn.Audio.Detail.IAudioInManagerForDebugger.RequestResumeAudioInsForDebug [1]".Debug(Log);
	}
	
	public unsafe partial class IAudioOutManager : _Base_IAudioOutManager {}
	public class _Base_IAudioOutManager : IpcInterface {
		static readonly Logger Logger = new("IAudioOutManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // ListAudioOuts
					ListAudioOuts(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 1: { // OpenAudioOut
					OpenAudioOut(im.GetData<uint>(8), im.GetData<ushort>(12), im.GetData<ushort>(14), im.GetData<ulong>(16), im.Pid, im.GetCopy(0), im.GetBuffer<byte>(0x5, 0), out var _0, out var _1, out var _2, out var _3, out var _4, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(1, 0, 16);
					om.SetData(8, _0);
					om.SetData(12, _1);
					om.SetData(16, _2);
					om.SetData(20, _3);
					om.Move(0, await CreateHandle(_4));
					break;
				}
				case 2: { // ListAudioOutsAuto
					ListAudioOutsAuto(out var _0, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 3: { // OpenAudioOutAuto
					OpenAudioOutAuto(im.GetData<uint>(8), im.GetData<ushort>(12), im.GetData<ushort>(14), im.GetData<ulong>(16), im.Pid, im.GetCopy(0), im.GetBuffer<byte>(0x21, 0), out var _0, out var _1, out var _2, out var _3, out var _4, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(1, 0, 16);
					om.SetData(8, _0);
					om.SetData(12, _1);
					om.SetData(16, _2);
					om.SetData(20, _3);
					om.Move(0, await CreateHandle(_4));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioOutManager: {im.CommandId}");
			}
		}
		
		public virtual void ListAudioOuts(out uint count, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void OpenAudioOut(uint sample_rate0, ushort unused, ushort channel_count0, ulong _3, ulong _4, uint _5, Buffer<byte> name_in, out uint sample_rate1, out uint channel_count1, out uint pcm_format, out uint _10, out Nn.Audio.Detail.IAudioOut _11, Buffer<byte> name_out) => throw new NotImplementedException();
		public virtual void ListAudioOutsAuto(out uint count, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void OpenAudioOutAuto(uint sample_rate0, ushort unused, ushort channel_count0, ulong _3, ulong _4, uint _5, Buffer<byte> _6, out uint sample_rate1, out uint channel_count1, out uint pcm_format, out uint _10, out Nn.Audio.Detail.IAudioOut _11, Buffer<byte> name_out) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAudioOutManagerForApplet : _Base_IAudioOutManagerForApplet {}
	public class _Base_IAudioOutManagerForApplet : IpcInterface {
		static readonly Logger Logger = new("IAudioOutManagerForApplet");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestSuspendAudioOuts
					RequestSuspendAudioOuts(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // RequestResumeAudioOuts
					RequestResumeAudioOuts(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetAudioOutsProcessMasterVolume
					var ret = GetAudioOutsProcessMasterVolume(im.GetData<ulong>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 3: { // SetAudioOutsProcessMasterVolume
					SetAudioOutsProcessMasterVolume(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetData<ulong>(24));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // GetAudioOutsProcessRecordVolume
					var ret = GetAudioOutsProcessRecordVolume(im.GetData<ulong>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 5: { // SetAudioOutsProcessRecordVolume
					SetAudioOutsProcessRecordVolume(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetData<ulong>(24));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioOutManagerForApplet: {im.CommandId}");
			}
		}
		
		public virtual void RequestSuspendAudioOuts(ulong _0, ulong _1) => "Stub hit for Nn.Audio.Detail.IAudioOutManagerForApplet.RequestSuspendAudioOuts [0]".Debug(Log);
		public virtual void RequestResumeAudioOuts(ulong _0, ulong _1) => "Stub hit for Nn.Audio.Detail.IAudioOutManagerForApplet.RequestResumeAudioOuts [1]".Debug(Log);
		public virtual uint GetAudioOutsProcessMasterVolume(ulong _0) => throw new NotImplementedException();
		public virtual void SetAudioOutsProcessMasterVolume(uint _0, ulong _1, ulong _2) => "Stub hit for Nn.Audio.Detail.IAudioOutManagerForApplet.SetAudioOutsProcessMasterVolume [3]".Debug(Log);
		public virtual uint GetAudioOutsProcessRecordVolume(ulong _0) => throw new NotImplementedException();
		public virtual void SetAudioOutsProcessRecordVolume(uint _0, ulong _1, ulong _2) => "Stub hit for Nn.Audio.Detail.IAudioOutManagerForApplet.SetAudioOutsProcessRecordVolume [5]".Debug(Log);
	}
	
	public unsafe partial class IAudioOutManagerForDebugger : _Base_IAudioOutManagerForDebugger {}
	public class _Base_IAudioOutManagerForDebugger : IpcInterface {
		static readonly Logger Logger = new("IAudioOutManagerForDebugger");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestSuspendAudioOutsForDebug
					RequestSuspendAudioOutsForDebug(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // RequestResumeAudioOutsForDebug
					RequestResumeAudioOutsForDebug(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioOutManagerForDebugger: {im.CommandId}");
			}
		}
		
		public virtual void RequestSuspendAudioOutsForDebug(ulong _0) => "Stub hit for Nn.Audio.Detail.IAudioOutManagerForDebugger.RequestSuspendAudioOutsForDebug [0]".Debug(Log);
		public virtual void RequestResumeAudioOutsForDebug(ulong _0) => "Stub hit for Nn.Audio.Detail.IAudioOutManagerForDebugger.RequestResumeAudioOutsForDebug [1]".Debug(Log);
	}
	
	public unsafe partial class IAudioRendererManager : _Base_IAudioRendererManager {}
	public class _Base_IAudioRendererManager : IpcInterface {
		static readonly Logger Logger = new("IAudioRendererManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenAudioRenderer
					var ret = OpenAudioRenderer(im.GetDataSpan<Nn.Audio.Detail.AudioRendererParameterInternal>(8), im.GetData<ulong>(8), im.GetData<ulong>(16), im.Pid, im.GetCopy(0), im.GetCopy(1));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 1: { // GetWorkBufferSize
					var ret = GetWorkBufferSize(im.GetDataSpan<Nn.Audio.Detail.AudioRendererParameterInternal>(8));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 2: { // GetAudioDeviceService
					var ret = GetAudioDeviceService(im.GetData<ulong>(8));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 3: { // OpenAudioRendererAuto
					var ret = OpenAudioRendererAuto(im.GetDataSpan<Nn.Audio.Detail.AudioRendererParameterInternal>(8), im.GetData<ulong>(8), im.GetData<ulong>(16), im.GetData<ulong>(24), im.Pid, im.GetCopy(0));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 4: { // GetAudioDeviceServiceWithRevisionInfo
					var ret = GetAudioDeviceServiceWithRevisionInfo(im.GetData<ulong>(8), im.GetData<uint>(16));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioRendererManager: {im.CommandId}");
			}
		}
		
		public virtual Nn.Audio.Detail.IAudioRenderer OpenAudioRenderer(Span<Nn.Audio.Detail.AudioRendererParameterInternal> _0, ulong _1, ulong _2, ulong _3, uint _4, uint _5) => throw new NotImplementedException();
		public virtual ulong GetWorkBufferSize(Span<Nn.Audio.Detail.AudioRendererParameterInternal> _0) => throw new NotImplementedException();
		public virtual Nn.Audio.Detail.IAudioDevice GetAudioDeviceService(ulong _0) => throw new NotImplementedException();
		public virtual Nn.Audio.Detail.IAudioRenderer OpenAudioRendererAuto(Span<Nn.Audio.Detail.AudioRendererParameterInternal> _0, ulong _1, ulong _2, ulong _3, ulong _4, uint _5) => throw new NotImplementedException();
		public virtual Nn.Audio.Detail.IAudioDevice GetAudioDeviceServiceWithRevisionInfo(ulong _0, uint _1) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAudioRendererManagerForApplet : _Base_IAudioRendererManagerForApplet {}
	public class _Base_IAudioRendererManagerForApplet : IpcInterface {
		static readonly Logger Logger = new("IAudioRendererManagerForApplet");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestSuspendAudioRenderers
					RequestSuspendAudioRenderers(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // RequestResumeAudioRenderers
					RequestResumeAudioRenderers(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetAudioRenderersProcessMasterVolume
					var ret = GetAudioRenderersProcessMasterVolume(im.GetData<ulong>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 3: { // SetAudioRenderersProcessMasterVolume
					SetAudioRenderersProcessMasterVolume(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetData<ulong>(24));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // RegisterAppletResourceUserId
					RegisterAppletResourceUserId(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // UnregisterAppletResourceUserId
					UnregisterAppletResourceUserId(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // GetAudioRenderersProcessRecordVolume
					var ret = GetAudioRenderersProcessRecordVolume(im.GetData<ulong>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 7: { // SetAudioRenderersProcessRecordVolume
					SetAudioRenderersProcessRecordVolume(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetData<ulong>(24));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioRendererManagerForApplet: {im.CommandId}");
			}
		}
		
		public virtual void RequestSuspendAudioRenderers(ulong _0, ulong _1) => "Stub hit for Nn.Audio.Detail.IAudioRendererManagerForApplet.RequestSuspendAudioRenderers [0]".Debug(Log);
		public virtual void RequestResumeAudioRenderers(ulong _0, ulong _1) => "Stub hit for Nn.Audio.Detail.IAudioRendererManagerForApplet.RequestResumeAudioRenderers [1]".Debug(Log);
		public virtual uint GetAudioRenderersProcessMasterVolume(ulong _0) => throw new NotImplementedException();
		public virtual void SetAudioRenderersProcessMasterVolume(uint _0, ulong _1, ulong _2) => "Stub hit for Nn.Audio.Detail.IAudioRendererManagerForApplet.SetAudioRenderersProcessMasterVolume [3]".Debug(Log);
		public virtual void RegisterAppletResourceUserId(ulong _0) => "Stub hit for Nn.Audio.Detail.IAudioRendererManagerForApplet.RegisterAppletResourceUserId [4]".Debug(Log);
		public virtual void UnregisterAppletResourceUserId(ulong _0) => "Stub hit for Nn.Audio.Detail.IAudioRendererManagerForApplet.UnregisterAppletResourceUserId [5]".Debug(Log);
		public virtual uint GetAudioRenderersProcessRecordVolume(ulong _0) => throw new NotImplementedException();
		public virtual void SetAudioRenderersProcessRecordVolume(uint _0, ulong _1, ulong _2) => "Stub hit for Nn.Audio.Detail.IAudioRendererManagerForApplet.SetAudioRenderersProcessRecordVolume [7]".Debug(Log);
	}
	
	public unsafe partial class IAudioRendererManagerForDebugger : _Base_IAudioRendererManagerForDebugger {}
	public class _Base_IAudioRendererManagerForDebugger : IpcInterface {
		static readonly Logger Logger = new("IAudioRendererManagerForDebugger");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestSuspendForDebug
					RequestSuspendForDebug(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // RequestResumeForDebug
					RequestResumeForDebug(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioRendererManagerForDebugger: {im.CommandId}");
			}
		}
		
		public virtual void RequestSuspendForDebug(ulong _0) => "Stub hit for Nn.Audio.Detail.IAudioRendererManagerForDebugger.RequestSuspendForDebug [0]".Debug(Log);
		public virtual void RequestResumeForDebug(ulong _0) => "Stub hit for Nn.Audio.Detail.IAudioRendererManagerForDebugger.RequestResumeForDebug [1]".Debug(Log);
	}
	
	public unsafe partial class ICodecController : _Base_ICodecController {}
	public class _Base_ICodecController : IpcInterface {
		static readonly Logger Logger = new("ICodecController");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // InitializeCodecController
					InitializeCodecController();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // FinalizeCodecController
					FinalizeCodecController();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // SleepCodecController
					SleepCodecController();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // WakeCodecController
					WakeCodecController();
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // SetCodecVolume
					SetCodecVolume(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // GetCodecVolumeMax
					var ret = GetCodecVolumeMax();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 6: { // GetCodecVolumeMin
					var ret = GetCodecVolumeMin();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 7: { // SetCodecActiveTarget
					SetCodecActiveTarget(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // GetCodecActiveTarget
					var ret = GetCodecActiveTarget();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 9: { // BindCodecHeadphoneMicJackInterrupt
					var ret = BindCodecHeadphoneMicJackInterrupt();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 10: { // IsCodecHeadphoneMicJackInserted
					var ret = IsCodecHeadphoneMicJackInserted();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 11: { // ClearCodecHeadphoneMicJackInterrupt
					ClearCodecHeadphoneMicJackInterrupt();
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // IsCodecDeviceRequested
					var ret = IsCodecDeviceRequested();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ICodecController: {im.CommandId}");
			}
		}
		
		public virtual void InitializeCodecController() => "Stub hit for Nn.Audio.Detail.ICodecController.InitializeCodecController [0]".Debug(Log);
		public virtual void FinalizeCodecController() => "Stub hit for Nn.Audio.Detail.ICodecController.FinalizeCodecController [1]".Debug(Log);
		public virtual void SleepCodecController() => "Stub hit for Nn.Audio.Detail.ICodecController.SleepCodecController [2]".Debug(Log);
		public virtual void WakeCodecController() => "Stub hit for Nn.Audio.Detail.ICodecController.WakeCodecController [3]".Debug(Log);
		public virtual void SetCodecVolume(uint _0) => "Stub hit for Nn.Audio.Detail.ICodecController.SetCodecVolume [4]".Debug(Log);
		public virtual uint GetCodecVolumeMax() => throw new NotImplementedException();
		public virtual uint GetCodecVolumeMin() => throw new NotImplementedException();
		public virtual void SetCodecActiveTarget(uint _0) => "Stub hit for Nn.Audio.Detail.ICodecController.SetCodecActiveTarget [7]".Debug(Log);
		public virtual uint GetCodecActiveTarget() => throw new NotImplementedException();
		public virtual uint BindCodecHeadphoneMicJackInterrupt() => throw new NotImplementedException();
		public virtual byte IsCodecHeadphoneMicJackInserted() => throw new NotImplementedException();
		public virtual void ClearCodecHeadphoneMicJackInterrupt() => "Stub hit for Nn.Audio.Detail.ICodecController.ClearCodecHeadphoneMicJackInterrupt [11]".Debug(Log);
		public virtual byte IsCodecDeviceRequested() => throw new NotImplementedException();
	}
	
	public unsafe partial class IFinalOutputRecorderManager : _Base_IFinalOutputRecorderManager {}
	public class _Base_IFinalOutputRecorderManager : IpcInterface {
		static readonly Logger Logger = new("IFinalOutputRecorderManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenFinalOutputRecorder
					OpenFinalOutputRecorder(im.GetBytes(8, 0x8), im.GetData<ulong>(16), im.GetCopy(0), out var _0, out var _1);
					om.Initialize(1, 0, 16);
					om.SetBytes(8, _0);
					om.Move(0, await CreateHandle(_1));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IFinalOutputRecorderManager: {im.CommandId}");
			}
		}
		
		public virtual void OpenFinalOutputRecorder(byte[] _0, ulong _1, uint _2, out byte[] _3, out Nn.Audio.Detail.IFinalOutputRecorder _4) => throw new NotImplementedException();
	}
	
	public unsafe partial class IFinalOutputRecorderManagerForApplet : _Base_IFinalOutputRecorderManagerForApplet {}
	public class _Base_IFinalOutputRecorderManagerForApplet : IpcInterface {
		static readonly Logger Logger = new("IFinalOutputRecorderManagerForApplet");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestSuspendFinalOutputRecorders
					RequestSuspendFinalOutputRecorders(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // RequestResumeFinalOutputRecorders
					RequestResumeFinalOutputRecorders(im.GetData<ulong>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IFinalOutputRecorderManagerForApplet: {im.CommandId}");
			}
		}
		
		public virtual void RequestSuspendFinalOutputRecorders(ulong _0, ulong _1) => "Stub hit for Nn.Audio.Detail.IFinalOutputRecorderManagerForApplet.RequestSuspendFinalOutputRecorders [0]".Debug(Log);
		public virtual void RequestResumeFinalOutputRecorders(ulong _0, ulong _1) => "Stub hit for Nn.Audio.Detail.IFinalOutputRecorderManagerForApplet.RequestResumeFinalOutputRecorders [1]".Debug(Log);
	}
	
	public unsafe partial class IFinalOutputRecorderManagerForDebugger : _Base_IFinalOutputRecorderManagerForDebugger {}
	public class _Base_IFinalOutputRecorderManagerForDebugger : IpcInterface {
		static readonly Logger Logger = new("IFinalOutputRecorderManagerForDebugger");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RequestSuspendForDebug
					RequestSuspendForDebug(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // RequestResumeForDebug
					RequestResumeForDebug(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IFinalOutputRecorderManagerForDebugger: {im.CommandId}");
			}
		}
		
		public virtual void RequestSuspendForDebug(ulong _0) => "Stub hit for Nn.Audio.Detail.IFinalOutputRecorderManagerForDebugger.RequestSuspendForDebug [0]".Debug(Log);
		public virtual void RequestResumeForDebug(ulong _0) => "Stub hit for Nn.Audio.Detail.IFinalOutputRecorderManagerForDebugger.RequestResumeForDebug [1]".Debug(Log);
	}
	
	public unsafe partial class IAudioDevice : _Base_IAudioDevice {}
	public class _Base_IAudioDevice : IpcInterface {
		static readonly Logger Logger = new("IAudioDevice");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // ListAudioDeviceName
					ListAudioDeviceName(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 1: { // SetAudioDeviceOutputVolume
					SetAudioDeviceOutputVolume(im.GetData<uint>(8), im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetAudioDeviceOutputVolume
					var ret = GetAudioDeviceOutputVolume(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 3: { // GetActiveAudioDeviceName
					GetActiveAudioDeviceName(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // QueryAudioDeviceSystemEvent
					var ret = QueryAudioDeviceSystemEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 5: { // GetActiveChannelCount
					var ret = GetActiveChannelCount();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 6: { // ListAudioDeviceNameAuto
					ListAudioDeviceNameAuto(out var _0, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 7: { // SetAudioDeviceOutputVolumeAuto
					SetAudioDeviceOutputVolumeAuto(im.GetData<uint>(8), im.GetBuffer<byte>(0x21, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // GetAudioDeviceOutputVolumeAuto
					var ret = GetAudioDeviceOutputVolumeAuto(im.GetBuffer<byte>(0x21, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 10: { // GetActiveAudioDeviceNameAuto
					GetActiveAudioDeviceNameAuto(im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // QueryAudioDeviceInputEvent
					var ret = QueryAudioDeviceInputEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 12: { // QueryAudioDeviceOutputEvent
					var ret = QueryAudioDeviceOutputEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioDevice: {im.CommandId}");
			}
		}
		
		public virtual void ListAudioDeviceName(out uint _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void SetAudioDeviceOutputVolume(uint _0, Buffer<byte> _1) => "Stub hit for Nn.Audio.Detail.IAudioDevice.SetAudioDeviceOutputVolume [1]".Debug(Log);
		public virtual uint GetAudioDeviceOutputVolume(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void GetActiveAudioDeviceName(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual uint QueryAudioDeviceSystemEvent() => throw new NotImplementedException();
		public virtual uint GetActiveChannelCount() => throw new NotImplementedException();
		public virtual void ListAudioDeviceNameAuto(out uint _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void SetAudioDeviceOutputVolumeAuto(uint _0, Buffer<byte> _1) => "Stub hit for Nn.Audio.Detail.IAudioDevice.SetAudioDeviceOutputVolumeAuto [7]".Debug(Log);
		public virtual float GetAudioDeviceOutputVolumeAuto(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void GetActiveAudioDeviceNameAuto(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual uint QueryAudioDeviceInputEvent() => throw new NotImplementedException();
		public virtual uint QueryAudioDeviceOutputEvent() => throw new NotImplementedException();
	}
	
	public unsafe partial class IAudioIn : _Base_IAudioIn {}
	public class _Base_IAudioIn : IpcInterface {
		static readonly Logger Logger = new("IAudioIn");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetAudioInState
					var ret = GetAudioInState();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 1: { // StartAudioIn
					StartAudioIn();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // StopAudioIn
					StopAudioIn();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // AppendAudioInBuffer
					AppendAudioInBuffer(im.GetData<ulong>(8), im.GetBuffer<Nn.Audio.AudioInBuffer>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // RegisterBufferEvent
					var ret = RegisterBufferEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 5: { // GetReleasedAudioInBuffer
					GetReleasedAudioInBuffer(out var _0, im.GetBuffer<Nn.Audio.AudioInBuffer>(0x6, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 6: { // ContainsAudioInBuffer
					var ret = ContainsAudioInBuffer(im.GetData<ulong>(8));
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 7: { // AppendAudioInBufferWithUserEvent
					AppendAudioInBufferWithUserEvent(im.GetData<ulong>(8), im.GetCopy(0), im.GetBuffer<Nn.Audio.AudioInBuffer>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // AppendAudioInBufferAuto
					AppendAudioInBufferAuto(im.GetData<ulong>(8), im.GetBuffer<Nn.Audio.AudioInBuffer>(0x21, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // GetReleasedAudioInBufferAuto
					GetReleasedAudioInBufferAuto(out var _0, im.GetBuffer<Nn.Audio.AudioInBuffer>(0x22, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 10: { // AppendAudioInBufferWithUserEventAuto
					AppendAudioInBufferWithUserEventAuto(im.GetData<ulong>(8), im.GetCopy(0), im.GetBuffer<Nn.Audio.AudioInBuffer>(0x21, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // GetAudioInBufferCount
					var ret = GetAudioInBufferCount();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 12: { // SetAudioInDeviceGain
					SetAudioInDeviceGain(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 13: { // GetAudioInDeviceGain
					var ret = GetAudioInDeviceGain();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioIn: {im.CommandId}");
			}
		}
		
		public virtual uint GetAudioInState() => throw new NotImplementedException();
		public virtual void StartAudioIn() => "Stub hit for Nn.Audio.Detail.IAudioIn.StartAudioIn [1]".Debug(Log);
		public virtual void StopAudioIn() => "Stub hit for Nn.Audio.Detail.IAudioIn.StopAudioIn [2]".Debug(Log);
		public virtual void AppendAudioInBuffer(ulong tag, Buffer<Nn.Audio.AudioInBuffer> _1) => "Stub hit for Nn.Audio.Detail.IAudioIn.AppendAudioInBuffer [3]".Debug(Log);
		public virtual uint RegisterBufferEvent() => throw new NotImplementedException();
		public virtual void GetReleasedAudioInBuffer(out uint count, Buffer<Nn.Audio.AudioInBuffer> _1) => throw new NotImplementedException();
		public virtual byte ContainsAudioInBuffer(ulong tag) => throw new NotImplementedException();
		public virtual void AppendAudioInBufferWithUserEvent(ulong tag, uint _1, Buffer<Nn.Audio.AudioInBuffer> _2) => "Stub hit for Nn.Audio.Detail.IAudioIn.AppendAudioInBufferWithUserEvent [7]".Debug(Log);
		public virtual void AppendAudioInBufferAuto(ulong tag, Buffer<Nn.Audio.AudioInBuffer> _1) => "Stub hit for Nn.Audio.Detail.IAudioIn.AppendAudioInBufferAuto [8]".Debug(Log);
		public virtual void GetReleasedAudioInBufferAuto(out uint count, Buffer<Nn.Audio.AudioInBuffer> _1) => throw new NotImplementedException();
		public virtual void AppendAudioInBufferWithUserEventAuto(ulong tag, uint _1, Buffer<Nn.Audio.AudioInBuffer> _2) => "Stub hit for Nn.Audio.Detail.IAudioIn.AppendAudioInBufferWithUserEventAuto [10]".Debug(Log);
		public virtual uint GetAudioInBufferCount() => throw new NotImplementedException();
		public virtual void SetAudioInDeviceGain(uint gain) => "Stub hit for Nn.Audio.Detail.IAudioIn.SetAudioInDeviceGain [12]".Debug(Log);
		public virtual uint GetAudioInDeviceGain() => throw new NotImplementedException();
	}
	
	public unsafe partial class IAudioOut : _Base_IAudioOut {}
	public class _Base_IAudioOut : IpcInterface {
		static readonly Logger Logger = new("IAudioOut");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetAudioOutState
					var ret = GetAudioOutState();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 1: { // StartAudioOut
					StartAudioOut();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // StopAudioOut
					StopAudioOut();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // AppendAudioOutBuffer
					AppendAudioOutBuffer(im.GetData<ulong>(8), im.GetBuffer<Nn.Audio.AudioOutBuffer>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // RegisterBufferEvent
					var ret = RegisterBufferEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 5: { // GetReleasedAudioOutBuffer
					GetReleasedAudioOutBuffer(out var _0, im.GetBuffer<Nn.Audio.AudioOutBuffer>(0x6, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 6: { // ContainsAudioOutBuffer
					var ret = ContainsAudioOutBuffer(im.GetData<ulong>(8));
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 7: { // AppendAudioOutBufferAuto
					AppendAudioOutBufferAuto(im.GetData<ulong>(8), im.GetBuffer<Nn.Audio.AudioOutBuffer>(0x21, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // GetReleasedAudioOutBufferAuto
					GetReleasedAudioOutBufferAuto(out var _0, im.GetBuffer<Nn.Audio.AudioOutBuffer>(0x22, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 9: { // GetAudioOutBufferCount
					var ret = GetAudioOutBufferCount();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 10: { // GetAudioOutPlayedSampleCount
					var ret = GetAudioOutPlayedSampleCount();
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 11: { // FlushAudioOutBuffers
					var ret = FlushAudioOutBuffers();
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioOut: {im.CommandId}");
			}
		}
		
		public virtual uint GetAudioOutState() => throw new NotImplementedException();
		public virtual void StartAudioOut() => "Stub hit for Nn.Audio.Detail.IAudioOut.StartAudioOut [1]".Debug(Log);
		public virtual void StopAudioOut() => "Stub hit for Nn.Audio.Detail.IAudioOut.StopAudioOut [2]".Debug(Log);
		public virtual void AppendAudioOutBuffer(ulong tag, Buffer<Nn.Audio.AudioOutBuffer> _1) => "Stub hit for Nn.Audio.Detail.IAudioOut.AppendAudioOutBuffer [3]".Debug(Log);
		public virtual uint RegisterBufferEvent() => throw new NotImplementedException();
		public virtual void GetReleasedAudioOutBuffer(out uint count, Buffer<Nn.Audio.AudioOutBuffer> _1) => throw new NotImplementedException();
		public virtual byte ContainsAudioOutBuffer(ulong tag) => throw new NotImplementedException();
		public virtual void AppendAudioOutBufferAuto(ulong tag, Buffer<Nn.Audio.AudioOutBuffer> _1) => "Stub hit for Nn.Audio.Detail.IAudioOut.AppendAudioOutBufferAuto [7]".Debug(Log);
		public virtual void GetReleasedAudioOutBufferAuto(out uint count, Buffer<Nn.Audio.AudioOutBuffer> _1) => throw new NotImplementedException();
		public virtual uint GetAudioOutBufferCount() => throw new NotImplementedException();
		public virtual ulong GetAudioOutPlayedSampleCount() => throw new NotImplementedException();
		public virtual byte FlushAudioOutBuffers() => throw new NotImplementedException();
	}
	
	public unsafe partial class IAudioRenderer : _Base_IAudioRenderer {}
	public class _Base_IAudioRenderer : IpcInterface {
		static readonly Logger Logger = new("IAudioRenderer");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetSampleRate
					var ret = GetSampleRate();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 1: { // GetSampleCount
					var ret = GetSampleCount();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 2: { // GetMixBufferCount
					var ret = GetMixBufferCount();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 3: { // GetState
					var ret = GetState();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 4: { // RequestUpdateAudioRenderer
					RequestUpdateAudioRenderer(im.GetBuffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader>(0x5, 0), im.GetBuffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader>(0x6, 0), im.GetBuffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader>(0x6, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Start
					Start();
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Stop
					Stop();
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // QuerySystemEvent
					var ret = QuerySystemEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 8: { // SetAudioRendererRenderingTimeLimit
					SetAudioRendererRenderingTimeLimit(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // GetAudioRendererRenderingTimeLimit
					var ret = GetAudioRendererRenderingTimeLimit();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 10: { // RequestUpdateAudioRendererAuto
					RequestUpdateAudioRendererAuto(im.GetBuffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader>(0x21, 0), im.GetBuffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader>(0x22, 0), im.GetBuffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader>(0x22, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // ExecuteAudioRendererRendering
					ExecuteAudioRendererRendering();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAudioRenderer: {im.CommandId}");
			}
		}
		
		public virtual uint GetSampleRate() => throw new NotImplementedException();
		public virtual uint GetSampleCount() => throw new NotImplementedException();
		public virtual uint GetMixBufferCount() => throw new NotImplementedException();
		public virtual uint GetState() => throw new NotImplementedException();
		public virtual void RequestUpdateAudioRenderer(Buffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader> _0, Buffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader> _1, Buffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader> _2) => throw new NotImplementedException();
		public virtual void Start() => "Stub hit for Nn.Audio.Detail.IAudioRenderer.Start [5]".Debug(Log);
		public virtual void Stop() => "Stub hit for Nn.Audio.Detail.IAudioRenderer.Stop [6]".Debug(Log);
		public virtual uint QuerySystemEvent() => throw new NotImplementedException();
		public virtual void SetAudioRendererRenderingTimeLimit(uint limit) => "Stub hit for Nn.Audio.Detail.IAudioRenderer.SetAudioRendererRenderingTimeLimit [8]".Debug(Log);
		public virtual uint GetAudioRendererRenderingTimeLimit() => throw new NotImplementedException();
		public virtual void RequestUpdateAudioRendererAuto(Buffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader> _0, Buffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader> _1, Buffer<Nn.Audio.Detail.AudioRendererUpdateDataHeader> _2) => throw new NotImplementedException();
		public virtual void ExecuteAudioRendererRendering() => "Stub hit for Nn.Audio.Detail.IAudioRenderer.ExecuteAudioRendererRendering [11]".Debug(Log);
	}
	
	public unsafe partial class IFinalOutputRecorder : _Base_IFinalOutputRecorder {}
	public class _Base_IFinalOutputRecorder : IpcInterface {
		static readonly Logger Logger = new("IFinalOutputRecorder");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetFinalOutputRecorderState
					var ret = GetFinalOutputRecorderState();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 1: { // StartFinalOutputRecorder
					StartFinalOutputRecorder();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // StopFinalOutputRecorder
					StopFinalOutputRecorder();
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // AppendFinalOutputRecorderBuffer
					AppendFinalOutputRecorderBuffer(im.GetData<ulong>(8), im.GetBuffer<Nn.Audio.AudioInBuffer>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // RegisterBufferEvent
					var ret = RegisterBufferEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 5: { // GetReleasedFinalOutputRecorderBuffer
					GetReleasedFinalOutputRecorderBuffer(out var _0, out var _1, im.GetBuffer<Nn.Audio.AudioInBuffer>(0x6, 0));
					om.Initialize(0, 0, 16);
					om.SetData(8, _0);
					om.SetData(16, _1);
					break;
				}
				case 6: { // ContainsFinalOutputRecorderBuffer
					var ret = ContainsFinalOutputRecorderBuffer(im.GetData<ulong>(8));
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 7: { // Unknown7
					var ret = Unknown7(im.GetData<ulong>(8));
					om.Initialize(0, 0, 8);
					om.SetData(8, ret);
					break;
				}
				case 8: { // AppendFinalOutputRecorderBufferAuto
					AppendFinalOutputRecorderBufferAuto(im.GetData<ulong>(8), im.GetBuffer<Nn.Audio.AudioInBuffer>(0x21, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // GetReleasedFinalOutputRecorderBufferAuto
					GetReleasedFinalOutputRecorderBufferAuto(out var _0, out var _1, im.GetBuffer<Nn.Audio.AudioInBuffer>(0x22, 0));
					om.Initialize(0, 0, 16);
					om.SetData(8, _0);
					om.SetData(16, _1);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IFinalOutputRecorder: {im.CommandId}");
			}
		}
		
		public virtual uint GetFinalOutputRecorderState() => throw new NotImplementedException();
		public virtual void StartFinalOutputRecorder() => "Stub hit for Nn.Audio.Detail.IFinalOutputRecorder.StartFinalOutputRecorder [1]".Debug(Log);
		public virtual void StopFinalOutputRecorder() => "Stub hit for Nn.Audio.Detail.IFinalOutputRecorder.StopFinalOutputRecorder [2]".Debug(Log);
		public virtual void AppendFinalOutputRecorderBuffer(ulong _0, Buffer<Nn.Audio.AudioInBuffer> _1) => "Stub hit for Nn.Audio.Detail.IFinalOutputRecorder.AppendFinalOutputRecorderBuffer [3]".Debug(Log);
		public virtual uint RegisterBufferEvent() => throw new NotImplementedException();
		public virtual void GetReleasedFinalOutputRecorderBuffer(out uint _0, out ulong _1, Buffer<Nn.Audio.AudioInBuffer> _2) => throw new NotImplementedException();
		public virtual byte ContainsFinalOutputRecorderBuffer(ulong _0) => throw new NotImplementedException();
		public virtual ulong Unknown7(ulong _0) => throw new NotImplementedException();
		public virtual void AppendFinalOutputRecorderBufferAuto(ulong _0, Buffer<Nn.Audio.AudioInBuffer> _1) => "Stub hit for Nn.Audio.Detail.IFinalOutputRecorder.AppendFinalOutputRecorderBufferAuto [8]".Debug(Log);
		public virtual void GetReleasedFinalOutputRecorderBufferAuto(out uint _0, out ulong _1, Buffer<Nn.Audio.AudioInBuffer> _2) => throw new NotImplementedException();
	}
}
