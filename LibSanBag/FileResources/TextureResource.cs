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

        public enum TextureType
        {
            DDS = 0,
            CRN = 1,
            PNG = 2,
            BMP = 3,
            JPG = 4
        }


        public static TextureResource Create(string version = "")
        {
            return new TextureResource();
        }

        /// <summary>
        /// Converts this texture to a different resolution, codec, or format
        /// </summary>
        /// <param name="codec">Type of image to convert this texture to</param>
        /// <param name="width">Width to resize image to. May not be available. Width of 0 preserves the original width.</param>
        /// <param name="height">Height to resize image to. May not be available. Height of 0 preserves the original height.</param>
        /// <returns>Converted image bytes</returns>
        public byte[] ConvertTo(TextureType codec, UInt64 width = 0, UInt64 height = 0)
        {
            if (TextureType.CRN == TextureType.CRN)
            {
                if (codec == TextureType.CRN)
                {
                    return Resource.Data;
                }

                var crnCodec = GetCrnTextureType(codec);
                return LibCRN.GetImageBytesFromCRN(Resource.Data, crnCodec, Resource.MipLevels.Count, null);
            }
            else
            {
                if (codec == TextureType.DDS)
                {
                    return Resource.Data;
                }

                var ddsCodec = GetDdsTextureType(codec);
                return LibDDS.GetImageBytesFromDds(Resource.Data, width, height, ddsCodec);
            }
        }

        public LibDDS.ConversionOptions.CodecType GetDdsTextureType(TextureType codec)
        {
            switch (codec)
            {
                case TextureType.DDS:
                    return LibDDS.ConversionOptions.CodecType.CODEC_DDS;
                case TextureType.PNG:
                    return LibDDS.ConversionOptions.CodecType.CODEC_PNG;
                case TextureType.BMP:
                    return LibDDS.ConversionOptions.CodecType.CODEC_BMP;
                default:
                    throw new NotImplementedException("Cannot convert DDS to " + codec.ToString());
            }
        }

        public LibCRN.ImageCodec GetCrnTextureType(TextureType codec)
        {
            switch (codec)
            {
                case TextureType.DDS:
                    return LibCRN.ImageCodec.DDS;
                case TextureType.CRN:
                    return LibCRN.ImageCodec.CRN;
                case TextureType.PNG:
                    return LibCRN.ImageCodec.PNG;
                case TextureType.BMP:
                    return LibCRN.ImageCodec.BMP;
                case TextureType.JPG:
                    return LibCRN.ImageCodec.JPG;
                default:
                    throw new NotImplementedException("Cannot convert CRN to " + codec.ToString());
            }
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
