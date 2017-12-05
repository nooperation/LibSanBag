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
    public class LuaScriptResource : BaseFileResource
    {
        /// <summary>
        /// Lua source filename
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Lua source code
        /// </summary>
        public string Source { get; set; }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
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
