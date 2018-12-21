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
    static class LibOodle
    {
        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_GetCompressedBufferSizeNeeded(ulong size);

        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_Compress(int codec, byte[] buffer, ulong bufferLength, IntPtr output, int level, IntPtr a, IntPtr b, IntPtr c);

        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_Decompress(byte[] compressed, ulong compressedSize, byte[] decompressed, ulong decompressedSize, int a, int b, int c, IntPtr d, IntPtr e, IntPtr f, IntPtr g, IntPtr h, IntPtr i, int j);

        private static bool _isDllAvailable;
        public static bool IsAvailable => _isDllAvailable;

        static LibOodle()
        {
            FindDependencies(new FileProvider());
        }

        /// <summary>
        /// Attempts to locate all of the dependencies required by OodleLz
        /// </summary>
        /// <returns>True if dependencies were found, otherwise false</returns>
        public static bool FindDependencies(IFileProvider fileProvider)
        {
            _isDllAvailable = false;
            if (fileProvider.FileExists("oo2core_1_win64.dll"))
            {
                _isDllAvailable = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Decompresses an oodle compressed buffer
        /// </summary>
        /// <param name="compressedData">Oodle compressed data</param>
        /// <param name="decompressedSize">Expected size of the compressed data</param>
        /// <returns>Decompressed data on success</returns>
        /// <exception cref="Exception">Failed to decompress</exception>
        public static byte[] Decompress(byte[] compressedData, ulong decompressedSize)
        {
            if (IsAvailable == false)
            {
                throw new Exception("OodleLz is not available");
            }

            var decompressedBuffer = new byte[decompressedSize];

            var x = OodleLZ_Compress(0, null, 0, IntPtr.Zero, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            var result = OodleLZ_Decompress(
                compressedData,
                (ulong)compressedData.Length,
                decompressedBuffer,
                decompressedSize,
                0,
                0,
                0,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0);

            // Remove the 4 byte compression header
            var fixedArray = new byte[decompressedSize - 4];
            Buffer.BlockCopy(decompressedBuffer, 4, fixedArray, 0, fixedArray.Length);
            return fixedArray;
        }
    }
}
