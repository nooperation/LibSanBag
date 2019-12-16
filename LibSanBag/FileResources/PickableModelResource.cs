using LibSanBag;
using LibSanBag.ResourceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class PickableModelResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static PickableModelResource Create(string version = "")
        {
            return new PickableModelResource();
        }

        public class PickableModel
        {
            public uint Version { get; internal set; }
            public ClusterDefinitionResource.ModelDefinition Model { get; internal set; }
            public string PickResource { get; internal set; }
        }
        private PickableModel Read_PickableModelResource(BinaryReader reader)
        {
            var result = new PickableModel();

            result.Version = ReadVersion(reader, 1, 0x1411C3BF0);
            result.Model = ReadComponent(reader, ClusterReader.Read_ModelDefinition);
            result.PickResource = ReadUUID(reader);

            return result;
        }

        private ClusterDefinitionResource ClusterReader;
        public PickableModel Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            ClusterReader = new ClusterDefinitionResource();
            ClusterReader.OverrideVersionMap(versionMap, componentMap);

            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_PickableModelResource(reader);
            }
        }
    }
}
