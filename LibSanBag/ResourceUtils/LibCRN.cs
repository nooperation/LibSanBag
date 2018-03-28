using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;

namespace LibSanBag.ResourceUtils
{
    public static class LibCRN
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ConversionOptions
        {
            internal int Width;
            internal int Height;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct ImageProperties
        {
            uint width;
            uint height;
        };

        [DllImport("LibCRN.dll")]
        private static extern bool ConvertCRNInMemory(
            byte[] ddsBytes,
            long ddsBytesSize,
            ConversionOptions options,
            out IntPtr outImageBytes, 
            out long outImageBytesSize,
            out ImageProperties outImageProperties
        );

        [DllImport("LibCRN.dll")]
        private static extern IntPtr GetError();

        [DllImport("LibCRN.dll")]
        private static extern void FreeMemory(IntPtr data);


        private static bool _isDllAvailable;

        /// <summary>
        /// Determines if LibCRN is available
        /// </summary>
        public static bool IsAvailable => _isDllAvailable;

        /// <summary>
        /// Attempts to locate all of the dependencies required by LibCRN
        /// </summary>
        /// <param name="fileProvider">File provider</param>
        /// <returns>True if LibCRN is available, otherwise false</returns>
        public static bool FindDependencies(IFileProvider fileProvider)
        {
            _isDllAvailable = fileProvider.FileExists("LibCRN.dll");
            return _isDllAvailable;
        }

        static LibCRN()
        {
            FindDependencies(new FileProvider());
        }

        /// <summary>
        /// Resturns the last error encountered by LibCRN
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
        /// Converts a CRN to a different resolution, codec, or format
        /// </summary>
        /// <param name="crnBytes">CRN bytes</param>
        /// <returns>Converted image bytes</returns>
        public static byte[] GetImageBytesFromCRN(byte[] crnBytes)
        {
            if (IsAvailable == false)
            {
                throw new Exception("LibCRN is not available");
            }

            IntPtr rawImageData;
            long rawImageDataSize;
            ConversionOptions options = new ConversionOptions();
            ImageProperties imageProperties;

            var result = ConvertCRNInMemory(crnBytes, crnBytes.Length, options, out rawImageData, out rawImageDataSize, out imageProperties);
            if (result == false)
            {
                throw new Exception("Failed to read CRN: " + GetErrorString());
            }

            var imageData = new byte[rawImageDataSize];
            Marshal.Copy(rawImageData, imageData, 0, (int)rawImageDataSize);

            FreeMemory(rawImageData);

            return imageData;
        }
    }
}
