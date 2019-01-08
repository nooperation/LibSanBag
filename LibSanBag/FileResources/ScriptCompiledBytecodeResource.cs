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
                ScriptSourceTextPath = string.Empty;

                var assemblyLength = br.ReadInt32();

                // It appears that some v1 headers have the old script source text path prefix?
                // If we don't have the 'MZ' executable ID then just assume it's the old script
                // source text path. The real assembly length will come after this if it exists
                if(decompressedBytes[4] != 'M' && decompressedBytes[5] != 'Z')
                {
                    var stringLength = assemblyLength;
                    var stringChars = br.ReadChars(stringLength);
                    ScriptSourceTextPath = new string(stringChars);

                    assemblyLength = br.ReadInt32();
                }

                AssemblyBytes = br.ReadBytes(assemblyLength);
            }
        }
    }
}
