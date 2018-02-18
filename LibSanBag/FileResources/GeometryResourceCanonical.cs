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
                default:
                    return new GeometryResourceCanonicalV1();
            }
        }
    }

    public class GeometryResourceCanonicalV1 : GeometryResourceCanonical
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ContentLength = br.ReadInt32();
                Content = br.ReadBytes(ContentLength);
            }
        }
    }
}
