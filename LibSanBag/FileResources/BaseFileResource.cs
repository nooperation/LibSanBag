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

                try
                {
                    decompressedBytes = OodleLz.DecompressResource(compressedStream);
                }
                catch (Exception)
                {
                    compressedStream.Seek(0, SeekOrigin.Begin);
                    using (var decompressedStream = new MemoryStream())
                    {
                        compressedStream.CopyTo(decompressedStream);
                        decompressedBytes = decompressedStream.ToArray();
                    }
                }
            }

            InitFromRawDecompressed(decompressedBytes);
        }

        public void InitFromStream(Stream compressedStream)
        {
            byte[] decompressedBytes = null;
            try
            {
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }
            catch (Exception)
            {
                compressedStream.Seek(0, SeekOrigin.Begin);
                using (var decompressedStream = new MemoryStream())
                {
                    compressedStream.CopyTo(decompressedStream);
                    decompressedBytes = decompressedStream.ToArray();
                }
            }

            InitFromRawDecompressed(decompressedBytes);
        }

        public void InitFromRawCompressed(byte[] compressedBytes)
        {
            byte[] decompressedBytes = null;

            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                try
                {
                    decompressedBytes = OodleLz.DecompressResource(compressedStream);
                }
                catch (Exception)
                {
                    compressedStream.Seek(0, SeekOrigin.Begin);
                    using (var decompressedStream = new MemoryStream())
                    {
                        compressedStream.CopyTo(decompressedStream);
                        decompressedBytes = decompressedStream.ToArray();
                    }
                }
            }

            InitFromRawDecompressed(decompressedBytes);
        }

        public abstract void InitFromRawDecompressed(byte[] decompressedBytes);
    }
}
