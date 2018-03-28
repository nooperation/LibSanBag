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
            PNG = 1,
            BMP = 2,
        }

        /// <summary>
        /// Raw compressed texture bytes
        /// </summary>
        public byte[] CompressedTextureBytes { get; set; }

        public static TextureResource Create(string version = "")
        {
            switch (version)
            {
                case "9a8d4bbd19b4cd55":
                    return new TextureResource_9a8d4bbd19b4cd55();
                case "bfc630a1f9234ffd":
                default:
                    return new TextureResource_bfc630a1f9234ffd();
            }
        }

        /// <summary>
        /// Converts this texture to a different resolution, codec, or format
        /// </summary>
        /// <param name="codec">Type of image to convert this texture to</param>
        /// <param name="width">New image width or 0 for original image width</param>
        /// <param name="height">New image height or 0 for original image height</param>
        /// <param name="format">Color format</param>
        /// <returns>Converted image bytes</returns>
        public abstract byte[] ConvertTo(TextureType codec);
    }

    public class TextureResource_9a8d4bbd19b4cd55 : TextureResource
    {
        public override bool IsCompressed => true;

        /// <summary>
        /// Converts this texture to a different resolution, codec, or format
        /// </summary>
        /// <param name="codec">Type of image to convert this texture to</param>
        /// <param name="width">New image width or 0 for original image width</param>
        /// <param name="height">New image height or 0 for original image height</param>
        /// <param name="format">Color format</param>
        /// <returns>Converted image bytes</returns>
        public override byte[] ConvertTo(TextureType codec)
        {
            var ddsCodec = LibDDS.ConversionOptions.CodecType.CODEC_BMP;

            switch (codec)
            {
                case TextureType.DDS:
                    return CompressedTextureBytes;
                case TextureType.PNG:
                    ddsCodec = LibDDS.ConversionOptions.CodecType.CODEC_PNG;
                    break;
                case TextureType.BMP:
                    ddsCodec = LibDDS.ConversionOptions.CodecType.CODEC_BMP;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(codec), codec, null);
            }

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
                    CompressedTextureBytes = textureBytes;
                }
                else
                {
                    throw new Exception("Could not find CRN header in decompressed data");
                }
            }
        }

        /// <summary>
        /// Converts this texture to a different resolution, codec, or format
        /// </summary>
        /// <param name="codec">Type of image to convert this texture to</param>
        /// <returns>Converted image bytes</returns>
        public override byte[] ConvertTo(TextureType codec)
        {
            var crnCodec = LibCRN.ImageCodec.BMP;
            switch (codec)
            {
                case TextureType.DDS:
                    crnCodec = LibCRN.ImageCodec.DDS;
                    break;
                case TextureType.PNG:
                    crnCodec = LibCRN.ImageCodec.PNG;
                    break;
                case TextureType.BMP:
                    crnCodec = LibCRN.ImageCodec.BMP;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(codec), codec, null);
            }

            //var ddsBytes = LibCRN.
            return LibCRN.GetImageBytesFromCRN(CompressedTextureBytes, crnCodec);
        }
    }
}
