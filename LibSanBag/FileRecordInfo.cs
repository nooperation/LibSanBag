using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibSanBag.Providers;

namespace LibSanBag
{
    public class FileRecordInfo
    {
        public enum ResourceType
        {
            BankResource,
            BlueprintResource,
            ClusterDefinition,
            EnvironmentResource,
            GeometryResourceResource,
            ModelMorphResource,
            PickResource,
            SoundResource,
            TextureResource,
            TextureSource,
            WorldChunkDefinition,
            WorldSource,
            ScriptMetadataResource,
            ScriptCompiledBytecodeResource,
            ScriptSourceTextResource,
            ClothingSource,
            ClusterSource,
            WorldChunkSource,
            LicenseResource,
            WorldDefinition,
            AudioMaterialResource,
            MaterialResource,
            ScriptResource,
            hkaAnimationBindingResource,
            hkaSkeletonResource,
            hknpMaterialResource,
            hknpPhysicsSystemDataResource,
            hknpShapeResource,
            ClothingImport,
            FileResource,
            TextureImport,
            GeometryResourceCanonical,
            GeometryResourceImport,
            SoundImport,
            hkbBehaviorGraphResource,
            hkbCharacterDataResource,
            LuaScriptResource,
            AudioGraphResource,
            TerrainRuntimeTextureData,
            TerrainRuntimeDecalTextureData,
            BehaviorProjectData,
            hkbProjectDataResource,
            PickableModelResource,
            SpeechGraphicsEngineResource,
            SpeechGraphicsAnimationResource,
            VertexDefinitionResourceResource,
            BufferResource,
            MeshResource,
            TerrainRuntimeData,
            TerrainSourceData,
            hkpConstraintDataResource,
            hkaSkeletonMapperResource,
            hknpRagdollDataResource,
            TestResource,
            ScriptImport,
            HeightmapImport,
            hknpShapeImport,
            AnimationCanonical,
            AnimationImport,
            Unknown,

        }

        public enum PayloadType
        {
            Payload,
            Manifest,
            Unknown,
        }

        public enum VariantType
        {
            NoVariants,     // "noVariants"
            Server,         // "server"
            PcClient,       // "pcClient"
            Payload,        // "payload"
            Manifest,       // "manifest"
            Debug,          // "debug"
            Capabilities,   // "capabilities"
            Null,           // "null"
            Unknown
        }

        public class DownloadResults
        {
            public string Name { get; set; }
            public string Version { get; set; }
            public byte[] Bytes { get; set; }
        }

        public static Regex PatternRecord = new Regex(
            @"(?<hash>[a-f0-9]{32})\." +
                @"((?<image_name>.*\.png)|" +
                @"(?<resource_type>[^\.]+)" +
                    @"\.v(?<version_hash>[a-f0-9]{16})" +
                    @"\.(?<payload_type>[^\.]+)" +
                    @"\.v(?<version_number>[0-9]+)" +
                    @"\.(?<variants_type>[a-zA-Z0-9]+)" +
                @")",
            RegexOptions.ExplicitCapture);

        public string Hash { get; set; }
        public string ImagePath { get; set; }
        public ResourceType Resource { get; set; }
        public string VersionHash { get; set; }
        public PayloadType Payload { get; set; }
        public int VersionNumber { get; set; }
        public VariantType Variant { get; set; }

        public bool IsRawImage => ImagePath != null;

        public static FileRecordInfo Create(string filename)
        {
            var match = PatternRecord.Match(filename);
            if (match.Success == false)
            {
                return null;
            }

            var result = new FileRecordInfo();
            result.Hash = match.Groups["hash"].Value;
            var image_match = match.Groups["image_name"];

            if (image_match.Success)
            {
                result.ImagePath = image_match.Value;
            }
            else
            {
                result.Resource = GetResourceType(match.Groups["resource_type"].Value);
                result.VersionHash = match.Groups["version_hash"].Value;
                result.Payload = GetPayloadType(match.Groups["payload_type"].Value);
                result.VersionNumber = int.Parse(match.Groups["version_number"].Value);
                result.Variant = GetVariantType(match.Groups["variants_type"].Value);
            }

            return result;
        }

