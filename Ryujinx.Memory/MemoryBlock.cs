﻿using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Ryujinx.Memory
{
    /// <summary>
    /// Represents a block of contiguous physical guest memory.
    /// </summary>
    public sealed class MemoryBlock : IWritableBlock, IDisposable
    {
        private readonly bool _usesSharedMemory;
        private readonly bool _isMirror;
        private IntPtr _sharedMemory;
        private IntPtr _pointer;

        /// <summary>
        /// Pointer to the memory block data.
        /// </summary>
        public IntPtr Pointer => _pointer;

        /// <summary>
        /// Size of the memory block.
        /// </summary>
        public ulong Size { get; }

        /// <summary>
        /// Creates a new instance of the memory block class.
        /// </summary>
        /// <param name="size">Size of the memory block in bytes</param>
        /// <param name="flags">Flags that controls memory block memory allocation</param>
        /// <exception cref="OutOfMemoryException">Throw when there's no enough memory to allocate the requested size</exception>
        /// <exception cref="PlatformNotSupportedException">Throw when the current platform is not supported</exception>
        public MemoryBlock(ulong size, MemoryAllocationFlags flags = MemoryAllocationFlags.None)
        {
            if (flags.HasFlag(MemoryAllocationFlags.Mirrorable))
            {
                _sharedMemory = MemoryManagement.CreateSharedMemory(size, flags.HasFlag(MemoryAllocationFlags.Reserve));
                _pointer = MemoryManagement.MapSharedMemory(_sharedMemory);
                _usesSharedMemory = true;
            }
            else if (flags.HasFlag(MemoryAllocationFlags.Reserve))
            {
                _pointer = MemoryManagement.Reserve(size);
            }
            else
            {
                _pointer = MemoryManagement.Allocate(size);
            }

            Size = size;
        }

        /// <summary>
        /// Creates a new instance of the memory block class, with a existing backing storage.
        /// </summary>
        /// <param name="size">Size of the memory block in bytes</param>
        /// <param name="sharedMemory">Shared memory to use as backing storage for this block</param>
        /// <exception cref="OutOfMemoryException">Throw when there's no enough address space left to map the shared memory</exception>
        /// <exception cref="PlatformNotSupportedException">Throw when the current platform is not supported</exception>
        private MemoryBlock(ulong size, IntPtr sharedMemory)
        {
            _pointer = MemoryManagement.MapSharedMemory(sharedMemory);
            Size = size;
            _usesSharedMemory = true;
            _isMirror = true;
        }

        /// <summary>
        /// Creates a memory block that shares the backing storage with this block.
        /// The memory and page commitments will be shared, however memory protections are separate.
        /// </summary>
        /// <returns>A new memory block that shares storage with this one</returns>
        /// <exception cref="NotSupportedException">Throw when the current memory block does not support mirroring</exception>
        /// <exception cref="OutOfMemoryException">Throw when there's no enough address space left to map the shared memory</exception>
        /// <exception cref="PlatformNotSupportedException">Throw when the current platform is not supported</exception>
        public MemoryBlock CreateMirror()
        {
            if (_sharedMemory == IntPtr.Zero)
            {
                throw new NotSupportedException("Mirroring is not supported on the memory block because the Mirrorable flag was not set.");
            }

            return new MemoryBlock(Size, _sharedMemory);
        }

        /// <summary>
        /// Commits a region of memory that has previously been reserved.
        /// This can be used to allocate memory on demand.
        /// </summary>
        /// <param name="offset">Starting offset of the range to be committed</param>
        /// <param name="size">Size of the range to be committed</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        public bool Commit(ulong offset, ulong size)
        {
            return MemoryManagement.Commit(GetPointerInternal(offset, size), size);
        }

        /// <summary>
        /// Decommits a region of memory that has previously been reserved and optionally comitted.
        /// This can be used to free previously allocated memory on demand.
        /// </summary>
        /// <param name="offset">Starting offset of the range to be decommitted</param>
        /// <param name="size">Size of the range to be decommitted</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        public bool Decommit(ulong offset, ulong size)
        {
            return MemoryManagement.Decommit(GetPointerInternal(offset, size), size);
        }

        /// <summary>
        /// Reprotects a region of memory.
        /// </summary>
        /// <param name="offset">Starting offset of the range to be reprotected</param>
        /// <param name="size">Size of the range to be reprotected</param>
        /// <param name="permission">New memory permissions</param>
        /// <param name="throwOnFail">True if a failed reprotect should throw</param>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        /// <exception cref="MemoryProtectionException">Throw when <paramref name="permission"/> is invalid</exception>
        public void Reprotect(ulong offset, ulong size, MemoryPermission permission, bool throwOnFail = true)
        {
            MemoryManagement.Reprotect(GetPointerInternal(offset, size), size, permission, throwOnFail);
        }

        /// <summary>
        /// Remaps a region of memory into this memory block.
        /// </summary>
        /// <param name="offset">Starting offset of the range to be remapped into</param>
        /// <param name="sourceAddress">Starting offset of the range to be remapped from</param>
        /// <param name="size">Size of the range to be remapped</param>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        /// <exception cref="MemoryProtectionException">Throw when <paramref name="permission"/> is invalid</exception>
        public void Remap(ulong offset, IntPtr sourceAddress, ulong size)
        {
            MemoryManagement.Remap(GetPointerInternal(offset, size), sourceAddress, size);
        }

        /// <summary>
        /// Reads bytes from the memory block.
        /// </summary>
        /// <param name="offset">Starting offset of the range being read</param>
        /// <param name="data">Span where the bytes being read will be copied to</param>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when the memory region specified for the the data is out of range</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read(ulong offset, Span<byte> data)
        {
            GetSpan(offset, data.Length).CopyTo(data);
        }

        /// <summary>
        /// Reads data from the memory block.
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="offset">Offset where the data is located</param>
        /// <returns>Data at the specified address</returns>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when the memory region specified for the the data is out of range</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Read<T>(ulong offset) where T : unmanaged
        {
            return GetRef<T>(offset);
        }

        /// <summary>
        /// Writes bytes to the memory block.
        /// </summary>
        /// <param name="offset">Starting offset of the range being written</param>
        /// <param name="data">Span where the bytes being written will be copied from</param>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when the memory region specified for the the data is out of range</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong offset, ReadOnlySpan<byte> data)
        {
            data.CopyTo(GetSpan(offset, data.Length));
        }

        /// <summary>
        /// Writes data to the memory block.
        /// </summary>
        /// <typeparam name="T">Type of the data being written</typeparam>
        /// <param name="offset">Offset to write the data into</param>
        /// <param name="data">Data to be written</param>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when the memory region specified for the the data is out of range</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(ulong offset, T data) where T : unmanaged
        {
            GetRef<T>(offset) = data;
        }

        /// <summary>
        /// Copies data from one memory location to another.
        /// </summary>
        /// <param name="dstOffset">Destination offset to write the data into</param>
        /// <param name="srcOffset">Source offset to read the data from</param>
        /// <param name="size">Size of the copy in bytes</param>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when <paramref name="srcOffset"/>, <paramref name="dstOffset"/> or <paramref name="size"/> is out of range</exception>
        public void Copy(ulong dstOffset, ulong srcOffset, ulong size)
        {
            const int MaxChunkSize = 1 << 24;

            for (ulong offset = 0; offset < size; offset += MaxChunkSize)
            {
                int copySize = (int)Math.Min(MaxChunkSize, size - offset);

                Write(dstOffset + offset, GetSpan(srcOffset + offset, copySize));
            }
        }

        /// <summary>
        /// Fills a region of memory with zeros.
        /// </summary>
        /// <param name="offset">Offset of the region to fill with zeros</param>
        /// <param name="size">Size in bytes of the region to fill</param>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        public void ZeroFill(ulong offset, ulong size)
        {
            const int MaxChunkSize = 1 << 24;

            for (ulong subOffset = 0; subOffset < size; subOffset += MaxChunkSize)
            {
                int copySize = (int)Math.Min(MaxChunkSize, size - subOffset);

                GetSpan(offset + subOffset, copySize).Fill(0);
            }
        }

        /// <summary>
        /// Gets a reference of the data at a given memory block region.
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="offset">Offset of the memory region</param>
        /// <returns>A reference to the given memory region data</returns>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T GetRef<T>(ulong offset) where T : unmanaged
        {
            IntPtr ptr = _pointer;

            if (ptr == IntPtr.Zero)
            {
                ThrowObjectDisposed();
            }

            int size = Unsafe.SizeOf<T>();

            ulong endOffset = offset + (ulong)size;

            if (endOffset > Size || endOffset < offset)
            {
                ThrowInvalidMemoryRegionException();
            }

            return ref Unsafe.AsRef<T>((void*)PtrAddr(ptr, offset));
        }

        /// <summary>
        /// Gets the pointer of a given memory block region.
        /// </summary>
        /// <param name="offset">Start offset of the memory region</param>
        /// <param name="size">Size in bytes of the region</param>
        /// <returns>The pointer to the memory region</returns>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public nuint GetPointer(ulong offset, ulong size) => (nuint)(ulong)GetPointerInternal(offset, size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IntPtr GetPointerInternal(ulong offset, ulong size)
        {
            IntPtr ptr = _pointer;

            if (ptr == IntPtr.Zero)
            {
                ThrowObjectDisposed();
            }

            ulong endOffset = offset + size;

            if (endOffset > Size || endOffset < offset)
            {
                ThrowInvalidMemoryRegionException();
            }

            return PtrAddr(ptr, offset);
        }

        /// <summary>
        /// Gets the <see cref="Span{T}"/> of a given memory block region.
        /// </summary>
        /// <param name="offset">Start offset of the memory region</param>
        /// <param name="size">Size in bytes of the region</param>
        /// <returns>Span of the memory region</returns>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<byte> GetSpan(ulong offset, int size)
        {
            return new Span<byte>((void*)GetPointerInternal(offset, (ulong)size), size);
        }

        /// <summary>
        /// Gets the <see cref="Memory{T}"/> of a given memory block region.
        /// </summary>
        /// <param name="offset">Start offset of the memory region</param>
        /// <param name="size">Size in bytes of the region</param>
        /// <returns>Memory of the memory region</returns>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Memory<byte> GetMemory(ulong offset, int size)
        {
            return new NativeMemoryManager<byte>((byte*)GetPointerInternal(offset, (ulong)size), size).Memory;
        }

        /// <summary>
        /// Gets a writable region of a given memory block region.
        /// </summary>
        /// <param name="offset">Start offset of the memory region</param>
        /// <param name="size">Size in bytes of the region</param>
        /// <returns>Writable region of the memory region</returns>
        /// <exception cref="ObjectDisposedException">Throw when the memory block has already been disposed</exception>
        /// <exception cref="InvalidMemoryRegionException">Throw when either <paramref name="offset"/> or <paramref name="size"/> are out of range</exception>
        public WritableRegion GetWritableRegion(ulong offset, int size)
        {
            return new WritableRegion(null, offset, GetMemory(offset, size));
        }

        /// <summary>
        /// Adds a 64-bits offset to a native pointer.
        /// </summary>
        /// <param name="pointer">Native pointer</param>
        /// <param name="offset">Offset to add</param>
        /// <returns>Native pointer with the added offset</returns>
        private IntPtr PtrAddr(IntPtr pointer, ulong offset)
        {
            return (IntPtr)(pointer.ToInt64() + (long)offset);
        }

        /// <summary>
        /// Frees the memory allocated for this memory block.
        /// </summary>
        /// <remarks>
        /// It's an error to use the memory block after disposal.
        /// </remarks>
        public void Dispose() => FreeMemory();

        ~MemoryBlock() => FreeMemory();

        private void FreeMemory()
        {
            IntPtr ptr = Interlocked.Exchange(ref _pointer, IntPtr.Zero);

            // If pointer is null, the memory was already freed or never allocated.
            if (ptr != IntPtr.Zero)
            {
                if (_usesSharedMemory)
                {
                    MemoryManagement.UnmapSharedMemory(ptr);

                    if (_sharedMemory != IntPtr.Zero && !_isMirror)
                    {
                        MemoryManagement.DestroySharedMemory(_sharedMemory);
                        _sharedMemory = IntPtr.Zero;
                    }
                }
                else
                {
                    MemoryManagement.Free(ptr);
                }
            }
        }

        private void ThrowObjectDisposed() => throw new ObjectDisposedException(nameof(MemoryBlock));
        private void ThrowInvalidMemoryRegionException() => throw new InvalidMemoryRegionException();
    }
}
