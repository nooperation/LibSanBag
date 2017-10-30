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
    public class LuaScriptResource
    {
        /// <summary>
        /// Lua source filename
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Lua source code
        /// </summary>
        public string Source { get; set; }

        public LuaScriptResource(Stream sourceStream, FileRecord fileRecord)
        {
            byte[] decompressedBytes = null;
            using (var compressedStream = new MemoryStream())
            {
                fileRecord.Save(sourceStream, compressedStream);
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            InitFrom(decompressedBytes);
        }

        public LuaScriptResource(Stream compressedStream)
        {
            var decompressedBytes = OodleLz.DecompressResource(compressedStream);

            InitFrom(decompressedBytes);
        }

        public LuaScriptResource(byte[] compressedBytes)
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
