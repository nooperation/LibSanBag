using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class GeometryResource : BaseFileResource
    {
        public List<uint> Indices { get; set; }
        public List<float> Vertices { get; set; }
        public List<float> Tangents { get; set; }
        public List<float> Bitangents { get; set; }
        public List<float> TexCoords0 { get; set; }
        public List<float> TexCoords1 { get; set; }
        public List<float> BlendWeights { get; set; }
        public List<uint> BlendIndices { get; set; }

        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            Indices = new List<uint>();
            Vertices = new List<float>();
            Tangents = new List<float>();
            Bitangents = new List<float>();
            TexCoords0 = new List<float>();
            TexCoords1 = new List<float>();
            BlendWeights = new List<float>();
            BlendIndices = new List<uint>();

            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var unknown2 = br.ReadUInt32();
                var numUnknownVals1 = br.ReadUInt32();
                var numUnknownVals1Flag = br.ReadUInt32();
                if (numUnknownVals1 != 0)
                {
                    br.ReadBytes((int)(32 * numUnknownVals1));

                    var unknown3 = br.ReadUInt32();
                    var numUnknownVals2 = br.ReadUInt32();
                    if (numUnknownVals2 != 0)
                    {
                        var numUnknownVals2Flag = br.ReadUInt32();
                        br.ReadBytes((int)(28 * numUnknownVals2));
                    }
                }
                else
                {
                    br.ReadBytes(4);
                    br.ReadBytes(4);
                }

                // 0x20 bytes
                var minX = br.ReadSingle();
                var minY = br.ReadSingle();
                var minZ = br.ReadSingle();
                var minW = br.ReadSingle();
                var maxX = br.ReadSingle();
                var maxY = br.ReadSingle();
                var maxZ = br.ReadSingle();
                var maxW = br.ReadSingle();

                // 0x18 bytes
                var unknown6 = br.ReadUInt32();
                var unknown7 = br.ReadUInt32();
                var unknown8 = br.ReadUInt32();
                var numIndices = br.ReadUInt32();
                var numTexCoords = br.ReadUInt32();
                var bytesPerIndex = br.ReadUInt32();

                var indexByteCount = br.ReadUInt32();
                var indexCount = indexByteCount / bytesPerIndex;

                if (bytesPerIndex == 4)
                {
                    for (int i = 0; i < indexCount; i++)
                    {
                        var index = br.ReadUInt32();
                        Indices.Add(index);
                    }
                }
                else if (bytesPerIndex == 2)
                {
                    for (int i = 0; i < indexCount; i++)
                    {
                        var index = br.ReadUInt16();
                        Indices.Add(index);
                    }
                }

                var unknown12 = br.ReadUInt32();
                var numTags = br.ReadUInt32();
                var unknown14 = br.ReadUInt32();
                var unknown15 = br.ReadUInt32();

                for(var tagIndex = 0; tagIndex < numTags; ++tagIndex)
                {
                    var tagLength = br.ReadInt32();
                    var tag = new string(br.ReadChars(tagLength));
                    var tagUnknown = br.ReadUInt32();
                    var payloadByteCount = br.ReadInt32();

                    // TODO: These are most likely incorrect datatypes
                    switch (tag)
                    {
                        case "position":
                            for (var i = 0; i < payloadByteCount/4; i++)
                            {
                                var vert = br.ReadSingle();
                                Vertices.Add(vert);
                            }
                            break;
                        case "tangent":
                            for (var i = 0; i < payloadByteCount / 4; i++)
                            {
                                var vert = br.ReadSingle();
                                Tangents.Add(vert);
                            }
                            break;
                        case "bitangent":
                            for (var i = 0; i < payloadByteCount / 4; i++)
                            {
                                var vert = br.ReadSingle();
                                Bitangents.Add(vert);
                            }
                            break;
                        case "texCoord0":
                            for (var i = 0; i < payloadByteCount / 4; i++)
                            {
                                var vert = br.ReadSingle();
                                TexCoords0.Add(vert);
                            }
                            break;
                        case "texCoord1":
                            for (var i = 0; i < payloadByteCount / 4; i++)
                            {
                                var vert = br.ReadSingle();
                                TexCoords1.Add(vert);
                            }
                            break;
                        case "blendWeights":
                            for (var i = 0; i < payloadByteCount / 4; i++)
                            {
                                var vert = br.ReadSingle();
                                BlendWeights.Add(vert);
                            }
                            break;
                        case "blendIndices":
                            for (var i = 0; i < payloadByteCount / 4; i++)
                            {
                                var vert = br.ReadUInt32();
                                BlendIndices.Add(vert);
                            }
                            break;
                        default:
                            Debug.WriteLine("Unknown tag: " + tag);
                            br.ReadBytes(payloadByteCount);
                            break;
                    }
                }
            }
        }
    }
}