        public static VariantType GetVariantType(string variant_type_string)
        {
            switch (variant_type_string)
            {
                case "noVariants": return VariantType.NoVariants;
                case "server": return VariantType.Server;
                case "pcClient": return VariantType.PcClient;
                case "payload": return VariantType.Payload;
                case "manifest": return VariantType.Manifest;
                case "debug": return VariantType.Debug;
                case "capabilities": return VariantType.Capabilities;
                case "null": return VariantType.Null;
            }

            return VariantType.Unknown;
        }

        public static string GetVariantTypeName(VariantType variant_type)
        {
            switch (variant_type)
            {
                case VariantType.NoVariants: return "noVariants";
                case VariantType.Server: return "server";
                case VariantType.PcClient: return "pcClient";
                case VariantType.Payload: return "payload";
                case VariantType.Manifest: return "manifest";
                case VariantType.Debug: return "debug";
                case VariantType.Capabilities: return "capabilities";
                case VariantType.Null: return "null";
            }

            return "unknown";
        }

        public static PayloadType GetPayloadType(string payload_type_string)
        {
            switch (payload_type_string)
            {
                case "payload": return PayloadType.Payload;
                case "manifest": return PayloadType.Manifest;
            }

            return PayloadType.Unknown;
        }

        public static string GetPayloadTypeName(PayloadType payload_type)
        {
            switch (payload_type)
            {
                case PayloadType.Payload: return "payload";
                case PayloadType.Manifest: return "manifest";
            }

            return "unknown";
        }

        public static ResourceType GetResourceType(string resource_type_string)
        {
            switch (resource_type_string)
            {
                case "Bank-Resource": return ResourceType.BankResource;
                case "Blueprint-Resource": return ResourceType.BlueprintResource;
                case "Cluster-Definition": return ResourceType.ClusterDefinition;
                case "Environment-Resource": return ResourceType.EnvironmentResource;
                case "GeometryResource-Resource": return ResourceType.GeometryResourceResource;
                case "ModelMorph-Resource": return ResourceType.ModelMorphResource;
                case "Pick-Resource": return ResourceType.PickResource;
                case "Sound-Resource": return ResourceType.SoundResource;
                case "Texture-Resource": return ResourceType.TextureResource;
                case "Texture-Source": return ResourceType.TextureSource;
                case "WorldChunk-Definition": return ResourceType.WorldChunkDefinition;
                case "World-Source": return ResourceType.WorldSource;
                case "ScriptMetadata-Resource": return ResourceType.ScriptMetadataResource;
                case "ScriptCompiledBytecode-Resource": return ResourceType.ScriptCompiledBytecodeResource;
                case "ScriptSourceText-Resource": return ResourceType.ScriptSourceTextResource;
                case "Cluster-Source": return ResourceType.ClusterSource;
                case "WorldChunk-Source": return ResourceType.WorldChunkSource;
                case "License-Resource": return ResourceType.LicenseResource;
                case "World-Definition": return ResourceType.WorldDefinition;
                case "AudioMaterial-Resource": return ResourceType.AudioMaterialResource;
                case "Material-Resource": return ResourceType.MaterialResource;
                case "Script-Resource": return ResourceType.ScriptResource;
                case "hkaAnimationBinding-Resource": return ResourceType.hkaAnimationBindingResource;
                case "hkaSkeleton-Resource": return ResourceType.hkaSkeletonResource;
                case "hknpMaterial-Resource": return ResourceType.hknpMaterialResource;
                case "hknpPhysicsSystemData-Resource": return ResourceType.hknpPhysicsSystemDataResource;
                case "hknpShape-Resource": return ResourceType.hknpShapeResource;
                case "File-Resource": return ResourceType.FileResource;
                case "Texture-Import": return ResourceType.TextureImport;
                case "GeometryResource-Canonical": return ResourceType.GeometryResourceCanonical;
                case "GeometryResource-Import": return ResourceType.GeometryResourceImport;
                case "Sound-Import": return ResourceType.SoundImport;
                case "hkbBehaviorGraph-Resource": return ResourceType.hkbBehaviorGraphResource;
                case "hkbCharacterData-Resource": return ResourceType.hkbCharacterDataResource;
                case "LuaScript-Resource": return ResourceType.LuaScriptResource;
                case "AudioGraph-Resource": return ResourceType.AudioGraphResource;
                case "Terrain-RuntimeTextureData": return ResourceType.TerrainRuntimeTextureData;
                case "Terrain-RuntimeDecalTextureData": return ResourceType.TerrainRuntimeDecalTextureData;
                case "BehaviorProject-Data": return ResourceType.BehaviorProjectData;
                case "hkbProjectData-Resource": return ResourceType.hkbProjectDataResource;
                case "PickableModel-Resource": return ResourceType.PickableModelResource;
                case "SpeechGraphicsEngine-Resource": return ResourceType.SpeechGraphicsEngineResource;
                case "SpeechGraphicsAnimation-Resource": return ResourceType.SpeechGraphicsAnimationResource;
                case "VertexDefinitionResource-Resource": return ResourceType.VertexDefinitionResourceResource;
                case "Buffer-Resource": return ResourceType.BufferResource;
                case "Mesh-Resource": return ResourceType.MeshResource;
                case "Terrain-RuntimeData": return ResourceType.TerrainRuntimeData;
                case "Terrain-SourceData": return ResourceType.TerrainSourceData;
                case "hkpConstraintData-Resource": return ResourceType.hkpConstraintDataResource;
                case "hkaSkeletonMapper-Resource": return ResourceType.hkaSkeletonMapperResource;
                case "hknpRagdollData-Resource": return ResourceType.hknpRagdollDataResource;
                case "Test-Resource": return ResourceType.TestResource;
                case "Heightmap-Import": return ResourceType.HeightmapImport;
                case "hknpShape-Import": return ResourceType.hknpShapeImport;
                case "Animation-Canonical": return ResourceType.AnimationCanonical;
                case "Animation-Import": return ResourceType.AnimationImport;
            }

            return ResourceType.Unknown;
        }

