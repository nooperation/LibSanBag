using LibSanBag;
using LibSanBag.ResourceUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public abstract class TextureResource : BaseFileResource
    {
        public enum TextureType
        {
            DDS = 0,
            CRN = 1,
            PNG = 2,
            BMP = 3,
            JPG = 4
        }

        public TextureType SourceType { get; set; }

        /// <summary>
        /// Raw compressed texture bytes
        /// </summary>
        public byte[] CompressedTextureBytes { get; set; }

        public int UnknownA { get; set; }
        public List<string> StreamedMips { get; set; } = new List<string>();
        public int UnknownB { get; set; }
        public int UnknownC { get; set; }

        public static TextureResource Create(string version = "")
        {
            switch (version)
            {
                case "9a8d4bbd19b4cd55":
                    return new TextureResource_9a8d4bbd19b4cd55();
                case "bfc630a1f9234ffd":
                case "30cfeb60ba0b3d8c":
                case "f1b6b03a0c99d181":
                case "1":
                    return new TextureResource_bfc630a1f9234ffd();
                default:
                    return new TextureResource_v2();
            }
        }

        /// <summary>
        /// Converts this texture to a different resolution, codec, or format
        /// </summary>
        /// <param name="codec">Type of image to convert this texture to</param>
        /// <param name="width">Width to resize image to. May not be available. Width of 0 preserves the original width.</param>
        /// <param name="height">Height to resize image to. May not be available. Height of 0 preserves the original height.</param>
        /// <returns>Converted image bytes</returns>
        public abstract byte[] ConvertTo(TextureType codec, int width = 0, int height = 0);

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
    }

    public class TextureResource_9a8d4bbd19b4cd55 : TextureResource
    {
        public override bool IsCompressed => true;

        /// <summary>
        /// Converts this texture to a different resolution, codec, or format
        /// </summary>
        /// <param name="codec">Type of image to convert this texture to</param>
        /// <param name="width">Width to resize image to. May not be available. Width of 0 preserves the original width.</param>
        /// <param name="height">Height to resize image to. May not be available. Height of 0 preserves the original height.</param>
        /// <returns>Converted image bytes</returns>
        public override byte[] ConvertTo(TextureType codec, int width = 0, int height = 0)
        {
            if (codec == TextureType.DDS)
            {
                return CompressedTextureBytes;
            }

            var ddsCodec = GetDdsTextureType(codec);
            return LibDDS.GetImageBytesFromDds(CompressedTextureBytes, 0, 0, ddsCodec);
        }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var numBytes = br.ReadInt32();
                var textureBytes = br.ReadBytes(numBytes);
                if (textureBytes[0] == 'D' && textureBytes[1] == 'D' && textureBytes[2] == 'S')
                {
                    SourceType = TextureType.DDS;
                    CompressedTextureBytes = textureBytes;
                }
                else
                {
                    throw new Exception("Could not find DDS header in decompressed data");
                }
            }
        }
    }

    public class TextureResource_bfc630a1f9234ffd : TextureResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var numBytes = br.ReadInt32();
                var textureBytes = br.ReadBytes(numBytes);
                if (textureBytes[0] == 'H' && textureBytes[1] == 'x')
                {
                    SourceType = TextureType.CRN;
                    CompressedTextureBytes = textureBytes;
                }
                else if (textureBytes[0] == 'D' && textureBytes[1] == 'D' && textureBytes[2] == 'S')
                {
                    SourceType = TextureType.DDS;
                    CompressedTextureBytes = textureBytes;
                }
                else
                {
                    throw new Exception("Could not find CRN or DDS header in decompressed data");
                }
            }
        }

        /// <summary>
        /// Converts this texture to a different resolution, codec, or format
        /// </summary>
        /// <param name="codec">Type of image to convert this texture to</param>
        /// <param name="width">Width to resize image to. May not be available. Width of 0 preserves the original width.</param>
        /// <param name="height">Height to resize image to. May not be available. Height of 0 preserves the original height.</param>
        /// <returns>Converted image bytes</returns>
        public override byte[] ConvertTo(TextureType codec, int width = 0, int height = 0)
        {
            if (SourceType == TextureType.CRN)
            {
                if (codec == TextureType.CRN)
                {
                    return CompressedTextureBytes;
                }

                var crnCodec = GetCrnTextureType(codec);
                return LibCRN.GetImageBytesFromCRN(CompressedTextureBytes, crnCodec, 0, null);
            }
            else
            {
                if (codec == TextureType.DDS)
                {
                    return CompressedTextureBytes;
                }

                var ddsCodec = GetDdsTextureType(codec);
                return LibDDS.GetImageBytesFromDds(CompressedTextureBytes, width, height, ddsCodec);
            }
        }
    }

    public class TextureResource_v2 : TextureResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var numBytes = br.ReadInt32();
                var textureBytes = br.ReadBytes(numBytes);
                if (textureBytes[0] == 'H' && textureBytes[1] == 'x')
                {
                    SourceType = TextureType.CRN;
                    CompressedTextureBytes = textureBytes;
                }
                else if (textureBytes[0] == 'D' && textureBytes[1] == 'D' && textureBytes[2] == 'S')
                {
                    SourceType = TextureType.DDS;
                    CompressedTextureBytes = textureBytes;
                }
                else
                {
                    throw new Exception("Could not find CRN or DDS header in decompressed data");
                }

                var numBytesRemaining = br.BaseStream.Length - br.BaseStream.Position;
                if(numBytesRemaining >= 4)
                {
                    UnknownA = br.ReadInt32();
                    var numAdditionalMips = br.ReadInt32();
                
                    if(numAdditionalMips > 0)
                    {
                        UnknownB = br.ReadInt32();
                        UnknownC = br.ReadInt32();

                        for(int i = 0; i < numAdditionalMips; ++i)
                        {
                            var hashLower = br.ReadUInt64();
                            var hashUpper = br.ReadUInt64();

                            var hashFinal = $"{hashLower:x16}{hashUpper:x16}";
                            StreamedMips.Add(hashFinal);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts this texture to a different resolution, codec, or format
        /// </summary>
        /// <param name="codec">Type of image to convert this texture to</param>
        /// <param name="width">Width to resize image to. May not be available. Width of 0 preserves the original width.</param>
        /// <param name="height">Height to resize image to. May not be available. Height of 0 preserves the original height.</param>
        /// <returns>Converted image bytes</returns>
        public override byte[] ConvertTo(TextureType codec, int width = 0, int height = 0)
        {
            if (SourceType == TextureType.CRN)
            {
                if (codec == TextureType.CRN)
                {
                    return CompressedTextureBytes;
                }

                var crnCodec = GetCrnTextureType(codec);
                return LibCRN.GetImageBytesFromCRN(CompressedTextureBytes, crnCodec, StreamedMips.Count, null);
            }
            else
            {
                if (codec == TextureType.DDS)
                {
                    return CompressedTextureBytes;
                }

                var ddsCodec = GetDdsTextureType(codec);
                return LibDDS.GetImageBytesFromDds(CompressedTextureBytes, width, height, ddsCodec);
            }
        }
    }
}
