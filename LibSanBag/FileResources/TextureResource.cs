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

            if (decompressedBytes[0] == 'D' && decompressedBytes[1] == 'D' && decompressedBytes[2] == 'S')
            {
                DdsBytes = decompressedBytes;
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
