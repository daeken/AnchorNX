using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AnchorNX {
	public unsafe class IncomingMessage {
		public readonly byte* Buffer;
		public readonly bool IsDomainObject, IsTipc;
		public readonly ushort Type;
		public readonly uint CommandId, ACount, BCount, XCount, MoveCount, CopyCount;
		public readonly ulong Pid;
		public readonly bool HasC, HasPid;
		public readonly uint DomainHandle, DomainCommand;
		readonly uint WLen, RawOffset, DataOffset, DescOffset, CopyOffset, MoveOffset;
		public OutgoingMessage Outgoing;
		public IncomingMessage(Memory<byte> ipcBuf, bool isDomainObject = false) {
			IsDomainObject = isDomainObject;
			fixed(byte* buffer = ipcBuf.Span)
				Buffer = buffer;
			var buf = (uint*) Buffer;
			Type = (ushort) (buf[0] & 0xFFFF);
			XCount = (buf[0] >> 16) & 0xF;
			ACount = (buf[0] >> 20) & 0xF;
			BCount = (buf[0] >> 24) & 0xF;
			WLen = buf[1] & 0x3FF;
			HasC = ((buf[1] >> 10) & 0x3) != 0;
			DomainHandle = 0;
			DomainCommand = 0;
			var pos = 2U;
			if(buf[1].HasBit(31)) {
				var hd = buf[pos++];
				HasPid = hd.HasBit(0);
				CopyCount = (hd >> 1) & 0xF;
				MoveCount = hd >> 5;
				if(HasPid) {
					Pid = *(ulong*) &buf[pos];
					pos += 2;
				}
				CopyOffset = pos * 4;
				pos += CopyCount;
				MoveOffset = pos * 4;
				pos += MoveCount;
			}

			DescOffset = pos * 4;

			pos += XCount * 2;
			pos += ACount * 3;
			pos += BCount * 3;
			RawOffset = pos * 4;
			if(Type >= 0xF) {
				IsTipc = true;
				CommandId = Type == 0xF ? 0U : (uint) Type - 0x10;
			} else {
				if((pos & 3) != 0)
					pos += 4 - (pos & 3);
				if(isDomainObject && Type is 4 or 6) {
					DomainHandle = buf[pos + 1];
					DomainCommand = buf[pos] & 0xFF;
					pos += 4;
				}

				Debug.Assert(Type == 2 || isDomainObject && DomainCommand == 2 || buf[pos] == 0x49434653); // SFCI
				DataOffset = pos * 4 + 8;
				
				CommandId = GetData<uint>(0);
			}
		}

		public T GetData<T>(uint offset) => new Span<T>(Buffer + DataOffset + offset, Unsafe.SizeOf<T>())[0];
		public byte[] GetBytes(uint offset, uint size) =>
			new Span<byte>(Buffer + DataOffset + offset, (int) size).ToArray();
		public Span<T> GetDataSpan<T>(uint offset) where T : unmanaged => new(Buffer + DataOffset + offset, 1);

		public static Func<IncomingMessage, object> DataGetter(Type T, uint offset) {
			switch(Activator.CreateInstance(T)) {
				case bool _: return im => im.GetData<byte>(offset) != 0;
				case byte _: return im => im.GetData<byte>(offset);
				case ushort _: return im => im.GetData<ushort>(offset);
				case uint _: return im => im.GetData<uint>(offset);
				case ulong _: return im => im.GetData<ulong>(offset);
				case sbyte _: return im => im.GetData<sbyte>(offset);
				case short _: return im => im.GetData<short>(offset);
				case int _: return im => im.GetData<int>(offset);
				case long _: return im => im.GetData<long>(offset);
				case float _: return im => im.GetData<float>(offset);
				case double _: return im => im.GetData<double>(offset);
				case { } _ when T.IsEnum: return DataGetter(Enum.GetUnderlyingType(T), offset);
				default: throw new NotSupportedException($"Can't create data getter of type {T.Name}");
			}
		}

		public static Func<IncomingMessage, object> BytesGetter(uint offset, uint size) => im =>
			new Span<byte>(im.Buffer + im.DataOffset + offset, (int) size).ToArray();

		public Buffer<T> GetBuffer<T>(uint type, int num) where T : unmanaged {
			if(type == 0x22) {
				var tbuf = GetBuffer<T>(0x6, num);
				return tbuf.Address != 0 ? tbuf : null;
			}
			if((type & 0x20) != 0)
				return GetBuffer<T>((type & ~0x20U) | 4U, num) ?? GetBuffer<T>((type & ~0x20U) | 8U, num);

			var ax = (type & 3) == 1 ? 1 : 0;
			var flags_ = type & 0xC0U;
			var flags = flags_ == 0x80 ? 3 : flags_ == 0x40 ? 1UL : 0UL;
			var cx = (type & 0xC) == 8 ? 1 : 0;
			
			switch((ax << 1) | cx) {
				case 0: { // B
					var t = (uint*) (Buffer + DescOffset + XCount * 8 + ACount * 12 + num * 12);
					ulong a = t[0], b = t[1], c = t[2];
					Debug.Assert((c & 0x3U) == flags);
					var buffer = new Buffer<T>(b | (((((c >> 2) << 4) & 0x70) | ((c >> 28) & 0xFU)) << 32),
						a | (((c >> 24) & 0xFU) << 32));
					if(BCount <= num || buffer.Size == 0)
						goto case 1; //  C buffer
					return buffer;
				}
				case 1: { // C
					var t = (uint*) (Buffer + RawOffset + WLen * 4);
					ulong a = t[0], b = t[1];
					return new Buffer<T>(a | ((b & 0xFFFFU) << 32), b >> 16);
				}
				case 2: { // A
					var t = (uint*) (Buffer + DescOffset + XCount * 8 + num * 12);
					ulong a = t[0], b = t[1], c = t[2];
					Debug.Assert((c & 0x3) == flags);
					var buffer = new Buffer<T>(b | (((((c >> 2) << 4) & 0x70) | ((c >> 28) & 0xFU)) << 32),
						a | (((c >> 24) & 0xFU) << 32));
					if(ACount <= num || buffer.Size == 0)
						goto case 3; // X buffer
					return buffer;
				}
				case 3: { // X
					var t = (uint*) (Buffer + DescOffset + num * 8);
					ulong a = t[0], b = t[1];
					return new Buffer<T>(b | ((((a >> 12) & 0xFU) | ((a >> 2) & 0x70U)) << 32), a >> 16);
				}
			}
			return null;
		}
		
		public uint GetMove(uint offset) {
			var buf = (uint*) Buffer;
			return IsDomainObject ? buf[(DataOffset >> 2) + 2 + offset] : buf[(MoveOffset >> 2) + offset];
		}

		public T GetMove<T>(uint offset) where T : IpcInterface {
			throw new NotImplementedException();
		}
		public uint GetCopy(uint offset) => ((uint*) Buffer)[(CopyOffset >> 2) + offset];
	}

	public unsafe class OutgoingMessage {
		readonly Memory<byte> IpcBuf;
		byte* Buffer;
		public bool IsDomainObject, IsTipc;
		public uint ErrCode;
		uint SfcoOffset, DataOffset, RealDataOffset, CopyCount;
		public readonly IncomingMessage Incoming;
		readonly ulong[] XBaseAddrs;
		int[] XBufSizes;

		public OutgoingMessage(Memory<byte> ipcBuf, ulong[] xBaseAddrs, bool isDomainObject, IncomingMessage im) {
			IpcBuf = ipcBuf;
			fixed(byte* buffer = ipcBuf.Span)
				Buffer = buffer;
			XBaseAddrs = xBaseAddrs;
			IsDomainObject = isDomainObject;
			IsTipc = im.IsTipc;
			Incoming = im;
		}

		public void Initialize(uint moveCount, uint copyCount, uint dataBytes, params int[] xSizes) {
			fixed(byte* buffer = IpcBuf.Span)
				Buffer = buffer;
			Debug.Assert(!IsTipc || xSizes.Length == 0);
			CopyCount = copyCount;
			var buf = (uint *) Buffer;
			for(var i = 0; i < 0x40; ++i)
				buf[i] = 0;
			buf[0] = ((uint) xSizes.Length & 0xF) << 16;
			buf[1] = 0;
			if(moveCount != 0 || copyCount != 0) {
				buf[1] = moveCount != 0 && !IsDomainObject || copyCount != 0 ? 1U << 31 : 0;
				buf[2] = (copyCount << 1) | ((IsDomainObject ? 0 : moveCount) << 5);
			}

			var pos = 2 + (moveCount != 0 && !IsDomainObject || copyCount != 0 ? 1 + moveCount + copyCount : 0);
			
			Debug.Assert(xSizes.Length <= XBaseAddrs.Length);
			XBufSizes = xSizes;
			for(var i = 0; i < xSizes.Length; ++i) {
				var addr = XBaseAddrs[i];
				buf[pos] = (uint) i | 
				           (uint) ((addr >> 36) << 6) | 
				           (uint) (((addr >> 32) & 0b1111) << 12) | 
				           ((uint) xSizes[i] << 16);
				buf[pos + 1] = (uint) addr;
				pos += 2;
			}

			if(IsTipc) {
				RealDataOffset = 0;
				var dataWords = (dataBytes & 3) != 0 ? (dataBytes >> 2) + 1 : dataBytes >> 2;
				if(moveCount != 0 || copyCount != 0)
					DataOffset = (3U + moveCount + copyCount) * 4;
				else
					DataOffset = 2U * 4;
				buf[1] |= DataOffset / 4 + 2 + dataWords;
			} else {
				if((pos & 3) != 0)
					pos += 4 - (pos & 3);
				if(IsDomainObject) {
					buf[pos] = moveCount;
					pos += 4;
				}
				RealDataOffset = IsDomainObject ? moveCount << 2 : 0;
				var dataWords = (RealDataOffset >> 2) + (dataBytes & 3) != 0 ? (dataBytes >> 2) + 1 : dataBytes >> 2;

				buf[1] |= 4U + (IsDomainObject ? 4U : 0) + 4 + dataWords;
	 
				SfcoOffset = pos * 4;
				DataOffset = SfcoOffset + 8;
				buf[pos] = 0x4f434653; // SFCO
				buf[pos + 1] = 1; // New version?
			}
		}

		public void Move(uint offset, uint handle) {
			var buf = (uint*) Buffer;
			if(IsDomainObject) {
				IpcInterface.Log($"Sending back domain object 0x{handle:X}");
				buf[(SfcoOffset >> 2) + 4 + offset] = handle;
			} else
				buf[3 + CopyCount + offset] = handle;
		}

		public void Copy(uint offset, uint handle) =>
			((uint*) Buffer)[3 + offset] = handle;

		public void Bake() {
			var buf = (uint*) Buffer;
			buf[DataOffset >> 2] = ErrCode;
		}
		
		public void SetData<T>(uint offset, T value) => 
			new Span<T>(Buffer + SfcoOffset + 8 + offset + (offset < 8 ? 0 : RealDataOffset), Unsafe.SizeOf<T>())[0] = value;
		public Span<T> GetDataSpan<T>(uint offset) where T : unmanaged => new(Buffer + DataOffset + offset + (offset < 8 ? 0 : RealDataOffset), 1);
		public void SetBytes(uint offset, byte[] data) =>
			data.CopyTo(new Span<byte>(Buffer + DataOffset + offset + (offset < 8 ? 0 : RealDataOffset), data.Length));

		public Buffer<T> GetXBuffer<T>(int index) where T : unmanaged =>
			new(XBaseAddrs[index], (ulong) XBufSizes[index]);
		
		public static Action<OutgoingMessage, object> DataSetter(Type T, uint offset) {
			switch(Activator.CreateInstance(T)) {
				case bool _: return (om, v) => om.SetData(offset, (byte) ((bool) v ? 1 : 0));
				case byte _: return (om, v) => om.SetData(offset, (byte) v);
				case ushort _: return (om, v) => om.SetData(offset, (ushort) v);
				case uint _: return (om, v) => om.SetData(offset, (uint) v);
				case ulong _: return (om, v) => om.SetData(offset, (ulong) v);
				case sbyte _: return (om, v) => om.SetData(offset, (sbyte) v);
				case short _: return (om, v) => om.SetData(offset, (short) v);
				case int _: return (om, v) => om.SetData(offset, (int) v);
				case long _: return (om, v) => om.SetData(offset, (long) v);
				case float _: return (om, v) => om.SetData(offset, (float) v);
				case double _: return (om, v) => om.SetData(offset, (double) v);
				default: throw new NotSupportedException($"Can't create data setter of type {T.Name}");
			}
		}

		public static Action<OutgoingMessage, object> BytesSetter(uint offset, uint size) => (om, v) =>
			((byte[]) v).CopyTo(new Span<byte>(
				om.Buffer + om.DataOffset + offset + (offset < 8 ? 0 : om.RealDataOffset), (int) size));
	}

	public class IpcException : Exception {
		public uint Code;
		public IpcException(uint code) => Code = code;
	}
	
	public class IpcIgnoreException : Exception {
	}

	public class IFake : IpcInterface {
		public readonly string Name;
		
		public IFake(string name) =>
			Name = name;

		public override async Task _Dispatch(IncomingMessage incoming, OutgoingMessage outgoing) {
			Log($"Attempted dispatch for command {incoming.CommandId} on fake '{Name}' (ignored) from pid {Box.PidName(OwningPid)}");
			throw new IpcIgnoreException();
		}
	}
	
	public abstract class IpcInterface {
		static readonly Logger Logger = new("IpcInterface");
		public static Action<string> Log = Logger.Log;
		
		public uint Handle;
		public bool IsDomainObject;
		public ulong OwningPid;
		IpcInterface DomainOwner;
		uint DomainHandleIter = 0xf001;
		const uint ThisHandle = 0xf000;
		readonly Dictionary<uint, object> DomainHandles = new();
		readonly Dictionary<uint, uint> DomainHandleMap = new();
		
		public abstract Task _Dispatch(IncomingMessage incoming, OutgoingMessage outgoing);

		protected virtual async Task Dispatch(Memory<byte> ipcBuf, IncomingMessage incoming, OutgoingMessage outgoing, Func<bool, Task> reply) {
			try {
				if(incoming.IsTipc)
					throw new NotImplementedException();
				await _Dispatch(incoming, outgoing);
			} catch(IpcException ie) {
				Log($"IPC Exception: 0x{ie.Code:X}");
				outgoing.Initialize(0, 0, 0);
				outgoing.ErrCode = ie.Code;
			}
			outgoing.Bake();
			ipcBuf.Span.Hexdump(Logger);
			await reply(false);
		}

		public async Task<uint> CreateHandle(object obj, bool copy = false) {
			if(DomainOwner != null) return await DomainOwner.CreateHandle(obj);
			if(obj is uint handle) return handle;
			Debug.Assert(!copy);
			if(IsDomainObject) {
				Debug.Assert(IsDomainObject);
				DomainHandles[DomainHandleIter] = obj;
				if(obj is IpcInterface ii) {
					ii.DomainOwner = this;
					ii.OwningPid = OwningPid;
				}

				return DomainHandleIter++;
			} else {
				var (rc, server, client) = await Box.HvcProxy.CreateSession();
				Debug.Assert(rc == 0);
				await Box.HvcProxy.AddSession(server, (IpcInterface) obj);
				return client;
			}
		}

		public async Task SyncMessage(Memory<byte> ipcBuf, ulong xBaseAddr, Func<bool, Task> reply) {
			ipcBuf.Span.Hexdump(Logger);
			var incoming = new IncomingMessage(ipcBuf, IsDomainObject);
			var outgoing = new OutgoingMessage(ipcBuf, new[] { xBaseAddr, xBaseAddr + 0x1000 }, IsDomainObject, incoming);
			incoming.Outgoing = outgoing;
			var ret = 0xF601U;
			var closeHandle = false;
			var target = this;
			if(IsDomainObject && incoming.DomainHandle != ThisHandle && incoming.Type is 4 or 6)
				target = (IpcInterface) DomainHandles[incoming.DomainHandle];
			Log($"Got message for pid {OwningPid}: domain command {incoming.DomainCommand} domain handle {incoming.DomainHandle:X} command id {incoming.CommandId} type {incoming.Type}");
			if(incoming.IsTipc) {
				if(incoming.Type == 0xF) {
					closeHandle = true;
					outgoing.Initialize(0, 0, 0);
					ret = 0x25a0b;
				} else {
					Log($"TIPC command {incoming.CommandId} for {target}");
					await target.Dispatch(ipcBuf, incoming, outgoing, reply);
					return;
				}
			} else {
				if(!IsDomainObject || incoming.DomainCommand == 1 || incoming.Type is 2 or 5 or 7)
					switch(incoming.Type) {
						case 2:
							closeHandle = true;
							outgoing.Initialize(0, 0, 0);
							ret = 0x25a0b;
							break;
						case 4:
						case 6:
							Log($"IPC command {incoming.CommandId} for {target}");
							await target.Dispatch(ipcBuf, incoming, outgoing, reply);
							return;
						case 5:
						case 7:
							switch(incoming.CommandId) {
								case 0: // ConvertSessionToDomain
									Log("Converting session to domain...");
									outgoing.Initialize(0, 0, 4);
									IsDomainObject = true;
									outgoing.SetData(8, ThisHandle);
									break;
								case 2: // CloneCurrentObject
								case 4: // DuplicateSessionEx
									Log("Duplicating session");
									var (rc, server, client) = await Box.HvcProxy.CreateSession();
									Log($"Create Session? {rc:X} -- {server:X} {client:X}");
									Debug.Assert(rc == 0);
									outgoing.IsDomainObject = false;
									outgoing.Initialize(1, 0, 0);
									outgoing.Move(0, client);
									await Box.HvcProxy.AddSession(server, this);
									break;
								case 3: // QueryPointerBufferSize
									outgoing.Initialize(0, 0, 4);
									outgoing.SetData(8, 0x4000U);
									break;
								default:
									throw new NotImplementedException(
										$"Unknown domain command ID: {incoming.CommandId}");
							}

							ret = 0;
							break;
						default:
							throw new NotImplementedException($"Unknown message type: {incoming.Type}");
					}
				else
					switch(incoming.DomainCommand) {
						case 2:
							Log($"Closing domain handle 0x{incoming.DomainHandle:X}");
							if(DomainHandles.TryGetValue(incoming.DomainHandle, out var obj) && obj is IpcInterface io)
								io.Close();
							DomainHandles.Remove(incoming.DomainHandle);
							outgoing.Initialize(0, 0, 0);
							outgoing.ErrCode = 0;
							ret = 0;
							break;
						default:
							throw new NotImplementedException($"Unknown domain command ID: {incoming.DomainCommand}");
					}
			}

			if(ret == 0)
				outgoing.Bake();
			ipcBuf.Span.Hexdump(Logger);
			await reply(closeHandle);
		}
		
		public virtual void Close() { }
	}
}