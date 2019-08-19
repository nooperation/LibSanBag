using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public abstract class ScriptMetadataResource : BaseFileResource
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct TypeCode
        {
            public const int System_Boolean = 0x4101;
            public const int System_SByte = 0x101;
            public const int System_Byte = 0x8101;
            public const int System_Int16 = 0x102;
            public const int System_UInt16 = 0x8102;
            public const int System_Int32 = 0x104;
            public const int System_UInt32 = 0x8104;
            public const int System_Int64 = 0x108;
            public const int System_UInt64 = 0x8108;
            public const int System_Single = 0x204;
            public const int System_Double = 0x208;
            public const int System_String = 0x400;
            public const int System_Object = 0x800;
            public const int Sansar_Simulation_RigidBodyComponent = 0x802;
            public const int Sansar_Simulation_AnimationComponent = 0x801;
            public const int Sansar_Simulation_AudioComponent = 0x803;
            public const int Sansar_Simulation_ClusterResource = 0x2002;
            public const int Sansar_Simulation_SoundResource = 0x2003;
            public const int Mono_Simd_Vector4f = 0x1001;
            public const int Sansar_Vector = 0x1002;
            public const int Sansar_Quaternion = 0x1003;
        }

        public static Dictionary<int, string> TypeCodeToNameMap = new Dictionary<int, string>()
        {
            {TypeCode.System_Boolean, "System.Boolean"},
            {TypeCode.System_SByte, "System.SByte"},
            {TypeCode.System_Byte, "System.Byte"},
            {TypeCode.System_Int16, "System.Int16"},
            {TypeCode.System_UInt16, "System.UInt16"},
            {TypeCode.System_Int32, "System.Int32"},
            {TypeCode.System_UInt32, "System.UInt32"},
            {TypeCode.System_Int64, "System.Int64"},
            {TypeCode.System_UInt64, "System.UInt64"},
            {TypeCode.System_Single, "System.Single"},
            {TypeCode.System_Double, "System.Double"},
            {TypeCode.System_String, "System.String"},
            {TypeCode.System_Object, "System.Object"},
            {TypeCode.Sansar_Simulation_RigidBodyComponent, "Sansar.Simulation.RigidBodyComponent"},
            {TypeCode.Sansar_Simulation_AnimationComponent, "Sansar.Simulation.AnimationComponent"},
            {TypeCode.Sansar_Simulation_AudioComponent, "Sansar.Simulation.AudioComponent"},
            {TypeCode.Sansar_Simulation_ClusterResource, "Sansar.Simulation.ClusterResource"},
            {TypeCode.Sansar_Simulation_SoundResource, "Sansar.Simulation.SoundResource"},
            {TypeCode.Mono_Simd_Vector4f, "Mono.Simd.Vector4f"},
            {TypeCode.Sansar_Vector, "Sansar.Vector"},
            {TypeCode.Sansar_Quaternion, "Sansar.Quaternion"},
        };

        public struct PropertyEntry
        {
            public string Name { get; set; }
            public string Type { get; set; }
            //public int TypeCode { get; internal set; }
            public List<PropertyAttribute> Attributes { get; set; }
        }

        public struct PropertyAttribute
        {
            public string Name { get; set; }
            public string Type { get; set; }
            //public int TypeCode { get; internal set; }
            public object Value { get; set; }
        }

        public class ScriptMetadata
        {
            public string ClassName { get; set; }
            public string DisplayName { get; set; }
            public string Tooltip { get; set; }
            public int UnknownA { get; set; }

            public List<PropertyEntry> Properties { get; set; } = new List<PropertyEntry>();

            public override string ToString()
            {
                return ClassName;
            }
        }

        public string AssemblyTooltip { get; set; }
        public string ScriptSourceTextName { get; set; }
        public string BuildWarnings { get; set; }
        public string DefaultScript { get; set; }
        public string OtherWarnings { get; set; }
        public int UnknownB { get; set; }
        public int ScriptCount { get; set; }
        public bool HasAssemblyTooltip { get; set; }
        public int AttributesVersion { get; set; }

        public List<ScriptMetadata> Scripts { get; set; } = new List<ScriptMetadata>();
        public List<KeyValuePair<string, string>> Strings { get; set; } = new List<KeyValuePair<string, string>>();

        public virtual string ReadString(BinaryReader decompressedStream)
        {
            var textLength = decompressedStream.ReadInt32();
            var text = new string(decompressedStream.ReadChars(textLength));

            return text;
        }

        public static ScriptMetadataResource Create(string version = "")
        {
            return new ScriptMetadataResource_v1();
        }

        public static Type GetTypeFor(string version = "")
        {
            return typeof(ScriptMetadataResource_v1);
        }
    }

    public class ScriptMetadataResource_v1 : ScriptMetadataResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ResourceVersion = decompressedStream.ReadInt32();
                if(ResourceVersion < 2)
                {
                    // TODO: Some really old stuff here
                    return;
                }

                if(ResourceVersion < 3)
                {
                    ScriptSourceTextName = ReadString(decompressedStream);
                }

                BuildWarnings = ReadString(decompressedStream);
                OtherWarnings = ReadString(decompressedStream); // Error strings? unknown strings
                UnknownB = decompressedStream.ReadInt32(); // 0, property type code or something. i don't know

                if(ResourceVersion >= 4)
                {
                    Scripts = ParseScripts_V4(decompressedStream);
                    DefaultScript = ReadString(decompressedStream);
                }
                else
                {
                    // TODO: See if we covered this in the legacy parsers
                    ScriptMetadata script = new ScriptMetadata();
                    script.ClassName = "";
                    script.DisplayName = "";
                    script.Properties = new List<PropertyEntry>();

                    ParseScriptPayload_V4(decompressedStream, script);

                    Scripts = new List<ScriptMetadata>() { script };
                }

                var stringsAreAvailable = decompressedStream.ReadInt32() != 0;
                if (stringsAreAvailable)
                {
                    var stringCount = decompressedStream.ReadInt32();
                    Strings = new List<KeyValuePair<string, string>>(stringCount);

                    for (var stringIndex = 0; stringIndex < stringCount; ++stringIndex)
                    {
                        var key = ReadString(decompressedStream);
                        var value = ReadString(decompressedStream);

                        Strings.Add(new KeyValuePair<string, string>(key, value));
                    }
                }

                if(ResourceVersion >= 5) 
                {
                    HasAssemblyTooltip = true;
                    AssemblyTooltip = ReadString(decompressedStream);
                }
            }
        }

        private int? ScriptsVersion = null;
        private List<ScriptMetadata> ParseScripts_V4(BinaryReader decompressedStream)
        {
            if(ScriptsVersion == null)
            {
                ScriptsVersion = decompressedStream.ReadInt32();
            }

            var scriptCount = decompressedStream.ReadInt32();

            List<ScriptMetadata> scripts = new List<ScriptMetadata>();
            for (int scriptIndex = 0; scriptIndex < scriptCount; scriptIndex++)
            {
                var script = ParseScript_V4(decompressedStream);
                scripts.Add(script);
            }

            return scripts;
        }

        private int? ScriptVersion = null;
        private ScriptMetadata ParseScript_V4(BinaryReader decompressedStream)
        {
            if(ScriptVersion == null)
            {
                ScriptVersion = decompressedStream.ReadInt32();
            }

            var script = new ScriptMetadata();
            script.Properties = new List<PropertyEntry>();
            script.ClassName = ReadString(decompressedStream);

            if (ScriptVersion >= 2)
            {
                // Unknown - does not seem to affect resource parsing (output only)
                script.UnknownA = decompressedStream.ReadInt32();
            }

            ParseScriptPayload_V4(decompressedStream, script);

            if(ScriptVersion >= 3)
            {
                script.DisplayName = ReadString(decompressedStream);
                script.Tooltip = ReadString(decompressedStream);
            }

            return script;
        }

        private int? ScriptPayloadVersion = null;
        private void ParseScriptPayload_V4(BinaryReader decompressedStream, ScriptMetadata script)
        {
            if (ScriptPayloadVersion == null)
            {
                ScriptPayloadVersion = decompressedStream.ReadInt32();
            }

            var propertyCount = decompressedStream.ReadInt32();

            script.Properties = new List<PropertyEntry>();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                var prop = ParseScriptProperty_V4(decompressedStream);
                script.Properties.Add(prop);
            }
        }

        private int? PropertyVersion = null;
        private PropertyEntry ParseScriptProperty_V4(BinaryReader decompressedStream)
        {
            if (PropertyVersion == null)
            {
                PropertyVersion = decompressedStream.ReadInt32();
            }

            var prop = new PropertyEntry();
            prop.Name = ReadString(decompressedStream);
            prop.Type = ReadString(decompressedStream);
            var typeCode = decompressedStream.ReadInt32();
            prop.Attributes = ReadScriptMetadata_Property_Attributes(decompressedStream);

            return prop;
        }

        private int? AttributeVersion = null;
        private List<PropertyAttribute> ReadScriptMetadata_Property_Attributes(BinaryReader decompressedStream)
        {
            if (AttributeVersion == null)
            {
                AttributeVersion = decompressedStream.ReadInt32();
            }

            var numAttributes = decompressedStream.ReadInt32();

            var attributes = new List<PropertyAttribute>();
            for (var attributeIndex = 0; attributeIndex < numAttributes; attributeIndex++)
            {
                var attr = ReadScriptMetadata_Attribute_Payload(decompressedStream);
                attributes.Add(attr);
            }

            return attributes;
        }

        private int? AttributePayloadVersion = null;
        private PropertyAttribute ReadScriptMetadata_Attribute_Payload(BinaryReader decompressedStream)
        {
            if (AttributePayloadVersion == null)
            {
                AttributePayloadVersion = decompressedStream.ReadInt32();
            }

            var attribute = new PropertyAttribute();
            attribute.Name = ReadString(decompressedStream);
            attribute.Value = "";

            if (AttributePayloadVersion < 6)
            {
                // Do some nasty crap here. NO-OP stuff?
            }

            var typeCode = decompressedStream.ReadInt32();
            if(TypeCodeToNameMap.ContainsKey(typeCode))
            {
                attribute.Type = TypeCodeToNameMap[typeCode];
            }
            else
            {
                attribute.Type = "!UNKNOWN!";
            }

            bool isMethodA;
            if ((typeCode & 0xF0000000) > 0)
            {
                isMethodA = (typeCode == 0x20000000);
            }
            else
            {
                isMethodA = ((typeCode >> 28) & 1) > 0;
            }

            if(isMethodA)
            {
                // TODO: Method A
                ReadScriptMetadata_Attribute_Payload_MethodA(decompressedStream);

                attribute.Value = "TODO: MethodA";
                return attribute;
            }

            bool isMethodB = false;
            if ((typeCode & 0xF0000000) > 0)
            {
                isMethodB = (typeCode == 0x10000000);
            }
            else
            {
                isMethodB = ((typeCode >> 28) & 1) > 0;
            }

            if(isMethodB)
            {
                // TODO: Method B
                ReadScriptMetadata_Property_Attributes(decompressedStream);

                attribute.Value = "TODO: MethodB";
                return attribute;
            }

            // TODO: Method C - This is the standard case. Stream just points to a value we need to read.
            attribute.Value = ReadScriptMetadata_Attribute_Payload_MethodC(decompressedStream, typeCode, AttributePayloadVersion >= 11);
            return attribute;
        }

        private void ReadScriptMetadata_Attribute_Payload_MethodA(BinaryReader decompressedStream)
        {
            if (AttributePayloadVersion == null)
            {
                AttributePayloadVersion = decompressedStream.ReadInt32();
            }

            var numAttributes = decompressedStream.ReadInt32();

            for (int i = 0; i < numAttributes; i++)
            {
                var unknown = ReadString(decompressedStream);
                ReadScriptMetadata_Attribute_Payload(decompressedStream);
            }
        }



        private object ReadScriptMetadata_Attribute_Payload_MethodC(BinaryReader decompressedStream, int attributeValueCode, bool isNewVersion)
        {
            object attributeValue = new object();

            switch (attributeValueCode)
            {
                case TypeCode.System_Boolean: // 4101
                    attributeValue = decompressedStream.ReadUInt64() != 0;
                    break;
                case TypeCode.System_SByte: //  101
                    attributeValue = decompressedStream.ReadSByte();
                    break;
                case TypeCode.System_Byte: // 8101
                    attributeValue = decompressedStream.ReadByte();
                    break;
                case TypeCode.System_Int16: //  102
                    attributeValue = decompressedStream.ReadInt16();
                    break;
                case TypeCode.System_UInt16: // 8102
                    attributeValue = decompressedStream.ReadUInt16();
                    break;
                case TypeCode.System_Int32: //  104
                    attributeValue = decompressedStream.ReadInt32();
                    break;
                case TypeCode.System_UInt32: // 8104
                    attributeValue = decompressedStream.ReadUInt32();
                    break;
                case TypeCode.System_Int64: //  108
                    attributeValue = decompressedStream.ReadInt64();
                    break;
                case TypeCode.System_UInt64: // 8108
                    attributeValue = decompressedStream.ReadUInt64();
                    break;
                case TypeCode.System_Single: //  204
                    attributeValue = decompressedStream.ReadSingle();
                    break;
                case TypeCode.System_Double: //  208
                    attributeValue = decompressedStream.ReadDouble();
                    break;
                case TypeCode.System_String: //  400
                case TypeCode.System_Object: //  800
                case TypeCode.Sansar_Simulation_RigidBodyComponent: //  802
                case TypeCode.Sansar_Simulation_AnimationComponent: //  801
                case TypeCode.Sansar_Simulation_AudioComponent: //  803
                case TypeCode.Sansar_Simulation_ClusterResource: // 2002
                case TypeCode.Sansar_Simulation_SoundResource: // 2003
                case TypeCode.Mono_Simd_Vector4f: // 1001
                case TypeCode.Sansar_Vector: // 1002
                case TypeCode.Sansar_Quaternion: // 1003
                    attributeValue = ReadString(decompressedStream);
                    break;
                default:
                    attributeValue = new object();
                    break;
            }

            return attributeValue;
        }
    }
}
