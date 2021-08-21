﻿using Ryujinx.Graphics.Video;
using System;

namespace Ryujinx.Graphics.Nvdec.H264
{
    public sealed class Decoder : IH264Decoder
    {
        public bool IsHardwareAccelerated => false;

        private const int WorkBufferSize = 0x200;

        private readonly byte[] _workBuffer = new byte[WorkBufferSize];

        private FFmpegContext _context = new FFmpegContext();

        private int _oldOutputWidth;
        private int _oldOutputHeight;

        public ISurface CreateSurface(int width, int height)
        {
            return new Surface(width, height);
        }

        public bool Decode(ref H264PictureInfo pictureInfo, ISurface output, ReadOnlySpan<byte> bitstream)
        {
            Surface outSurf = (Surface)output;

            if (outSurf.RequestedWidth != _oldOutputWidth ||
                outSurf.RequestedHeight != _oldOutputHeight)
            {
                _context.Dispose();
                _context = new FFmpegContext();

                _oldOutputWidth = outSurf.RequestedWidth;
                _oldOutputHeight = outSurf.RequestedHeight;
            }

            Span<byte> bs = Prepend(bitstream, SpsAndPpsReconstruction.Reconstruct(ref pictureInfo, _workBuffer));

            return _context.DecodeFrame(outSurf, bs) == 0;
        }

        private static byte[] Prepend(ReadOnlySpan<byte> data, ReadOnlySpan<byte> prep)
        {
            byte[] output = new byte[data.Length + prep.Length];

            prep.CopyTo(output);
            data.CopyTo(new Span<byte>(output).Slice(prep.Length));

            return output;
        }

        public void Dispose() => _context.Dispose();
    }
}
