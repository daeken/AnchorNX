using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AnchorNX {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MmioAttribute : Attribute {
		public readonly ulong Address;
		public MmioAttribute(ulong address) => Address = address;
	}

	public interface IMmioArray {
		int Count { get; }
		Type Type { get; }
		ulong BaseAddress { get; set; }
		bool CanRead { get; }
		bool CanWrite { get; }
		object Get(int index);
		void Set(int index, object value);
	}

	public class MmioArray<T> : IMmioArray {
		public int Count { get; }
		public Type Type => typeof(T);
		public ulong BaseAddress { get; set; }
		public bool CanRead => Getter != null;
		public bool CanWrite => Setter != null;
		readonly Func<int, T> Getter;
		readonly Action<int, T> Setter;

		public MmioArray(int count, Func<int, T> getter, Action<int, T> setter = null) {
			Count = count;
			Getter = getter;
			Setter = setter;
		}

		public MmioArray(int count, T defaultValue = default, bool writable = false) {
			var arr = new T[count];
			for(var i = 0; i < count; ++i) arr[i] = defaultValue;
			Count = count;
			Getter = i => arr[i];
			Setter = writable
				? (i, value) => arr[i] = value
				: null;
		}

		public T this[int index] {
			get => Getter(index);
			set => Setter(index, value);
		}

		public object Get(int index) => Getter(index);
		public void Set(int index, object value) => Setter(index, (T) value);
	}
	
	public abstract class MmioDevice {
		public static readonly Dictionary<ulong, MmioDevice> Devices = new();
		public abstract (ulong Start, ulong End) AddressRange { get; }
		readonly object[][] Accessors;
		public virtual bool ZeroFaker => false;

		protected MmioDevice() {
			Debug.Assert((AddressRange.Start & 0xFFF) == 0);
			var size = AddressRange.End - AddressRange.Start + 1;
			Accessors = new object[4][];
			for(var i = 0; i < 4; ++i) {
				Accessors[i] = new object[size];
				size /= 2;
			}
			
			foreach(var pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				if(pi.GetCustomAttribute<MmioAttribute>() is { } attr) {
					var tsize = Marshal.SizeOf(pi.PropertyType);
					var tindex = tsize switch {
						1 => 0, 2 => 1, 4 => 2, 8 => 3, 
						_ => throw new NotImplementedException()
					};
					var paddr = attr.Address;
					if(paddr < AddressRange.Start || paddr + (ulong) tsize > AddressRange.End)
						throw new IndexOutOfRangeException();
					var index = attr.Address - AddressRange.Start;
					if(index % (ulong) tsize != 0)
						throw new Exception();
					index /= (ulong) tsize;
					Accessors[tindex][index] = pi;
				}
			
			foreach(var fi in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				if(fi.GetCustomAttribute<MmioAttribute>() is { } attr) {
					var farr = fi.GetValue(this);
					if(farr is not IMmioArray ima) throw new NotSupportedException();
					var tsize = Marshal.SizeOf(ima.Type);
					var tindex = tsize switch {
						1 => 0, 2 => 1, 4 => 2, 8 => 3, 
						_ => throw new NotImplementedException()
					};
					var paddr = attr.Address;
					if(paddr < AddressRange.Start || paddr + (ulong) (ima.Count * tsize) > AddressRange.End)
						throw new IndexOutOfRangeException();
					ima.BaseAddress = paddr;
					var index = attr.Address - AddressRange.Start;
					if(index % (ulong) tsize != 0)
						throw new Exception();
					index /= (ulong) tsize;
					for(var i = 0; i < ima.Count; ++i)
						Accessors[tindex][(int) index + i] = ima;
				}

			for(var root = AddressRange.Start; root < AddressRange.End; root += 0x1000)
				Devices[root] = this;
		}

		public bool Get(ulong addr, int sizeIndex, out ulong value) {
			Console.WriteLine($"Foo? {addr:X}");
			var offset = (addr - AddressRange.Start) >> sizeIndex;
			var accessor = Accessors[sizeIndex][offset];
			switch(accessor) {
				case PropertyInfo { CanRead: true } pi:
					value = (ulong) Convert.ChangeType(pi.GetValue(this), typeof(ulong));
					return true;
				case IMmioArray { CanRead: true } ma:
					value = (ulong) Convert.ChangeType(ma.Get((int) ((addr - ma.BaseAddress) >> sizeIndex)), typeof(ulong));
					return true;
			}

			if(ZeroFaker) {
				Console.WriteLine($"MMIO device {GetType().Name} returned FAKE value for address 0x{addr:X} with size {8 << sizeIndex}");
				value = 0;
				return true;
			}

			throw new Exception($"Attempted read from MMIO device {GetType().Name} on address 0x{addr:X} with size {8 << sizeIndex}");
		}

		public bool Set(ulong addr, int sizeIndex, ulong value) {
			Console.WriteLine($"Bar? {addr:X}");
			var offset = (addr - AddressRange.Start) >> sizeIndex;
			var accessor = Accessors[sizeIndex][offset];
			switch(accessor) {
				case PropertyInfo { CanWrite: true } pi:
					pi.SetValue(this, Convert.ChangeType(value, pi.PropertyType));
					return true;
				case IMmioArray { CanWrite: true } ma:
					ma.Set((int) ((addr - ma.BaseAddress) >> sizeIndex), Convert.ChangeType(value, ma.Type));
					return true;
			}

			if(ZeroFaker) {
				Console.WriteLine($"MMIO device {GetType().Name} handled FAKE write for address 0x{addr:X} with size {8 << sizeIndex} and value 0x{value:X}");
				return true;
			}

			throw new Exception($"Attempted write to MMIO device {GetType().Name} on address 0x{addr:X} with size {8 << sizeIndex} and value 0x{value:X}");
		}

		public static bool Read(ulong addr, int sizeIndex, out ulong value) {
			value = 0;
			return Devices.TryGetValue(addr & ~0xFFFUL, out var dev) && dev.Get(addr, sizeIndex, out value);
		}

		public static bool Write(ulong addr, int sizeIndex, ulong value) =>
			Devices.TryGetValue(addr & ~0xFFFUL, out var dev) && dev.Set(addr, sizeIndex, value);
	}
}