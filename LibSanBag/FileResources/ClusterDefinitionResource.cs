using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibSanBag.FileResources
{
    public class ClusterDefinitionResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static ClusterDefinitionResource Create(string version = "")
        {
            return new ClusterDefinitionResource();
        }

        private string ReadStringVersioned(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F4E40);
            var name = ReadString(reader);

            return name;
        }

        public class GrabPointDefinition
        {
            public uint Version { get; set; }
            public uint Type { get; set; }
            public Transform LocalOffset { get; set; }
            public bool IsSticky { get; set; }
            public string BaseDefinition { get; set; }
            public bool AimAtCursor { get; set; }
        }
        private GrabPointDefinition Read_RigidBody_GrabPointDefinition(BinaryReader reader)
        {
            var result = new GrabPointDefinition();

            result.Version = ReadVersion(reader, 4, 0x14170FBC0);

            result.Type = reader.ReadUInt32();
            result.LocalOffset = Read_Transform(reader);

            if (result.Version >= 2)
            {
                result.IsSticky = reader.ReadBoolean();
            }
            if (result.Version >= 3)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }
            if (result.Version >= 4)
            {
                result.AimAtCursor = reader.ReadBoolean();
            }

            return result;
        }

        public class AudioResourcePoolSound
        {
            public uint Version { get; set; }
            public bool Enabled { get; set; }
            public int LoudnessOffset { get; set; }
            public float PitchRange { get; set; }
            public uint Data { get; set; }
            public List<string> Sounds { get; set; }
        }
        private AudioResourcePoolSound Read_RigidBody_AudioResourcePoolSound(BinaryReader reader)
        {
            var result = new AudioResourcePoolSound();

            result.Version = ReadVersion(reader, 1, 0x14120B5B0);

            result.Enabled = reader.ReadBoolean();
            result.LoudnessOffset = reader.ReadInt32();
            result.PitchRange = reader.ReadSingle();

            result.Sounds = Read_List(reader, ReadUUID, 1, 0x14121C2C0);

            return result;
        }

        public List<AudioResourcePoolSound> Read_RigidBody_AudioResourcePoolSounds(BinaryReader reader)
        {
            var result = new List<AudioResourcePoolSound>();

            var version = ReadVersion(reader, 1, 0x141203830);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var data = reader.ReadUInt32();
                var poolSound = Read_RigidBody_AudioResourcePoolSound(reader);

                poolSound.Data = data;
                result.Add(poolSound);
            }

            return result;
        }

        public class SitPointDefinition
        {
            public uint Version { get; set; }
            public uint Type { get; set; }
            public Transform LocalOffset { get; set; }
            public string BaseDefinition { get; set; }
        }
        private SitPointDefinition Read_RigidBody_SitPointDefinition(BinaryReader reader)
        {
            var result = new SitPointDefinition();

            result.Version = ReadVersion(reader, 2, 0x14170FBD0);

            result.Type = reader.ReadUInt32();
            result.LocalOffset = Read_Transform(reader);

            if (result.Version >= 2)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }

            return result;
        }

        public class SelectionBeamPointDefinition
        {
            public uint Version { get; set; }
            public Transform LocalOffset { get; set; }
            public string BaseDefinition { get; set; }
        }
        private SelectionBeamPointDefinition Read_RigidBody_SelectionBeamPointDefinition(BinaryReader reader)
        {
            var result = new SelectionBeamPointDefinition();

            result.Version = ReadVersion(reader, 1, 0x14170FBE0);

            result.LocalOffset = Read_Transform(reader);
            result.BaseDefinition = ReadStringVersioned(reader);

            return result;
        }

        public class RigidBody
        {
            public uint Version { get; set; }
            public string BodyResourceHandle { get; set; }

            [JsonIgnore]
            public byte[] BodyCinfo { get; set; }
            public int BodyCinfoLength => BodyCinfo?.Length ?? 0;

            public List<string> Materials { get; set; }
            public string Shape { get; set; }
            public string AudioMaterial { get; set; }
            public bool CanGrab { get; set; }
            public List<GrabPointDefinition> GrabPointDefinitions { get; set; }
            public List<AudioResourcePoolSound> AudioResourcePoolSounds { get; set; }
            public List<SitPointDefinition> SitPointDefinitions { get; set; }
            public bool CanRide { get; set; }
            public string BaseDefinition { get; set; }
            public List<SelectionBeamPointDefinition> SelectionBeamPointDefinitions { get; set; }
            public int EditInstanceId { get; set; }
            public int EditLinkId { get; set; }
            public uint CollisionFilterOverride { get; set; }
        }
        private RigidBody Read_RigidBody(BinaryReader reader)
        {
            var result = new RigidBody();

            result.Version = ReadVersion(reader, 13, 0x1417056C0);

            result.BodyResourceHandle = ReadUUID(reader);

            var bodyCinfoLength = reader.ReadUInt32();
            result.BodyCinfo = reader.ReadBytes((int)bodyCinfoLength);

            result.Materials = Read_List(reader, ReadUUID, 1, 0x14170C770);
            result.Shape = ReadUUID(reader);


            if(result.Version >= 2)
            {
                result.AudioMaterial = ReadUUID(reader);
            }
            if(result.Version >= 3)
            {
                result.CanGrab = reader.ReadBoolean();
            }
            if(result.Version >= 4)
            {
                result.GrabPointDefinitions = Read_List(reader, Read_RigidBody_GrabPointDefinition, 1, 0x14170CCA0);
            }
            if(result.Version >= 6)
            {
                result.AudioResourcePoolSounds = Read_RigidBody_AudioResourcePoolSounds(reader);
            }
            if(result.Version >= 7)
            {
                result.SitPointDefinitions = Read_List(reader, Read_RigidBody_SitPointDefinition, 1, 0x14170CCB0);
            }
            if(result.Version >= 8)
            {
                result.CanRide = reader.ReadBoolean();
            }
            if(result.Version >= 9)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }
            if(result.Version >= 10)
            {
                result.SelectionBeamPointDefinitions = Read_List(reader, Read_RigidBody_SelectionBeamPointDefinition, 1, 0x14170CCC0);
            }
            if(result.Version >= 12)
            {
                result.EditInstanceId = reader.ReadInt32();
                result.EditLinkId = reader.ReadInt32();
            }
            if(result.Version >= 13)
            {
                result.CollisionFilterOverride = reader.ReadUInt32();
            }

            return result;
        }

        public class AnimationComponentNode
        {
            public uint Version { get; set; }
            public string AnimationBindingUuid { get; set; }
            public string AnimationName { get; set; }
            public float PlaybackSpeed { get; set; }
            public uint StartTime { get; set; }
            public uint PlaybackMode { get; set; }
            public uint AnimationBindingRemapIndex { get; set; }
        }
        private AnimationComponentNode Read_AnimationComponent_Node(BinaryReader reader)
        {
            var result = new AnimationComponentNode();

            result.Version = ReadVersion(reader, 3, 0x14170F870);

            result.AnimationBindingUuid = ReadUUID(reader);
            result.AnimationName = ReadString(reader);

            result.PlaybackSpeed = reader.ReadSingle();
            result.StartTime = reader.ReadUInt32();
            result.PlaybackMode = reader.ReadUInt32();

            if (result.Version == 2)
            {
                result.AnimationBindingRemapIndex = reader.ReadUInt32();
            }

            return result;
        }

        public class AnimationOverride
        {
            public uint Version { get; set; }
            public string AnimationOverrideName { get; set; }
            public string AnimationName { get; set; }
            public string AnimationBindingUuid { get; set; }
            public string AnimationSkeletonName { get; set; }
        }
        private AnimationOverride Read_AnimationComponent_AnimationOverride(BinaryReader reader)
        {
            var result = new AnimationOverride();

            result.Version = ReadVersion(reader, 2, 0x14170FBF0);

            result.AnimationOverrideName = ReadString(reader);
            result.AnimationName = ReadString(reader);
            result.AnimationBindingUuid = ReadUUID(reader);

            if (result.Version >= 2)
            {
                result.AnimationSkeletonName = ReadString(reader);
            }

            return result;
        }

        private string Read_AnimationComponent_AnimationSkeletonMapper(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170FC00);

            var skeletonMapperUUID = ReadUUID(reader);

            return skeletonMapperUUID;
        }

        public class OffsetTransform
        {
            public uint Version { get; set; }

            public List<float> Translation { get; set; }
            public List<float> Quaternion { get; set; }
            public List<float> Scale { get; set; }
            public string Data { get; set; }
        }
        public OffsetTransform Read_AnimationComponent_OffsetTransform(BinaryReader reader)
        {
            var result = new OffsetTransform();

            result.Version = ReadVersion(reader, 1, 0x141205320);

            result.Translation = ReadVectorF(reader, 4);
            result.Quaternion = ReadVectorF(reader, 4);
            result.Scale = ReadVectorF(reader, 4);

            return result;
        }

        public List<OffsetTransform> Read_AnimationComponent_OffsetTransformsMap(BinaryReader reader)
        {
            var result = new List<OffsetTransform>();

            var version = ReadVersion(reader, 1, 0x1411F80D0);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var data = ReadString(reader);
                var offsetTransform = Read_AnimationComponent_OffsetTransform(reader);

                offsetTransform.Data = data;
                result.Add(offsetTransform);
            }

            return result;
        }

        public class AnimationComponent
        {
            public uint Version { get; set; }
            public string ProjectDataUuid { get; set; }
            public string ProxyShapeUuid { get; set; }
            public string SkeletonUuid { get; set; }
            public string AnimationBindingUuid { get; set; }
            public uint CreationMode { get; set; }
            public bool ClientOnly { get; set; }
            public float PlaybackSpeed { get; set; }
            public uint StartTime { get; set; }
            public uint PlaybackMode { get; set; }
            public bool BeginOnLoad { get; set; }
            public List<AnimationComponentNode> AnimationNodes { get; set; }
            public uint AnimationBindingRemapIndex { get; set; }
            public string BaseDefinition { get; set; }
            public float Scale { get; set; }
            public List<AnimationOverride> AnimationOverrides { get; set; }
            public int EditInstanceId { get; set; }
            public int EditLinkId { get; set; }
            public List<string> AnimationSkeletonMappers { get; set; }
            public List<OffsetTransform> SgOffsetTransformsMap { get; set; }
        }
        private AnimationComponent Read_AnimationComponent(BinaryReader reader)
        {
            var result = new AnimationComponent();

            result.Version = ReadVersion(reader, 12, 0x1417056D0);
            result.ProjectDataUuid = ReadUUID(reader);
            result.ProxyShapeUuid = ReadUUID(reader);
            result.SkeletonUuid = ReadUUID(reader);
            result.AnimationBindingUuid = ReadUUID(reader);

            result.CreationMode = reader.ReadUInt32();
            result.ClientOnly = reader.ReadBoolean();

            if (result.Version >= 2)
            {
                result.PlaybackSpeed = reader.ReadSingle();
                result.StartTime = reader.ReadUInt32();
            }

            if (result.Version >= 3)
            {
                result.PlaybackMode = reader.ReadUInt32();
                result.BeginOnLoad = reader.ReadBoolean();
            }

            if (result.Version < 4)
            {
                // default animation stuff
            }
            else
            {
                result.AnimationNodes = Read_List(reader, Read_AnimationComponent_Node, 1, 0x14170C780);
            }

            if ((result.Version - 5) <= 2)
            {
                result.AnimationBindingRemapIndex = reader.ReadUInt32();
            }

            if (result.Version >= 6)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }
            if (result.Version >= 7)
            {
                result.Scale = reader.ReadSingle();
            }
            if (result.Version >= 8)
            {
                result.AnimationOverrides = Read_List(reader, Read_AnimationComponent_AnimationOverride, 1, 0x14170CE40);
            }
            if (result.Version >= 9)
            {
                result.EditInstanceId = reader.ReadInt32();
                result.EditLinkId = reader.ReadInt32();
            }
            if (result.Version >= 10)
            {
                result.AnimationSkeletonMappers = Read_List(reader, Read_AnimationComponent_AnimationSkeletonMapper, 1, 0x14170CE50);
            }

            if (result.Version == 11)
            {
                result.SgOffsetTransformsMap = Read_AnimationComponent_OffsetTransformsMap(reader);
            }

            return result;
        }

        public class PoseComponent
        {
            public uint Version { get; set; }
        }
        private PoseComponent Read_PoseComponent(BinaryReader reader)
        {
            var result = new PoseComponent();

            result.Version = ReadVersion(reader, 1, 0x1417056E0);

            return result;
        }

        public class CharComponent
        {
            public uint Version { get; set; }
            public string SpeechAlgorithmUuid { get; set; }
            public string SpeechCharacterUuid { get; set; }
            public bool HandsRigged { get; set; }
            public string BaseDefinition { get; set; }
        }
        private CharComponent Read_CharComponent(BinaryReader reader)
        {
            var result = new CharComponent();

            result.Version = ReadVersion(reader, 3, 0x1417056F0);

            result.SpeechAlgorithmUuid = ReadUUID(reader);
            result.SpeechCharacterUuid = ReadUUID(reader);

            if (result.Version >= 2)
            {
                result.HandsRigged = reader.ReadBoolean();
            }

            if (result.Version >= 3)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }

            return result;
        }

        public class CameraComponent
        {
            public uint Version { get; set; }
            public uint NearPlane { get; set; }
            public uint FarPlane { get; set; }
            public uint DiagonalFovRadius { get; set; }
            public string BaseDefinition { get; set; }
            public int EditInstanceId { get; set; }
            public int EditLinkId { get; set; }
        }
        private CameraComponent Read_CameraComponent(BinaryReader reader)
        {
            var result = new CameraComponent();

            result.Version = ReadVersion(reader, 3, 0x141705700);

            result.NearPlane = reader.ReadUInt32();
            result.FarPlane = reader.ReadUInt32();
            result.DiagonalFovRadius = reader.ReadUInt32();

            if (result.Version >= 2)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }

            if (result.Version >= 3)
            {
                result.EditInstanceId = reader.ReadInt32();
                result.EditLinkId = reader.ReadInt32();
            }

            return result;
        }

        public class LightComponent
        {
            public uint Version { get; set; }
            public uint LightType { get; set; }
            public string Name { get; set; }
            public List<float> RgbIntensity { get; set; }
            public uint Range { get; set; }
            public bool IsShadowCaster { get; set; }
            public bool EnableDynamicCasters { get; set; }
            public float SpotAngle { get; set; }
            public float SpotAngularFalloff { get; set; }
            public float SpotNearClip { get; set; }
            public bool IsStatic { get; set; }
            public string BaseDefinition { get; set; }
            public int EditInstanceId { get; set; }
            public int EditLinkId { get; set; }
        }
        private LightComponent Read_LightComponent(BinaryReader reader)
        {
            var result = new LightComponent();

            result.Version = ReadVersion(reader, 5, 0x141705710);

            result.LightType = reader.ReadUInt32();

            result.Name = ReadString(reader);

            result.RgbIntensity = ReadVectorF(reader, 4); // TODO: Double check dimensions

            result.Range = reader.ReadUInt32();
            result.IsShadowCaster = reader.ReadBoolean();

            if (result.Version >= 3)
            {
                result.EnableDynamicCasters = reader.ReadBoolean();
            }

            result.SpotAngle = reader.ReadUInt32();
            result.SpotAngularFalloff = reader.ReadUInt32();
            result.SpotNearClip = reader.ReadUInt32();

            if (result.Version >= 2)
            {
                result.IsStatic = reader.ReadBoolean();
            }

            if (result.Version >= 4)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }

            if (result.Version >= 5)
            {
                result.EditInstanceId = reader.ReadInt32();
                result.EditLinkId = reader.ReadInt32();
            }

            return result;
        }

        public class PartLod
        {
            public uint Version { get; set; }
            public uint IndexStart { get; set; }
            public uint IndexCount { get; set; }
            public uint ErrorLower { get; set; }
            public uint ErrorUpper { get; set; }
        }
        private PartLod Read_ModelDefinition_Part_Lod(BinaryReader reader)
        {
            var result = new PartLod();

            result.Version = ReadVersion(reader, 1, 0x1412224B0);

            result.IndexStart = reader.ReadUInt32();
            result.IndexCount = reader.ReadUInt32();
            result.ErrorLower = reader.ReadUInt32();
            result.ErrorUpper = reader.ReadUInt32();

            return result;
        }

        public class ModelDefinitionPart
        {
            public uint Version { get; set; }
            public string MaterialUuid { get; set; }
            public List<PartLod> Lods { get; set; }
            public uint IndexStart { get; set; }
            public uint IndexCount { get; set; }
            public ulong Flags { get; set; }
            public float SurfaceArea { get; set; }
        }
        private ModelDefinitionPart Read_ModelDefinition_Part(BinaryReader reader)
        {
            var result = new ModelDefinitionPart();

            result.Version = ReadVersion(reader, 4, 0x14120E190);

            result.MaterialUuid = ReadUUID(reader);

            if (result.Version >= 4)
            {
                result.Lods = Read_List(reader, Read_ModelDefinition_Part_Lod, 1, 0x14121CBA0);
            }
            else
            {
                result.IndexStart = reader.ReadUInt32();
                result.IndexCount = reader.ReadUInt32();
            }

            if (result.Version < 2)
            {
                // noop
            }
            else
            {
                result.Flags = reader.ReadUInt64();
            }

            if (result.Version < 3)
            {
                // noop
                result.SurfaceArea = 1315859240;
            }
            else
            {
                result.SurfaceArea = reader.ReadSingle();
            }

            return result;
        }

        public class ModelDefinition
        {
            public uint Version { get; set; }
            public string GeometryUuid { get; set; }
            public List<ModelDefinitionPart> Parts { get; set; }
        }
        public ModelDefinition Read_ModelDefinition(BinaryReader reader)
        {
            var result = new ModelDefinition();

            result.Version = ReadVersion(reader, 2, 0x1411FD8F0);

            result.GeometryUuid = ReadUUID(reader);

            result.Parts = Read_List(reader, Read_ModelDefinition_Part, 1, 0x141206410);

            if (result.Version < 2)
            {
                // noop
            }

            return result;
        }

        public class MeshComponent_V3
        {
            public uint Version { get; set; }
            public string Name { get; set; }
            public ModelDefinition ModelDefinition { get; set; }
            public float Scale { get; set; }
            public float MaxRenderDistance { get; set; }
            public bool IsScriptable { get; set; }
            public bool IsVisible { get; set; }
            public int EditInstanceId { get; set; }
            public int EditLinkId { get; set; }
        }
        private MeshComponent_V3 Read_MeshComponent_v3(BinaryReader reader)
        {
            var result = new MeshComponent_V3();

            result.Version = ReadVersion(reader, 5, 0x141706B40);
            result.Name = ReadString(reader);

            result.ModelDefinition = ReadComponent(reader, Read_ModelDefinition);

            result.Scale = reader.ReadSingle();

            if (result.Version >= 2)
            {
                result.MaxRenderDistance = reader.ReadSingle();
            }
            if (result.Version >= 3)
            {
                result.IsScriptable = reader.ReadBoolean();
            }
            if (result.Version >= 4)
            {
                result.IsVisible = reader.ReadBoolean();
            }
            if (result.Version >= 5)
            {
                result.EditInstanceId = reader.ReadInt32();
                result.EditLinkId = reader.ReadInt32();
            }

            return result;
        }

        public class StaticMeshComponent
        {
            public uint Version { get; set; }
            public MeshComponent_V3 MeshComponent_V3 { get; set; }
            public string Name { get; set; }
            public ModelDefinition ModelDefinition { get; set; }
            public bool IsShadowCaster { get; set; }
            public float Scale { get; set; }
        }
        private StaticMeshComponent Read_StaticMeshComponent(BinaryReader reader)
        {
            var result = new StaticMeshComponent();

            result.Version = ReadVersion(reader, 3, 0x141705720);

            if (result.Version >= 3)
            {
                result.MeshComponent_V3 = Read_MeshComponent_v3(reader);
            }
            else
            {
                result.Name = ReadString(reader);
                result.ModelDefinition = ReadComponent(reader, Read_ModelDefinition);
                result.IsShadowCaster = reader.ReadBoolean();

                if (result.Version >= 2)
                {
                    result.Scale = reader.ReadSingle();
                }
            }

            return result;
        }

        public class RiggedMeshComponent
        {
            public uint Version { get; set; }
            public MeshComponent_V3 MeshComponent_V3 { get; set; }
            public string Name { get; set; }
            public ModelDefinition ModelDefinition { get; set; }
            public bool IsShadowCaster { get; set; }
            public float Scale { get; set; }
        }
        private RiggedMeshComponent Read_RiggedMeshComponent(BinaryReader reader)
        {
            var result = new RiggedMeshComponent();

            result.Version = ReadVersion(reader, 3, 0x141705730);

            if (result.Version >= 3)
            {
                result.MeshComponent_V3 = Read_MeshComponent_v3(reader);
            }
            else
            {
                result.Name = ReadString(reader);
                result.ModelDefinition = ReadComponent(reader, Read_ModelDefinition);
                result.IsShadowCaster = reader.ReadBoolean();

                if (result.Version >= 2)
                {
                    result.Scale = reader.ReadSingle();
                }
            }

            return result;
        }

        public class AudioShape
        {
            public uint Version { get; set; }
            public uint Type { get; set; }
            public uint SphereRadius { get; set; }
            public AABB Aabb { get; set; }
        }
        private AudioShape Read_Audiocomponent_Shape(BinaryReader reader)
        {
            var result = new AudioShape();

            result.Version = ReadVersion(reader, 1, 0x14170C990);

            result.Type = reader.ReadUInt32();
            if(result.Type == 2)
            {
                result.SphereRadius = reader.ReadUInt32();
            }
            else if (result.Type == 3)
            {
                result.Aabb = Read_AABB(reader);
            }

            return result;
        }

        public class AudioComponent
        {
            public uint Version { get; set; }
            public string BankResourceUuid { get; set; }
            public List<string> SoundResources { get; set; }
            public AudioShape Shape { get; set; }
            public string BaseDefinition { get; set; }
            public int EditInstanceId { get; set; }
            public int EditLinkId { get; set; }
        }
        private AudioComponent Read_AudioComponent(BinaryReader reader)
        {
            var result = new AudioComponent();

            result.Version = ReadVersion(reader, 4, 0x141705740);

            if (result.Version < 3)
            {
                result.BankResourceUuid = ReadUUID(reader);
            }

            result.SoundResources = Read_List(reader, ReadUUID, 1, 0x14121C2C0);

            result.Shape = Read_Audiocomponent_Shape(reader);

            if (result.Version >= 2)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }
            if (result.Version >= 4)
            {
                result.EditInstanceId = reader.ReadInt32();
                result.EditLinkId = reader.ReadInt32();
            }

            return result;
        }

        public class TerrainComponentRuntimeData
        {
            public uint Version { get; set; }
            public uint Unused { get; set; }
            public List<float> Unknown { get; set; }
            public uint Version2 { get; set; }
            public string VoxelTextureDataUuid { get; set; }
            public uint Version3 { get; set; }
            public string HeightMapUuid { get; set; }
            public string MaterialMapUuid { get; set; }
            public string MaterialMapBUuid { get; set; }
            public string RuntimeTextureDataUuid { get; set; }
            public UInt64 UnusedB { get; set; }
            public UInt64 UnusedC { get; set; }
        }
        private TerrainComponentRuntimeData Read_TerrainComponent_RuntimeData_inner(BinaryReader reader)
        {
            var result = new TerrainComponentRuntimeData();

            result.Version = ReadVersion(reader, 3, 0x141710770);

            if (result.Version >= 2)
            {
                // skip 4 bytes...
                result.Unused = reader.ReadUInt32();
            }

            // m128i - no idea
            result.Unknown = ReadVectorF(reader, 4);

            result.Version2 = ReadVersion(reader, 1, 0x141710930);
            result.VoxelTextureDataUuid = ReadUUID(reader);

            result.Version3 = ReadVersion(reader, 2, 0x141710940);
            result.HeightMapUuid = ReadUUID(reader);
            result.MaterialMapUuid = ReadUUID(reader);

            if(result.Version3 >= 2)
            {
                result.MaterialMapBUuid = ReadUUID(reader);
            }

            // might not be readuuid...
            result.RuntimeTextureDataUuid = ReadUUID(reader);

            if(result.Version >= 3)
            {
                // skip 16 bytes...
                result.UnusedB = reader.ReadUInt64(); // skip?
                result.UnusedC = reader.ReadUInt64(); // skip?
            }

            return result;
        }

        public class TerrainComponent
        {
            public uint Version { get; set; }
            public string Name { get; set; }
            public uint Version2 { get; set; }
            public List<TerrainComponentRuntimeData> RuntimeData { get; set; }
            public AABB Aabb { get; set; }
            public UInt64 AudioMaterialIndex { get; set; }
        }
        private TerrainComponent Read_TerrainComponent(BinaryReader reader)
        {
            var result = new TerrainComponent();

            result.Version = ReadVersion(reader, 3, 0x141706740);
            result.Name = ReadString(reader);
            result.Version2 = ReadVersion(reader, 1, 0x14170D340);

            result.RuntimeData = Read_List(reader, Read_TerrainComponent_RuntimeData_inner, 1, 0x14170FC10);

            if (result.Version >= 2)
            {
                result.Aabb = Read_AABB(reader);
            }

            if (result.Version >= 3)
            {
                result.AudioMaterialIndex = reader.ReadUInt64();
            }

            return result;
        }

        public class IKBodyComponent
        {
            public uint Version { get; set; }
        }
        private IKBodyComponent Read_IKBodyComponent(BinaryReader reader)
        {
            var result = new IKBodyComponent();

            result.Version = ReadVersion(reader, 1, 0x141706750);

            return result;
        }

        public class MovementComponent
        {
            public string Name { get; set; }
        }
        private MovementComponent Read_MovementComponent(BinaryReader reader)
        {
            // TODO: No version??
            var result = new MovementComponent();

            result.Name = ReadStringVersioned(reader);

            return result;
        }

        public class SpawnPointComponent
        {
            public uint Version { get; set; }
            public byte InitialSpawnPoint { get; set; }
            public string BaseDefinition { get; set; }
        }
        private SpawnPointComponent Read_SpawnPointComponent(BinaryReader reader)
        {
            var result = new SpawnPointComponent();

            result.Version = ReadVersion(reader, 2, 0x141706760);
            result.InitialSpawnPoint = reader.ReadByte();

            if (result.Version >= 2)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }

            return result;
        }

        public class EntityInfo
        {
            public uint Version { get; set; }
            public Transform EntityToObject { get; set; }
            public RigidBody RigidBodyDef { get; set; }
            public AnimationComponent AnimationComponentDef { get; set; }
            public PoseComponent PoseComponentDef{ get; set; }
            public CharComponent CharComponentDef { get; set; }
            public CameraComponent CameraComponentDef { get; set; }
            public LightComponent LightComponentDef { get; set; }
            public StaticMeshComponent StaticMeshComponentDef { get; set; }
            public RiggedMeshComponent RiggedMeshComponentDef { get; set; }
            public AudioComponent AudioComponentDef { get; set; }
            public TerrainComponent TerrainComponentDef { get; set; }
            public IKBodyComponent IkBodyComponentDef { get; set; }
            public MovementComponent MovementComponentDef { get; set; }
            public SpawnPointComponent SpawnPointComponentDef { get; set; }
            public string Name { get; set; }
            public bool FixedInScene { get; set; }
        }
        private EntityInfo Read_EntityInfo(BinaryReader reader)
        {
            var result = new EntityInfo();

            result.Version = ReadVersion(reader, 5, 0x1416F8590);

            result.EntityToObject = Read_Transform(reader);

            result.RigidBodyDef = ReadComponent(reader, Read_RigidBody);
            result.AnimationComponentDef = ReadComponent(reader, Read_AnimationComponent);
            result.PoseComponentDef = ReadComponent(reader, Read_PoseComponent);
            result.CharComponentDef = ReadComponent(reader, Read_CharComponent);
            result.CameraComponentDef = ReadComponent(reader, Read_CameraComponent);
            result.LightComponentDef = ReadComponent(reader, Read_LightComponent);
            result.StaticMeshComponentDef = ReadComponent(reader, Read_StaticMeshComponent);
            result.RiggedMeshComponentDef = ReadComponent(reader, Read_RiggedMeshComponent);
            result.AudioComponentDef = ReadComponent(reader, Read_AudioComponent);

            if (result.Version >= 2)
            {
                result.TerrainComponentDef = ReadComponent(reader, Read_TerrainComponent);
            }
            if (result.Version >= 3)
            {
                result.IkBodyComponentDef = ReadComponent(reader, Read_IKBodyComponent);
            }
            if (result.Version >= 4)
            {
                result.MovementComponentDef = ReadComponent(reader, Read_MovementComponent);
            }
            if (result.Version >= 5)
            {
                result.SpawnPointComponentDef = ReadComponent(reader, Read_SpawnPointComponent);
            }

            result.Name = ReadString(reader);
            result.FixedInScene = reader.ReadBoolean();

            return result;
        }

        public class ObjectClusterTransform
        {
            public uint Version { get; set; }
            public List<List<float>> Rotation { get; set; }
            public List<float> Translation { get; set; }
        }
        public ObjectClusterTransform Read_ObjectClusterTransform(BinaryReader reader)
        {
            var result = new ObjectClusterTransform();

            result.Version = ReadVersion(reader, 1, 0x141198AB0);

            
            result.Rotation = Read_RotationMatrix(reader);
            result.Translation = ReadVectorF(reader, 3); // just a guess

            return result;
        }

        class InteractionResult
        {
            public uint Version { get; set; }
            public string Prompt { get; set; }
            public bool Enabled { get; set; }
            public uint Label_Version { get; set; }
            public string Label { get; set; }
            public bool Label_Enabled { get; set; }
            public string Key { get; set; }
        }
        private InteractionResult Read_ScriptComponent_Interaction(BinaryReader reader)
        {
            var result = new InteractionResult();

            result.Version = ReadVersion(reader, 2, 0x1411D4980);
            result.Prompt = ReadString(reader);
            result.Enabled = reader.ReadBoolean();

            // todo: need examples...
            if (result.Version < 2)
            {
                var inner_version = ReadVersion(reader, 3, 0x1411E4F00);
                result.Label = ReadString_VersionSafe(reader, inner_version, 3);

                if (inner_version < 3)
                {
                    result.Label_Enabled = reader.ReadBoolean();
                }
                if (inner_version >= 2)
                {
                    result.Key = ReadString_VersionSafe(reader, inner_version, 3);
                }
            }

            return result;
        }

        public enum ScriptTypeCodes
        {
            System_Boolean = 0x4101,
            System_SByte = 0x101,
            System_Byte = 0x8101,
            System_Int16 = 0x102,
            System_UInt16 = 0x8102,
            System_Int32 = 0x104,
            System_UInt32 = 0x8104,
            System_Int64 = 0x108,
            System_UInt64 = 0x8108,
            System_Single = 0x204,
            System_Double = 0x208,
            System_String = 0x400,
            System_Object = 0x800,
            Sansar_Simulation_AnimationComponent = 0x801,
            Sansar_Simulation_RigidBodyComponent = 0x802,
            Sansar_Simulation_AudioComponent = 0x803,
            Sansar_Simulation_LightComponent = 0x804,
            Sansar_Simulation_MeshComponent = 0x805,
            Sansar_Simulation_CameraComponent = 0x806,
            Sansar_Simulation_UnknownResourceA = 0x2000,
            Sansar_Simulation_UnknownResourceB = 0x2001,
            Sansar_Simulation_ClusterResource = 0x2002,
            Sansar_Simulation_SoundResource = 0x2003,
            Sansar_Simulation_Interaction = 0x10000,
            Sansar_Simulation_QuestDefinition = 0x20000,
            Sansar_Simulation_ObjectiveDefinition = 0x20001,
            Sansar_Simulation_QuestCharacter = 0x20002,
            Mono_Simd_Vector4f = 0x1001,
            Sansar_Vector = 0x1002,
            Sansar_Quaternion = 0x1003,
            Sansar_Color = 0x1004,
        }
        private static string ScriptTypeToString(ScriptTypeCodes scriptType)
        {
            if(((uint)scriptType & (1<<28)) > 0)
            {
                var typeMask = (1 << 28) - 1;
                var subtypeName = ScriptTypeToString((ScriptTypeCodes)((uint)scriptType & typeMask));

                return $"List<{subtypeName}>";
            }

            // TODO: What collection type has the 1<<29 flag?

            try
            {
                var name = Enum.GetName(typeof(ScriptTypeCodes), scriptType);
                name = name.Replace("_", ".");
                return name;
            }
            catch (Exception)
            {
                return $"Unknown ({scriptType})";
            }
        }

        private object Read_ScriptComponent_value(BinaryReader reader, ScriptTypeCodes scriptTypeCode, bool versionAtLeast11)
        {
            object result = null;

            switch (scriptTypeCode)
            {
                case ScriptTypeCodes.System_SByte: //0x101
                    result = (char)reader.ReadInt64();
                    break;
                case ScriptTypeCodes.System_Int16: //0x102
                    result = (short)reader.ReadInt64();
                    break;
                case ScriptTypeCodes.System_Int32: //0x104
                    result = (int)reader.ReadInt64();
                    break;
                case ScriptTypeCodes.System_Int64: //0x108
                    result = reader.ReadInt64();
                    break;
                case ScriptTypeCodes.System_Single: //0x204
                    result = reader.ReadDouble();
                    break;
                case ScriptTypeCodes.System_Double: //0x208
                    result = reader.ReadDouble();
                    break;
                case ScriptTypeCodes.System_String: //0x400
                    result = ReadString(reader);
                    break;
                case ScriptTypeCodes.System_Object: //0x800
                    result = reader.ReadUInt64(); //skip 8 bytes
                    break;
                case ScriptTypeCodes.Sansar_Simulation_AnimationComponent: //0x801
                case ScriptTypeCodes.Sansar_Simulation_RigidBodyComponent: //0x802
                case ScriptTypeCodes.Sansar_Simulation_AudioComponent: //0x803
                case ScriptTypeCodes.Sansar_Simulation_LightComponent: //0x804
                case ScriptTypeCodes.Sansar_Simulation_MeshComponent: //0x805
                case ScriptTypeCodes.Sansar_Simulation_CameraComponent: //0x806
                {
                    if (versionAtLeast11)
                    {
                        var instanceId = reader.ReadUInt32();
                        var linkId = reader.ReadUInt32();
                    }
                    else
                    {
                        result = reader.ReadUInt64();
                    }
                    break;
                }
                case ScriptTypeCodes.Mono_Simd_Vector4f: //0x1001
                case ScriptTypeCodes.Sansar_Vector: //0x1002
                case ScriptTypeCodes.Sansar_Quaternion: //0x1003
                case ScriptTypeCodes.Sansar_Color: // = 0x1004
                {
                    result = ReadVectorF(reader, 4);
                    break;
                }
                case ScriptTypeCodes.Sansar_Simulation_UnknownResourceA: //0x2000
                case ScriptTypeCodes.Sansar_Simulation_UnknownResourceB: //0x2001
                {
                    result = reader.ReadUInt64(); // Skip 8 bytes
                    break;
                }
                case ScriptTypeCodes.Sansar_Simulation_ClusterResource: //0x2002
                case ScriptTypeCodes.Sansar_Simulation_SoundResource: //0x2003
                {
                    result = ReadUUID(reader);
                    break;
                }
                case ScriptTypeCodes.System_Boolean: //0x4101
                    result = (reader.ReadUInt64() != 0);
                    break;
                case ScriptTypeCodes.System_Byte: //0x8101
                    result = (byte)reader.ReadUInt64();
                    break;
                case ScriptTypeCodes.System_UInt16: //0x8102
                    result = (ushort)reader.ReadUInt64();
                    break;
                case ScriptTypeCodes.System_UInt32: //0x8104
                    result = (uint)reader.ReadUInt64();
                    break;
                case ScriptTypeCodes.System_UInt64: //0x8108
                    result = reader.ReadUInt64();
                    break;
                case ScriptTypeCodes.Sansar_Simulation_Interaction: //0x10000
                    result = Read_ScriptComponent_Interaction(reader);
                    break;
                case ScriptTypeCodes.Sansar_Simulation_QuestDefinition: //0x20000
                    result = ReadUUID_B(reader);
                    break;
                case ScriptTypeCodes.Sansar_Simulation_ObjectiveDefinition: //0x20001
                    result = ReadUUID_B(reader);
                    break;
                case ScriptTypeCodes.Sansar_Simulation_QuestCharacter: //0x20002
                    result = ReadUUID_B(reader);
                    break;
                default:
                    Console.WriteLine($"Unknown type code: {scriptTypeCode:x}");
                    break;
            }

            return result;
        }

        public class ScriptParameter
        {
            public uint Version { get; set; }
            public string Name { get; set; }
            
            public ScriptTypeCodes Type { get; set; }
            public string TypeName => ScriptTypeToString(Type);

            public List<ScriptParameter> Children { get; set; }
            public object Value { get; set; }
        }
        internal ScriptParameter Read_ScriptComponent_parameter(BinaryReader reader)
        {
            var result = new ScriptParameter();

            result.Version = ReadVersion(reader, 11, 0x1411C1D70);
            result.Name = ReadString(reader);

            if (result.Version < 6)
            {
                // noop
            }

            result.Type = (ScriptTypeCodes)reader.ReadUInt32();
            if (((uint)result.Type & (1 << 29)) != 0)
            {
                result.Children = Read_List(reader, Read_ScriptComponent_parameter, 1, 0x1411CF7F0);
            }
            else if (((uint)result.Type & (1 << 28)) != 0)
            {
                result.Children = Read_List(reader, Read_ScriptComponent_parameter, 1, 0x1411CF800);
            }
            else
            {
                result.Value = Read_ScriptComponent_value(reader, result.Type, result.Version >= 11);
            }

            return result;
        }

        public class ScriptComponent
        {
            public uint Version { get; set; }
            public string Name { get; set; }
            public uint Type { get; set; }
            public string ScriptMetadataResourceUuid { get; set; }
            public string ScriptCompiledBytecodeResourceUuid { get; set; }
            public List<ScriptParameter> Parameters { get; set; } = new List<ScriptParameter>();
            public string BaseDefinition { get; set; }
        }
        public ScriptComponent Read_ScriptComponent(BinaryReader reader)
        {
            var result = new ScriptComponent();

            result.Version = ReadVersion(reader, 4, 0x1416EE010);
            result.Name = ReadString(reader);

            if (result.Version < 3)
            {
                // noop
            }
            else
            {
                result.Type = reader.ReadUInt32();
            }

            result.ScriptMetadataResourceUuid = ReadUUID(reader);
            if (result.Version < 2)
            {
                result.ScriptCompiledBytecodeResourceUuid = ReadUUID(reader);
            }

            var paramCount = reader.ReadUInt32();
            for (int i = 0; i < paramCount; i++)
            {
                var parameter = Read_ScriptComponent_parameter(reader);
                result.Parameters.Add(parameter);
            }

            if (result.Version >= 4)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }

            return result;
        }

        public class AudioPlaySoundAtLocationEvent
        {
            public uint Version { get; set; }
            public List<float> Location { get; set; }
            public AudioShape Shape { get; set; }
            public string SoundResourceId { get; set; }
            public float Loudness { get; set; }
        }
        private AudioPlaySoundAtLocationEvent Read_EventRouter_AudioPlaySoundAtLocationEvent(BinaryReader reader)
        {
            var result = new AudioPlaySoundAtLocationEvent();

            result.Version = ReadVersion(reader, 2, 0x14170EFF0);

            result.Location = ReadVectorF(reader, 4); // might be integers?

            result.Shape = Read_Audiocomponent_Shape(reader);

            result.SoundResourceId = ReadUUID(reader);
            result.Loudness = reader.ReadSingle();

            return result;
        }

        public class AudioPlayUrlEvent
        {
            public uint Version { get; set; }
            public string Url { get; set; }
            public float Loudness { get; set; }
        }
        private AudioPlayUrlEvent Read_EventRouter_AudioPlayUrlEvent(BinaryReader reader)
        {
            var result = new AudioPlayUrlEvent();

            result.Version = ReadVersion(reader, 1, 0x14170F000);

            result.Url = ReadString(reader);
            result.Loudness = reader.ReadSingle();

            return result;
        }

        public class AudioPlayChannelEvent
        {
            public uint Version { get; set; }
            public uint StreamChannel { get; set; }
            public string Url { get; set; }
            public float Loudness { get; set; }
        }
        private AudioPlayChannelEvent Read_EventRouter_AudioPlayChannelEvent(BinaryReader reader)
        {
            var result = new AudioPlayChannelEvent();

            result.Version = ReadVersion(reader, 2, 0x14170F010);

            if (result.Version >= 2)
            {
                result.StreamChannel = reader.ReadUInt32();
            }
            else
            {
                result.Url = ReadString(reader);
            }

            result.Loudness = reader.ReadSingle();

            return result;
        }

        public class AudioUuidEvent
        {
            public uint Version { get; set; }
            public string Uuid { get; set; }
        }
        private AudioUuidEvent Read_EventRouter_AudioUuidEvent(BinaryReader reader)
        {
            var result = new AudioUuidEvent();

            result.Version = ReadVersion(reader, 1, 0x14170EFE0);

            result.Uuid = ReadUUID_B(reader);

            return result;
        }

        public class AudioBindStreamEvent
        {
            public uint Version { get; set; }
            public uint StreamTagHash { get; set; }
            public float Loudness { get; set; }
        }
        private AudioBindStreamEvent Read_EventRouter_AudioBindStreamEvent(BinaryReader reader)
        {
            var result = new AudioBindStreamEvent();

            result.Version = ReadVersion(reader, 1, 0x14170EFC0);
            result.StreamTagHash = reader.ReadUInt32();
            result.Loudness = reader.ReadSingle();

            return result;
        }

        public class AudioUnbindStreamEvent
        {
            public uint Version { get; set; }
            public uint StreamTagHash { get; set; }
        }
        private AudioUnbindStreamEvent Read_EventRouter_AudioUnbindStreamEvent(BinaryReader reader)
        {
            var result = new AudioUnbindStreamEvent();

            result.Version = ReadVersion(reader, 1, 0x14170EFD0);
            result.StreamTagHash = reader.ReadUInt32();

            return result;
        }

        public class AudioStopSoundEvent
        {
            public uint Version { get; set; }
            public string SoundResourceId { get; set; }
            public bool Immediate { get; set; }
        }
        private AudioStopSoundEvent Read_EventRouter_AudioStopSoundEvent(BinaryReader reader)
        {
            var result = new AudioStopSoundEvent();

            result.Version = ReadVersion(reader, 1, 0x14170EFB0);

            result.SoundResourceId = ReadUUID(reader);
            result.Immediate = reader.ReadBoolean();

            return result;
        }

        public class AudioPlaySoundEvent
        {
            public uint Version { get; set; }
            public string SoundResourceId { get; set; }
            public float Loudness { get; set; }
            public bool Loop { get; set; }
        }
        private AudioPlaySoundEvent Read_EventRouter_AudioPlaySoundEvent(BinaryReader reader)
        {
            var result = new AudioPlaySoundEvent();

            result.Version = ReadVersion(reader, 1, 0x14170EFA0);

            result.SoundResourceId = ReadUUID(reader);
            result.Loudness = reader.ReadSingle();
            result.Loop = reader.ReadBoolean();

            return result;
        }

        public class EventParams
        {
            public AudioPlaySoundEvent AudioPlaySoundEvent { get; set; }
            public AudioStopSoundEvent AudioStopSoundEvent { get; set; }
            public AudioBindStreamEvent AudioBindStreamEvent { get; set; }
            public AudioUuidEvent AudioUuidEvent { get; set; }
            public AudioPlaySoundAtLocationEvent AudioPlaySoundAtLocationEvent { get; set; }
            public AudioPlayUrlEvent AudioPlayUrlEvent { get; set; }
            public AudioPlayChannelEvent AudioPlayChannelEvent { get; set; }
            public AudioUnbindStreamEvent AudioUnbindStreamEvent { get; set; }
        }
        private EventParams Read_EventRouter_EventParams(BinaryReader reader, uint eventType)
        {
            var result = new EventParams();

            if (eventType == 1)
            {
                result.AudioPlaySoundEvent = Read_EventRouter_AudioPlaySoundEvent(reader);
            }
            else if (eventType == 2)
            {
                result.AudioStopSoundEvent = Read_EventRouter_AudioStopSoundEvent(reader);
            }
            else if (eventType == 4)
            {
                result.AudioBindStreamEvent = Read_EventRouter_AudioBindStreamEvent(reader);
            }
            else if (eventType == 16)
            {
                result.AudioUuidEvent = Read_EventRouter_AudioUuidEvent(reader);
            }
            else if (eventType == 64)
            {
                result.AudioPlaySoundAtLocationEvent = Read_EventRouter_AudioPlaySoundAtLocationEvent(reader);
            }
            else if (eventType == 256)
            {
                result.AudioPlayUrlEvent = Read_EventRouter_AudioPlayUrlEvent(reader);
            }
            else if (eventType > 16)
            {
                result.AudioPlayChannelEvent = Read_EventRouter_AudioPlayChannelEvent(reader);
            }
            else if(eventType < 16)
            {
                result.AudioUnbindStreamEvent = Read_EventRouter_AudioUnbindStreamEvent(reader);
            }

            return result;
        }

        public class EventRouterEvent
        {
            public uint Version { get; set; }
            public uint SourceEmitter { get; set; }
            public ushort SourceEntityIndex { get; set; }
            public uint SourceEvent { get; set; }
            public uint TargetReceiver { get; set; }
            public ushort TargetEntityIndex  { get; set; }
            public uint TargetEvent { get; set; }
            public EventParams TargetEventParams { get; set; }
        }
        private EventRouterEvent Read_EventRouter_Events(BinaryReader reader)
        {
            var result = new EventRouterEvent();

            result.Version = ReadVersion(reader, 1, 0x1417056B0);

            result.SourceEmitter = reader.ReadUInt32();
            result.SourceEntityIndex = reader.ReadUInt16();
            result.SourceEvent = reader.ReadUInt32();
            result.TargetReceiver = reader.ReadUInt32();
            result.TargetEntityIndex = reader.ReadUInt16();
            result.TargetEvent = reader.ReadUInt32();

            result.TargetEventParams = Read_EventRouter_EventParams(reader, result.TargetEvent);

            return result;
        }

        private uint Read_ClusterDefinition_JointType(BinaryReader reader)
        {
            var data = reader.ReadUInt32();
            return data;
        }

        public class JointDefinition
        {
            public uint Version { get; set; }
            public ulong EntityA { get; set; }
            public ulong EntityB { get; set; }
            public uint Type { get; set; }
            public string AttachmentBoneName { get; set; }
            public string ConstraintDataUuid { get; set; }
        }
        private JointDefinition Read_JointDefinition(BinaryReader reader)
        {
            var result = new JointDefinition();

            result.Version = ReadVersion(reader, 1, 0x1416F3520);

            result.EntityA = reader.ReadUInt64();
            result.EntityB = reader.ReadUInt64();
            result.Type = reader.ReadUInt32();

            if (result.Type != 1)
            {
                if (result.Type == 2)
                {
                    result.AttachmentBoneName = ReadString(reader);
                }
                else
                {
                    result.ConstraintDataUuid = ReadUUID(reader);
                }
            }

            return result;
        }

        public class ScriptedInteractionAction
        {
            public uint Version { get; set; }
            public string Label { get; set; }
            public string Key { get; set; }
        }
        private ScriptedInteractionAction Read_ClusterDefinition_ScriptedInteraction_Action(BinaryReader reader)
        {
            var result = new ScriptedInteractionAction();

            result.Version = ReadVersion(reader, 3, 0x141706730);

            result.Label = ReadString_VersionSafe(reader, result.Version, 3);

            if (result.Version >= 2)
            {
                result.Key = ReadString_VersionSafe(reader, result.Version, 3);
            }

            return result;
        }

        public class ScriptedInteraction
        {
            public uint Version { get; set; }
            public string Prompt { get; set; }
            public List<ScriptedInteractionAction> Actions { get; set; }
        }
        private ScriptedInteraction Read_ClusterDefinition_ScriptedInteraction(BinaryReader reader)
        {
            var result = new ScriptedInteraction();

            result.Version = ReadVersion(reader, 2, 0x1416F8830);
            result.Prompt = ReadString(reader);

            if (result.Version < 2)
            {
                result.Actions = Read_List(reader, Read_ClusterDefinition_ScriptedInteraction_Action, 1, 0x1416FDFC0);
            }

            return result;
        }

        public class ClusterObject
        {
            public uint Version { get; set; }
            public ObjectClusterTransform ObjectToCluster { get; set; }
            public List<EntityInfo> EntityInfos { get; set; }
            public List<ScriptComponent> ScriptDefs { get; set; }
            public List<EventRouterEvent> EventRouter { get; set; }
            public List<uint> JointTypes { get; set; }
            public List<JointDefinition> JoinDefinitions { get; set; } = new List<JointDefinition>();
            public List<ScriptedInteraction> ScriptedInteractions { get; set; }
            public string Name { get; set; }
            public int EditInstanceId { get; set; }
        }
        private ClusterObject Read_Objects(BinaryReader reader)
        {
            var result = new ClusterObject();

            result.Version = ReadVersion(reader, 5, 0x1416EDFF0);

            result.ObjectToCluster =  Read_ObjectClusterTransform(reader);

            result.EntityInfos = Read_List(reader, Read_EntityInfo, 1, 0x1416F31F0);

            if (result.Version < 3)
            {
                // this is busted after this point...
                result.ScriptDefs = Read_List(reader, n => ReadComponent(n, Read_ScriptComponent), 1, 0x1416E9720);
            }
            else
            {
                result.ScriptDefs = Read_List(reader, Read_ScriptComponent, 1, 0x1416E9710);
            }

            result.EventRouter = ReadComponent(reader, (n) => {
                return Read_List(reader, Read_EventRouter_Events, 1, 0x1416FD570);
            });

            result.JointTypes = Read_List(reader, Read_ClusterDefinition_JointType, 1, 0x1416F3200);
            for (int i = 0; i < result.JointTypes.Count; i++)
            {
                var joinDefinition = Read_JointDefinition(reader);
                result.JoinDefinitions.Add(joinDefinition);
            }

            if (result.Version >= 2)
            {
                result.ScriptedInteractions = Read_List(reader, Read_ClusterDefinition_ScriptedInteraction, 1, 0x1416F4530);
            }
            if (result.Version >= 4)
            {
                result.Name = ReadString(reader);
            }
            if (result.Version >= 5)
            {
                result.EditInstanceId = reader.ReadInt32();
            }

            return result;
        }

        public class JointDefinitions
        {
            public uint Version { get; set; }
            public ulong ObjectA { get; set; }
            public ulong ObjectB { get; set; }
            public uint JointType { get; set; }
            public JointDefinition JointDefinition { get; set; }
        }
        private JointDefinitions Read_Joints(BinaryReader reader)
        {
            var result = new JointDefinitions();

            result.Version = ReadVersion(reader, 1, 0x1416EE000);

            result.ObjectA = reader.ReadUInt64();
            result.ObjectB = reader.ReadUInt64();
            result.JointType = reader.ReadUInt32();

            result.JointDefinition = ReadComponent(reader, Read_JointDefinition);

            return result;
        }

        public string Name { get; set; } = "";
        public uint Version { get; set; }
        public List<ClusterObject> ObjectsDefs { get; set; }
        public List<JointDefinitions> JointDefs { get; set; }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Version = ReadVersion(reader, 1, 0x1410E3B70);
                
                this.ObjectsDefs = Read_List(reader, Read_Objects, 1, 0x1416E96E0);
                this.JointDefs = Read_List(reader, Read_Joints, 1, 0x1416E96F0);
            }
        }
    }
}
