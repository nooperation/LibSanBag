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
        /// <summary>
        /// Raw DDS texture bytes
        /// </summary>
        public byte[] DdsBytes { get; set; }

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
        public byte[] ConvertTo(
            LibDDS.ConversionOptions.CodecType codec,
            int width = 0,
            int height = 0,
            LibDDS.ConversionOptions.DXGI_FORMAT format = LibDDS.ConversionOptions.DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_FLOAT)
        {
            return LibDDS.GetImageBytesFromDds(DdsBytes, width, height, codec, format);
        }
    }

    public class TextureResource_9a8d4bbd19b4cd55 : TextureResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var numBytes = br.ReadInt32();
                var textureBytes = br.ReadBytes(numBytes);
                if (textureBytes[0] == 'D' && textureBytes[1] == 'D' && textureBytes[2] == 'S')
                {
                    DdsBytes = textureBytes;
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
                    DdsBytes = textureBytes;
                }
                else
                {
                    throw new Exception("Could not find CRN header in decompressed data");
                }
            }
        }
    }
}
