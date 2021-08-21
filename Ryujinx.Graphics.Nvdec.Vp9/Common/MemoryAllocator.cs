using Ryujinx.Common.Memory;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ryujinx.Graphics.Nvdec.Vp9.Common
{
    internal class MemoryAllocator : IDisposable
    {
        private const int PoolEntries = 10;

        private struct PoolItem
        {
            public IntPtr Pointer;
            public int Length;
            public bool InUse;
        }

        private PoolItem[] _pool = new PoolItem[PoolEntries];

        public ArrayPtr<T> Allocate<T>(int length) where T : unmanaged
        {
            int lengthInBytes = Unsafe.SizeOf<T>() * length;

            IntPtr ptr = IntPtr.Zero;

            for (int i = 0; i < PoolEntries; i++)
            {
                ref PoolItem item = ref _pool[i];

                if (!item.InUse && item.Length == lengthInBytes)
                {
                    item.InUse = true;
                    ptr = item.Pointer;
                    break;
                }
            }

            if (ptr == IntPtr.Zero)
            {
                ptr = Marshal.AllocHGlobal(lengthInBytes);

                for (int i = 0; i < PoolEntries; i++)
                {
                    ref PoolItem item = ref _pool[i];

                    if (!item.InUse)
                    {
                        item.InUse = true;
                        if (item.Pointer != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(item.Pointer);
                        }
                        item.Pointer = ptr;
                        item.Length = lengthInBytes;
                        break;
                    }
                }
            }

            return new ArrayPtr<T>(ptr, length);
        }

        public unsafe void Free<T>(ArrayPtr<T> arr) where T : unmanaged
        {
            IntPtr ptr = (IntPtr)arr.ToPointer();

            for (int i = 0; i < PoolEntries; i++)
            {
                ref PoolItem item = ref _pool[i];

                if (item.Pointer == ptr)
                {
                    item.InUse = false;
                    break;
                }
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < PoolEntries; i++)
            {
                ref PoolItem item = ref _pool[i];

                if (item.Pointer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(item.Pointer);
                    item.Pointer = IntPtr.Zero;
                }
            }
        }
    }
}