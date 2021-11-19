using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using AnchorNX.IpcServices.Nns.Hosbinder.Types;
using Ryujinx.Common;
using Ryujinx.Common.Utilities;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	public class Parcel {
		readonly byte[] _rawData;
		int _objectPosition;

		int _payloadPosition;

		public Parcel(byte[] rawData) {
			_rawData = rawData;

			_payloadPosition = 0;
			_objectPosition = 0;
		}

		public Parcel(uint payloadSize, uint objectsSize) {
			var headerSize = (uint) Unsafe.SizeOf<ParcelHeader>();

			_rawData = new byte[BitUtils.AlignUp(headerSize + payloadSize + objectsSize, 4)];

			Header.PayloadSize = payloadSize;
			Header.ObjectsSize = objectsSize;
			Header.PayloadOffset = headerSize;
			Header.ObjectOffset = Header.PayloadOffset + Header.ObjectsSize;
		}

		Span<byte> Raw => new(_rawData);

		ref ParcelHeader Header => ref MemoryMarshal.Cast<byte, ParcelHeader>(_rawData)[0];

		Span<byte> Payload => Raw.Slice((int) Header.PayloadOffset, (int) Header.PayloadSize);

		Span<byte> Objects => Raw.Slice((int) Header.ObjectOffset, (int) Header.ObjectsSize);

		public string ReadInterfaceToken() {
			// Ignore the policy flags
			var strictPolicy = ReadInt32();

			return ReadString16();
		}

		public string ReadString16() {
			var size = ReadInt32();

			if(size < 0) return "";

			var data = ReadInPlace((size + 1) * 2);

			// Return the unicode string without the last character (null terminator)
			return Encoding.Unicode.GetString(data.Slice(0, size * 2));
		}

		public int ReadInt32() {
			return ReadUnmanagedType<int>();
		}

		public uint ReadUInt32() {
			return ReadUnmanagedType<uint>();
		}

		public bool ReadBoolean() {
			return ReadUnmanagedType<uint>() != 0;
		}

		public long ReadInt64() {
			return ReadUnmanagedType<long>();
		}

		public ulong ReadUInt64() {
			return ReadUnmanagedType<ulong>();
		}

		public T ReadFlattenable<T>() where T : unmanaged, IFlattenable {
			var flattenableSize = ReadInt64();

			var result = new T();

			Debug.Assert(flattenableSize == result.GetFlattenedSize());

			result.Unflatten(this);

			return result;
		}

		public T ReadUnmanagedType<T>() where T : unmanaged {
			var data = ReadInPlace(Unsafe.SizeOf<T>());

			return MemoryMarshal.Cast<byte, T>(data)[0];
		}

		public ReadOnlySpan<byte> ReadInPlace(int size) {
			ReadOnlySpan<byte> result = Payload.Slice(_payloadPosition, size);

			_payloadPosition += BitUtils.AlignUp(size, 4);

			return result;
		}

		public void WriteObject<T>(T obj, string serviceName) where T : IBinder {
			var id = IHOSBinderDriver.GetBinderId(obj);
			if(id == -1) id = IHOSBinderDriver.RegisterBinderObject(obj);
			Console.WriteLine($"Parcel sending object with id {id}");
			var flatBinderObject = new FlatBinderObject {
				Type = 2,
				Flags = 0,
				BinderId = id
			};

			Encoding.ASCII.GetBytes(serviceName).CopyTo(flatBinderObject.ServiceName);

			WriteUnmanagedType(ref flatBinderObject);

			// TODO: figure out what this value is

			WriteInplaceObject(new byte[4] { 0, 0, 0, 0 });
		}

		public AndroidStrongPointer<T> ReadStrongPointer<T>() where T : unmanaged, IFlattenable {
			var hasObject = ReadBoolean();

			if(hasObject) {
				var obj = ReadFlattenable<T>();

				return new AndroidStrongPointer<T>(obj);
			}

			return new AndroidStrongPointer<T>();
		}

		public void WriteStrongPointer<T>(ref AndroidStrongPointer<T> value) where T : unmanaged, IFlattenable {
			WriteBoolean(!value.IsNull);

			if(!value.IsNull) WriteFlattenable(ref value.Object);
		}

		public void WriteFlattenable<T>(ref T value) where T : unmanaged, IFlattenable {
			WriteInt64(value.GetFlattenedSize());

			value.Flatten(this);
		}

		public void WriteStatus(Status status) {
			WriteUnmanagedType(ref status);
		}

		public void WriteBoolean(bool value) {
			WriteUnmanagedType(ref value);
		}

		public void WriteInt32(int value) {
			WriteUnmanagedType(ref value);
		}

		public void WriteUInt32(uint value) {
			WriteUnmanagedType(ref value);
		}

		public void WriteInt64(long value) {
			WriteUnmanagedType(ref value);
		}

		public void WriteUInt64(ulong value) {
			WriteUnmanagedType(ref value);
		}

		public void WriteUnmanagedSpan<T>(ReadOnlySpan<T> value) where T : unmanaged {
			WriteInplace(MemoryMarshal.Cast<T, byte>(value));
		}

		public void WriteUnmanagedType<T>(ref T value) where T : unmanaged {
			WriteInplace(SpanHelpers.AsByteSpan(ref value));
		}

		public void WriteInplace(ReadOnlySpan<byte> data) {
			var result = Payload.Slice(_payloadPosition, data.Length);

			data.CopyTo(result);

			_payloadPosition += BitUtils.AlignUp(data.Length, 4);
		}

		public void WriteInplaceObject(ReadOnlySpan<byte> data) {
			var result = Objects.Slice(_objectPosition, data.Length);

			data.CopyTo(result);

			_objectPosition += BitUtils.AlignUp(data.Length, 4);
		}

		void UpdateHeader() {
			var headerSize = (uint) Unsafe.SizeOf<ParcelHeader>();

			Header.PayloadSize = (uint) _payloadPosition;
			Header.ObjectsSize = (uint) _objectPosition;
			Header.PayloadOffset = headerSize;
			Header.ObjectOffset = Header.PayloadOffset + Header.PayloadSize;
		}

		public ReadOnlySpan<byte> Finish() {
			UpdateHeader();

			return Raw.Slice(0, (int) (Header.PayloadSize + Header.ObjectsSize + Unsafe.SizeOf<ParcelHeader>()));
		}

		[StructLayout(LayoutKind.Sequential, Size = 0x28)]
		struct FlatBinderObject {
			public int Type;
			public int Flags;
			public long BinderId;
			public readonly long Cookie;

			byte _serviceNameStart;

			public Span<byte> ServiceName => MemoryMarshal.CreateSpan(ref _serviceNameStart, 0x8);
		}
	}
}