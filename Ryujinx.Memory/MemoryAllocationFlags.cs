﻿using System;

namespace Ryujinx.Memory
{
    /// <summary>
    /// Flags that controls allocation and other properties of the memory block memory.
    /// </summary>
    [Flags]
    public enum MemoryAllocationFlags
    {
        /// <summary>
        /// No special allocation settings.
        /// </summary>
        None = 0,

        /// <summary>
        /// Reserve a region of memory on the process address space,
        /// without actually allocation any backing memory.
        /// </summary>
        Reserve = 1 << 0,

        /// <summary>
        /// Enables read and write tracking of the memory block.
        /// This currently does nothing and is reserved for future use.
        /// </summary>
        Tracked = 1 << 1,

        /// <summary>
        /// Enables mirroring of the memory block through aliasing of memory pages.
        /// When enabled, this allows creating more memory blocks sharing the same backing storage.
        /// </summary>
        Mirrorable = 1 << 2
    }
}
