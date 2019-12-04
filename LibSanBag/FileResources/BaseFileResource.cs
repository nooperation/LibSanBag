using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.ResourceUtils;
using Newtonsoft.Json;

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

        internal void OverrideVersionMap(Dictionary<ulong, uint> newVersionMap)
        {
            this.versionMap = newVersionMap;
        }

        protected Dictionary<ulong, uint> versionMap = new Dictionary<ulong, uint>();
        protected uint ReadVersion(BinaryReader reader, uint defaultVersion, ulong? versionType)
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

        protected string ReadString(BinaryReader decompressedStream)
        {
            var textLength = decompressedStream.ReadInt32();

            var rawTextBytes = decompressedStream.ReadBytes(textLength);
            var text = ASCIIEncoding.UTF8.GetString(rawTextBytes);

            return text;
        }

        protected string ReadString_VersionSafe(BinaryReader reader, uint version, int max_version)
        {
            if (version >= max_version)
            {
                return "";
            }

            return ReadString(reader);
        }

        protected T ReadComponent<T>(BinaryReader reader, Func<BinaryReader, T> func)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                return func(reader);
            }

            return default(T);
        }

        protected void ReadComponent(BinaryReader reader, Action<BinaryReader> func)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                func(reader);
            }
        }

        protected List<T> Read_List<T>(BinaryReader reader, Func<BinaryReader, T> func, uint currentVersion, ulong versionType)
        {
            List<T> result = new List<T>();

            var version = ReadVersion(reader, currentVersion, versionType);

            var count = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                var value = func(reader);
                result.Add(value);
            }

            return result;
        }

        protected List<float> ReadVectorF(BinaryReader reader, int dimensions)
        {
            var result = new List<float>();

            for (int i = 0; i < dimensions; i++)
            {
                var val = reader.ReadSingle();
                result.Add(val);
            }

            return result;
        }

        protected string ReadUUID(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x141196890);

            var item1 = reader.ReadUInt64();
            var item2 = reader.ReadUInt64();

            var left = BitConverter.GetBytes(item1);
            var right = BitConverter.GetBytes(item2);

            StringBuilder sb = new StringBuilder();
            for (int i = left.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", left[i]);
            }
            for (int i = right.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", right[i]);
            }

            var uuid = sb.ToString();
            return uuid;
        }

        public class Transform
        {
            public List<float> Q { get; set; }
            public List<float> T { get; set; }
        }
        protected Transform Read_Transform(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411A0F00);

            var q = ReadVectorF(reader, 4);
            var t = ReadVectorF(reader, 3);

            return new Transform()
            {
                Q = q,
                T = t
            };
        }

        public class AABB
        {
            public List<float> Min { get; set; }
            public List<float> Max { get; set; }
        }
        protected AABB Read_AABB(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141205310);

            var min = ReadVectorF(reader, 4);
            var max = ReadVectorF(reader, 4);

            return new AABB()
            {
                Min = min,
                Max = max
            };
        }

        protected byte[] Read_Array(BinaryReader reader)
        {
            var dataLength = reader.ReadInt32();
            var data = reader.ReadBytes(dataLength);

            return data;
        }

        protected List<List<float>> Read_RotationMatrix(BinaryReader reader, int dimension = 3)
        {
            var version = ReadVersion(reader, 1, 0x14119F1D0);

            var matrix = new List<List<float>>();
            for (int rowIndex = 0; rowIndex < dimension; rowIndex++)
            {
                var row = new List<float>();
                for (int colIndex = 0; colIndex < dimension; colIndex++)
                {
                    var col = reader.ReadSingle();
                    row.Add(col);
                }
                matrix.Add(row);
            }

            return matrix;
        }
    }
}
