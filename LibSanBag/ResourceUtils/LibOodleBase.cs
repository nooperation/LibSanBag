using System;
using LibSanBag.Providers;

namespace LibSanBag.ResourceUtils
{
    public abstract class LibOodleBase
    {
        public abstract ulong Compress(int codec, byte[] buffer, ulong bufferLength, IntPtr output, int level, IntPtr a, IntPtr b, IntPtr c) ;
        public abstract ulong Decompress(byte[] compressed, ulong compressedSize, byte[] decompressed, ulong decompressedSize, int a, int b, int c, IntPtr d, IntPtr e, IntPtr f, IntPtr g, IntPtr h, IntPtr i, int j);

        public static LibOodleBase CreateLibOodle(IFileProvider fileProvider)
        {
            if(LibOodle7.FindDependencies(fileProvider))
            {
                return new LibOodle7();
            }
            if(LibOodle6.FindDependencies(fileProvider))
            {
                return new LibOodle6();
            }

            return null;
        }

        /// <summary>
        /// Decompresses an oodle compressed buffer
        /// </summary>
        /// <param name="compressedData">Oodle compressed data</param>
        /// <param name="decompressedSize">Expected size of the compressed data</param>
        /// <returns>Decompressed data on success</returns>
        /// <exception cref="Exception">Failed to decompress</exception>
        public virtual byte[] Decompress(byte[] compressedData, ulong decompressedSize)
        {
            var decompressedBuffer = new byte[decompressedSize];

            // TODO: This first call to compress was needed long ago for some reason, decompress would fail otherwise with oodle2
            var x = Compress(0, null, 0, IntPtr.Zero, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            var result = Decompress(
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
