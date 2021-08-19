#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Tma {
	public unsafe partial class IHtcManager : _Base_IHtcManager {}
	public class _Base_IHtcManager : IpcInterface {
		static readonly Logger Logger = new("IHtcManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // GetEnvironmentVariable
					GetEnvironmentVariable(im.GetBuffer<byte>(0x5, 0), out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 1: { // GetEnvironmentVariableLength
					var ret = GetEnvironmentVariableLength(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 2: { // BindHostConnectionEvent
					var ret = BindHostConnectionEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 3: { // BindHostDisconnectionEvent
					var ret = BindHostDisconnectionEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 4: { // BindHostConnectionEventForSystem
					var ret = BindHostConnectionEventForSystem();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 5: { // BindHostDisconnectionEventForSystem
					var ret = BindHostDisconnectionEventForSystem();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 6: { // GetBridgeIpAddress
					GetBridgeIpAddress(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // GetBridgePort
					GetBridgePort(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // SetUsbDetachedForDebug
					SetUsbDetachedForDebug(im.GetData<byte>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // GetBridgeSubnetMask
					GetBridgeSubnetMask(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // GetBridgeMacAddress
					GetBridgeMacAddress(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IHtcManager: {im.CommandId}");
			}
		}
		
		public virtual void GetEnvironmentVariable(Buffer<byte> _0, out uint _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual uint GetEnvironmentVariableLength(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual uint BindHostConnectionEvent() => throw new NotImplementedException();
		public virtual uint BindHostDisconnectionEvent() => throw new NotImplementedException();
		public virtual uint BindHostConnectionEventForSystem() => throw new NotImplementedException();
		public virtual uint BindHostDisconnectionEventForSystem() => throw new NotImplementedException();
		public virtual void GetBridgeIpAddress(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void GetBridgePort(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void SetUsbDetachedForDebug(byte _0) => "Stub hit for Nn.Tma.IHtcManager.SetUsbDetachedForDebug [8]".Debug(Log);
		public virtual void GetBridgeSubnetMask(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void GetBridgeMacAddress(Buffer<byte> _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IHtcsManager : _Base_IHtcsManager {}
	public class _Base_IHtcsManager : IpcInterface {
		static readonly Logger Logger = new("IHtcsManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					Unknown0(out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 1: { // Unknown1
					Unknown1(im.GetData<uint>(8), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 2: { // Unknown2
					Unknown2(im.GetBytes(8, 0x42), im.GetData<uint>(76), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 3: { // Unknown3
					Unknown3(im.GetBytes(8, 0x42), im.GetData<uint>(76), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 4: { // Unknown4
					Unknown4(im.GetData<uint>(8), im.GetData<uint>(12), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 5: { // Unknown5
					Unknown5(im.GetData<uint>(8), out var _0, out var _1, out var _2);
					om.Initialize(0, 0, 76);
					om.SetBytes(8, _0);
					om.SetData(76, _1);
					om.SetData(80, _2);
					break;
				}
				case 6: { // Unknown6
					Unknown6(im.GetData<uint>(8), im.GetData<uint>(12), out var _0, out var _1, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 16);
					om.SetData(8, _0);
					om.SetData(16, _1);
					break;
				}
				case 7: { // Unknown7
					Unknown7(im.GetData<uint>(8), im.GetData<uint>(12), im.GetBuffer<byte>(0x5, 0), out var _0, out var _1);
					om.Initialize(0, 0, 16);
					om.SetData(8, _0);
					om.SetData(16, _1);
					break;
				}
				case 8: { // Unknown8
					Unknown8(im.GetData<uint>(8), im.GetData<uint>(12), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 9: { // Unknown9
					Unknown9(im.GetData<uint>(8), im.GetData<uint>(12), im.GetData<uint>(16), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 10: { // GetPeerNameAny
					GetPeerNameAny(out var _0);
					om.Initialize(0, 0, 32);
					om.SetBytes(8, _0);
					break;
				}
				case 11: { // GetDefaultHostName
					GetDefaultHostName(out var _0);
					om.Initialize(0, 0, 32);
					om.SetBytes(8, _0);
					break;
				}
				case 12: { // CreateSocketOld
					CreateSocketOld(out var _0, out var _1);
					om.Initialize(1, 0, 4);
					om.SetData(8, _0);
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 13: { // CreateSocket
					CreateSocket(im.GetData<byte>(8), out var _0, out var _1);
					om.Initialize(1, 0, 4);
					om.SetData(8, _0);
					om.Move(0, await CreateHandle(_1));
					break;
				}
				case 100: { // RegisterProcessId
					RegisterProcessId(im.GetData<ulong>(8), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 101: { // MonitorManager
					MonitorManager(im.GetData<ulong>(8), im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IHtcsManager: {im.CommandId}");
			}
		}
		
		public virtual void Unknown0(out uint _0, out uint _1) => throw new NotImplementedException();
		public virtual void Unknown1(uint _0, out uint _1, out uint _2) => throw new NotImplementedException();
		public virtual void Unknown2(byte[] _0, uint _1, out uint _2, out uint _3) => throw new NotImplementedException();
		public virtual void Unknown3(byte[] _0, uint _1, out uint _2, out uint _3) => throw new NotImplementedException();
		public virtual void Unknown4(uint _0, uint _1, out uint _2, out uint _3) => throw new NotImplementedException();
		public virtual void Unknown5(uint _0, out byte[] _1, out uint _2, out uint _3) => throw new NotImplementedException();
		public virtual void Unknown6(uint _0, uint _1, out uint _2, out ulong _3, Buffer<byte> _4) => throw new NotImplementedException();
		public virtual void Unknown7(uint _0, uint _1, Buffer<byte> _2, out uint _3, out ulong _4) => throw new NotImplementedException();
		public virtual void Unknown8(uint _0, uint _1, out uint _2, out uint _3) => throw new NotImplementedException();
		public virtual void Unknown9(uint _0, uint _1, uint _2, out uint _3, out uint _4) => throw new NotImplementedException();
		public virtual void GetPeerNameAny(out byte[] _0) => throw new NotImplementedException();
		public virtual void GetDefaultHostName(out byte[] _0) => throw new NotImplementedException();
		public virtual void CreateSocketOld(out uint _0, out IpcInterface _1) => throw new NotImplementedException();
		public virtual void CreateSocket(byte _0, out uint _1, out IpcInterface _2) => throw new NotImplementedException();
		public virtual void RegisterProcessId(ulong _0, ulong _1) => "Stub hit for Nn.Tma.IHtcsManager.RegisterProcessId [100]".Debug(Log);
		public virtual void MonitorManager(ulong _0, ulong _1) => "Stub hit for Nn.Tma.IHtcsManager.MonitorManager [101]".Debug(Log);
	}
	
	public unsafe partial class ISocket : _Base_ISocket {}
	public class _Base_ISocket : IpcInterface {
		static readonly Logger Logger = new("ISocket");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // _Close
					_Close(out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 1: { // Connect
					Connect(im.GetBytes(8, 0x42), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 2: { // Bind
					Bind(im.GetBytes(8, 0x42), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 3: { // Listen
					Listen(im.GetData<uint>(8), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 4: { // Accept
					Accept(out var _0, out var _1, out var _2);
					om.Initialize(1, 0, 72);
					om.SetBytes(8, _0);
					om.SetData(76, _1);
					om.Move(0, await CreateHandle(_2));
					break;
				}
				case 5: { // Recv
					Recv(im.GetData<uint>(8), out var _0, out var _1, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 16);
					om.SetData(8, _0);
					om.SetData(16, _1);
					break;
				}
				case 6: { // Send
					Send(im.GetData<uint>(8), im.GetBuffer<byte>(0x21, 0), out var _0, out var _1);
					om.Initialize(0, 0, 16);
					om.SetData(8, _0);
					om.SetData(16, _1);
					break;
				}
				case 7: { // Shutdown
					Shutdown(im.GetData<uint>(8), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 8: { // Fcntl
					Fcntl(im.GetData<uint>(8), im.GetData<uint>(12), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ISocket: {im.CommandId}");
			}
		}
		
		public virtual void _Close(out uint _0, out uint _1) => throw new NotImplementedException();
		public virtual void Connect(byte[] _0, out uint _1, out uint _2) => throw new NotImplementedException();
		public virtual void Bind(byte[] _0, out uint _1, out uint _2) => throw new NotImplementedException();
		public virtual void Listen(uint _0, out uint _1, out uint _2) => throw new NotImplementedException();
		public virtual void Accept(out byte[] _0, out uint _1, out IpcInterface _2) => throw new NotImplementedException();
		public virtual void Recv(uint _0, out uint _1, out ulong _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void Send(uint _0, Buffer<byte> _1, out uint _2, out ulong _3) => throw new NotImplementedException();
		public virtual void Shutdown(uint _0, out uint _1, out uint _2) => throw new NotImplementedException();
		public virtual void Fcntl(uint _0, uint _1, out uint _2, out uint _3) => throw new NotImplementedException();
	}
}
