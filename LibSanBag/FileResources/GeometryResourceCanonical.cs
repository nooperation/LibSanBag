using System.IO;

namespace LibSanBag.FileResources
{
    public class GeometryResourceCanonical : BaseFileResource
    {
        public static GeometryResourceCanonical Create(string version = "")
        {
            return new GeometryResourceCanonical();
        }

        public class GeometryCanonical
        {
            public uint Version { get; set; }
            public byte[] Bytes { get; internal set; }
        }
        public GeometryCanonical Read_GeometryCanonical(BinaryReader reader)
        {
            var result = new GeometryCanonical();

            result.Version = ReadVersion(reader, 1, 0xFF14118B750);
            result.Bytes = Read_Array(reader);

            return result;
        }

        public GeometryCanonical Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_GeometryCanonical(reader);
            }
        }
    }
}
