using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ResourceUtils
{
    static class OodleLz
    {
        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_GetCompressedBufferSizeNeeded(ulong size);

        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_Compress(int codec, byte[] buffer, ulong bufferLength, IntPtr output, int level, IntPtr a, IntPtr b, IntPtr c);

        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_Decompress(byte[] compressed, ulong compressedSize, byte[] decompressed, ulong decompressedSize, int a, int b, int c, IntPtr d, IntPtr e, IntPtr f, IntPtr g, IntPtr h, IntPtr i, int j);

        public static byte[] Decompress(byte[] compressedData)
        {
            var decompressedSize = BitConverter.ToUInt32(compressedData, 12) + 8;
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

        public static byte[] DecompressTextureResource(Stream textureResourceStream)
        {
            textureResourceStream.Seek(0, SeekOrigin.Begin);

            using (var br = new BinaryReader(textureResourceStream, Encoding.ASCII, true))
            {
                var foundCompressedDataHeader = false;

                for (int i = 0; i < 16 && textureResourceStream.Position < textureResourceStream.Length; i++)
                {
                    var currentByte = br.ReadByte();
                    if (currentByte == 0x8C)
                    {
                        --textureResourceStream.Position;
                        foundCompressedDataHeader = true;
                        break;
                    }
                }

                if (foundCompressedDataHeader == false)
                {
                    throw new Exception("Failed to find compressed header");
                }

                var compressedDataArray = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position - 10));
                return Decompress(compressedDataArray);
            }
        }

        public static byte[] DecompressTextureResource(string targetPath)
        {
            using (var textureResourceStream = File.OpenRead(targetPath))
            {
                return DecompressTextureResource(textureResourceStream);
            }
        }
    }
}
