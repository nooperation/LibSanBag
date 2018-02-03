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
                default:
                    return new ScriptCompiledBytecodeResourceV1();
            }
        }
    }

    public class ScriptCompiledBytecodeResourceV1 : ScriptCompiledBytecodeResource
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
}
