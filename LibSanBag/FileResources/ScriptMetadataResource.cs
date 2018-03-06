using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public abstract class ScriptMetadataResource : BaseFileResource
    {
        public struct PropertyEntry
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public List<KeyValuePair<string, string>> Attributes { get; set; }
        }

        public string AssemblyName { get; set; }
        public string Warnings { get; set; }
        public List<PropertyEntry> Properties { get; set; }
        public List<KeyValuePair<string, string>> Strings { get; set; }

        public static ScriptMetadataResource Create(string version = "")
        {
            switch (version)
            {

                case "0a316ea155e30eda":
                    return new ScriptMetadataResourceV1();
                case "67df52a55a73f7d3":
                default:
                    return new ScriptMetadataResourceV3();
            }
        }
    }

    public class ScriptMetadataResourceV3 : ScriptMetadataResource
    {
        public struct TypeCode
        {
            public static readonly int System_Boolean = 4101;
            public static readonly int System_SByte   =  101;
            public static readonly int System_Byte    = 8101;
            public static readonly int System_Int16   =  102;
            public static readonly int System_UInt16  = 8102;
            public static readonly int System_Int32   =  104;
            public static readonly int System_UInt32  = 8104;
            public static readonly int System_Int64   =  108;
            public static readonly int System_UInt64  = 8108;
            public static readonly int System_Single  =  204;
            public static readonly int System_Double  =  208;
            public static readonly int System_String  =  400;
            public static readonly int System_Object  =  800;
            public static readonly int Sansar_Simulation_RigidBodyComponent =  802;
            public static readonly int Sansar_Simulation_AnimationComponent =  801;
            public static readonly int Sansar_Simulation_AudioComponent     =  803;
            public static readonly int Sansar_Simulation_ClusterResource    = 2002;
            public static readonly int Sansar_Simulation_SoundResource      = 2003;
            public static readonly int Mono_Simd_Vector4f = 1001;
            public static readonly int Sansar_Vector      = 1002;
            public static readonly int Sansar_Quaternion  = 1003;
        }

        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var warningsLength = decompressedStream.ReadInt32();
                Warnings = new string(decompressedStream.ReadChars(warningsLength));

                var unknownA = decompressedStream.ReadInt32(); // 0
                var unknownB = decompressedStream.ReadInt32(); // 0

                var unknownC = decompressedStream.ReadInt32(); // 1
                var propertyCount = decompressedStream.ReadInt32();
                Properties = new List<PropertyEntry>(propertyCount);

                if (propertyCount > 0)
                {
                    var unknownE = decompressedStream.ReadInt32();
                }

                for (var i = 0; i < propertyCount; ++i)
                {
                    var attributes = new List<KeyValuePair<string, string>>();

                    var nameLength = decompressedStream.ReadInt32();
                    var name= new string(decompressedStream.ReadChars(nameLength));

                    var typeLength = decompressedStream.ReadInt32();
                    var type = new string(decompressedStream.ReadChars(typeLength));

                    var shouldCheckForAttributes = true;

                    var typeCode = decompressedStream.ReadInt32();
                    if (i == 0 && (typeCode == 0x400 || typeCode == 0x1003 || typeCode == 0x208))
                    {
                        var unknownG = decompressedStream.ReadInt32();
                        shouldCheckForAttributes = unknownG > 0;
                    }

                    if(shouldCheckForAttributes)
                    {
                        var numAttributes = decompressedStream.ReadInt32();
                        if (numAttributes > 5)
                        {
                            var attributeKeyLength = numAttributes;
                            var attributeKey = new string(decompressedStream.ReadChars(attributeKeyLength));

                            var attributeValueCode = decompressedStream.ReadInt32();
                            var attributeValueLength = decompressedStream.ReadInt32();
                            var attributeValue = new string(decompressedStream.ReadChars(attributeValueLength));

                            attributes.Add(new KeyValuePair<string, string>(attributeKey, attributeValue));
                        }
                        else
                        {
                            if (i == 0 && (typeCode == 0x400 || typeCode == 0x1003 || typeCode == 0x208))
                            {
                                var unknownZ = decompressedStream.ReadInt32();
                            }

                            for (var attributeIndex = 0; attributeIndex < numAttributes; attributeIndex++)
                            {
                                var attributeKeyLength = decompressedStream.ReadInt32();
                                var attributeKey = new string(decompressedStream.ReadChars(attributeKeyLength));
                                var attributeValueCode = decompressedStream.ReadInt32();

                                if (attributeValueCode == 0x400)
                                {
                                    var attributeValueLength = decompressedStream.ReadInt32();
                                    var attributeValue = new string(decompressedStream.ReadChars(attributeValueLength));

                                    attributes.Add(new KeyValuePair<string, string>(attributeKey, attributeValue));
                                }
                                else if (attributeValueCode == 0x208)
                                {
                                    var attributeValue = decompressedStream.ReadDouble().ToString();
                                    attributes.Add(new KeyValuePair<string, string>(attributeKey, attributeValue));
                                }
                                else
                                {
                                    var attributeValue = decompressedStream.ReadUInt32().ToString();
                                    attributes.Add(new KeyValuePair<string, string>(attributeKey, attributeValue));
                                    decompressedStream.ReadUInt32();
                                }
                            }
                        }
                    }

                    Properties.Add(new PropertyEntry()
                    {
                        Name = name,
                        Type = type,
                        Attributes = attributes
                    });
                }

                var unknownI = decompressedStream.ReadInt32();

                var stringCount = decompressedStream.ReadInt32();
                Strings = new List<KeyValuePair<string, string>>(stringCount);

                for (var stringIndex = 0; stringIndex < stringCount; ++stringIndex)
                {
                    var keyLength = decompressedStream.ReadInt32();
                    var key = new string(decompressedStream.ReadChars(keyLength));

                    var valueLength = decompressedStream.ReadInt32();
                    var value = new string(decompressedStream.ReadChars(valueLength));

                    Strings.Add(new KeyValuePair<string, string>(key, value));
                }
            }
        }
    }

    public class ScriptMetadataResourceV1 : ScriptMetadataResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var propertyCount = decompressedStream.ReadInt32();
                Properties = new List<PropertyEntry>(propertyCount);

                for (var i = 0; i < propertyCount; ++i)
                {
                    var nameLength = decompressedStream.ReadInt32();
                    var name = new string(decompressedStream.ReadChars(nameLength));

                    var typeLength = decompressedStream.ReadInt32();
                    var type = new string(decompressedStream.ReadChars(typeLength));

                    Properties.Add(new PropertyEntry()
                    {
                        Name = name,
                        Type = type
                    });
                }
            }
        }
    }
}