using Ryujinx.Graphics.GAL;
using Ryujinx.Graphics.Gpu.Image;

namespace Ryujinx.Graphics.Gpu.Engine.Types
{
    /// <summary>
    /// Color texture format.
    /// </summary>
    enum ColorFormat
    {
        R32G32B32A32Float = 0xc0,
        R32G32B32A32Sint  = 0xc1,
        R32G32B32A32Uint  = 0xc2,
        R32G32B32X32Float = 0xc3,
        R32G32B32X32Sint  = 0xc4,
        R32G32B32X32Uint  = 0xc5,
        R16G16B16X16Unorm = 0xc6,
        R16G16B16X16Snorm = 0xc7,
        R16G16B16X16Sint  = 0xc8,
        R16G16B16X16Uint  = 0xc9,
        R16G16B16A16Float = 0xca,
        R32G32Float       = 0xcb,
        R32G32Sint        = 0xcc,
        R32G32Uint        = 0xcd,
        R16G16B16X16Float = 0xce,
        B8G8R8A8Unorm     = 0xcf,
        B8G8R8A8Srgb      = 0xd0,
        R10G10B10A2Unorm  = 0xd1,
        R10G10B10A2Uint   = 0xd2,
        R8G8B8A8Unorm     = 0xd5,
        R8G8B8A8Srgb      = 0xd6,
        R8G8B8X8Snorm     = 0xd7,
        R8G8B8X8Sint      = 0xd8,
        R8G8B8X8Uint      = 0xd9,
        R16G16Unorm       = 0xda,
        R16G16Snorm       = 0xdb,
        R16G16Sint        = 0xdc,
        R16G16Uint        = 0xdd,
        R16G16Float       = 0xde,
        R11G11B10Float    = 0xe0,
        R32Sint           = 0xe3,
        R32Uint           = 0xe4,
        R32Float          = 0xe5,
        B8G8R8X8Unorm     = 0xe6,
        B8G8R8X8Srgb      = 0xe7,
        B5G6R5Unorm       = 0xe8,
        B5G5R5A1Unorm     = 0xe9,
        R8G8Unorm         = 0xea,
        R8G8Snorm         = 0xeb,
        R8G8Sint          = 0xec,
        R8G8Uint          = 0xed,
        R16Unorm          = 0xee,
        R16Snorm          = 0xef,
        R16Sint           = 0xf0,
        R16Uint           = 0xf1,
        R16Float          = 0xf2,
        R8Unorm           = 0xf3,
        R8Snorm           = 0xf4,
        R8Sint            = 0xf5,
        R8Uint            = 0xf6,
        B5G5R5X1Unorm     = 0xf8,
        R8G8B8X8Unorm     = 0xf9,
        R8G8B8X8Srgb      = 0xfa
    }

