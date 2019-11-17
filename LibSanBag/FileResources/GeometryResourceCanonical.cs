using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public abstract class GeometryResourceCanonical : BaseFileResource
    {
        public int ContentLength { get; set; }
        public byte[] Content { get; set; }

        public static GeometryResourceCanonical Create(string version = "")
        {
            switch (version)
            {
                case "51b89e39caab7b79":
                default:
                    return new GeometryResourceCanonical_51b89e39caab7b79();
            }
        }
    }

    public class GeometryResourceCanonical_51b89e39caab7b79 : GeometryResourceCanonical
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ResourceVersion = br.ReadInt32();

                ContentLength = br.ReadInt32();
                Content = br.ReadBytes(ContentLength);
            }
        }
    }
}
