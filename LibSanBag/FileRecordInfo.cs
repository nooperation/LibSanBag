using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibSanBag.Providers;

namespace LibSanBag
{
    public class FileRecordInfo
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
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
            Payload,        // "payload"
            Manifest,       // "manifest"
            Debug,          // "debug"
            Capabilities,   // "capabilities"
            Null,           // "null"
            Unknown,
        }

        public enum VariantType
        {
            NoVariants,     // "noVariants"
            Server,         // "server"
            PcClient,       // "pcClient"
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
                    @"\.v(?<version_hash>[a-f0-9])" +
                    @"\.(?<payload_type>[^\.]+)" +
                    @"\.v(?<unknown_version_number>[0-9]+)" +
                    @"\.(?<variants_type>[a-zA-Z0-9]+)" +
                @")",
            RegexOptions.ExplicitCapture);

        public string Hash { get; set; }
        public string ImagePath { get; set; }
        public ResourceType Resource { get; set; }
        public string VersionHash { get; set; }
        public PayloadType Payload { get; set; }
        public int UnknownVersionNumber { get; set; }
        public VariantType Variant { get; set; }

        public bool IsRawImage => ImagePath != null;

        /// <summary>
        /// Creates a new FileRecordInfo by parsing the name of the specified filename.
        /// </summary>
        /// <param name="filename">Name of the file to attempt to create a FileRecordInfo out of</param>
        /// <returns>FileRecordInfo on success, null on failure</returns>
        public static FileRecordInfo Create(string filename)
        {
            var match = PatternRecord.Match(filename);
            if (match.Success == false)
            {
                return null;
            }

            var result = new FileRecordInfo {
                Hash = match.Groups["hash"].Value
            };

            var imageMatch = match.Groups["image_name"];

            if (imageMatch.Success)
            {
                result.ImagePath = imageMatch.Value;
            }
            else
            {
                result.Resource = GetResourceType(match.Groups["resource_type"].Value);
                result.VersionHash = match.Groups["version_hash"].Value;
                result.Payload = GetPayloadType(match.Groups["payload_type"].Value);
                result.UnknownVersionNumber = int.Parse(match.Groups["unknown_version_number"].Value);
                result.Variant = GetVariantType(match.Groups["variants_type"].Value);
            }

            return result;
        }

        /// <summary>
        /// Gets a VariantType by name
        /// </summary>
        /// <param name="variantTypeString">Name of the variant type</param>
        /// <returns>Enumeration value of the requested variant type or Unknown on failure.</returns>
        public static VariantType GetVariantType(string variantTypeString)
        {
            switch (variantTypeString)
            {
                case "noVariants": return VariantType.NoVariants;
                case "server": return VariantType.Server;
                case "pcClient": return VariantType.PcClient;
            }

            return VariantType.Unknown;
        }

        /// <summary>
        /// Gets the variant type name from a VariantType enumeration.
        /// </summary>
        /// <param name="variantType">Variant type to get the string representation of</param>
        /// <returns>String representation of the specified variant type</returns>
        public static string GetVariantTypeName(VariantType variantType)
        {
            switch (variantType)
            {
                case VariantType.NoVariants: return "noVariants";
                case VariantType.Server: return "server";
                case VariantType.PcClient: return "pcClient";
            }

            return "unknown";
        }

        /// <summary>
        /// Gets a payload type by string
        /// </summary>
        /// <param name="payloadTypeString">String representation of a payload type</param>
        /// <returns>PayloadType on success or Unknown on failure.</returns>
        public static PayloadType GetPayloadType(string payloadTypeString)
        {
            switch (payloadTypeString)
            {
                case "payload": return PayloadType.Payload;
                case "manifest": return PayloadType.Manifest;
                case "debug": return PayloadType.Debug;
                case "capabilities": return PayloadType.Capabilities;
                case "null": return PayloadType.Null;
            }

            return PayloadType.Unknown;
        }

        /// <summary>
        /// Gets a string representation of a payload type
        /// </summary>
        /// <param name="payloadType">Payload type to get the string representation of</param>
        /// <returns>String representation of the specified payload type</returns>
        public static string GetPayloadTypeName(PayloadType payloadType)
        {
            switch (payloadType)
            {
                case PayloadType.Payload: return "payload";
                case PayloadType.Manifest: return "manifest";
                case PayloadType.Debug: return "debug";
                case PayloadType.Capabilities: return "capabilities";
                case PayloadType.Null: return "null";
            }

            return "unknown";
        }

        /// <summary>
        /// Gets a resource type by string.
        /// </summary>
        /// <param name="resourceTypeString">String representation of a resource type</param>
        /// <returns>ResourceType for the specified resource type string or unknown on failure</returns>
        public static ResourceType GetResourceType(string resourceTypeString)
        {
            switch (resourceTypeString)
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
                case "Clothing-Source": return ResourceType.ClothingSource;
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
                case "Clothing-Import": return ResourceType.ClothingImport;
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
                case "Script-Import": return ResourceType.ScriptImport;
                case "Heightmap-Import": return ResourceType.HeightmapImport;
                case "hknpShape-Import": return ResourceType.hknpShapeImport;
                case "Animation-Canonical": return ResourceType.AnimationCanonical;
                case "Animation-Import": return ResourceType.AnimationImport;
            }

            return ResourceType.Unknown;
        }

        /// <summary>
        /// Gets a string representation of a resource type
        /// </summary>
        /// <param name="resourceType">Resource type to get the string representation of</param>
        /// <returns>String representation of the specified resource type</returns>
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
                case ResourceType.ClothingSource: return "Clothing-Source";
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
                case ResourceType.ClothingImport: return "Clothing-Import";
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
                case ResourceType.ScriptImport: return "Script-Import";
                case ResourceType.HeightmapImport: return "Heightmap-Import";
                case ResourceType.hknpShapeImport: return "hknpShape-Import";
                case ResourceType.AnimationCanonical: return "Animation-Canonical";
                case ResourceType.AnimationImport: return "Animation-Import";
            }

            return "Unknown";
        }

        /// <summary>
        /// Asynchronously downloads a resource. Attempts to find the latest resource version.
        /// </summary>
        /// <param name="resourceId">Hash of the resource to download</param>
        /// <param name="resourceType">Resource type</param>
        /// <param name="payloadType">Payload type</param>
        /// <param name="variantType">Variant type</param>
        /// <param name="client">Client used for downloading</param>
        /// <param name="progress">Optional progress report callback</param>
        /// <returns>Task for downloading the specified resource</returns>
        public static async Task<DownloadResults> DownloadResourceAsync(string resourceId, ResourceType resourceType, PayloadType payloadType, VariantType variantType, IHttpClientProvider client, IProgress<ProgressEventArgs> progress = null)
        {
            Exception lastException = null;

            var resourceTypeName = GetResourceTypeName(resourceType);
            var versions = AssetVersions.GetResourceVersions(resourceType);
            var payloadTypeName = GetPayloadTypeName(payloadType);
            var variantTypeName = GetVariantTypeName(variantType);

            var currentResourceIndex = 0;
            var totalResources = versions.Count;

            foreach (string version in versions)
            {
                progress?.Report(new ProgressEventArgs
                {
                    BytesDownloaded = 0,
                    CurrentResourceIndex = currentResourceIndex,
                    Resource = version,
                    Status = ProgressStatus.Idling,
                    TotalBytes = 1,
                    TotalResources = totalResources
                });

                var progressMiddleman = new Progress<ProgressEventArgs>(args =>{
                    progress?.Report(new ProgressEventArgs
                    {
                        BytesDownloaded = args.BytesDownloaded,
                        TotalBytes = args.TotalBytes,
                        Resource = args.Resource,
                        Status = args.Status,
                        // ReSharper disable once AccessToModifiedClosure
                        CurrentResourceIndex = currentResourceIndex,
                        TotalResources = totalResources
                    });
                });

                try
                {
                    var itemName = $"{ resourceId }.{ resourceTypeName}.v{version.ToLower()}.{payloadTypeName}.v0.{variantTypeName}";
                    var address = $"http://sansar-asset-production.s3-us-west-2.amazonaws.com/{itemName}";
                    var bytes = await client.GetByteArrayAsync(address, progressMiddleman).ConfigureAwait(false);

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
                    progress?.Report(new ProgressEventArgs
                    {
                        BytesDownloaded = 0,
                        CurrentResourceIndex = currentResourceIndex,
                        Resource = version,
                        Status = ProgressStatus.Error,
                        TotalBytes = 1,
                        TotalResources = totalResources
                    });
                }

                ++currentResourceIndex;
            }

            if (lastException != null)
            {
                throw lastException;
            }

            return null;
        }

        /// <summary>
        /// Asynchronously downloads a resource. Resource version must be specified.
        /// </summary>
        /// <param name="resourceId">Hash of the resource to download</param>
        /// <param name="resourceType">Resource type</param>
        /// <param name="payloadType">Payload type</param>
        /// <param name="variantType">Variant type</param>
        /// <param name="version">Version string</param>
        /// <param name="client">Client used for downloading</param>
        /// <param name="progress">Optional progress report callback</param>
        /// <returns>Task for downloading the specified resource</returns>
        public static async Task<DownloadResults> DownloadResourceAsync(string resourceId, ResourceType resourceType, PayloadType payloadType, VariantType variantType, string version, IHttpClientProvider client, IProgress<ProgressEventArgs> progress = null)
        {
            var resourceTypeName = GetResourceTypeName(resourceType);
            var payloadTypeName = GetPayloadTypeName(payloadType);
            var variantTypeName = GetVariantTypeName(variantType);

            var itemName = $"{ resourceId }.{ resourceTypeName}.v{version}.{payloadTypeName}.v0.{variantTypeName}";
            var address = $"http://sansar-asset-production.s3-us-west-2.amazonaws.com/{itemName}";

            progress?.Report(new ProgressEventArgs
            {
                BytesDownloaded = 0,
                CurrentResourceIndex = 0,
                Resource = address,
                Status = ProgressStatus.Idling,
                TotalBytes = 1,
                TotalResources = 1
            });

            var progressMiddleman = new Progress<ProgressEventArgs>(args =>
            {
                progress?.Report(new ProgressEventArgs
                {
                    BytesDownloaded = args.BytesDownloaded,
                    TotalBytes = args.TotalResources,
                    Resource = args.Resource,
                    Status = args.Status,
                    CurrentResourceIndex = 0,
                    TotalResources = 1
                });
            });

            return new DownloadResults
            {
                Name = itemName,
                Version = version,
                Bytes = await client.GetByteArrayAsync(address, progressMiddleman).ConfigureAwait(false)
            };
        }
    }
}
