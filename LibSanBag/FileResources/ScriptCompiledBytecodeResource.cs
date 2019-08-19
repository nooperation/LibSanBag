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
    public abstract class ScriptCompiledBytecodeResource : BaseFileResource
    {
        public string ScriptSourceTextPath { get; set; }
        public byte[] AssemblyBytes { get; set; }

        public static ScriptCompiledBytecodeResource Create(string version = "")
        {
            switch (version)
            {
                case "v1":
                case "695aad7e1181dc46":
                case "c84707da067146a9":
                case "e6ac3244f1076f7b":
                default:
                    return new ScriptCompiledBytecodeResource_v1();
            }
        }
    }

    public class ScriptCompiledBytecodeResource_v1 : ScriptCompiledBytecodeResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ResourceVersion = br.ReadInt32();

                if(ResourceVersion < 2)
                {
                    var scriptSourceTextPathLength = br.ReadInt32();
                    var scriptSourceTextPathChars = br.ReadChars(scriptSourceTextPathLength);
                    ScriptSourceTextPath = new string(scriptSourceTextPathChars);
                }
                else
                {
                    ScriptSourceTextPath = string.Empty;
                }

                var assemblyLength = br.ReadInt32();
                AssemblyBytes = br.ReadBytes(assemblyLength);
            }
        }
    }
}
