﻿using Ryujinx.Memory.Range;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Ryujinx.Memory.Tracking
{
    /// <summary>
    /// A tracking handle for a given region of virtual memory. The Dirty flag is updated whenever any changes are made,
    /// and an action can be performed when the region is read to or written from.
    /// </summary>
    public class RegionHandle : IRegionHandle, IRange
    {
        /// <summary>
        /// If more than this number of checks have been performed on a dirty flag since its last reprotect,
        /// then it is dirtied infrequently.
        /// </summary>
        private static int CheckCountForInfrequent = 3;

        /// <summary>
        /// Number of frequent dirty/consume in a row to make this handle volatile.
        /// </summary>
        private static int VolatileThreshold = 5;

        public bool Dirty { get; private set; }
        public bool Unmapped { get; private set; }

        public ulong Address { get; }
        public ulong Size { get; }
        public ulong EndAddress { get; }

        internal IMultiRegionHandle Parent { get; set; }
        internal int SequenceNumber { get; set; }

        private event Action _onDirty;

        private RegionSignal _preAction; // Action to perform before a read or write. This will block the memory access.
        private readonly List<VirtualRegion> _regions;
        private readonly MemoryTracking _tracking;
        private bool _disposed;

        private int _checkCount = 0;
        private int _volatileCount = 0;
        private bool _volatile;

        internal MemoryPermission RequiredPermission
        {
            get
            {
                // If this is unmapped, allow reprotecting as RW as it can't be dirtied.
                // This is required for the partial unmap cases where part of the data are still being accessed.
                if (Unmapped)
                {
                    return MemoryPermission.ReadAndWrite;
                }

                if (_preAction != null)
                {
                    return MemoryPermission.None;
                }

                return Dirty ? MemoryPermission.ReadAndWrite : MemoryPermission.Read;
            }
        }

        internal RegionSignal PreAction => _preAction;

        /// <summary>
        /// Create a new region handle. The handle is registered with the given tracking object,
        /// and will be notified of any changes to the specified region.
        /// </summary>
        /// <param name="tracking">Tracking object for the target memory block</param>
        /// <param name="address">Virtual address of the region to track</param>
        /// <param name="size">Size of the region to track</param>
        /// <param name="mapped">True if the region handle starts mapped</param>
        internal RegionHandle(MemoryTracking tracking, ulong address, ulong size, bool mapped = true)
        {
            Dirty = mapped;
            Unmapped = !mapped;
            Address = address;
            Size = size;
            EndAddress = address + size;

            _tracking = tracking;
            _regions = tracking.GetVirtualRegionsForHandle(address, size);
            foreach (var region in _regions)
            {
                region.Handles.Add(this);
            }
        }

        /// <summary>
        /// Clear the volatile state of this handle.
        /// </summary>
        private void ClearVolatile()
        {
            _volatileCount = 0;
            _volatile = false;
        }

        /// <summary>
        /// Check if this handle is dirty, or if it is volatile. (changes very often)
        /// </summary>
        /// <returns>True if the handle is dirty or volatile, false otherwise</returns>
        public bool DirtyOrVolatile()
        {
            _checkCount++;
            return Dirty || _volatile;
        }

        /// <summary>
        /// Signal that a memory action occurred within this handle's virtual regions.
        /// </summary>
        /// <param name="write">Whether the region was written to or read</param>
        internal void Signal(ulong address, ulong size, bool write)
        {
            RegionSignal action = Interlocked.Exchange(ref _preAction, null);

            // If this handle was already unmapped (even if just partially),
            // then we have nothing to do until it is mapped again.
            // The pre-action should be still consumed to avoid flushing on remap.
            if (Unmapped)
            {
                return;
            }

            action?.Invoke(address, size);

            if (write)
            {
                bool oldDirty = Dirty;
                Dirty = true;
                if (!oldDirty)
                {
                    _onDirty?.Invoke();
                }
                Parent?.SignalWrite();
            }
        }

        /// <summary>
        /// Force this handle to be dirty, without reprotecting.
        /// </summary>
        public void ForceDirty()
        {
            Dirty = true;
        }

        /// <summary>
        /// Consume the dirty flag for this handle, and reprotect so it can be set on the next write.
        /// </summary>
        public void Reprotect(bool asDirty = false)
        {
            if (_volatile) return;

            Dirty = asDirty;

            bool protectionChanged = false;

            lock (_tracking.TrackingLock)
            {
                foreach (VirtualRegion region in _regions)
                {
                    protectionChanged |= region.UpdateProtection();
                }
            }

            if (!protectionChanged)
            {
                // Counteract the check count being incremented when this handle was forced dirty.
                // It doesn't count for protected write tracking.

                _checkCount--;
            }
            else if (!asDirty)
            {
                if (_checkCount > 0 && _checkCount < CheckCountForInfrequent)
                {
                    if (++_volatileCount >= VolatileThreshold && _preAction == null)
                    {
                        _volatile = true;
                        return;
                    }
                }
                else
                {
                    _volatileCount = 0;
                }

                _checkCount = 0;
            }
        }

        /// <summary>
        /// Register an action to perform when the tracked region is read or written.
        /// The action is automatically removed after it runs.
        /// </summary>
        /// <param name="action">Action to call on read or write</param>
        public void RegisterAction(RegionSignal action)
        {
            ClearVolatile();

            RegionSignal lastAction = Interlocked.Exchange(ref _preAction, action);
            if (lastAction == null && action != lastAction)
            {
                lock (_tracking.TrackingLock)
                {
                    foreach (VirtualRegion region in _regions)
                    {
                        region.UpdateProtection();
                    }
                }
            }
        }

        /// <summary>
        /// Register an action to perform when the region is written to.
        /// This action will not be removed when it is called - it is called each time the dirty flag is set.
        /// </summary>
        /// <param name="action">Action to call on dirty</param>
        public void RegisterDirtyEvent(Action action)
        {
            _onDirty += action;
        }

        /// <summary>
        /// Add a child virtual region to this handle.
        /// </summary>
        /// <param name="region">Virtual region to add as a child</param>
        internal void AddChild(VirtualRegion region)
        {
            _regions.Add(region);
        }

        /// <summary>
        /// Signal that this handle has been mapped or unmapped.
        /// </summary>
        /// <param name="mapped">True if the handle has been mapped, false if unmapped</param>
        internal void SignalMappingChanged(bool mapped)
        {
            if (Unmapped == mapped)
            {
                Unmapped = !mapped;

                if (Unmapped)
                {
                    ClearVolatile();
                    Dirty = false;
                }
            }
        }

        /// <summary>
        /// Check if this region overlaps with another.
        /// </summary>
        /// <param name="address">Base address</param>
        /// <param name="size">Size of the region</param>
        /// <returns>True if overlapping, false otherwise</returns>
        public bool OverlapsWith(ulong address, ulong size)
        {
            return Address < address + size && address < EndAddress;
        }

        /// <summary>
        /// Dispose the handle. Within the tracking lock, this removes references from virtual regions.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            _disposed = true;

            lock (_tracking.TrackingLock)
            {
                foreach (VirtualRegion region in _regions)
                {
                    region.RemoveHandle(this);
                }
            }
        }
    }
}
