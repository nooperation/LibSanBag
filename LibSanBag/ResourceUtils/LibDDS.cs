using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using LibSanBag.Providers;

namespace LibSanBag.ResourceUtils
{
    public static class LibDDS
    {
        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct ConversionOptions
        {
            public enum CodecType
            {
                CODEC_DDS,
                CODEC_BMP = 1,     // Windows Bitmap (.bmp)
                CODEC_JPEG,        // Joint Photographic Experts Group (.jpg, .jpeg)
                CODEC_PNG,         // Portable Network Graphics (.png)
                CODEC_TIFF,        // Tagged Image File Format  (.tif, .tiff)
                CODEC_GIF,         // Graphics Interchange Format  (.gif)
                CODEC_WMP,         // Windows Media Photo / HD Photo / JPEG XR (.hdp, .jxr, .wdp)
                CODEC_ICO,         // Windows Icon (.ico)
            }

            public enum DXGI_FORMAT
            {
                DXGI_FORMAT_UNKNOWN = 0,
                DXGI_FORMAT_R32G32B32A32_TYPELESS = 1,
                DXGI_FORMAT_R32G32B32A32_FLOAT = 2,
                DXGI_FORMAT_R32G32B32A32_UINT = 3,
                DXGI_FORMAT_R32G32B32A32_SINT = 4,
                DXGI_FORMAT_R32G32B32_TYPELESS = 5,
                DXGI_FORMAT_R32G32B32_FLOAT = 6,
                DXGI_FORMAT_R32G32B32_UINT = 7,
                DXGI_FORMAT_R32G32B32_SINT = 8,
                DXGI_FORMAT_R16G16B16A16_TYPELESS = 9,
                DXGI_FORMAT_R16G16B16A16_FLOAT = 10,
                DXGI_FORMAT_R16G16B16A16_UNORM = 11,
                DXGI_FORMAT_R16G16B16A16_UINT = 12,
                DXGI_FORMAT_R16G16B16A16_SNORM = 13,
                DXGI_FORMAT_R16G16B16A16_SINT = 14,
                DXGI_FORMAT_R32G32_TYPELESS = 15,
                DXGI_FORMAT_R32G32_FLOAT = 16,
                DXGI_FORMAT_R32G32_UINT = 17,
                DXGI_FORMAT_R32G32_SINT = 18,
                DXGI_FORMAT_R32G8X24_TYPELESS = 19,
                DXGI_FORMAT_D32_FLOAT_S8X24_UINT = 20,
                DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS = 21,
                DXGI_FORMAT_X32_TYPELESS_G8X24_UINT = 22,
                DXGI_FORMAT_R10G10B10A2_TYPELESS = 23,
                DXGI_FORMAT_R10G10B10A2_UNORM = 24,
                DXGI_FORMAT_R10G10B10A2_UINT = 25,
                DXGI_FORMAT_R11G11B10_FLOAT = 26,
                DXGI_FORMAT_R8G8B8A8_TYPELESS = 27,
                DXGI_FORMAT_R8G8B8A8_UNORM = 28,
                DXGI_FORMAT_R8G8B8A8_UNORM_SRGB = 29,
                DXGI_FORMAT_R8G8B8A8_UINT = 30,
                DXGI_FORMAT_R8G8B8A8_SNORM = 31,
                DXGI_FORMAT_R8G8B8A8_SINT = 32,
                DXGI_FORMAT_R16G16_TYPELESS = 33,
                DXGI_FORMAT_R16G16_FLOAT = 34,
                DXGI_FORMAT_R16G16_UNORM = 35,
                DXGI_FORMAT_R16G16_UINT = 36,
                DXGI_FORMAT_R16G16_SNORM = 37,
                DXGI_FORMAT_R16G16_SINT = 38,
                DXGI_FORMAT_R32_TYPELESS = 39,
                DXGI_FORMAT_D32_FLOAT = 40,
                DXGI_FORMAT_R32_FLOAT = 41,
                DXGI_FORMAT_R32_UINT = 42,
                DXGI_FORMAT_R32_SINT = 43,
                DXGI_FORMAT_R24G8_TYPELESS = 44,
                DXGI_FORMAT_D24_UNORM_S8_UINT = 45,
                DXGI_FORMAT_R24_UNORM_X8_TYPELESS = 46,
                DXGI_FORMAT_X24_TYPELESS_G8_UINT = 47,
                DXGI_FORMAT_R8G8_TYPELESS = 48,
                DXGI_FORMAT_R8G8_UNORM = 49,
                DXGI_FORMAT_R8G8_UINT = 50,
                DXGI_FORMAT_R8G8_SNORM = 51,
                DXGI_FORMAT_R8G8_SINT = 52,
                DXGI_FORMAT_R16_TYPELESS = 53,
                DXGI_FORMAT_R16_FLOAT = 54,
                DXGI_FORMAT_D16_UNORM = 55,
                DXGI_FORMAT_R16_UNORM = 56,
                DXGI_FORMAT_R16_UINT = 57,
                DXGI_FORMAT_R16_SNORM = 58,
                DXGI_FORMAT_R16_SINT = 59,
                DXGI_FORMAT_R8_TYPELESS = 60,
                DXGI_FORMAT_R8_UNORM = 61,
                DXGI_FORMAT_R8_UINT = 62,
                DXGI_FORMAT_R8_SNORM = 63,
                DXGI_FORMAT_R8_SINT = 64,
                DXGI_FORMAT_A8_UNORM = 65,
                DXGI_FORMAT_R1_UNORM = 66,
                DXGI_FORMAT_R9G9B9E5_SHAREDEXP = 67,
                DXGI_FORMAT_R8G8_B8G8_UNORM = 68,
                DXGI_FORMAT_G8R8_G8B8_UNORM = 69,
                DXGI_FORMAT_BC1_TYPELESS = 70,
                DXGI_FORMAT_BC1_UNORM = 71,
                DXGI_FORMAT_BC1_UNORM_SRGB = 72,
                DXGI_FORMAT_BC2_TYPELESS = 73,
                DXGI_FORMAT_BC2_UNORM = 74,
                DXGI_FORMAT_BC2_UNORM_SRGB = 75,
                DXGI_FORMAT_BC3_TYPELESS = 76,
                DXGI_FORMAT_BC3_UNORM = 77,
                DXGI_FORMAT_BC3_UNORM_SRGB = 78,
                DXGI_FORMAT_BC4_TYPELESS = 79,
                DXGI_FORMAT_BC4_UNORM = 80,
                DXGI_FORMAT_BC4_SNORM = 81,
                DXGI_FORMAT_BC5_TYPELESS = 82,
                DXGI_FORMAT_BC5_UNORM = 83,
                DXGI_FORMAT_BC5_SNORM = 84,
                DXGI_FORMAT_B5G6R5_UNORM = 85,
                DXGI_FORMAT_B5G5R5A1_UNORM = 86,
                DXGI_FORMAT_B8G8R8A8_UNORM = 87,
                DXGI_FORMAT_B8G8R8X8_UNORM = 88,
                DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM = 89,
                DXGI_FORMAT_B8G8R8A8_TYPELESS = 90,
                DXGI_FORMAT_B8G8R8A8_UNORM_SRGB = 91,
                DXGI_FORMAT_B8G8R8X8_TYPELESS = 92,
                DXGI_FORMAT_B8G8R8X8_UNORM_SRGB = 93,
                DXGI_FORMAT_BC6H_TYPELESS = 94,
                DXGI_FORMAT_BC6H_UF16 = 95,
                DXGI_FORMAT_BC6H_SF16 = 96,
                DXGI_FORMAT_BC7_TYPELESS = 97,
                DXGI_FORMAT_BC7_UNORM = 98,
                DXGI_FORMAT_BC7_UNORM_SRGB = 99,
                DXGI_FORMAT_AYUV = 100,
                DXGI_FORMAT_Y410 = 101,
                DXGI_FORMAT_Y416 = 102,
                DXGI_FORMAT_NV12 = 103,
                DXGI_FORMAT_P010 = 104,
                DXGI_FORMAT_P016 = 105,
                DXGI_FORMAT_420_OPAQUE = 106,
                DXGI_FORMAT_YUY2 = 107,
                DXGI_FORMAT_Y210 = 108,
                DXGI_FORMAT_Y216 = 109,
                DXGI_FORMAT_NV11 = 110,
                DXGI_FORMAT_AI44 = 111,
                DXGI_FORMAT_IA44 = 112,
                DXGI_FORMAT_P8 = 113,
                DXGI_FORMAT_A8P8 = 114,
                DXGI_FORMAT_B4G4R4A4_UNORM = 115,

                DXGI_FORMAT_P208 = 130,
                DXGI_FORMAT_V208 = 131,
                DXGI_FORMAT_V408 = 132,

                DXGI_FORMAT_FORCE_UINT = -1
            }

            public static ConversionOptions Default
            {
                get
                {
                    ConversionOptions defaultOptions;
                    defaultOptions.Width = 0;
                    defaultOptions.Height = 0;
                    defaultOptions.Codec = CodecType.CODEC_PNG;
                    defaultOptions.Format = DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;

                    return defaultOptions;
                }
            }

            internal UInt64 Width;
            internal UInt64 Height;
            internal CodecType Codec;
            internal DXGI_FORMAT Format;
        }

        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct ImageProperties
        {
            UInt64 width;
            UInt64 height;
            ConversionOptions.DXGI_FORMAT format;
        }

