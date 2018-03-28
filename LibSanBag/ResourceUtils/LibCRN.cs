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
        public enum ImageCodec
        {
            DDS = 0,
            CRN,
            KTX,
            TGA,
            PNG,
            JPG,
            JPEG,
            BMP,
            GIF,
            TIF,
            TIFF,
            PPM,
            PGM,
            PSD,
            JP2
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConversionOptions
        {
            public ImageCodec Codec;
        }

        [DllImport("LibCRN.dll")]
        private static extern bool ConvertCrnInMemory(
            byte[] ddsBytes,
            long ddsBytesSize,
            ConversionOptions options,
            out IntPtr outImageBytes, 
            out long outImageBytesSize
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
        /// Converts a CRN to a different type in memory
        /// </summary>
        /// <param name="crnBytes">CRN bytes</param>
        /// <param name="codec">Image Codec</param>
        /// <returns>Converted image bytes</returns>
        public static byte[] GetImageBytesFromCRN(byte[] crnBytes, ImageCodec codec)
        {
            if (IsAvailable == false)
            {
                throw new Exception("LibCRN is not available");
            }

            IntPtr rawImageData;
            long rawImageDataSize;
            ConversionOptions options = new ConversionOptions();
            options.Codec = codec;

            var result = ConvertCrnInMemory(crnBytes, crnBytes.Length, options, out rawImageData, out rawImageDataSize);
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