    static class ColorFormatConverter
    {
        /// <summary>
        /// Converts the color texture format to a host compatible format.
        /// </summary>
        /// <param name="format">Color format</param>
        /// <returns>Host compatible format enum value</returns>
        public static FormatInfo Convert(this ColorFormat format)
        {
            return format switch
            {
                ColorFormat.R32G32B32A32Float => new FormatInfo(Format.R32G32B32A32Float, 1, 1, 16, 4),
                ColorFormat.R32G32B32A32Sint  => new FormatInfo(Format.R32G32B32A32Sint,  1, 1, 16, 4),
                ColorFormat.R32G32B32A32Uint  => new FormatInfo(Format.R32G32B32A32Uint,  1, 1, 16, 4),
                ColorFormat.R32G32B32X32Float => new FormatInfo(Format.R32G32B32A32Float, 1, 1, 16, 4),
                ColorFormat.R32G32B32X32Sint  => new FormatInfo(Format.R32G32B32A32Sint,  1, 1, 16, 4),
                ColorFormat.R32G32B32X32Uint  => new FormatInfo(Format.R32G32B32A32Uint,  1, 1, 16, 4),
                ColorFormat.R16G16B16X16Unorm => new FormatInfo(Format.R16G16B16A16Unorm, 1, 1, 8,  4),
                ColorFormat.R16G16B16X16Snorm => new FormatInfo(Format.R16G16B16A16Snorm, 1, 1, 8,  4),
                ColorFormat.R16G16B16X16Sint  => new FormatInfo(Format.R16G16B16A16Sint,  1, 1, 8,  4),
                ColorFormat.R16G16B16X16Uint  => new FormatInfo(Format.R16G16B16A16Uint,  1, 1, 8,  4),
                ColorFormat.R16G16B16A16Float => new FormatInfo(Format.R16G16B16A16Float, 1, 1, 8,  4),
                ColorFormat.R32G32Float       => new FormatInfo(Format.R32G32Float,       1, 1, 8,  2),
                ColorFormat.R32G32Sint        => new FormatInfo(Format.R32G32Sint,        1, 1, 8,  2),
                ColorFormat.R32G32Uint        => new FormatInfo(Format.R32G32Uint,        1, 1, 8,  2),
                ColorFormat.R16G16B16X16Float => new FormatInfo(Format.R16G16B16A16Float, 1, 1, 8,  4),
                ColorFormat.B8G8R8A8Unorm     => new FormatInfo(Format.B8G8R8A8Unorm,     1, 1, 4,  4),
                ColorFormat.B8G8R8A8Srgb      => new FormatInfo(Format.B8G8R8A8Srgb,      1, 1, 4,  4),
                ColorFormat.R10G10B10A2Unorm  => new FormatInfo(Format.R10G10B10A2Unorm,  1, 1, 4,  4),
                ColorFormat.R10G10B10A2Uint   => new FormatInfo(Format.R10G10B10A2Uint,   1, 1, 4,  4),
                ColorFormat.R8G8B8A8Unorm     => new FormatInfo(Format.R8G8B8A8Unorm,     1, 1, 4,  4),
                ColorFormat.R8G8B8A8Srgb      => new FormatInfo(Format.R8G8B8A8Srgb,      1, 1, 4,  4),
                ColorFormat.R8G8B8X8Snorm     => new FormatInfo(Format.R8G8B8A8Snorm,     1, 1, 4,  4),
                ColorFormat.R8G8B8X8Sint      => new FormatInfo(Format.R8G8B8A8Sint,      1, 1, 4,  4),
                ColorFormat.R8G8B8X8Uint      => new FormatInfo(Format.R8G8B8A8Uint,      1, 1, 4,  4),
                ColorFormat.R16G16Unorm       => new FormatInfo(Format.R16G16Unorm,       1, 1, 4,  2),
                ColorFormat.R16G16Snorm       => new FormatInfo(Format.R16G16Snorm,       1, 1, 4,  2),
                ColorFormat.R16G16Sint        => new FormatInfo(Format.R16G16Sint,        1, 1, 4,  2),
                ColorFormat.R16G16Uint        => new FormatInfo(Format.R16G16Uint,        1, 1, 4,  2),
                ColorFormat.R16G16Float       => new FormatInfo(Format.R16G16Float,       1, 1, 4,  2),
                ColorFormat.R11G11B10Float    => new FormatInfo(Format.R11G11B10Float,    1, 1, 4,  3),
                ColorFormat.R32Sint           => new FormatInfo(Format.R32Sint,           1, 1, 4,  1),
                ColorFormat.R32Uint           => new FormatInfo(Format.R32Uint,           1, 1, 4,  1),
                ColorFormat.R32Float          => new FormatInfo(Format.R32Float,          1, 1, 4,  1),
                ColorFormat.B8G8R8X8Unorm     => new FormatInfo(Format.B8G8R8A8Unorm,     1, 1, 4,  4),
                ColorFormat.B8G8R8X8Srgb      => new FormatInfo(Format.B8G8R8A8Srgb,      1, 1, 4,  4),
                ColorFormat.B5G6R5Unorm       => new FormatInfo(Format.B5G6R5Unorm,       1, 1, 2,  3),
                ColorFormat.B5G5R5A1Unorm     => new FormatInfo(Format.B5G5R5A1Unorm,     1, 1, 2,  4),
                ColorFormat.R8G8Unorm         => new FormatInfo(Format.R8G8Unorm,         1, 1, 2,  2),
                ColorFormat.R8G8Snorm         => new FormatInfo(Format.R8G8Snorm,         1, 1, 2,  2),
                ColorFormat.R8G8Sint          => new FormatInfo(Format.R8G8Sint,          1, 1, 2,  2),
                ColorFormat.R8G8Uint          => new FormatInfo(Format.R8G8Uint,          1, 1, 2,  2),
                ColorFormat.R16Unorm          => new FormatInfo(Format.R16Unorm,          1, 1, 2,  1),
                ColorFormat.R16Snorm          => new FormatInfo(Format.R16Snorm,          1, 1, 2,  1),
                ColorFormat.R16Sint           => new FormatInfo(Format.R16Sint,           1, 1, 2,  1),
                ColorFormat.R16Uint           => new FormatInfo(Format.R16Uint,           1, 1, 2,  1),
                ColorFormat.R16Float          => new FormatInfo(Format.R16Float,          1, 1, 2,  1),
                ColorFormat.R8Unorm           => new FormatInfo(Format.R8Unorm,           1, 1, 1,  1),
                ColorFormat.R8Snorm           => new FormatInfo(Format.R8Snorm,           1, 1, 1,  1),
                ColorFormat.R8Sint            => new FormatInfo(Format.R8Sint,            1, 1, 1,  1),
                ColorFormat.R8Uint            => new FormatInfo(Format.R8Uint,            1, 1, 1,  1),
                ColorFormat.B5G5R5X1Unorm     => new FormatInfo(Format.B5G5R5X1Unorm,     1, 1, 2,  4),
                ColorFormat.R8G8B8X8Unorm     => new FormatInfo(Format.R8G8B8A8Unorm,     1, 1, 4,  4),
                ColorFormat.R8G8B8X8Srgb      => new FormatInfo(Format.R8G8B8A8Srgb,      1, 1, 4,  4),
                _                             => FormatInfo.Default
            };
        }
    }
}