        [DllImport("LibDDS.dll")]
        private static extern bool ConvertDdsInMemory(
            byte[] ddsBytes,
            long ddsBytesSize,
            ConversionOptions options,
            out IntPtr outImageBytes,
            out long outImageBytesSize,
            out ImageProperties originalImageProperties
        );

        [DllImport("LibDDS.dll")]
        private static extern IntPtr GetError();

        [DllImport("LibDDS.dll")]
        private static extern void FreeMemory(IntPtr data);

        /// <summary>
        /// Determines if LibDDS is available
        /// </summary>
        public static bool IsAvailable { get; private set; }

        /// <summary>
        /// Attempts to locate all of the dependencies required by LibDDS
        /// </summary>
        /// <param name="fileProvider">File provider</param>
        /// <returns>True if LibDDS is available, otherwise false</returns>
        public static bool FindDependencies(IFileProvider fileProvider)
        {
            IsAvailable = fileProvider.FileExists("LibDDS.dll");
            return IsAvailable;
        }

        static LibDDS()
        {
            FindDependencies(new FileProvider());
        }

        /// <summary>
        /// Resturns the last error encountered by LibDDS
        /// </summary>
        /// <returns>Error string</returns>
        private static string GetErrorString()
        {
            try
            {
                var rawString = GetError();
                var errorString = Marshal.PtrToStringAnsi(rawString);
                return errorString;
            }
            catch (Exception ex)
            {
                return "Failed to get error string: " + ex.Message;
            }
        }

