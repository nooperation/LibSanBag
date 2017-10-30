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
    public class ScriptSourceTextResource
    {
        /// <summary>
        /// Script filename
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Script source code
        /// </summary>
        public string Source { get; set; }

        public ScriptSourceTextResource(Stream sourceStream, FileRecord fileRecord)
        {
            byte[] decompressedBytes = null;
            using (var compressedStream = new MemoryStream())
            {
                fileRecord.Save(sourceStream, compressedStream);
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            InitFrom(decompressedBytes);
        }

        public ScriptSourceTextResource(Stream compressedStream)
        {
            var decompressedBytes = OodleLz.DecompressResource(compressedStream);

            InitFrom(decompressedBytes);
        }

        public ScriptSourceTextResource(byte[] compressedBytes)
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
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var nameLength = decompressedStream.ReadInt32();
                var nameChars = decompressedStream.ReadChars(nameLength);
                Filename = new string(nameChars);

                var sourceLength = decompressedStream.ReadInt32();
                var sourceChars = decompressedStream.ReadChars(sourceLength);
                Source = new string(sourceChars);
            }
        }
    }
}
