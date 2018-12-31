﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources.Legacy
{
    public abstract class ScriptMetadataResourceLegacy : BaseFileResource
    {
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

        public struct PropertyEntry
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public List<KeyValuePair<string, string>> Attributes { get; set; }
        }

        public string ScriptSourceTextName { get; set; }
        public string AssemblyName { get; set; }
        public string Warnings { get; set; }
        public List<PropertyEntry> Properties { get; set; } = new List<PropertyEntry>();
        public List<KeyValuePair<string, string>> Strings { get; set; } = new List<KeyValuePair<string, string>>();

        public virtual KeyValuePair<string, string> ReadAttribute(BinaryReader decompressedStream)
        {
            var attributeKey = ReadString(decompressedStream);
            var attributeValueCode = decompressedStream.ReadInt32();

            // NOTE: Sansar only seems to store Int64 and Double types for value types
            object attributeValue = null;
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

            return new KeyValuePair<string, string>(attributeKey, attributeValue.ToString());
        }

        public virtual PropertyEntry ReadProperty(BinaryReader decompressedStream, bool isFirstProperty, ref bool hasEncounteredFirstAttribute)
        {
            var name = ReadString(decompressedStream);
            var type = ReadString(decompressedStream);
            var typeCode = decompressedStream.ReadInt32();

            var attributes = new List<KeyValuePair<string, string>>();
            var attributesAreAvailable = true;
            if (isFirstProperty)
            {
                attributesAreAvailable = decompressedStream.ReadInt32() != 0;
            }

            if (attributesAreAvailable)
            {
                var numAttributes = decompressedStream.ReadInt32();
                if (numAttributes > 0)
                {
                    if (hasEncounteredFirstAttribute == false)
                    {
                        // Is this a version? it seems to increment every few versions (currently 7 with 40d13e1007d2d696 and 6 with bae7f85fc2f176e7)
                        var unknown_five = decompressedStream.ReadInt32();
                        hasEncounteredFirstAttribute = true;
                    }

                    for (var attributeIndex = 0; attributeIndex < numAttributes; attributeIndex++)
                    {
                        var attribute = ReadAttribute(decompressedStream);
                        attributes.Add(attribute);
                    }
                }
            }

            return new PropertyEntry()
            {
                Name = name,
                Type = type,
                Attributes = attributes
            };
        }

        public virtual string ReadString(BinaryReader decompressedStream)
        {
            var textLength = decompressedStream.ReadInt32();
            var text = new string(decompressedStream.ReadChars(textLength));

            return text;
        }

        public static bool IsLegacyVersion(string version)
        {
            if(version.Length == 16)
            {
                return true;
            }

            return false;
        }

        public static ScriptMetadataResourceLegacy Create(string version)
        {
            switch (version)
            {
                case "6f2e88a41a7f1dce":
                    return new ScriptMetadataResource_6f2e88a41a7f1dce();
                case "123cecc882e4a53f":
                case "d37572c792d9190a":
                case "6bec8a6d0387ee27":
                case "40d13e1007d2d696":
                case "bae7f85fc2f176e7":
                    return new ScriptMetadataResource_bae7f85fc2f176e7();
                case "67df52a55a73f7d3":
                    return new ScriptMetadataResource_67df52a55a73f7d3();
                case "02575c46762a7c3c":
                case "d97016058b281211":
                case "0b604dc8c94bc188":
                case "b8e35358a76fa32a":
                    return new ScriptMetadataResource_b8e35358a76fa32a();
                case "d75de17df1892f86":
                    return new ScriptMetadataResource_d75de17df1892f86();
                case "0a316ea155e30eda":
                    return new ScriptMetadataResource_0a316ea155e30eda();
                default:
                    throw new Exception("Invalid legacy version: " + version);
            }
        }

        public static Type GetTypeFor(string version)
        {
            switch (version)
            {
                case "6f2e88a41a7f1dce":
                    return typeof(ScriptMetadataResource_6f2e88a41a7f1dce);
                case "123cecc882e4a53f":
                case "d37572c792d9190a":
                case "6bec8a6d0387ee27":
                case "40d13e1007d2d696":
                case "bae7f85fc2f176e7":
                    return typeof(ScriptMetadataResource_bae7f85fc2f176e7);
                case "67df52a55a73f7d3":
                    return typeof(ScriptMetadataResource_67df52a55a73f7d3);
                case "02575c46762a7c3c":
                case "d97016058b281211":
                case "0b604dc8c94bc188":
                case "b8e35358a76fa32a":
                    return typeof(ScriptMetadataResource_b8e35358a76fa32a);
                case "d75de17df1892f86":
                    return typeof(ScriptMetadataResource_d75de17df1892f86);
                case "0a316ea155e30eda":
                    return typeof(ScriptMetadataResource_0a316ea155e30eda);
                default:
                    throw new Exception("Invalid legacy version: " + version);
            }
        }
    }
}