        public static string GetResourceTypeName(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.BankResource: return "Bank-Resource";
                case ResourceType.BlueprintResource: return "Blueprint-Resource";
                case ResourceType.ClusterDefinition: return "Cluster-Definition";
                case ResourceType.EnvironmentResource: return "Environment-Resource";
                case ResourceType.GeometryResourceResource: return "GeometryResource-Resource";
                case ResourceType.ModelMorphResource: return "ModelMorph-Resource";
                case ResourceType.PickResource: return "Pick-Resource";
                case ResourceType.SoundResource: return "Sound-Resource";
                case ResourceType.TextureResource: return "Texture-Resource";
                case ResourceType.TextureSource: return "Texture-Source";
                case ResourceType.WorldChunkDefinition: return "WorldChunk-Definition";
                case ResourceType.WorldSource: return "World-Source";
                case ResourceType.ScriptMetadataResource: return "ScriptMetadata-Resource";
                case ResourceType.ScriptCompiledBytecodeResource: return "ScriptCompiledBytecode-Resource";
                case ResourceType.ScriptSourceTextResource: return "ScriptSourceText-Resource";
                case ResourceType.ClusterSource: return "Cluster-Source";
                case ResourceType.WorldChunkSource: return "WorldChunk-Source";
                case ResourceType.LicenseResource: return "License-Resource";
                case ResourceType.WorldDefinition: return "World-Definition";
                case ResourceType.AudioMaterialResource: return "AudioMaterial-Resource";
                case ResourceType.MaterialResource: return "Material-Resource";
                case ResourceType.ScriptResource: return "Script-Resource";
                case ResourceType.hkaAnimationBindingResource: return "hkaAnimationBinding-Resource";
                case ResourceType.hkaSkeletonResource: return "hkaSkeleton-Resource";
                case ResourceType.hknpMaterialResource: return "hknpMaterial-Resource";
                case ResourceType.hknpPhysicsSystemDataResource: return "hknpPhysicsSystemData-Resource";
                case ResourceType.hknpShapeResource: return "hknpShape-Resource";
                case ResourceType.FileResource: return "File-Resource";
                case ResourceType.TextureImport: return "Texture-Import";
                case ResourceType.GeometryResourceCanonical: return "GeometryResource-Canonical";
                case ResourceType.GeometryResourceImport: return "GeometryResource-Import";
                case ResourceType.SoundImport: return "Sound-Import";
                case ResourceType.hkbBehaviorGraphResource: return "hkbBehaviorGraph-Resource";
                case ResourceType.hkbCharacterDataResource: return "hkbCharacterData-Resource";
                case ResourceType.LuaScriptResource: return "LuaScript-Resource";
                case ResourceType.AudioGraphResource: return "AudioGraph-Resource";
                case ResourceType.TerrainRuntimeTextureData: return "Terrain-RuntimeTextureData";
                case ResourceType.TerrainRuntimeDecalTextureData: return "Terrain-RuntimeDecalTextureData";
                case ResourceType.BehaviorProjectData: return "BehaviorProject-Data";
                case ResourceType.hkbProjectDataResource: return "hkbProjectData-Resource";
                case ResourceType.PickableModelResource: return "PickableModel-Resource";
                case ResourceType.SpeechGraphicsEngineResource: return "SpeechGraphicsEngine-Resource";
                case ResourceType.SpeechGraphicsAnimationResource: return "SpeechGraphicsAnimation-Resource";
                case ResourceType.VertexDefinitionResourceResource: return "VertexDefinitionResource-Resource";
                case ResourceType.BufferResource: return "Buffer-Resource";
                case ResourceType.MeshResource: return "Mesh-Resource";
                case ResourceType.TerrainRuntimeData: return "Terrain-RuntimeData";
                case ResourceType.TerrainSourceData: return "Terrain-SourceData";
                case ResourceType.hkpConstraintDataResource: return "hkpConstraintData-Resource";
                case ResourceType.hkaSkeletonMapperResource: return "hkaSkeletonMapper-Resource";
                case ResourceType.hknpRagdollDataResource: return "hknpRagdollData-Resource";
                case ResourceType.TestResource: return "Test-Resource";
                case ResourceType.HeightmapImport: return "Heightmap-Import";
                case ResourceType.hknpShapeImport: return "hknpShape-Import";
                case ResourceType.AnimationCanonical: return "Animation-Canonical";
                case ResourceType.AnimationImport: return "Animation-Import";
            }

