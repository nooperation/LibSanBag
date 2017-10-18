﻿using LibSanBag;
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
        public byte[] DdsBytes { get; set; }

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

        public TextureResource(Stream compressedStream)
        {
            var decompressedBytes = OodleLz.DecompressResource(compressedStream);

            InitFrom(decompressedBytes);
        }

        public TextureResource(byte[] compressedBytes)
        {
            byte[] decompressedBytes = null;

            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            InitFrom(decompressedBytes);
        }

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
