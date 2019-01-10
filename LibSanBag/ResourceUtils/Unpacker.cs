using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using LibSanBag.Providers;

namespace LibSanBag.ResourceUtils
{
    public static class Unpacker
    {
        public static bool IsAvailable { get; private set; }
        private static LibOodleBase _libOodle = null;

        static Unpacker()
        {
            IsAvailable = FindDependencies(new FileProvider());
        }

        /// <summary>
        /// Attempts to locate all of the dependencies required by unpacker
        /// </summary>
        /// <returns>True if dependencies were found, otherwise false</returns>
        public static bool FindDependencies(IFileProvider fileProvider)
        {
            _libOodle = LibOodleBase.CreateLibOodle(fileProvider);
            if(_libOodle == null)
            {
                IsAvailable = false;
                return false;
            }

            IsAvailable = true;
            return IsAvailable;
        }

        /// <summary>
        /// Decompresses a sansar resource/asset
        /// </summary>
        /// <param name="resourceStream">Stream containing sansar resource</param>
        /// <returns>Raw decompressed resource data on success</returns>
        /// <exception cref="NotImplementedException">Unsupported compression</exception>
        /// <exception cref="Exception">Failed to decompress</exception>
        public static byte[] DecompressResource(Stream resourceStream)
        {
            if(!IsAvailable)
            {
                throw new Exception("No decompression libraries found.");
            }

            resourceStream.Seek(0, SeekOrigin.Begin);

            using (var br = new BinaryReader(resourceStream, Encoding.ASCII, true))
            {
                var firstByte = br.ReadByte();
                ulong decompressedSize;

                if (firstByte == 0xF1)
                {
                    decompressedSize = br.ReadUInt16();
                }
                else if (firstByte == 0xF2)
                {
                    decompressedSize = br.ReadUInt32();
                }
                else if (firstByte > 0xF2)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    br.BaseStream.Seek(3, SeekOrigin.Current);

                    var remainingBytes = br.BaseStream.Length - br.BaseStream.Position - 10;
                    return br.ReadBytes((int)remainingBytes);
                }

                var expectedOodleMagicByte = br.ReadByte();
                br.BaseStream.Position--;

                if (expectedOodleMagicByte != 0x8C)
                {
                    throw new Exception($"Failed to find oodle magic. Expected 0x8C, but found 0x{expectedOodleMagicByte:X2}");
                }

                var compressedDataArray = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position - 10));

                var decompressed = _libOodle.Decompress(compressedDataArray, decompressedSize);
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
