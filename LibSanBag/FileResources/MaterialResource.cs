using System.Collections.Generic;
using System.IO;

namespace LibSanBag.FileResources
{
    public class MaterialResource : BaseFileResource
    {
        public static MaterialResource Create(string version = "")
        {
            return new MaterialResource();
        }

        public class TextureParameters
        {
            public uint Version { get; set; }
            public string Name { get; internal set; }
            public string Texture { get; internal set; }
        }
        private TextureParameters Read_TextureParameters(BinaryReader reader)
        {
            var result = new TextureParameters();

            result.Version = ReadVersion(reader, 1, 0x141223930);
            result.Name = ReadString(reader);
            result.Texture = ReadUUID(reader);

            return result;
        }

        public class FloatParameters
        {
            public uint Version { get; set; }
            public string Name { get; internal set; }
            public float Value { get; internal set; }
        }
        private FloatParameters Read_FloatParameters(BinaryReader reader)
        {
            var result = new FloatParameters();

            result.Version = ReadVersion(reader, 1, 0x141223920);
            result.Name = ReadString(reader);
            result.Value = reader.ReadSingle();

            return result;
        }

        public class Float4Parameters
        {
            public uint Version { get; set; }
            public string Name { get; internal set; }
            public List<float> Value { get; internal set; }
        }
        private Float4Parameters Read_Float4Parameters(BinaryReader reader)
        {
            var result = new Float4Parameters();

            result.Version = ReadVersion(reader, 1, 0x141223910);
            result.Name = ReadString(reader);
            result.Value = ReadVectorF(reader, 4);

            return result;
        }

        public class BooleanParameters
        {
            public uint Version { get; set; }
            public string Name { get; internal set; }
            public bool Value { get; internal set; }
        }
        private BooleanParameters Read_BooleanParameters(BinaryReader reader)
        {
            var result = new BooleanParameters();

            result.Version = ReadVersion(reader, 1, 0x141223940);
            result.Name = ReadString(reader);
            result.Value = reader.ReadBoolean();

            return result;
        }

        public class Material
        {
            public uint Version { get; set; }
            public string Type { get; internal set; }
            public string Name { get; internal set; }
            public List<TextureParameters> TextureParameters { get; internal set; }
            public List<FloatParameters> FloatParameters { get; internal set; }
            public List<Float4Parameters> Float4Parameters { get; internal set; }
            public List<BooleanParameters> BoolParameters { get; internal set; }
            public bool CanOcclude { get; internal set; }
            public uint PropertyFlags { get; internal set; }
        }
        private Material Read_MaterialResource(BinaryReader reader)
        {
            var result = new Material();

            result.Version = ReadVersion(reader, 3, 0x141211B30);
            result.Type = ReadString(reader);
            result.Name = ReadString(reader);
            result.TextureParameters = Read_List(reader, Read_TextureParameters, 1, 0x141223400);
            result.FloatParameters = Read_List(reader, Read_FloatParameters, 1, 0x141223410);
            result.Float4Parameters = Read_List(reader, Read_Float4Parameters, 1, 0x141223420);

            if (result.Version >= 3)
            {
                result.BoolParameters = Read_List(reader, Read_BooleanParameters, 1, 0x141223540);
            }

            if(result.Version < 2)
            {
                result.CanOcclude = reader.ReadBoolean();
            }
            else
            {
                result.PropertyFlags = reader.ReadUInt32();
            }

            return result;
        }

        public Material Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_MaterialResource(reader);
            }
        }
    }
}
