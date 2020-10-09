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
    public class AudioGraphResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static AudioGraphResource Create(string version = "")
        {
            return new AudioGraphResource();
        }

        public class Node
        {
            public ulong NodeId { get; internal set; }
            public int NodeType { get; internal set; }
            public uint Version { get; internal set; }
            public List<uint> Nodes { get; internal set; }
            public string SoundResource { get; internal set; }
        }
        private Node Read_Node(BinaryReader reader, ulong version)
        {
            var result = new Node();

            result.NodeId = reader.ReadUInt32();
            result.NodeType = reader.ReadInt32();

            if(result.NodeType != 0)
            {
                result.Version = ReadVersion(reader, 1, 0x141217DA0);
                result.Nodes = Read_List(reader, n => n.ReadUInt32(), 1, 0x141220E20);
            }
            else
            {
                result.Version = ReadVersion(reader, 1, 0x141217D90);
                result.SoundResource = ReadUUID(reader);
            }

            return result;
        }

        public class AudioGraph
        {
            public uint Version { get; internal set; }
            public long NodeCount { get; internal set; }
            public List<Node> Nodes { get; internal set; }
        }
        private AudioGraph Read_AudioGraphResource(BinaryReader reader)
        {
            var result = new AudioGraph();

            result.Version = ReadVersion(reader, 1, 0x1412011B0);
            result.NodeCount = reader.ReadInt64();

            var nodes = new List<Node>();
            for (long i = 0; i < result.NodeCount; i++)
            {
                var node = Read_Node(reader, result.Version);
                nodes.Add(node);
            }
            result.Nodes = nodes;

            return result;
        }

        public AudioGraph Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_AudioGraphResource(reader);
            }
        }
    }
}
