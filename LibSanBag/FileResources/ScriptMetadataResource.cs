﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public abstract class ScriptMetadataResource : BaseFileResource
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
        public string AssemblyTooltip { get; set; }
        public string Warnings { get; set; }
        public string DisplayName { get; set; }
        public List<PropertyEntry> Properties { get; set; } = new List<PropertyEntry>();
        public List<KeyValuePair<string, string>> Strings { get; set; } = new List<KeyValuePair<string, string>>();

        
        public int UnknownA { get; set; }
        public int UnknownB { get; set; }
        public int UnknownC { get; set; }
        public int ScriptCount { get; set; }
        public int UnknownE { get; set; }
        public int UnknownF { get; set; }
        public int UnknownG { get; set; }
        public string UnknownName1 { get; set; }
        public string UnknownName2 { get; set; }
        public bool HasAssemblytooltip { get; set; }

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

        public static ScriptMetadataResource Create(string version = "")
        {
            switch (version)
            {
                default:
                case "1":
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
            }
        }

        public static Type GetTypeFor(string version = "")
        {
            switch (version)
            {
                default:
                case "1":
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
            }
        }
    }

    public class ScriptMetadataResource_6f2e88a41a7f1dce : ScriptMetadataResource
    {
        public override bool IsCompressed => true;

        public virtual void ReadScript(BinaryReader decompressedStream, bool isFirstScript, ref bool arePropertiesAvailable, ref bool hasEncounteredFirstAttribute)
        {
            AssemblyName = ReadString(decompressedStream);
            if(UnknownE > 1)
            {
                UnknownF = decompressedStream.ReadInt32();
            }

            if(isFirstScript)
            {
                var Unknown0G = decompressedStream.ReadInt32();
                arePropertiesAvailable = (decompressedStream.ReadInt32() == 1);
            }

            if (arePropertiesAvailable)
            {
                var propertyCount = decompressedStream.ReadInt32();
                Properties = new List<PropertyEntry>(propertyCount);

                if (propertyCount > 0)
                {
                    for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                    {
                        var property = ReadProperty(decompressedStream, propertyIndex == 0, ref hasEncounteredFirstAttribute);
                        Properties.Add(property);
                    }
                }
            }

            if(UnknownE == 3)
            {
                UnknownName1 = ReadString(decompressedStream); // Script Display name?
                UnknownName2 = ReadString(decompressedStream); // Script Assembly name?
            }
        }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                Warnings = ReadString(decompressedStream);

                UnknownA = decompressedStream.ReadInt32(); // 0
                UnknownB = decompressedStream.ReadInt32(); // 0

                UnknownC = decompressedStream.ReadInt32();
                if (UnknownC > 0)
                {
                    ScriptCount = decompressedStream.ReadInt32();

                    if (ScriptCount > 0)
                    {
                        UnknownE = decompressedStream.ReadInt32();
                    }
                }

                var hasEncounteredFirstAttribute = false;
                var arePropertiesAvailable = false;
                for (int i = 0; i < ScriptCount; i++)
                {
                    ReadScript(decompressedStream, i == 0, ref arePropertiesAvailable, ref hasEncounteredFirstAttribute);
                }

                if (ScriptCount > 0)
                {
                    // Maybe default script? i don't know
                    DisplayName = ReadString(decompressedStream);
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

                // I'm sure there's a flag or something for this...
                if(decompressedStream.BaseStream.Length - decompressedStream.BaseStream.Position >= 4)
                {
                    HasAssemblytooltip = true;
                    AssemblyTooltip = ReadString(decompressedStream);
                }
            }
        }
    }

    public class ScriptMetadataResource_bae7f85fc2f176e7 : ScriptMetadataResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                Warnings = ReadString(decompressedStream);

                var unknownA = decompressedStream.ReadInt32(); // 0
                var unknownB = decompressedStream.ReadInt32(); // 0

                var unknownC = decompressedStream.ReadInt32();
                var unknownD = 0;
                if (unknownC > 0)
                {
                    unknownD = decompressedStream.ReadInt32();
                    if (unknownD > 0)
                    {
                        var unknownE = decompressedStream.ReadInt32();
                        AssemblyName = ReadString(decompressedStream);
                    }
                }

                var propertiesAreAvailable = decompressedStream.ReadInt32();
                if (propertiesAreAvailable > 0)
                {
                    var hasEncounteredFirstAttribute = false;
                    var propertyCount = decompressedStream.ReadInt32();
                    Properties = new List<PropertyEntry>(propertyCount);

                    if (propertyCount > 0)
                    {
                        var unknownF = decompressedStream.ReadInt32();
                        for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                        {
                            var property = ReadProperty(decompressedStream, propertyIndex == 0, ref hasEncounteredFirstAttribute);
                            Properties.Add(property);
                        }
                    }
                }

                if (unknownD > 0)
                {
                    var assemblyNameAgain = ReadString(decompressedStream);
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
            }
        }
    }

    public class ScriptMetadataResource_67df52a55a73f7d3 : ScriptMetadataResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                Warnings = ReadString(decompressedStream);

                var unknownA = decompressedStream.ReadInt32(); // 0
                var unknownB = decompressedStream.ReadInt32(); // 0

                var propertiesAreAvailable = decompressedStream.ReadInt32() != 0;
                if (propertiesAreAvailable)
                {
                    var hasEncounteredFirstAttribute = false;
                    var propertyCount = decompressedStream.ReadInt32();
                    Properties = new List<PropertyEntry>(propertyCount);

                    if (propertyCount > 0)
                    {
                        var unknownE = decompressedStream.ReadInt32();
                        for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                        {
                            var property = ReadProperty(decompressedStream, propertyIndex == 0, ref hasEncounteredFirstAttribute);
                            Properties.Add(property);
                        }
                    }
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
            }
        }
    }

    public class ScriptMetadataResource_b8e35358a76fa32a : ScriptMetadataResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ScriptSourceTextName = ReadString(decompressedStream);
                Warnings = ReadString(decompressedStream);

                var unknownA = decompressedStream.ReadInt32(); // 0
                var unknownB = decompressedStream.ReadInt32(); // 0

                var propertiesAreAvailable = decompressedStream.ReadInt32() != 0;
                if (propertiesAreAvailable)
                {
                    var hasEncounteredFirstAttribute = false;
                    var propertyCount = decompressedStream.ReadInt32();
                    Properties = new List<PropertyEntry>(propertyCount);

                    if (propertyCount > 0)
                    {
                        var unknownE = decompressedStream.ReadInt32();
                        for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                        {
                            var property = ReadProperty(decompressedStream, propertyIndex == 0, ref hasEncounteredFirstAttribute);
                            Properties.Add(property);
                        }
                    }
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
            }
        }
    }

    public class ScriptMetadataResource_d75de17df1892f86 : ScriptMetadataResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                Warnings = ReadString(decompressedStream);

                var unknownA = decompressedStream.ReadInt32(); // 0
                var unknownB = decompressedStream.ReadInt32(); // 0
                var unknownC = decompressedStream.ReadInt32(); // 0

                var propertiesAreAvailable = decompressedStream.ReadInt32() != 0;
                if (propertiesAreAvailable)
                {
                    var hasEncounteredFirstAttribute = false;
                    var propertyCount = decompressedStream.ReadInt32();
                    Properties = new List<PropertyEntry>(propertyCount);

                    if (propertyCount > 0)
                    {
                        var unknownE = decompressedStream.ReadInt32();
                        for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                        {
                            var property = ReadProperty(decompressedStream, propertyIndex == 0, ref hasEncounteredFirstAttribute);
                            Properties.Add(property);
                        }
                    }
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
            }
        }
    }

    public class ScriptMetadataResource_0a316ea155e30eda : ScriptMetadataResource
    {
        public override bool IsCompressed => true;

        public PropertyEntry ReadProperty(BinaryReader decompressedStream)
        {
            var name = ReadString(decompressedStream);
            var type = ReadString(decompressedStream);

            return new PropertyEntry()
            {
                Attributes = new List<KeyValuePair<string, string>>(),
                Name = name,
                Type = type
            };
        }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                Warnings = "";
                Strings = new List<KeyValuePair<string, string>>();

                var propertyCount = decompressedStream.ReadInt32();
                Properties = new List<PropertyEntry>(propertyCount);

                if (propertyCount > 0)
                {
                    for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                    {
                        var property = ReadProperty(decompressedStream);
                        Properties.Add(property);
                    }
                }
            }
        }
    }
}
