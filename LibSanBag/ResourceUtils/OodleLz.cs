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
    public static class OodleLz
    {
        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_GetCompressedBufferSizeNeeded(ulong size);

        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_Compress(int codec, byte[] buffer, ulong bufferLength, IntPtr output, int level, IntPtr a, IntPtr b, IntPtr c);

        [DllImport("oo2core_1_win64.dll")]
        private static extern ulong OodleLZ_Decompress(byte[] compressed, ulong compressedSize, byte[] decompressed, ulong decompressedSize, int a, int b, int c, IntPtr d, IntPtr e, IntPtr f, IntPtr g, IntPtr h, IntPtr i, int j);

        private static bool _isDllAvailable;
        public static bool IsAvailable => _isDllAvailable;

        /// <summary>
        /// Sets up the current environment in attempt to find sansar's oodle dll
        /// </summary>
        /// <returns>True if oodle was found, otherwise false</returns>
        public static bool SetupEnvironment(IFileProvider fileProvider, IEnvironmentProvider environmentProvider, IRegistryProvider registryProvider)
        {
            _isDllAvailable = false;

            try
            {
                if (fileProvider.FileExists("oo2core_1_win64.dll"))
                {
                    _isDllAvailable = true;
                    return true;
                }

                var sansarDirectory = ResourceUtils.Utils.GetSansarDirectory(registryProvider);
                if (sansarDirectory != null)
                {
                    sansarDirectory = Path.GetFullPath(sansarDirectory + "\\Client");

                    if (fileProvider.FileExists(sansarDirectory + "\\oo2core_1_win64.dll"))
                    {
                        var currentPath = environmentProvider.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
                        environmentProvider.SetEnvironmentVariable("PATH", currentPath + ";" + sansarDirectory, EnvironmentVariableTarget.Process);
                        _isDllAvailable = true;
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
             SetupEnvironment(new FileProvider(), new EnvironmentProvider(), new RegistryProvider());
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

        /// <summary>
        /// Decompresses a sansar resource/asset
        /// </summary>
        /// <param name="resourceStream">Stream containing sansar resource</param>
        /// <returns>Raw decompressed resource data on success</returns>
        /// <exception cref="Exception">Failed to decompress</exception>
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
                else if (firstByte == 0x01 || firstByte == 0x02 || firstByte == 0x03)
                {
                    br.BaseStream.Seek(3, SeekOrigin.Current);

                    var remainingBytes = br.BaseStream.Length - br.BaseStream.Position;
                    return br.ReadBytes((int)remainingBytes);
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

        /// <summary>
        /// Decompresses a sansar resource/asset
        /// </summary>
        /// <param name="resourcePath">Path to a sansar resource</param>
        /// <returns>Raw decompressed resource data on success</returns>
        /// <exception cref="Exception">Failed to decompress</exception>
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
