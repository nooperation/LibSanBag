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
    public class TextureResource
    {
        /// <summary>
        /// Raw DDS texture bytes
        /// </summary>
        public byte[] DdsBytes { get; set; }

        /// <summary>
        /// Creates a new TextureResource from the specified FileRecord using the source stream
        /// </summary>
        /// <param name="sourceStream">Bag stream containing the FileRecord</param>
        /// <param name="fileRecord">FileRecord to extract TextureResource from using the source stream</param>
        public TextureResource(Stream sourceStream, FileRecord fileRecord)
        {
            byte[] decompressedBytes = null;
            using (var compressedStream = new MemoryStream())
            {
                fileRecord.Save(sourceStream, compressedStream);
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            InitFrom(decompressedBytes);
        }

        /// <summary>
        /// Initializes this from a stream containing a compressed TextureResource file
        /// </summary>
        /// <param name="compressedStream">Stream containing compressed TextureResource data</param>
        public TextureResource(Stream compressedStream)
        {
            var decompressedBytes = OodleLz.DecompressResource(compressedStream);

            InitFrom(decompressedBytes);
        }

        /// <summary>
        /// Initializes this from a byte collection containing a compressed TextureResource file
        /// </summary>
        /// <param name="compressedBytes">Collection containing compressed TextureResource data</param>
        public TextureResource(byte[] compressedBytes)
        {
            byte[] decompressedBytes = null;

            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            InitFrom(decompressedBytes);
        }

        /// <summary>
        /// Initializes this from a byte collection containing a TextureResource file
        /// </summary>
        /// <param name="decompressedBytes">Collection containing decompressed TextureResource data</param>
        private void InitFrom(byte[] decompressedBytes)
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
}
