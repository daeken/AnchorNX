#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nns.Nvdrv {
	public unsafe partial class INvDrvDebugFSServices : _Base_INvDrvDebugFSServices {}
	public class _Base_INvDrvDebugFSServices : IpcInterface {
		static readonly Logger Logger = new("INvDrvDebugFSServices");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // OpenLog
					var ret = OpenLog(im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // CloseLog
					CloseLog(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // ReadLog
					ReadLog(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(null, im.GetBuffer<byte>(0x5, 0), out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					var ret = Unknown4(null, im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x5, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to INvDrvDebugFSServices: {im.CommandId}");
			}
		}
		
		public virtual object OpenLog(uint _0) => throw new NotImplementedException();
		public virtual void CloseLog(object _0) => "Stub hit for Nns.Nvdrv.INvDrvDebugFSServices.CloseLog [1]".Debug(Log);
		public virtual void ReadLog(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown3(object _0, Buffer<byte> _1, out object _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual object Unknown4(object _0, Buffer<byte> _1, Buffer<byte> _2) => throw new NotImplementedException();
	}
	
	public unsafe partial class INvDrvServices : _Base_INvDrvServices {}
	public class _Base_INvDrvServices : IpcInterface {
		static readonly Logger Logger = new("INvDrvServices");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Open
					Open(im.GetBuffer<byte>(0x5, 0), out var _0, out var _1);
					om.Initialize(0, 0, 8);
					om.SetData(8, _0);
					om.SetData(12, _1);
					break;
				}
				case 1: { // Ioctl
					Ioctl(im.GetData<uint>(8), im.GetData<uint>(12), im.GetBuffer<byte>(0x21, 0), out var _0, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 2: { // _Close
					var ret = _Close(im.GetData<uint>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 3: { // Initialize
					var ret = Initialize(im.GetData<uint>(8), im.GetCopy(0), im.GetCopy(1));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 4: { // QueryEvent
					QueryEvent(im.GetData<uint>(8), im.GetData<uint>(12), out var _0, out var _1);
					om.Initialize(0, 1, 4);
					om.SetData(8, _0);
					om.Copy(0, await CreateHandle(_1, copy: true));
					break;
				}
				case 5: { // MapSharedMem
					var ret = MapSharedMem(im.GetData<uint>(8), im.GetData<uint>(12), im.GetCopy(0));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 6: { // GetStatus
					var ret = GetStatus();
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // ForceSetClientPID
					var ret = ForceSetClientPID(im.GetData<ulong>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 8: { // SetClientPID
					var ret = SetClientPID(im.GetData<ulong>(8), im.Pid);
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 9: { // DumpGraphicsMemoryInfo
					DumpGraphicsMemoryInfo();
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // Unknown10
					var ret = Unknown10(im.GetData<uint>(8), im.GetCopy(0));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 11: { // Ioctl2
					Ioctl2(im.GetData<uint>(8), im.GetData<uint>(12), im.GetBuffer<byte>(0x21, 0), im.GetBuffer<byte>(0x21, 1), out var _0, im.GetBuffer<byte>(0x22, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 12: { // Ioctl3
					Ioctl3(im.GetData<uint>(8), im.GetData<uint>(12), im.GetBuffer<byte>(0x21, 0), out var _0, im.GetBuffer<byte>(0x22, 0), im.GetBuffer<byte>(0x22, 1));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 13: { // Unknown13
					Unknown13(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to INvDrvServices: {im.CommandId}");
			}
		}
		
		public virtual void Open(Buffer<byte> path, out uint fd, out uint error_code) => throw new NotImplementedException();
		public virtual void Ioctl(uint fd, uint rq_id, Buffer<byte> _2, out uint error_code, Buffer<byte> _4) => throw new NotImplementedException();
		public virtual uint _Close(uint fd) => throw new NotImplementedException();
		public virtual uint Initialize(uint transfer_memory_size, uint current_process, uint transfer_memory) => throw new NotImplementedException();
		public virtual void QueryEvent(uint fd, uint event_id, out uint _2, out uint _3) => throw new NotImplementedException();
		public virtual uint MapSharedMem(uint fd, uint nvmap_handle, uint _2) => throw new NotImplementedException();
		public virtual object GetStatus() => throw new NotImplementedException();
		public virtual uint ForceSetClientPID(ulong pid) => throw new NotImplementedException();
		public virtual uint SetClientPID(ulong _0, ulong _1) => throw new NotImplementedException();
		public virtual void DumpGraphicsMemoryInfo() => "Stub hit for Nns.Nvdrv.INvDrvServices.DumpGraphicsMemoryInfo [9]".Debug(Log);
		public virtual uint Unknown10(uint _0, uint _1) => throw new NotImplementedException();
		public virtual void Ioctl2(uint _0, uint _1, Buffer<byte> _2, Buffer<byte> _3, out uint _4, Buffer<byte> _5) => throw new NotImplementedException();
		public virtual void Ioctl3(uint _0, uint _1, Buffer<byte> _2, out uint _3, Buffer<byte> _4, Buffer<byte> _5) => throw new NotImplementedException();
		public virtual void Unknown13(object _0) => "Stub hit for Nns.Nvdrv.INvDrvServices.Unknown13 [13]".Debug(Log);
	}
}
