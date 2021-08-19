#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Irsensor {
	public unsafe partial class IIrSensorServer : _Base_IIrSensorServer {}
	public class _Base_IIrSensorServer : IpcInterface {
		static readonly Logger Logger = new("IIrSensorServer");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 302: { // ActivateIrsensor
					ActivateIrsensor(im.GetData<ulong>(8), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 303: { // DeactivateIrsensor
					DeactivateIrsensor(im.GetData<ulong>(8), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 304: { // GetIrsensorSharedMemoryHandle
					var ret = GetIrsensorSharedMemoryHandle(im.GetData<ulong>(8), im.Pid);
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 305: { // StopImageProcessor
					StopImageProcessor(im.GetData<uint>(8), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 306: { // RunMomentProcessor
					RunMomentProcessor(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetBytes(24, 0x20), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 307: { // RunClusteringProcessor
					RunClusteringProcessor(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetBytes(24, 0x28), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 308: { // RunImageTransferProcessor
					RunImageTransferProcessor(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetBytes(24, 0x18), im.GetData<ulong>(48), im.Pid, im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 309: { // GetImageTransferProcessorState
					GetImageTransferProcessorState(im.GetData<uint>(8), im.GetData<ulong>(16), im.Pid, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 16);
					om.SetBytes(8, _0);
					break;
				}
				case 310: { // RunTeraPluginProcessor
					RunTeraPluginProcessor(im.GetData<uint>(8), im.GetBytes(12, 0x8), im.GetData<ulong>(24), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 311: { // GetNpadIrCameraHandle
					var ret = GetNpadIrCameraHandle(im.GetData<uint>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 312: { // RunPointingProcessor
					RunPointingProcessor(im.GetData<uint>(8), im.GetBytes(12, 0xC), im.GetData<ulong>(24), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 313: { // SuspendImageProcessor
					SuspendImageProcessor(im.GetData<uint>(8), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 314: { // CheckFirmwareVersion
					CheckFirmwareVersion(im.GetData<uint>(8), im.GetBytes(12, 0x4), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 315: { // SetFunctionLevel
					SetFunctionLevel(im.GetData<uint>(8), im.GetBytes(12, 0x4), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 316: { // RunImageTransferExProcessor
					RunImageTransferExProcessor(im.GetData<uint>(8), im.GetData<ulong>(16), im.GetBytes(24, 0x20), im.GetData<ulong>(56), im.Pid, im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 317: { // RunIrLedProcessor
					RunIrLedProcessor(im.GetData<uint>(8), im.GetBytes(12, 0x8), im.GetData<ulong>(24), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 318: { // StopImageProcessorAsync
					StopImageProcessorAsync(im.GetData<uint>(8), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 319: { // ActivateIrsensorWithFunctionLevel
					ActivateIrsensorWithFunctionLevel(im.GetBytes(8, 0x4), im.GetData<ulong>(16), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IIrSensorServer: {im.CommandId}");
			}
		}
		
		public virtual void ActivateIrsensor(ulong _0, ulong _1) => "Stub hit for Nn.Irsensor.IIrSensorServer.ActivateIrsensor [302]".Debug(Log);
		public virtual void DeactivateIrsensor(ulong _0, ulong _1) => "Stub hit for Nn.Irsensor.IIrSensorServer.DeactivateIrsensor [303]".Debug(Log);
		public virtual uint GetIrsensorSharedMemoryHandle(ulong _0, ulong _1) => throw new NotImplementedException();
		public virtual void StopImageProcessor(uint _0, ulong _1, ulong _2) => "Stub hit for Nn.Irsensor.IIrSensorServer.StopImageProcessor [305]".Debug(Log);
		public virtual void RunMomentProcessor(uint _0, ulong _1, byte[] _2, ulong _3) => "Stub hit for Nn.Irsensor.IIrSensorServer.RunMomentProcessor [306]".Debug(Log);
		public virtual void RunClusteringProcessor(uint _0, ulong _1, byte[] _2, ulong _3) => "Stub hit for Nn.Irsensor.IIrSensorServer.RunClusteringProcessor [307]".Debug(Log);
		public virtual void RunImageTransferProcessor(uint _0, ulong _1, byte[] _2, ulong _3, ulong _4, uint _5) => "Stub hit for Nn.Irsensor.IIrSensorServer.RunImageTransferProcessor [308]".Debug(Log);
		public virtual void GetImageTransferProcessorState(uint _0, ulong _1, ulong _2, out byte[] _3, Buffer<byte> _4) => throw new NotImplementedException();
		public virtual void RunTeraPluginProcessor(uint _0, byte[] _1, ulong _2, ulong _3) => "Stub hit for Nn.Irsensor.IIrSensorServer.RunTeraPluginProcessor [310]".Debug(Log);
		public virtual uint GetNpadIrCameraHandle(uint _0) => throw new NotImplementedException();
		public virtual void RunPointingProcessor(uint _0, byte[] _1, ulong _2, ulong _3) => "Stub hit for Nn.Irsensor.IIrSensorServer.RunPointingProcessor [312]".Debug(Log);
		public virtual void SuspendImageProcessor(uint _0, ulong _1, ulong _2) => "Stub hit for Nn.Irsensor.IIrSensorServer.SuspendImageProcessor [313]".Debug(Log);
		public virtual void CheckFirmwareVersion(uint _0, byte[] _1, ulong _2, ulong _3) => "Stub hit for Nn.Irsensor.IIrSensorServer.CheckFirmwareVersion [314]".Debug(Log);
		public virtual void SetFunctionLevel(uint _0, byte[] _1, ulong _2, ulong _3) => "Stub hit for Nn.Irsensor.IIrSensorServer.SetFunctionLevel [315]".Debug(Log);
		public virtual void RunImageTransferExProcessor(uint _0, ulong _1, byte[] _2, ulong _3, ulong _4, uint _5) => "Stub hit for Nn.Irsensor.IIrSensorServer.RunImageTransferExProcessor [316]".Debug(Log);
		public virtual void RunIrLedProcessor(uint _0, byte[] _1, ulong _2, ulong _3) => "Stub hit for Nn.Irsensor.IIrSensorServer.RunIrLedProcessor [317]".Debug(Log);
		public virtual void StopImageProcessorAsync(uint _0, ulong _1, ulong _2) => "Stub hit for Nn.Irsensor.IIrSensorServer.StopImageProcessorAsync [318]".Debug(Log);
		public virtual void ActivateIrsensorWithFunctionLevel(byte[] _0, ulong _1, ulong _2) => "Stub hit for Nn.Irsensor.IIrSensorServer.ActivateIrsensorWithFunctionLevel [319]".Debug(Log);
	}
	
	public unsafe partial class IIrSensorSystemServer : _Base_IIrSensorSystemServer {}
	public class _Base_IIrSensorSystemServer : IpcInterface {
		static readonly Logger Logger = new("IIrSensorSystemServer");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 500: { // SetAppletResourceUserId
					SetAppletResourceUserId(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 501: { // RegisterAppletResourceUserId
					RegisterAppletResourceUserId(im.GetData<byte>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 502: { // UnregisterAppletResourceUserId
					UnregisterAppletResourceUserId(im.GetData<ulong>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 503: { // EnableAppletToGetInput
					EnableAppletToGetInput(im.GetData<byte>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IIrSensorSystemServer: {im.CommandId}");
			}
		}
		
		public virtual void SetAppletResourceUserId(ulong _0) => "Stub hit for Nn.Irsensor.IIrSensorSystemServer.SetAppletResourceUserId [500]".Debug(Log);
		public virtual void RegisterAppletResourceUserId(byte _0, ulong _1) => "Stub hit for Nn.Irsensor.IIrSensorSystemServer.RegisterAppletResourceUserId [501]".Debug(Log);
		public virtual void UnregisterAppletResourceUserId(ulong _0) => "Stub hit for Nn.Irsensor.IIrSensorSystemServer.UnregisterAppletResourceUserId [502]".Debug(Log);
		public virtual void EnableAppletToGetInput(byte _0, ulong _1) => "Stub hit for Nn.Irsensor.IIrSensorSystemServer.EnableAppletToGetInput [503]".Debug(Log);
	}
}
