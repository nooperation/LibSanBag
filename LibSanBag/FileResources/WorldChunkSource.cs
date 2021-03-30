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
    public class WorldChunkSource : BaseFileResource
    {
        public static WorldChunkSource Create(string version = "")
        {
            return new WorldChunkSource();
        }

        public class EnvironmentPrecomputeParameters
        {
            public uint Version { get; internal set; }
            public float Spacing { get; internal set; }
            public int NumRays { get; internal set; }
            public int NumBounces { get; internal set; }
        }
        private EnvironmentPrecomputeParameters Read_EnvironmentPrecomputeParameters(BinaryReader reader)
        {
            var result = new EnvironmentPrecomputeParameters();

            result.Version = ReadVersion(reader, 1, 0x1411ABAA0);
            result.Spacing = reader.ReadSingle();
            result.NumRays = reader.ReadInt32();
            result.NumBounces = reader.ReadInt32();

            return result;
        }

        public class AudioChunk
        {
            public uint Version { get; internal set; }
            public EnvironmentPrecomputeParameters EnvironmentPrecomputeParameters { get; internal set; }
            public bool DoEnvironmentPrecompute { get; internal set; }
        }
        private AudioChunk Read_AudioChunk(BinaryReader reader)
        {
            var result = new AudioChunk();

            result.Version = ReadVersion(reader, 1, 0x1411A0F20);
            result.EnvironmentPrecomputeParameters = Read_EnvironmentPrecomputeParameters(reader);
            result.DoEnvironmentPrecompute = reader.ReadBoolean();

            return result;
        }

        public class RenderChunk_V4
        {
            public uint Version { get; internal set; }
            public string SourceBakeQuality { get; internal set; }
            public string DefinitionBakeQuality { get; internal set; }
            public WorldChunkDefinitionResource.RenderChunk RenderChunk { get; internal set; }
        }
        private RenderChunk_V4 Read_RenderChunk_V4(BinaryReader reader)
        {
            var result = new RenderChunk_V4();

            result.Version = ReadVersion(reader, 1, 0x1411A0F10);
            result.SourceBakeQuality = ReadString(reader);
            result.DefinitionBakeQuality = ReadString(reader);
            result.RenderChunk = WorldChunkDefReader.Read_RenderChunk(reader);

            return result;
        }

        public class ClusterInstantiation
        {
            public uint Version { get; internal set; }
            public Transform SourceTransform { get; internal set; }
            public uint ClusterId { get; internal set; }
            public WorldSource.WorldObject ClusterSource { get; internal set; }
            public List<float> UserRotationValues { get; internal set; }
            public int InstanceId { get; internal set; }
        }
        private ClusterInstantiation Read_ClusterInstantiation(BinaryReader reader)
        {
            var result = new ClusterInstantiation();

            result.Version = ReadVersion(reader, 4, 0x1411ADFD0);
            result.SourceTransform = Read_Transform(reader);
            result.ClusterId = reader.ReadUInt32();
            result.ClusterSource = WorldSourceReader.Read_WorldObject(reader);

            if(result.Version >= 2)
            {
                result.UserRotationValues = ReadVectorF(reader, 4);
            }

            if(result.Version >= 3)
            {
                result.InstanceId = reader.ReadInt32();
            }

            return result;
        }

        public class WorldChunk
        {
            public uint Version { get; internal set; }
            public List<ClusterInstantiation> ClusterInstantiations { get; internal set; }
            public WorldChunkDefinitionResource.RenderChunk RenderChunk_V1 { get; internal set; }
            public RenderChunk_V4 RenderChunk { get; internal set; }
            public AudioChunk AudioChunk { get; internal set; }
        }
        private WorldChunk Read_WorldChunkSource(BinaryReader reader)
        {
            var result = new WorldChunk();

            result.Version = ReadVersion(reader, 4, 0x1411395A0);
            result.ClusterInstantiations = Read_List(reader, Read_ClusterInstantiation, 1, 0x1411A03E0);

            if(result.Version < 4)
            {
                result.RenderChunk_V1 = WorldChunkDefReader.Read_RenderChunk(reader);
            }
            else
            {
                result.RenderChunk = Read_RenderChunk_V4(reader);
            }

            result.AudioChunk = Read_AudioChunk(reader);

            return result;
        }

        public WorldChunk Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_WorldChunkSource(reader);
            }
        }

        #region ParserInit

        private WorldSource WorldSourceReader;
        private WorldChunkDefinitionResource WorldChunkDefReader;
        public WorldChunkSource()
        {
            WorldSourceReader = new WorldSource();
            WorldSourceReader.OverrideVersionMap(versionMap, componentMap);

            WorldChunkDefReader = new WorldChunkDefinitionResource();
            WorldChunkDefReader.OverrideVersionMap(versionMap, componentMap);
        }

        internal override void OverrideVersionMap(Dictionary<ulong, uint> newVersionMap, Dictionary<uint, object> newComponentMap)
        {
            this.versionMap = newVersionMap;
            this.componentMap = newComponentMap;

            WorldSourceReader.OverrideVersionMap(newVersionMap, newComponentMap);
            WorldChunkDefReader.OverrideVersionMap(newVersionMap, newComponentMap);
        }

        #endregion
    }
}
