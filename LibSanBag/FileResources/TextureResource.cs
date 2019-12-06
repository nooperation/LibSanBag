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
    public class TextureResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static TextureResource Create(string version = "")
        {
            return new TextureResource();
        }

        public class TextureMip
        {
            public uint Version { get; set; }
            public string TextureMipResource { get; set; }
        }
        private TextureMip Read_MipLevels(BinaryReader reader)
        {
            var result = new TextureMip();

            result.Version = ReadVersion(reader, 1, 0x1411D0CF0);
            result.TextureMipResource = ReadUUID(reader);

            return result;
        }

        public class Texture
        {
            public uint Version { get; set; }

            [JsonIgnore]
            public byte[] Data { get; set; }
            public int DataLength => Data?.Length ?? 0;
            public List<TextureMip> MipLevels { get; set; }
        }
        private Texture Read_TextureResource(BinaryReader reader)
        {
            var result = new Texture();

            result.Version = ReadVersion(reader, 4, 0x1411A06D0);
            result.Data = Read_Array(reader);

            if (result.Version >= 3)
            {
                result.MipLevels = Read_List(reader, Read_MipLevels, 1, 0x1411C3BE0);
            }

            return result;
        }

        public Texture Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_TextureResource(reader);
            }
        }
    }
}
