using System;

namespace Ryujinx.Graphics.Texture
{
    public struct SizeInfo
    {
        private readonly int[] _mipOffsets;

        private readonly int _levels;
        private readonly int _depth;
        private readonly bool _is3D;

        public readonly int[] AllOffsets;
        public readonly int[] SliceSizes;
        public int LayerSize { get; }
        public int TotalSize { get; }

        public SizeInfo(int size)
        {
            _mipOffsets = new int[] { 0 };
            AllOffsets  = new int[] { 0 };
            SliceSizes  = new int[] { size };
            _depth      = 1;
            _levels     = 1;
            LayerSize   = size;
            TotalSize   = size;
            _is3D       = false;
        }

        internal SizeInfo(
            int[] mipOffsets,
            int[] allOffsets,
            int[] sliceSizes,
            int   depth,
            int   levels,
            int   layerSize,
            int   totalSize,
            bool  is3D)
        {
            _mipOffsets = mipOffsets;
            AllOffsets  = allOffsets;
            SliceSizes  = sliceSizes;
            _depth      = depth;
            _levels     = levels;
            LayerSize   = layerSize;
            TotalSize   = totalSize;
            _is3D       = is3D;
        }

        public int GetMipOffset(int level)
        {
            if ((uint)level >= _mipOffsets.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(level));
            }

            return _mipOffsets[level];
        }

        public bool FindView(int offset, out int firstLayer, out int firstLevel)
        {
            int index = Array.BinarySearch(AllOffsets, offset);

            if (index < 0)
            {
                firstLayer = 0;
                firstLevel = 0;

                return false;
            }

            if (_is3D)
            {
                firstLayer = index;
                firstLevel = 0;

                int levelDepth = _depth;

                while (firstLayer >= levelDepth)
                {
                    firstLayer -= levelDepth;
                    firstLevel++;
                    levelDepth = Math.Max(levelDepth >> 1, 1);
                }
            }
            else
            {
                firstLayer = index / _levels;
                firstLevel = index - (firstLayer * _levels);
            }

            return true;
        }
    }
}