using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.ResourceUtils
{
    public static class OodleLz
    {
        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_GetCompressedBufferSizeNeeded(ulong size);

        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_Compress(int codec, byte[] buffer, ulong bufferLength, IntPtr output, int level, IntPtr a, IntPtr b, IntPtr c);

        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_Decompress(byte[] compressed, ulong compressedSize, byte[] decompressed, ulong decompressedSize, int a, int b, int c, IntPtr d, IntPtr e, IntPtr f, IntPtr g, IntPtr h, IntPtr i, int j);

        private static bool _isDllAvailable = false;
        public static bool IsAvailable => _isDllAvailable;

        private static bool SetupEnvironment()
        {
            try
            {
                if (File.Exists("oo2core_1_win64.dll"))
                {
                    return true;
                }

                var sansarDirectory = ResourceUtils.Utils.GetSansarDirectory();
                if (sansarDirectory != null)
                {
                    sansarDirectory = Path.GetFullPath(sansarDirectory + "\\Client");
                    var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);

                    if (File.Exists(sansarDirectory + "\\oo2core_1_win64.dll"))
                    {
                        Environment.SetEnvironmentVariable("PATH", currentPath + ";" + sansarDirectory, EnvironmentVariableTarget.Process);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        static OodleLz()
        {
            _isDllAvailable = SetupEnvironment();
        }

        public static byte[] Decompress(byte[] compressedData, ulong decompressedSize)
        {
            if (IsAvailable == false)
            {
                return null;
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

            var fixedArray = new byte[decompressedSize - 8];
            Buffer.BlockCopy(decompressedBuffer, 8, fixedArray, 0, fixedArray.Length);
            return fixedArray;
        }

        public static byte[] DecompressResource(Stream resourceStream)
        {
            resourceStream.Seek(0, SeekOrigin.Begin);

            using (var br = new BinaryReader(resourceStream, Encoding.ASCII, true))
            {
                var firstByte = br.ReadByte();
                var decompressedSize = (ulong)0;

                if (firstByte == 0xF1)
                {
                    decompressedSize = br.ReadUInt16();
                }
                else if (firstByte == 0xF2)
                {
                    decompressedSize = br.ReadUInt32();
                }
                else if (firstByte == 0x01)
                {
                    br.BaseStream.Seek(3, SeekOrigin.Current);
                    decompressedSize = br.ReadUInt32();
                    return br.ReadBytes((int)decompressedSize);
                }
                else
                {
                    throw new Exception($"Unknown header. Expected 0x01, 0xF1 or 0xF2, but found 0x{firstByte:X2}");
                }

                var expectedOodleMagicByte = br.ReadByte();
                br.BaseStream.Position--;

                if (expectedOodleMagicByte != 0x8C)
                {
                    throw new Exception($"Failed to find oodle magic. Expected 0x8C, but found 0x{expectedOodleMagicByte:X2}");
                }

                var compressedDataArray = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position - 10));
                var decompressed = Decompress(compressedDataArray, decompressedSize);

                return decompressed;
            }
        }

        public static byte[] DecompressResource(string resourcePath)
        {
            using (var textureResourceStream = File.OpenRead(resourcePath))
            {
                var decompressed = DecompressResource(textureResourceStream);
                return decompressed;
            }
        }
    }
}
