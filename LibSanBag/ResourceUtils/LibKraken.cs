using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;

namespace LibSanBag.ResourceUtils
{
    static class LibKraken
    {
        [DllImport("LibKraken.dll")]
        private static extern ulong Kraken_Decompress(byte[] compressed, ulong compressedSize, byte[] decompressed, ulong decompressedSize);

        private static bool _isDllAvailable;
        public static bool IsAvailable => _isDllAvailable;

        static LibKraken()
        {
            FindDependencies(new FileProvider());
        }

        /// <summary>
        /// Attempts to locate all of the dependencies required by LibKraken
        /// </summary>
        /// <returns>True if dependencies were found, otherwise false</returns>
        public static bool FindDependencies(IFileProvider fileProvider)
        {
            _isDllAvailable = false;
            if (fileProvider.FileExists("LibKraken.dll"))
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
                throw new Exception("LibKraken is not available");
            }

            var decompressedBuffer = new byte[decompressedSize];

            var result = Kraken_Decompress(
                compressedData,
                (ulong)compressedData.Length,
                decompressedBuffer,
                decompressedSize);

            // Remove the 4 byte compression header
            var fixedArray = new byte[decompressedSize - 4];
            Buffer.BlockCopy(decompressedBuffer, 4, fixedArray, 0, fixedArray.Length);
            return fixedArray;
        }
    }
}
