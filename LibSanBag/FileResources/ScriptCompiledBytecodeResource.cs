using Newtonsoft.Json;
using System.IO;

namespace LibSanBag.FileResources
{
    public class ScriptCompiledBytecodeResource : BaseFileResource
    {
        public static ScriptCompiledBytecodeResource Create(string version = "")
        {
            return new ScriptCompiledBytecodeResource();
        }

        public class ScriptCompiledBytecode
        {
            public int Version { get; internal set; }
            public string ScriptSourceTextPath { get; internal set; } = string.Empty;

            [JsonIgnore]
            public byte[] AssemblyBytes { get; internal set; }
        }
        private ScriptCompiledBytecode Read_ScriptCompiledBytecode(BinaryReader reader)
        {
            var result = new ScriptCompiledBytecode();

            result.Version = reader.ReadInt32();

            if (result.Version < 2)
            {
                result.ScriptSourceTextPath = ReadString(reader);
            }

            result.AssemblyBytes = Read_Array(reader);

            return result;
        }

        public ScriptCompiledBytecode Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_ScriptCompiledBytecode(br);
            }
        }
    }
}
