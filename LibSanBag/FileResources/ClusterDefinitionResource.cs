using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibSanBag.FileResources
{
    public class ClusterDefinitionResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        /// <summary>
        /// Script filename
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Script source code
        /// </summary>
        public string Source { get; set; }

        public static ClusterDefinitionResource Create(string version = "")
        {
            return new ClusterDefinitionResource();
        }

        public virtual string ReadString(BinaryReader decompressedStream)
        {
            var textLength = decompressedStream.ReadInt32();
            var text = new string(decompressedStream.ReadChars(textLength));

           //Console.WriteLine($"ReadString: {text}");
            return text;
        }

        private string ReadUUID(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x141196890);

            var item1 = reader.ReadUInt64();
            var item2 = reader.ReadUInt64();

            var left = BitConverter.GetBytes(item1);
            var right = BitConverter.GetBytes(item2);

            StringBuilder sb = new StringBuilder();
            for (int i = left.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", left[i]);
            }
            for (int i = right.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", right[i]);
            }

            var uuid = sb.ToString();
            // Console.WriteLine($"ReadUUID: {uuid}");

            return uuid;
        }

        private string ReadStringVersioned(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F4E40);
            var name = ReadString(reader);

            return name;
        }

        private string ReadUUID_B(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x141160230);

            var item1 = reader.ReadUInt64();
            var item2 = reader.ReadUInt64();

            var left = BitConverter.GetBytes(item1);
            var right = BitConverter.GetBytes(item2);

            StringBuilder sb = new StringBuilder();

            for (int i = left.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", left[i]);

                if (i == 4 || i == 2 || i ==0)
                {
                    sb.Append('-');
                }
            }
            for (int i = right.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", right[i]);

                if (i == 6)
                {
                    sb.Append('-');
                }
            }

            var uuid = sb.ToString();
           // Console.WriteLine($"ReadUUID_B: {uuid}");

            return uuid;
        }

        private Dictionary<ulong, uint> versionMap = new Dictionary<ulong, uint>();
        private uint ReadVersion(BinaryReader reader, uint defaultVersion, ulong? versionType)
        {
            if (versionType != null && versionMap.ContainsKey(versionType.Value))
            {
                return versionMap[versionType.Value];
            }

            var version = reader.ReadUInt32();

            if (versionType != null)
            {
                versionMap[versionType.Value] = version;
            }

            return version;
        }

        private List<T> Read_List<T>(BinaryReader reader, Func<BinaryReader, T> func, uint currentVersion, ulong versionType)
        {
            List<T> result = new List<T>();

            var version = ReadVersion(reader, currentVersion, versionType);

            var count = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                var value = func(reader);
                result.Add(value);
            }

            return result;
        }

        private T ReadComponent<T>(BinaryReader reader, Func<BinaryReader, T> func)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                return func(reader);
            }

            return default(T);
        }

        private void ReadComponent(BinaryReader reader, Action<BinaryReader> func)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                func(reader);
            }
        }

        class Transform
        {
            public List<float> Q { get; set; }
            public List<float> T { get; set; }
        }

        private List<float> ReadVectorF(BinaryReader reader, int dimensions)
        {
            var result = new List<float>();

            for (int i = 0; i < dimensions; i++)
            {
                var val = reader.ReadSingle();
                result.Add(val);
            }

            return result;
        }

        private Transform Read_Transform(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411A0F00);

            var q = ReadVectorF(reader, 4);
            var t = ReadVectorF(reader, 3);

            return new Transform()
            {
                Q = q,
                T = t
            };
        }

        struct GrabPointDefinition
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

        struct AudioResourcePoolSound
        {
            public uint Version { get; set; }
            public bool Enabled { get; set; }
            public int LoudnessOffset { get; set; }
            public int PitchRange { get; set; }
            public uint Data { get; set; }
            public List<string> Sounds { get; set; }
        }
        private AudioResourcePoolSound Read_RigidBody_AudioResourcePoolSound(BinaryReader reader)
        {
            var result = new AudioResourcePoolSound();

            result.Version = ReadVersion(reader, 1, 0x14120B5B0);

            result.Enabled = reader.ReadBoolean();
            result.LoudnessOffset = reader.ReadInt32();
            result.PitchRange = reader.ReadInt32();

            result.Sounds = Read_List(reader, ReadUUID, 1, 0x14121C2C0);

            return result;
        }

        private List<AudioResourcePoolSound> Read_RigidBody_AudioResourcePoolSounds(BinaryReader reader)
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

        struct SitPointDefinition
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

        struct SelectionBeamPointDefinition
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


        struct RigidBody
        {
            public uint Version { get; set; }
            public string BodyResourceHandle { get; set; }
            public byte[] BodyCinfo { get; set; }
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
            public uint EditInstanceId { get; set; }
            public uint EditLinkId { get; set; }
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
                result.EditInstanceId = reader.ReadUInt32();
                result.EditLinkId = reader.ReadUInt32();
            }
            if(result.Version >= 13)
            {
                result.CollisionFilterOverride = reader.ReadUInt32();
            }

            return result;
        }

        struct AnimationComponentNode
        {
            public uint Version { get; set; }
            public string AnimationBindingUuid { get; set; }
            public string AnimationName { get; set; }
            public uint PlaybackSpeed { get; set; }
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

            result.PlaybackSpeed = reader.ReadUInt32();
            result.StartTime = reader.ReadUInt32();
            result.PlaybackMode = reader.ReadUInt32();

            if (result.Version == 2)
            {
                result.AnimationBindingRemapIndex = reader.ReadUInt32();
            }

            return result;
        }

        struct AnimationOverride
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
            var innerVersion = ReadVersion(reader, 1, 0x14170FC00);

            var skeletonMapperUUID = ReadUUID(reader);

            return skeletonMapperUUID;
        }

        struct OffsetTransform
        {
            public uint Version { get; set; }

            public List<float> Translation { get; set; }
            public List<float> Quaternion { get; set; }
            public List<float> Scale { get; set; }
            public string Data { get; set; }
        }
        private OffsetTransform Read_AnimationComponent_OffsetTransform(BinaryReader reader)
        {
            var result = new OffsetTransform();

            result.Version = ReadVersion(reader, 1, 0x141205320);

            result.Translation = ReadVectorF(reader, 4);
            result.Quaternion = ReadVectorF(reader, 4);
            result.Scale = ReadVectorF(reader, 4);

            return result;
        }

        private List<OffsetTransform> Read_AnimationComponent_OffsetTransformsMap(BinaryReader reader)
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

        struct AnimationComponent
        {
            public uint Version { get; set; }
            public string ProjectDataUuid { get; set; }
            public string ProxyShapeUuid { get; set; }
            public string SkeletonUuid { get; set; }
            public string AnimationBindingUuid { get; set; }
            public uint CreationMode { get; set; }
            public bool ClientOnly { get; set; }
            public uint PlaybackSpeed { get; set; }
            public uint StartTime { get; set; }
            public uint PlaybackMode { get; set; }
            public bool BeginOnLoad { get; set; }
            public List<AnimationComponentNode> AnimationNodes { get; set; }
            public uint AnimationBindingRemapIndex { get; set; }
            public string BaseDefinition { get; set; }
            public uint Scale { get; set; }
            public List<AnimationOverride> AnimationOverrides { get; set; }
            public uint EditInstanceId { get; set; }
            public uint EditLinkId { get; set; }
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
                result.PlaybackSpeed = reader.ReadUInt32();
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
                result.Scale = reader.ReadUInt32();
            }
            if (result.Version >= 8)
            {
                result.AnimationOverrides = Read_List(reader, Read_AnimationComponent_AnimationOverride, 1, 0x14170CE40);
            }
            if (result.Version >= 9)
            {
                result.EditInstanceId = reader.ReadUInt32();
                result.EditLinkId = reader.ReadUInt32();
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

        private void Read_PoseComponent(BinaryReader reader)
        {
            var version_inner = ReadVersion(reader, 1, 0x1417056E0);
        }

        struct CharComponent
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


        struct CameraComponent
        {
            public uint Version { get; set; }
            public uint NearPlane { get; set; }
            public uint FarPlane { get; set; }
            public uint DiagonalFovRadius { get; set; }
            public string BaseDefinition { get; set; }
            public uint EditInstanceId { get; set; }
            public uint EditLinkId { get; set; }
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
                result.EditInstanceId = reader.ReadUInt32();
                result.EditLinkId = reader.ReadUInt32();
            }

            return result;
        }

        private void Read_LightComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x141705710);

            var lightType = reader.ReadUInt32();

            var name = ReadString(reader);

            var rgbIntensity = ReadVectorF(reader, 4); // TODO: Double check dimensions

            var range = reader.ReadUInt32();
            var isShadowCaster = reader.ReadByte();

            if (version >= 3)
            {
                var enableDynamicCasters = reader.ReadByte();
            }

            var spot_angle = reader.ReadUInt32();
            var spot_angularFalloff = reader.ReadUInt32();
            var spot_nearClip = reader.ReadUInt32();

            if (version >= 2)
            {
                var isStatic =reader.ReadByte();
            }

            if (version >= 4)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }

            if (version >= 5)
            {
                var editInstanceId = reader.ReadUInt32();
                var editLinkId = reader.ReadUInt32();
            }
        }

        struct PartLod
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

        struct ModelDefinitionPart
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

        private void Read_ModelDefinition(BinaryReader reader)
        {
            var version_inner = ReadVersion(reader, 2, 0x1411FD8F0);

            var geometryUUID = ReadUUID(reader);

            var parts = Read_List(reader, Read_ModelDefinition_Part, 1, 0x141206410);

            if (version_inner < 2)
            {
                // noop
            }
        }

        private void Read_MeshComponent_v3(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x141706B40);
            var name = ReadString(reader);

            // modelDefinition
            ReadComponent(reader, Read_ModelDefinition);

            var scale = reader.ReadUInt32();

            if (version >= 2)
            {
                var maxRenderDistance = reader.ReadUInt32();
            }
            if (version >= 3)
            {
                var inScriptable = reader.ReadByte();
            }
            if (version >= 4)
            {
                var isVisible = reader.ReadByte();
            }
            if (version >= 5)
            {
                var editInstanceId = reader.ReadUInt32();
                var editLinkId = reader.ReadUInt32();
            }
        }
        private void Read_StaticMeshComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141705720);

            if (version >= 3)
            {
                Read_MeshComponent_v3(reader);
            }
            else
            {
                var name = ReadString(reader);

                // modelDefinition
                ReadComponent(reader, Read_ModelDefinition);

                // skip?? TODO: Remove me most likely
                var shadowCaster = reader.ReadByte();

                if (version >= 2)
                {
                    var scale = reader.ReadUInt32();
                }
            }
        }

        private void Read_RiggedMeshComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141705730);

            if (version >= 3)
            {
                Read_MeshComponent_v3(reader);
            }
            else
            {
                var name = ReadString(reader);

                // modelDefinition
                ReadComponent(reader, Read_ModelDefinition);

                // yeah i don't know. maybe? i think we skip this byte though
                var shadowCaster = reader.ReadByte();

                if (version >= 2)
                {
                    var scale = reader.ReadUInt32();
                }
            }
        }

        // read_audiocomponent_shape ?
        private void Read_Audiocomponent_Shape(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170C990);

            var type = reader.ReadUInt32();
            if(type == 2)
            {
                var sphereRadius = reader.ReadUInt32();
            }
            else if (type == 3)
            {
                var aabb = Read_AABB(reader);
            }
        }

        private void Read_AudioComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4, 0x141705740);

            if (version < 3)
            {
                var bankResourceUUID = ReadUUID(reader);
            }

            var soundResources = Read_List(reader, ReadUUID, 1, 0x14121C2C0);

            // shape
            Read_Audiocomponent_Shape(reader);

            if (version >= 2)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }
            if (version >= 4)
            {
                var editInstanceId = reader.ReadUInt32();
                var editLinkId = reader.ReadUInt32();
            }

        }

        struct AABB
        {
            public List<float> Min { get; set; }
            public List<float> Max { get; set; }
        }

        private AABB Read_AABB(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141205310);

            var min = ReadVectorF(reader, 4);
            var max = ReadVectorF(reader, 4);

            return new AABB()
            {
                Min = min,
                Max = max
            }; ;
        }

        struct TerrainComponentRuntimeData
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


        private void Read_TerrainComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141706740);
            var name = ReadString(reader);
            var versionB = ReadVersion(reader, 1, 0x14170D340);

            var runtimeData = Read_List(reader, Read_TerrainComponent_RuntimeData_inner, 1, 0x14170FC10);

            if (version >= 2)
            {
                var aabb = Read_AABB(reader);
            }

            if (version >= 3)
            {
                var audioMaterialIndex = reader.ReadUInt64();
            }
        }

        private void Read_IKBodyComponent(BinaryReader reader)
        {
            var version_inner = ReadVersion(reader, 1, 0x141706750);
        }

        private void Read_MovementComponent(BinaryReader reader)
        {
            var name = ReadStringVersioned(reader);
        }

        private void Read_SpawnPointComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x141706760);
            var initialSpawnPoint = reader.ReadByte();

            if (version >= 2)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }
        }

        private void ClusterDefinition_reader4(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x1416F8590);

            var entityToObject = Read_Transform(reader);

            ReadComponent(reader, Read_RigidBody);                // rigidBodyDef
            ReadComponent(reader, Read_AnimationComponent);       // animationComponentDef
            ReadComponent(reader, Read_PoseComponent);            // poseComponentDef
            ReadComponent(reader, Read_CharComponent);            // charComponentDef
            ReadComponent(reader, Read_CameraComponent);          // cameraComponentDef
            ReadComponent(reader, Read_LightComponent);           // lightComponentDef
            ReadComponent(reader, Read_StaticMeshComponent);      // staticMeshComponentDef
            ReadComponent(reader, Read_RiggedMeshComponent);      // riggedMeshComponentDef
            ReadComponent(reader, Read_AudioComponent);           // audioComponentDef

            if (version >= 2)
            {
                ReadComponent(reader, Read_TerrainComponent);     // terrainComponentDef
            }
            if (version >= 3)
            {
                ReadComponent(reader, Read_IKBodyComponent);      // ikBodyComponentDef
            }
            if (version >= 4)
            {
                ReadComponent(reader, Read_MovementComponent);    // movementComponentDef
            }
            if (version >= 5)
            {
                ReadComponent(reader, Read_SpawnPointComponent);  // spawnPointComponentDef
            }

            var name = ReadString(reader);
            var fixedInScene = reader.ReadByte();
        }

        private List<List<float>> Read_RotationMatrix(BinaryReader reader, int dimension=3)
        {
            var version = ReadVersion(reader, 1, 0x14119F1D0);

            var matrix = new List<List<float>>();
            for (int rowIndex = 0; rowIndex < dimension; rowIndex++)
            {
                var row = new List<float>();
                for (int colIndex = 0; colIndex < dimension; colIndex++)
                {
                    var col = reader.ReadSingle();
                    row.Add(col);
                }
                matrix.Add(row);
            }

            return matrix;
        }

        private void ClusterDefinition_reader1(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141198AB0);

            var rotation = Read_RotationMatrix(reader);

            // guessing this is some translation component
            var translation = ReadVectorF(reader, 3);
        }

        class InteractionResult
        {
            public string Prompt { get; set; }
            public string Label { get; set; }
            public bool Enabled { get; set; }
            public string Key { get; set; }

            public override string ToString()
            {
                return $"Prompt: {Prompt} | Label: {Label} | Enabled={Enabled} | Key: {Key}";
            }
        }

        private InteractionResult Read_ScriptComponent_Interaction(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x1411D4980);
            var prompt = ReadString(reader);
            var enabled = reader.ReadByte();

            var label = string.Empty;
            var key = string.Empty;
            if (version < 2)
            {
                var inner_version = ReadVersion(reader, 3, 0x1411E4F00);
                label = ReadString_VersionSafe(reader, inner_version, 3);

                if (inner_version < 3)
                {
                    var enabled_old = reader.ReadByte(); // skip byte?
                }
                if (inner_version >= 2)
                {
                    key = ReadString_VersionSafe(reader, inner_version, 3);
                }
            }

            return new InteractionResult()
            {
                Label = label,
                Prompt = prompt,
                Key = key
            };
        }

        public struct ScriptTypeCodes
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
            public const int Sansar_Simulation_AnimationComponent = 0x801;
            public const int Sansar_Simulation_RigidBodyComponent = 0x802;
            public const int Sansar_Simulation_AudioComponent = 0x803;
            public const int Sansar_Simulation_LightComponent = 0x804;
            public const int Sansar_Simulation_MeshComponent = 0x805;
            public const int Sansar_Simulation_CameraComponent = 0x806;
            public const int Sansar_Simulation_UnknownResourceA = 0x2000;
            public const int Sansar_Simulation_UnknownResourceB = 0x2001;
            public const int Sansar_Simulation_ClusterResource = 0x2002;
            public const int Sansar_Simulation_SoundResource = 0x2003;
            public const int Sansar_Simulation_Interaction = 0x10000;
            public const int Sansar_Simulation_QuestDefinition = 0x20000;
            public const int Sansar_Simulation_ObjectiveDefinition = 0x20001;
            public const int Sansar_Simulation_QuestCharacter = 0x20002;
            public const int Mono_Simd_Vector4f = 0x1001;
            public const int Sansar_Vector = 0x1002;
            public const int Sansar_Quaternion = 0x1003;
            public const int Sansar_Color = 0x1004;
        }

        private object Read_ScriptComponent_value(BinaryReader reader, uint scriptTypeCode, bool versionAtLeast11)
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
                    var second = ReadVectorF(reader, 4);
                    result = $"<{string.Join(",", second)}>";
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

        class ScriptParameter
        {
            public uint Version { get; set; }
            public string Name { get; set; }
            public uint Type { get; set; }
            public List<ScriptParameter> Children { get; set; } = new List<ScriptParameter>();
            public object Value { get; set; }
        }
        private ScriptParameter Read_ScriptComponent_parameter(BinaryReader reader)
        {
            var result = new ScriptParameter();

            result.Version = ReadVersion(reader, 11, 0x1411C1D70);
            result.Name = ReadString(reader);

            if (result.Version < 6)
            {
                // noop
            }

            result.Type = reader.ReadUInt32();
            if ((result.Type & (1 << 29)) != 0)
            {
                result.Children = Read_List(reader, Read_ScriptComponent_parameter, 1, 0x1411CF7F0);
            }
            else if ((result.Type & (1 << 28)) != 0)
            {
                result.Children = Read_List(reader, Read_ScriptComponent_parameter, 1, 0x1411CF800);
            }
            else
            {
                result.Value = Read_ScriptComponent_value(reader, result.Type, result.Version >= 11);
            }

            return result;
        }

        struct ScriptComponent
        {
            public uint Version { get; set; }
            public string Name { get; set; }
            public uint Type { get; set; }
            public string ScriptMetadataResourceUuid { get; set; }
            public string ScriptCompiledBytecodeResourceUuid { get; set; }
            public ScriptParameter Parameter { get; set; }
            public string BaseDefinition { get; set; }
        }
        private ScriptComponent Read_ScriptComponent(BinaryReader reader)
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
                result.Parameter = Read_ScriptComponent_parameter(reader);
            }

            if (result.Version >= 4)
            {
                result.BaseDefinition = ReadStringVersioned(reader);
            }

            return result;
        }

        private void Read_ClusterDefinition_ScriptDefs_V1(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416E9720);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                // "value"
                ReadComponent(reader, Read_ScriptComponent);
            }
        }

        private void Read_EventRouter_AudioPlaySoundAtLocationEvent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170EFF0);

            var location = ReadVectorF(reader, 4); // might be integers?

            // var shape = 
            Read_Audiocomponent_Shape(reader);

            var soundResourceId = ReadUUID(reader);
            var loudness = reader.ReadUInt32();
        }

        private void Read_EventRouter_AudioPlayUrlEvent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170F000);

            var url = ReadString(reader);
            var loudness = reader.ReadUInt32();
        }

        private void Read_EventRouter_AudioPlayChannelEvent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170F010);

            if (version >= 2)
            {
                var streamChannel = reader.ReadUInt32();
            }
            else
            {
                var url = ReadString(reader);
            }

            var loudness = reader.ReadUInt32();
        }

        private void Read_EventRouter_AudioUuidEvent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170EFE0);

            var m_uuid = ReadUUID_B(reader);
        }

        private void Read_EventRouter_AudioBindStreamEvent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170EFC0);
            var streamTagHash = reader.ReadUInt32();
            var loudness = reader.ReadUInt32();
        }

        private void Read_EventRouter_AudioUnbindStreamEvent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170EFD0);
            var streamTagHash = reader.ReadUInt32();
        }

        private void Read_EventRouter_AudioStopSoundEvent(BinaryReader reader)
        {
            var versionA = ReadVersion(reader, 1, 0x14170EFB0);

            var soundResourceId = ReadUUID(reader);
            var immediate = reader.ReadByte();
        }


        private void Read_EventRouter_AudioPlaySoundEvent(BinaryReader reader)
        {
            var versionA = ReadVersion(reader, 1, 0x14170EFA0);

            var soundResourceId = ReadUUID(reader);
            var loudness = reader.ReadUInt32();
            var loop = reader.ReadByte();
        }

        private void Read_EventRouter_EventParams(BinaryReader reader, uint eventType)
        {
            if (eventType == 1)
            {
                // var audioPlaySoundEvent = 
                Read_EventRouter_AudioPlaySoundEvent(reader);
            }
            else if (eventType == 2)
            {
                // var audioStopSoundEvent = 
                Read_EventRouter_AudioStopSoundEvent(reader);
            }
            else if (eventType == 4)
            {
                // var audioBindStreamEvent = 
                Read_EventRouter_AudioBindStreamEvent(reader);
            }
            else if (eventType == 16)
            {
                // var audioUuidEvent = 
                Read_EventRouter_AudioUuidEvent(reader);
            }
            else if (eventType == 64)
            {
                // var audioPlaySoundAtLocationEvent = 
                Read_EventRouter_AudioPlaySoundAtLocationEvent(reader);
            }
            else if (eventType == 256)
            {
                // var audioPlayUrlEvent = 
                Read_EventRouter_AudioPlayUrlEvent(reader);
            }
            else if (eventType > 16)
            {
                // var audioPlayChannelEvent = 
                Read_EventRouter_AudioPlayChannelEvent(reader);
            }
            else if(eventType < 16)
            {
                // var audioUnbindStreamEvent = 
                Read_EventRouter_AudioUnbindStreamEvent(reader);
            }
        }

        private void Read_EventRouter_Events(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1417056B0);

            var sourceEmitter = reader.ReadUInt32();
            var sourceEntityIndex = reader.ReadUInt16();
            var sourceEvent = reader.ReadUInt32();
            var targetReceiver = reader.ReadUInt32();
            var targetEntityIndex = reader.ReadUInt16();
            var targetEvent = reader.ReadUInt32();

            // targetEventParams = 
            Read_EventRouter_EventParams(reader, targetEvent);
        }

        private uint Read_ClusterDefinition_JointType(BinaryReader reader)
        {
            var data = reader.ReadUInt32();
            return data;
        }

        struct JointDefinition
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

            if (result.T != 1)
            {
                if (result.T == 2)
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

        private string ReadString_VersionSafe(BinaryReader reader, uint version, int max_version)
        {
            if (version >= max_version)
            {
                return "";
            }

            return ReadString(reader);
        }

        struct ScriptedInteractionAction
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

        struct ScriptedInteraction
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

        private void ClusterDefinition_reader0(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x1416EDFF0);

            // objectToCluster = 
            ClusterDefinition_reader1(reader);

            var entityInfos = Read_List(reader, ClusterDefinition_reader4, 1, 0x1416F31F0);

            if (version < 3)
            {
                var scriptDefs = Read_List(reader, n => ReadComponent(n, Read_ScriptComponent), 1, 0x1416E9720);
                Read_ClusterDefinition_ScriptDefs_V1(reader);
            }
            else
            {
                var scriptDefs = Read_List(reader, Read_ScriptComponent, 1, 0x1416E9710);
            }

            var eventRouter = Read_List(reader, Read_EventRouter_Events, 1, 0x1416FD570);

            var jointTypes = Read_List(reader, Read_ClusterDefinition_JointType, 1, 0x1416F3200);
            for (int i = 0; i < jointTypes.Count; i++)
            {
                // var joinDefinition = 
                Read_JointDefinition(reader);
            }

            if (version >= 2)
            {
                var scriptedInteractions = Read_List(reader, Read_ClusterDefinition_ScriptedInteraction, 1, 0x1416F4530);
            }
            if (version >= 4)
            {
                this.Name = ReadString(reader);
            }
            if (version >= 5)
            {
                var editInstanceId = reader.ReadUInt32();
            }
        }

        struct JointDefinitions
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

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var version = ReadVersion(reader, 1, 0x1410E3B70);

                var objectDefs = Read_List(reader, ClusterDefinition_reader0, 1, 0x1416E96E0);
                var jointDefs = Read_List(reader, Read_Joints, 1, 0x1416E96F0);

                Filename = Name;
                Source = Name;
            }
        }
    }
}
