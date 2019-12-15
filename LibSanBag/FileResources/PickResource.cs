using LibSanBag;
using LibSanBag.ResourceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class PickResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static PickResource Create(string version = "")
        {
            return new PickResource();
        }

        public class Pick
        {
            public uint Version { get; internal set; }
            public uint MeshBufferLength { get; internal set; }

            [JsonIgnore]
            public byte[] HavokShape { get; internal set; }
        }

        private Pick Read_PickResource(BinaryReader reader)
        {
            var result = new Pick();

            result.Version = ReadVersion(reader, 1, 0x1411E1F50);
            result.MeshBufferLength = reader.ReadUInt32();
            result.HavokShape = reader.ReadBytes((int)result.MeshBufferLength);

            return result;
        }

        public Pick Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_PickResource(reader);
            }
        }
    }
}
