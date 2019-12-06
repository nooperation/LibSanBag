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
    public class ScriptSourceTextResource : BaseFileResource
    {
        public override bool IsCompressed => true;
        public SourceScriptText Resource { get; set; }

        public class SourceScriptText
        {
            public uint Version { get; set; }
            public string SourceFileName { get; set; }
            public List<string> SourceTexts { get; set; }
            public string SourceText { get; set; }
            public string ApiVersion { get; set; }
            public List<string> SourceNames { get; set; }
        }
        private SourceScriptText Read_ScriptSourceText(BinaryReader reader)
        {
            var result = new SourceScriptText();

            result.Version = ReadVersion(reader, 5, 0x14176AB20);
            result.SourceFileName = ReadString_VersionSafe(reader, result.Version, 2);

            if(result.Version >= 4)
            {
                result.SourceTexts = Read_List(reader, ReadString, 1, 0x14119ADB0);
            }
            else
            {
                result.SourceText = ReadString(reader);
            }

            if(result.Version >= 3)
            {
                result.ApiVersion = ReadString(reader);
            }

            if(result.Version >= 5)
            {
                result.SourceNames = Read_List(reader, ReadString, 1, 0x14119ADB0);
            }

            return result;
        }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_ScriptSourceText(decompressedStream);
            }
        }

        public static ScriptSourceTextResource Create(string version = "")
        {
            return new ScriptSourceTextResource();
        }
    }
}
