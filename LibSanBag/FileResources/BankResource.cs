using Newtonsoft.Json;
using System.IO;

namespace LibSanBag.FileResources
{
    public class BankResource : BaseFileResource
    {
        public static BankResource Create(string version = "")
        {
            return new BankResource();
        }

        public class BankComponent
        {
            public uint Version { get; internal set; }
            public string Name { get; internal set; }

            [JsonIgnore]
            public byte[] Data { get; internal set; }
        }
        private BankComponent Read_Embedded(BinaryReader reader)
        {
            var result = new BankComponent();

            result.Version = ReadVersion(reader, 1, 0x1411E4F10);
            result.Name = ReadString(reader);
            result.Data = Read_Array(reader);

            return result;
        }

        public class Bank
        {
            public uint Version { get; internal set; }
            public BankComponent BankItem { get; internal set; }
        }
        private Bank Read_BankResource(BinaryReader reader)
        {
            var result = new Bank();

            result.Version = ReadVersion(reader, 1, 0x1411B5080);
            result.BankItem = ReadComponent(reader, Read_Embedded);

            return result;
        }

        public Bank Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_BankResource(reader);
            }
        }
    }
}
