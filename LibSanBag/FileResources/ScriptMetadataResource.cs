using System;
using System.Collections.Generic;
using System.IO;
using static LibSanBag.FileResources.ClusterDefinitionResource;

namespace LibSanBag.FileResources
{
    public class ScriptMetadataResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static ScriptMetadataResource Create(string version = "")
        {
            return new ScriptMetadataResource();
        }

        public class ScriptProperty
        {
            public uint Version { get; set; }
            public string Name { get; set; }
            public string TypeString { get; set; }
            public uint Type { get; set; }
            public List<ScriptParameter> Attributes { get; set; }
        }
        private ScriptProperty Read_Property(BinaryReader reader)
        {
            var result = new ScriptProperty();

            result.Version = ReadVersion(reader, 1, 0x1411DD3E0);

            result.Name = ReadString(reader);
            result.TypeString = ReadString(reader);
            result.Type = reader.ReadUInt32();
            result.Attributes = Read_List(reader, cluster.Read_ScriptComponent_parameter, 1, 0x1411CF800);

            return result;
        }

        public class ScriptClass
        {
            public uint Version { get; set; }
            public string Name { get; set; }
            public uint ScriptType { get; set; }
            public List<ScriptProperty> Properties { get; set; }
            public string DisplayName { get; set; }
            public string Tooltip { get; set; }
        }
        private ScriptClass Read_ScriptClass(BinaryReader reader)
        {
            var result = new ScriptClass();

            result.Version = ReadVersion(reader, 3, 0x1411DD3F0);
            result.Name = ReadString(reader);

            if(result.Version >= 2)
            {
                result.ScriptType = reader.ReadUInt32();
            }

            result.Properties = Read_List(reader, Read_Property, 1, 0x1411D2360);

            if(result.Version >= 3)
            {
                result.DisplayName = ReadString(reader);
                result.Tooltip = ReadString(reader);
            }

            return result;
        }

        public class ScriptTag
        {
            public string Data { get; set; }
            public string Value { get; set; }
        }
        private ScriptTag Read_ScriptTag(BinaryReader reader)
        {
            var result = new ScriptTag();

            result.Data = ReadString(reader);
            result.Value = ReadString(reader);

            return result;
        }

        public class ScriptMetadata
        {
            public uint Version { get; set; }
            public List<KeyValuePair<string, string>> Parameters;
            public string SourceFileName { get; set; }
            public string Info { get; set; }
            public string Errors { get; set; }
            public uint Flags { get; set; }
            public List<ScriptClass> ScriptClasses { get; set; }
            public string DefaultScript { get; set; }
            public List<ScriptProperty> Properties { get; set; }
            public List<ScriptTag> Tags { get; set; }
            public string Tooltip { get; set; }
        }
        private ScriptMetadata Read_ScriptMetadataResource(BinaryReader reader)
        {
            var result = new ScriptMetadata();

            result.Version = ReadVersion(reader, 5, 0x141115FB0); // C3038F16B1058B48

            if (result.Version < 2)
            {
                result.Parameters = new List<KeyValuePair<string, string>>();

                var ParameterCount = reader.ReadUInt32(); // skip
                for (int i = 0; i < ParameterCount; i++)
                {
                    var ParamKey = ReadString(reader);
                    var ParamValue = ReadString(reader);

                    result.Parameters.Add(new KeyValuePair<string, string>(ParamKey, ParamValue));
                }

                return result;
            }

            if(result.Version < 3)
            {
                result.SourceFileName = ReadString(reader);
            }

            result.Info = ReadString(reader);
            result.Errors = ReadString(reader);
            result.Flags = reader.ReadUInt32();

            if(result.Version >= 4)
            {
                result.ScriptClasses = Read_List(reader, Read_ScriptClass, 1, 0x1411D2370);
                result.DefaultScript = ReadString(reader);
            }
            else
            {
                result.Properties = Read_List(reader, Read_Property, 1, 0x1411D2360);
            }

            result.Tags = Read_List(reader, Read_ScriptTag, 1, 0x1411D2380);
            if (result.Version >= 5)
            {
                result.Tooltip = ReadString(reader);
            }

            return result;
        }

        public ScriptMetadata Resource { get; set; }

        private ClusterDefinitionResource cluster = new ClusterDefinitionResource();
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            cluster.OverrideVersionMap(versionMap);
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_ScriptMetadataResource(decompressedStream);
            }
        }
    }
}
