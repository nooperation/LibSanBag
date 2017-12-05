using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.ResourceUtils;

namespace LibSanBag.FileResources
{
    public abstract class BaseFileResource
    {
        public void InitFromRecord(Stream sourceStream, FileRecord fileRecord)
        {
            byte[] decompressedBytes = null;
            using (var compressedStream = new MemoryStream())
            {
                fileRecord.Save(sourceStream, compressedStream);
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            InitFromRawDecompressed(decompressedBytes);
        }

        public void InitFromStream(Stream compressedStream)
        {
            var decompressedBytes = OodleLz.DecompressResource(compressedStream);

            InitFromRawDecompressed(decompressedBytes);
        }

        public void InitFromRawCompressed(byte[] compressedBytes)
        {
            byte[] decompressedBytes = null;

            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            InitFromRawDecompressed(decompressedBytes);
        }

        public abstract void InitFromRawDecompressed(byte[] decompressedBytes);
    }
}