            return "Unknown";
        }

        public static async Task<DownloadResults> DownloadResourceAsync(string resourceId, ResourceType resourceType, PayloadType payloadType, VariantType variantType, IHttpClientProvider client)
        {
            Exception lastException = null;

            var resourceTypeName = GetResourceTypeName(resourceType);
            var versions = AssetVersions.GetResourceVersions(resourceType);
            var payloadTypeName = GetPayloadTypeName(payloadType);
            var variantTypeName = GetVariantTypeName(variantType);

            foreach (string version in versions)
            {
                try
                {
                    var itemName = $"{ resourceId }.{ resourceTypeName}.v{version.ToLower()}.{payloadTypeName}.v0.{variantTypeName}";
                    var address = $"http://sansar-asset-production.s3-us-west-2.amazonaws.com/{itemName}";
                    var bytes = await client.GetByteArrayAsync(address);

                    return new DownloadResults
                    {
                        Name = itemName,
                        Version = version,
                        Bytes = bytes
                    };
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }
            }

            if (lastException != null)
            {
                throw lastException;
            }

            return null;
        }

        public static async Task<DownloadResults> DownloadResourceAsync(string resourceId, ResourceType resourceType, PayloadType payloadType, VariantType variantType, string version, IHttpClientProvider client)
        {
            var resourceTypeName = GetResourceTypeName(resourceType);
            var payloadTypeName = GetPayloadTypeName(payloadType);
            var variantTypeName = GetVariantTypeName(variantType);

            var itemName = $"{ resourceId }.{ resourceTypeName}.v{version}.{payloadTypeName}.v0.{variantTypeName}";
            var address = $"http://sansar-asset-production.s3-us-west-2.amazonaws.com/{itemName}";

            return new DownloadResults
            {
                Name = itemName,
                Version = version,
                Bytes = await client.GetByteArrayAsync(address)
            };
        }
    }
}
