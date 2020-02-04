using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LibSanBag.FileResources
{
    public class TextureSource : BaseFileResource
    {
        public static TextureSource Create(string version = "")
        {
            return new TextureSource();
        }

        public class Thumbnail
        {
            public uint Version { get; internal set; }
            [JsonIgnore]
            public byte[] Data { get; internal set; }
            public ulong Width { get; internal set; }
            public ulong Height { get; internal set; }
            public string Format { get; internal set; }
            public List<float> AverageColor { get; internal set; }
        }
        private Thumbnail Read_Thumbnail(BinaryReader reader)
        {
            var result = new Thumbnail();

            result.Version = ReadVersion(reader, 4, 0x1411C4E30);
            result.Data = Read_Array(reader);
            result.Width = reader.ReadUInt64();
            result.Height = reader.ReadUInt64();

            if(result.Version < 4)
            {
                result.Format = ReadString(reader);
            }

            if(result.Version >= 2)
            {
                result.AverageColor = ReadVectorF(reader, 4);
            }

            return result;
        }

        public class Texture
        {
            public uint Version { get; internal set; }
            public string Name { get; internal set; }
            public string TextureId { get; internal set; }
            public Thumbnail Thumbnail { get; internal set; }
            public ulong Width { get; internal set; }
            public ulong Height { get; internal set; }
            public uint OpacityFlags { get; internal set; }
            public bool SemiOpaque { get; internal set; }
            public string Format { get; internal set; }
        }
        private Texture Read_TextureSource(BinaryReader reader)
        {
            var result = new Texture();

            result.Version = ReadVersion(reader, 3, 0x141114210);

            if(result.Version < 2)
            {
                result.Name = ReadString(reader);
            }

            result.TextureId = ReadUUID(reader);
            result.Thumbnail = Read_Thumbnail(reader);
            result.Width = reader.ReadUInt64();
            result.Height = reader.ReadUInt64();

            if(result.Version >= 3)
            {
                result.OpacityFlags = reader.ReadUInt32();
            }
            else
            {
                result.SemiOpaque = reader.ReadBoolean();
            }

            result.Format = ReadString(reader);
            return result;
        }

        public Texture Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_TextureSource(reader);
            }
        }
    }
}
