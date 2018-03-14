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
                case "c84707da067146a9":
                case "e6ac3244f1076f7b":
                    return new ScriptCompiledBytecodeResource_e6ac3244f1076f7b();
                case "695aad7e1181dc46":
                default:
                    return new ScriptCompiledBytecodeResource_695aad7e1181dc46();
            }
        }
    }

    public class ScriptCompiledBytecodeResource_e6ac3244f1076f7b : ScriptCompiledBytecodeResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var stringLength = br.ReadInt32();
                var stringChars = br.ReadChars(stringLength);
                ScriptSourceTextPath = new string(stringChars);

                var assemblyLength = br.ReadInt32();
                AssemblyBytes = br.ReadBytes(assemblyLength);
            }
        }
    }

    public class ScriptCompiledBytecodeResource_695aad7e1181dc46 : ScriptCompiledBytecodeResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ScriptSourceTextPath = string.Empty;

                var assemblyLength = br.ReadInt32();
                AssemblyBytes = br.ReadBytes(assemblyLength);
            }
        }
    }
}
