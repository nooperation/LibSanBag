using LibSanBag;
using LibSanBag.ResourceUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.FileResources;

namespace LibSanBag.FileResources
{
    public abstract class ScriptSourceTextResource : BaseFileResource
    {
        /// <summary>
        /// Script filename
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Script source code
        /// </summary>
        public string Source { get; set; }

        public static ScriptSourceTextResource Create(string version = "")
        {
            switch (version)
            {
                case "6301a7d31aa6f628":
                case "dedd8914f8dfe71e":
                    return new ScriptSourceTextResourceV1();
                default:
                    return new ScriptSourceTextResourceV2();
            }
        }
    }

    public class ScriptSourceTextResourceV2 : ScriptSourceTextResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                Filename = string.Empty;

                var sourceLength = decompressedStream.ReadInt32();
                var sourceChars = decompressedStream.ReadChars(sourceLength);
                Source = new string(sourceChars);
            }
        }
    }

    public class ScriptSourceTextResourceV1 : ScriptSourceTextResource
    {
        public override bool IsCompressed => true;

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
