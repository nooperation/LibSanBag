using System;
using System.Runtime.InteropServices;
using LibSanBag.Providers;

namespace LibSanBag.ResourceUtils
{
    internal class LibOodle6 : LibOodleBase
    {
        [DllImport("oo2core_6_win64.dll")]
        private static extern ulong OodleLZ_Compress(int codec, byte[] buffer, ulong bufferLength, IntPtr output, int level, IntPtr a, IntPtr b, IntPtr c);

        [DllImport("oo2core_6_win64.dll")]
        private static extern ulong OodleLZ_Decompress(byte[] compressed, ulong compressedSize, byte[] decompressed, ulong decompressedSize, int a, int b, int c, IntPtr d, IntPtr e, IntPtr f, IntPtr g, IntPtr h, IntPtr i, int j);

        public static bool FindDependencies(IFileProvider fileProvider)
        {
            return fileProvider.FileExists("oo2core_6_win64.dll");
        }

        public override ulong Compress(int codec, byte[] buffer, ulong bufferLength, IntPtr output, int level, IntPtr a, IntPtr b, IntPtr c) 
            => OodleLZ_Compress(codec, buffer, bufferLength, output, level, a, b, c);
        public override ulong Decompress(byte[] compressed, ulong compressedSize, byte[] decompressed, ulong decompressedSize, int a, int b, int c, IntPtr d, IntPtr e, IntPtr f, IntPtr g, IntPtr h, IntPtr i, int j) 
            => OodleLZ_Decompress(compressed, compressedSize, decompressed, decompressedSize, a, b, c, d, e, f, g, h, i, j);
    }
}
