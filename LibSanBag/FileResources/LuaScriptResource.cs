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
    public abstract class LuaScriptResource : BaseFileResource
    {
        /// <summary>
        /// Lua source filename
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Lua source code
        /// </summary>
        public string Source { get; set; }

        public static LuaScriptResource Create(string version = "")
        {
            switch (version)
            {
                case "2487dccddadf7656":
                default:
                    return new LuaScriptResource_2487dccddadf7656();
            }
        }
    }

    public class LuaScriptResource_2487dccddadf7656 : LuaScriptResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ResourceVersion = decompressedStream.ReadInt32();

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
