using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class WorldChunkDefinitionResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static WorldChunkDefinitionResource Create(string version = "")
        {
            return new WorldChunkDefinitionResource();
        }

        public class ClusterInstantiation
        {
            public uint Version { get; set; }
            public Transform WorldTransform { get; set; }
            public string ClusterDefinitionId { get; set; }
            public UInt32 VisibilityTomeIdRangeStart { get; set; }
        }
        private ClusterInstantiation Read_ClusterInstantiation(BinaryReader reader)
        {
            var result = new ClusterInstantiation();

            result.Version = ReadVersion(reader, 1, 0x1416EE060);

            result.WorldTransform = Read_Transform(reader);
            result.ClusterDefinitionId = ReadUUID(reader);
            result.VisibilityTomeIdRangeStart = reader.ReadUInt32();

            return result;
        }

        public class SpatialIndexInfo
        {
            public uint Version { get; set; }
            public List<float> Origin { get; set; }
            public List<float> Dimensions { get; set; }
            public int RootNodesX { get; set; }
            public int RootNodesY { get; set; }
            public int RootNodesZ { get; set; }
            public float RootCellExtent { get; set; }
        }
        private SpatialIndexInfo Read_SpatialIndex_Info(BinaryReader reader)
        {
            var result = new SpatialIndexInfo();

            result.Version = ReadVersion(reader, 1, 0x1411CDF10);

            result.Origin = ReadVectorF(reader, 4);
            result.Dimensions = ReadVectorF(reader, 4);
            result.RootNodesX = reader.ReadInt32();
            result.RootNodesY = reader.ReadInt32();
            result.RootNodesZ = reader.ReadInt32();
            result.RootCellExtent = reader.ReadSingle();

            return result;
        }


        public class SpatialIndexNode
        {
            public uint Version { get; set; }
            public ulong PopulationBitfield { get; set; }
            public uint FirstChildIndex { get; set; }
        }
        private SpatialIndexNode Read_SpatialIndex_Node(BinaryReader reader)
        {
            var result = new SpatialIndexNode();

            result.Version = ReadVersion(reader, 1, 0x1411D9930);
            result.PopulationBitfield = reader.ReadUInt64();
            result.FirstChildIndex = reader.ReadUInt32();

            return result;
        }

        public class LevelOffsets
        {
            public uint Version { get; set; }
            public List<float> Offsets { get; set; } = new List<float>();
        }
        private LevelOffsets Read_LevelOffsets(BinaryReader reader, int count)
        {
            var result = new LevelOffsets();

            result.Version = ReadVersion(reader, 1, 0x1410BB720);

            for (int i = 0; i < count; i++)
            {
                var offset = reader.ReadSingle();
                result.Offsets.Add(offset);
            }

            return result;
        }

        public class SpatialIndex
        {
            public uint Version { get; set; }
            public SpatialIndexInfo Info { get; set; }
            public List<SpatialIndexNode> Nodes { get; set; }
            public LevelOffsets LevelOffsets { get; set; }
            public int NumLevels { get; set; }
        }
        private SpatialIndex Read_SpatialIndex(BinaryReader reader)
        {
            var result = new SpatialIndex();

            result.Version = ReadVersion(reader, 1, 0x1411C03B0);

            result.Info = Read_SpatialIndex_Info(reader);
            result.Nodes = Read_List(reader, Read_SpatialIndex_Node, 1, 0x1411CD350);
            result.LevelOffsets = Read_LevelOffsets(reader, 12);

            result.NumLevels = reader.ReadInt32();

            return result;
        }

        public class SkyTransferMatrix
        {
            public uint Version { get; set; }
            public LevelOffsets Col0 { get; set; }
            public LevelOffsets Col1 { get; set; }
            public LevelOffsets Col2 { get; set; }
            public LevelOffsets Col3 { get; set; }
        }
        private SkyTransferMatrix Read_SkyTransfer_Matrix(BinaryReader reader)
        {
            var result = new SkyTransferMatrix();

            result.Version = ReadVersion(reader, 1, 0x1411CC710);

            result.Col0 = Read_LevelOffsets(reader, 5);
            result.Col1 = Read_LevelOffsets(reader, 5);
            result.Col2 = Read_LevelOffsets(reader, 5);
            result.Col3 = Read_LevelOffsets(reader, 5);

            return result;
        }


        public class LightTransport
        {
            public uint Version { get; set; }
            public SpatialIndex SpatialIndex { get; set; }
            public List<int> ProbeEmission { get; set; }
            public List<SkyTransferMatrix> SkyTransferMatrices { get; set; }
            public List<int> ProbeVideoEmission { get; set; }
            public List<int> SkyTransferProbeIds { get; set; }
            public List<int> InfillProbeIds { get; set; }
            public List<int> InfillNeighborIds { get; set; }
            public LevelOffsets InfillPhaseOffsets { get; set; }
            public List<UInt64> FullNeighborhoodBitfield { get; set; }
            public List<UInt64> FringeCellLocs { get; set; }
        }
        private LightTransport Read_LightTransport(BinaryReader reader)
        {
            var result = new LightTransport();

            result.Version = ReadVersion(reader, 2, 0x1411ADFC0);

            result.SpatialIndex = Read_SpatialIndex(reader);
            result.ProbeEmission = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            result.SkyTransferMatrices = Read_List(reader, Read_SkyTransfer_Matrix, 1, 0x1411BF160);

            if(result.Version < 2)
            {
                // noop
            }
            else
            {
                result.ProbeVideoEmission = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            }

            result.SkyTransferProbeIds = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            result.InfillProbeIds = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            result.InfillNeighborIds = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);

            result.InfillPhaseOffsets = Read_LevelOffsets(reader, 6);

            result.FullNeighborhoodBitfield = Read_List(reader, n => n.ReadUInt64(), 1, 0x1411BF170);
            result.FringeCellLocs = Read_List(reader, n => n.ReadUInt64(), 1, 0x1411BF170);

            return result;
        }

        public class RenderChunk
        {
            public uint Version { get; set; }

            [JsonIgnore]
            public byte[] VisibilityTomeData { get; set; }
            public int VisibilityTomeDataLength => VisibilityTomeData?.Length ?? 0;

            public uint VisibilityTomeIdMask { get; set; }
            public uint VisibilityTomeIdRangeEnd { get; set; }
            public LightTransport LightTransport { get; set; }
        }
        private RenderChunk Read_RenderChunk(BinaryReader reader)
        {
            var result = new RenderChunk();

            result.Version = ReadVersion(reader, 2, 0x14119B820);

            result.VisibilityTomeData = Read_Array(reader);
            result.VisibilityTomeIdMask = reader.ReadUInt32();
            result.VisibilityTomeIdRangeEnd = reader.ReadUInt32();

            if (result.Version >= 2)
            {
                result.LightTransport = Read_LightTransport(reader);
            }

            return result;
        }

        public class WorldChunkDef
        {
            public uint Version { get; set; }
            public uint Version2 { get; set; }
            public List<ClusterInstantiation> ClusterInstantiations { get; set; }
            public RenderChunk RenderChunk { get; set; }
            public uint Version3 { get; set; }
            public string EnvironmentId { get; set; }
            public List<string> AudioChunk { get; set; }
        }
        private WorldChunkDef Read_WorldChunkDefinition(BinaryReader reader)
        {
            var result = new WorldChunkDef();

            result.Version = ReadVersion(reader, 3, 0x1410E3B80);
            if (result.Version == 2)
            {
                result.Version2 = ReadVersion(reader, 3, 0x1410E3B80);
            }

            result.ClusterInstantiations = Read_List(reader, Read_ClusterInstantiation, 1, 0x1416E9730);
            result.RenderChunk = Read_RenderChunk(reader);

            result.Version3 = ReadVersion(reader, 2, 0x1416E9750);
            result.EnvironmentId = ReadUUID(reader);

            if (result.Version3 >= 2)
            {
                result.AudioChunk = Read_List(reader, ReadUUID, 1, 0x1416F0360);
            }

            return result;
        }

        public WorldChunkDef Resource { get; set; }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_WorldChunkDefinition(reader);
            }
        }
    }
}
