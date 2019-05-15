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
        public abstract bool IsCompressed { get; }

        public void InitFromRecord(Stream sourceStream, FileRecord fileRecord)
        {
            using (var compressedStream = new MemoryStream())
            {
                fileRecord.Save(sourceStream, compressedStream);
                InitFromStream(compressedStream);
            }
        }

        public void InitFromRawCompressed(byte[] compressedBytes)
        {
            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                InitFromStream(compressedStream);
            }
        }

        public void InitFromStream(Stream compressedStream)
        {
            byte[] decompressedBytes = null;
            compressedStream.Seek(0, SeekOrigin.Begin);

            if (IsCompressed)
            {
                try
                {
                    decompressedBytes = Unpacker.DecompressResource(compressedStream);
                    InitFromRawDecompressed(decompressedBytes);
                    return;
                }
                catch (Exception)
                {
                }
            }

            using (var decompressedStream = new MemoryStream())
            {
                compressedStream.CopyTo(decompressedStream);
                decompressedBytes = decompressedStream.ToArray();
            }
            InitFromRawDecompressed(decompressedBytes);
        }

        public abstract void InitFromRawDecompressed(byte[] decompressedBytes);
    }
}
