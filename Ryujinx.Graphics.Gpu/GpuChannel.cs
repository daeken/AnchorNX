﻿using Ryujinx.Graphics.Gpu.Engine.GPFifo;
using Ryujinx.Graphics.Gpu.Image;
using Ryujinx.Graphics.Gpu.Memory;
using System;
using System.Threading;

namespace Ryujinx.Graphics.Gpu
{
    /// <summary>
    /// Represents a GPU channel.
    /// </summary>
    public class GpuChannel : IDisposable
    {
        private readonly GpuContext _context;
        private readonly GPFifoDevice _device;
        private readonly GPFifoProcessor _processor;
        private MemoryManager _memoryManager;

        /// <summary>
        /// Channel buffer bindings manager.
        /// </summary>
        internal BufferManager BufferManager { get; }

        /// <summary>
        /// Channel texture bindings manager.
        /// </summary>
        internal TextureManager TextureManager { get; }

        /// <summary>
        /// Current channel memory manager.
        /// </summary>
        internal MemoryManager MemoryManager => _memoryManager;

        /// <summary>
        /// Creates a new instance of a GPU channel.
        /// </summary>
        /// <param name="context">GPU context that the channel belongs to</param>
        internal GpuChannel(GpuContext context)
        {
            _context = context;
            _device = context.GPFifo;
            _processor = new GPFifoProcessor(context, this);
            BufferManager = new BufferManager(context, this);
            TextureManager = new TextureManager(context, this);
        }

        /// <summary>
        /// Binds a memory manager to the channel.
        /// All submitted and in-flight commands will use the specified memory manager for any memory operations.
        /// </summary>
        /// <param name="memoryManager">The new memory manager to be bound</param>
        public void BindMemory(MemoryManager memoryManager)
        {
            var oldMemoryManager = Interlocked.Exchange(ref _memoryManager, memoryManager ?? throw new ArgumentNullException(nameof(memoryManager)));

            memoryManager.Physical.IncrementReferenceCount();

            if (oldMemoryManager != null)
            {
                oldMemoryManager.Physical.BufferCache.NotifyBuffersModified -= BufferManager.Rebind;
                oldMemoryManager.Physical.DecrementReferenceCount();
            }

            memoryManager.Physical.BufferCache.NotifyBuffersModified += BufferManager.Rebind;
        }

        /// <summary>
        /// Writes data directly to the state of the specified class.
        /// </summary>
        /// <param name="classId">ID of the class to write the data into</param>
        /// <param name="offset">State offset in bytes</param>
        /// <param name="value">Value to be written</param>
        public void Write(ClassId classId, int offset, uint value)
        {
            _processor.Write(classId, offset, (int)value);
        }

        /// <summary>
        /// Push a GPFIFO entry in the form of a prefetched command buffer.
        /// It is intended to be used by nvservices to handle special cases.
        /// </summary>
        /// <param name="commandBuffer">The command buffer containing the prefetched commands</param>
        public void PushHostCommandBuffer(int[] commandBuffer)
        {
            _device.PushHostCommandBuffer(_processor, commandBuffer);
        }

        /// <summary>
        /// Pushes GPFIFO entries.
        /// </summary>
        /// <param name="entries">GPFIFO entries</param>
        public void PushEntries(ReadOnlySpan<ulong> entries)
        {
            _device.PushEntries(_processor, entries);
        }

        /// <summary>
        /// Disposes the GPU channel.
        /// It's an error to use the GPU channel after disposal.
        /// </summary>
        public void Dispose()
        {
            _context.DeferredActions.Enqueue(Destroy);
        }

        /// <summary>
        /// Performs disposal of the host GPU resources used by this channel, that are not shared.
        /// This must only be called from the render thread.
        /// </summary>
        private void Destroy()
        {
            TextureManager.Dispose();

            var oldMemoryManager = Interlocked.Exchange(ref _memoryManager, null);
            if (oldMemoryManager != null)
            {
                oldMemoryManager.Physical.BufferCache.NotifyBuffersModified -= BufferManager.Rebind;
                oldMemoryManager.Physical.DecrementReferenceCount();
            }
        }
    }
}
