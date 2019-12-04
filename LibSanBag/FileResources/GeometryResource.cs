using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class GeometryResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static GeometryResource Create(string version = "")
        {
            return new GeometryResource();
        }

        public class GeometryType
        {
            public uint Version { get; set; }
            public string Name { get; set; }
            public uint Format { get; set; }
        }
        private GeometryType Read_GeometryType(BinaryReader reader)
        {
            var result = new GeometryType();

            result.Version = ReadVersion(reader, 1, 0x1411FD900);
            result.Name = ReadString(reader);
            result.Format = reader.ReadUInt32();

            return result;
        }

        public class VertexStream
        {
            public uint Version { get; set; }
            public GeometryType Type { get; set; }

            [JsonIgnore]
            public byte[] Data { get; set; }
            public int DataLength => Data?.Length ?? 0;
        }
        private VertexStream Read_VertexStream(BinaryReader reader)
        {
            var result = new VertexStream();

            result.Version = ReadVersion(reader, 1, 0x1411E4EF0);

            result.Type = Read_GeometryType(reader);
            result.Data = Read_Array(reader);

            return result;
        }

        public class GeometryComponent
        {
            public uint Version { get; set; }
            public uint IndexCount { get; set; }
            public uint VertextCount { get; set; }
            public uint IndexFormat { get; set; }
            [JsonIgnore]
            public byte[] IndexData { get; set; }
            public int IndexDataLength => IndexData?.Length ?? 0;
            public List<VertexStream> VertexStreams{ get; set; }
        }
        private GeometryComponent Read_GeometryComponent(BinaryReader reader)
        {
            var result = new GeometryComponent();

            result.Version = ReadVersion(reader, 1, 0x14120B5A0);

            result.IndexCount = reader.ReadUInt32();
            result.VertextCount = reader.ReadUInt32();
            result.IndexFormat = reader.ReadUInt32(); 

            var indexDataLength = reader.ReadInt32();
            result.IndexData = reader.ReadBytes(indexDataLength);
            result.VertexStreams = Read_List(reader, Read_VertexStream, 1, 0x1411D9B70);

            return result;
        }

        public uint Version { get; set; }
        public List<AABB> ArticulatedBounds { get; set; }
        public List<Transform> InverseBindPose { get; set; }
        public AABB Bounds { get; set; }
        public GeometryComponent GeometryData { get; set; }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Version = ReadVersion(reader, 1, 0x1411D9B90); // C303A54169058B48

                this.ArticulatedBounds = Read_List(reader, Read_AABB, 1, 0x141205430);
                this.InverseBindPose = Read_List(reader, Read_Transform, 1, 0x1411F3EA0);
                this.Bounds = Read_AABB(reader);

                this.GeometryData = ReadComponent(reader, Read_GeometryComponent);
            }
        }
    }
}