        /// <summary>
        /// Converts a DDS to a different resolution, codec, or format
        /// </summary>
        /// <param name="ddsBytes">DDS bytes</param>
        /// <param name="width">New image width or 0 for original image width</param>
        /// <param name="height">New image height or 0 for original image height</param>
        /// <param name="codec">Type of image to convert DDS to</param>
        /// <param name="format">Color format</param>
        /// <returns>Converted image bytes</returns>
        public static byte[] GetImageBytesFromDds(
            byte[] ddsBytes,
            UInt64 width = 0,
            UInt64 height = 0,
            ConversionOptions.CodecType codec = LibDDS.ConversionOptions.CodecType.CODEC_JPEG,
            ConversionOptions.DXGI_FORMAT format = ConversionOptions.DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM)
        {
            if (IsAvailable == false)
            {
                throw new Exception("LibDDS is not available");
            }

            IntPtr rawImageData;
            long rawImageDataSize;

            var options = ConversionOptions.Default;
            options.Width = width;
            options.Height = height;
            options.Format = format;
            options.Codec = codec;

            ImageProperties originalImageProperties;
            var result = ConvertDdsInMemory(ddsBytes, ddsBytes.Length, options, out rawImageData, out rawImageDataSize, out originalImageProperties);
            if (result == false)
            {
                throw new Exception("Failed to read DDS: " + GetErrorString());
            }

            var imageData = new byte[rawImageDataSize];
            Marshal.Copy(rawImageData, imageData, 0, (int)rawImageDataSize);

            FreeMemory(rawImageData);

            return imageData;
        }
    }
}
