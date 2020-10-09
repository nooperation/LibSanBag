using System.Collections.Generic;
using System.IO;

namespace LibSanBag.FileResources
{
    public class PickableModelResource : BaseFileResource
    {
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
            result.Model = ReadComponent(reader, clusterReader.Read_ModelDefinition);
            result.PickResource = ReadUUID(reader);

            return result;
        }

        public PickableModel Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_PickableModelResource(reader);
            }
        }

        #region ParserInit
        private ClusterDefinitionResource clusterReader;
        public PickableModelResource()
        {
            clusterReader = new ClusterDefinitionResource();
            clusterReader.OverrideVersionMap(versionMap, componentMap);
        }

        internal override void OverrideVersionMap(Dictionary<ulong, uint> newVersionMap, Dictionary<uint, object> newComponentMap)
        {
            this.versionMap = newVersionMap;
            this.componentMap = newComponentMap;

            clusterReader.OverrideVersionMap(newVersionMap, newComponentMap);
        }
        #endregion
    }
}
