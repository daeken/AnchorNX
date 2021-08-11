using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AnchorNX {
	[AttributeUsage(AttributeTargets.Property)]
	public class MmioAttribute : Attribute {
		public readonly ulong Address;
		public MmioAttribute(ulong address) => Address = address;
	}
	
	public abstract class MmioDevice {
		public static readonly Dictionary<ulong, MmioDevice> Devices = new();
		public abstract (ulong Start, ulong End) AddressRange { get; }
		readonly PropertyInfo[][] Accessors;
		public virtual bool ZeroFaker => false;

		protected MmioDevice() {
			Debug.Assert((AddressRange.Start & 0xFFF) == 0);
			var size = AddressRange.End - AddressRange.Start + 1;
			Accessors = new PropertyInfo[4][];
			for(var i = 0; i < 4; ++i) {
				Accessors[i] = new PropertyInfo[size];
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
					if(paddr < AddressRange.Start || paddr > AddressRange.End)
						throw new IndexOutOfRangeException();
					var index = attr.Address - AddressRange.Start;
					if(index % (ulong) tsize != 0)
						throw new Exception();
					index /= (ulong) tsize;
					Accessors[tindex][index] = pi;
				}

			for(var root = AddressRange.Start; root < AddressRange.End; root += 0x1000)
				Devices[root] = this;
		}

		public bool Get(ulong addr, int sizeIndex, out ulong value) {
			var offset = (addr - AddressRange.Start) >> sizeIndex;
			var pi = Accessors[sizeIndex][offset];
			if(pi == null || !pi.CanRead) {
				if(ZeroFaker) {
					Console.WriteLine($"MMIO device {GetType().Name} returned FAKE value for address 0x{addr:X} with size {8 << sizeIndex}");
					value = 0;
					return true;
				}
				throw new Exception($"Attempted read from MMIO device {GetType().Name} on address 0x{addr:X} with size {8 << sizeIndex}");
			}

			value = (ulong) Convert.ChangeType(pi.GetValue(this), typeof(ulong));
			return true;
		}

		public bool Set(ulong addr, int sizeIndex, ulong value) {
			var offset = (addr - AddressRange.Start) >> sizeIndex;
			var pi = Accessors[sizeIndex][offset];
			if(pi == null || !pi.CanWrite) {
				if(ZeroFaker) {
					Console.WriteLine($"MMIO device {GetType().Name} handled FAKE write for address 0x{addr:X} with value 0x{value:X} size {8 << sizeIndex}");
					return true;
				}
				throw new Exception($"Attempted write to MMIO device {GetType().Name} on address 0x{addr:X} with value 0x{value} and size {8 << sizeIndex}");
			}

			pi.SetValue(this, Convert.ChangeType(value, pi.PropertyType));
			return true;
		}
	}
}