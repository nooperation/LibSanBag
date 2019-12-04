using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.ResourceUtils;

namespace LibSanBag.FileResources
{
    public abstract class BaseFileResource
    {
        public abstract bool IsCompressed { get; }
        public int ResourceVersion { get; set; }

        public void InitFromRecord(Stream sourceStream, FileRecord fileRecord)
        {
            using (var compressedStream = new MemoryStream())
            {
                fileRecord.Save(sourceStream, compressedStream);
                InitFromStream(compressedStream);
            }
        }

        public void InitFromRawCompressed(byte[] compressedBytes)
        {
            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                InitFromStream(compressedStream);
            }
        }

        public void InitFromStream(Stream compressedStream)
        {
            byte[] decompressedBytes = null;
            compressedStream.Seek(0, SeekOrigin.Begin);

            if (IsCompressed)
            {
                try
                {
                    decompressedBytes = Unpacker.DecompressResource(compressedStream);
                    InitFromRawDecompressed(decompressedBytes);
                    return;
                }
                catch (Exception)
                {
                }
            }

            using (var decompressedStream = new MemoryStream())
            {
                compressedStream.CopyTo(decompressedStream);
                decompressedBytes = decompressedStream.ToArray();
            }
            InitFromRawDecompressed(decompressedBytes);
        }

        public abstract void InitFromRawDecompressed(byte[] decompressedBytes);

        private Dictionary<ulong, uint> versionMap = new Dictionary<ulong, uint>();
        internal uint ReadVersion(BinaryReader reader, uint defaultVersion, ulong? versionType)
        {
            if (versionType != null && versionMap.ContainsKey(versionType.Value))
            {
                return versionMap[versionType.Value];
            }

            var version = reader.ReadUInt32();

            if (versionType != null)
            {
                versionMap[versionType.Value] = version;
            }

            return version;
        }

        internal virtual string ReadString(BinaryReader decompressedStream)
        {
            var textLength = decompressedStream.ReadInt32();
            var text = new string(decompressedStream.ReadChars(textLength));

            return text;
        }

        internal T ReadComponent<T>(BinaryReader reader, Func<BinaryReader, T> func)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                return func(reader);
            }

            return default(T);
        }

        internal void ReadComponent(BinaryReader reader, Action<BinaryReader> func)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                func(reader);
            }
        }

    }
}
