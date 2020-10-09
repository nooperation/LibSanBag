using System.IO;

namespace LibSanBag.FileResources
{
    public class LuaScriptResource : BaseFileResource
    {
        public static LuaScriptResource Create(string version = "")
        {
            return new LuaScriptResource();
        }

        public class LuaScript
        {
            public uint Version { get; internal set; }
            public string Filename { get; internal set; }
            public string Source { get; internal set; }
        }
        private LuaScript Read_LuaScript(BinaryReader reader)
        {
            var result = new LuaScript();

            result.Version = ReadVersion(reader, 1, 0x14121FD50);
            result.Filename = ReadString(reader);
            result.Source = ReadString(reader);

            return result;
        }

        public LuaScript Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_LuaScript(decompressedStream);
            }
        }
    }
}
