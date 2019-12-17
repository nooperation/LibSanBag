using LibSanBag;
using LibSanBag.ResourceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class WorldSource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static WorldSource Create(string version = "")
        {
            return new WorldSource();
        }

        public class TrackingKey
        {
            public uint Version { get; internal set; }
            public int Type { get; internal set; }
            public ParentId_V19 MemberData { get; internal set; }
            public int PropertyIndex { get; internal set; }
            public string Name { get; internal set; }
            public int ArrayIndex { get; internal set; }
        }
        private TrackingKey Read_TrackingKey(BinaryReader reader)
        {
            var result = new TrackingKey();

            result.Version = ReadVersion(reader, 3, 0x1411C03C0);
            result.Type = reader.ReadInt32();
            result.MemberData = Read_ParentId_V19(reader);
            result.PropertyIndex = reader.ReadInt32();

            if(result.Version >= 2)
            {
                result.Name = ReadString(reader);
            }
            if(result.Version >= 3)
            {
                result.ArrayIndex = reader.ReadInt32();
            }

            return result;
        }

        public class ParentId_V21
        {
            public uint Version { get; internal set; }
            public string UUID { get; internal set; }
            public int ElementType { get; internal set; }
            public uint LinkId { get; internal set; }
        }
        private ParentId_V21 Read_ParentId_V21(BinaryReader reader)
        {
            var result = new ParentId_V21();

            result.Version = ReadVersion(reader, 1, 0x1411C03D0);
            result.UUID = ReadUUID(reader);
            result.ElementType = reader.ReadInt32();
            result.LinkId = reader.ReadUInt32();

            return result;
        }

        public class Parenting_V21
        {
            public uint Version { get; internal set; }
            public ParentId_V21 Id { get; internal set; }
            public List<ParentId_V21> Members { get; internal set; }
        }
        private Parenting_V21 Read_Parenting_V21(BinaryReader reader)
        {
            var result = new Parenting_V21();

            result.Version = ReadVersion(reader, 1, 0x1411AE000);
            result.Id = Read_ParentId_V21(reader);
            result.Members = Read_List(reader, Read_ParentId_V21, 1, 0x1411BF190);

            return result;
        }

        public class ParentId_V19
        {
            public uint Version { get; internal set; }
            public string InstanceId { get; internal set; }
            public BlueprintResource.V1_InnerL_v4_innerC ElementId { get; internal set; }
        }
        private ParentId_V19 Read_ParentId_V19(BinaryReader reader)
        {
            var result = new ParentId_V19();

            result.Version = ReadVersion(reader, 2, 0x1411BEFD0);
            result.InstanceId = ReadUUID(reader);
            if(result.Version >= 2)
            {
                result.ElementId = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerC(reader);
            }

            return result;
        }

        public class Parenting_V19
        {
            public uint Version { get; internal set; }
            public ParentId_V19 Id { get; internal set; }
            public List<ParentId_V19> Members { get; internal set; }
        }
        private Parenting_V19 Read_Parenting_V19(BinaryReader reader)
        {
            var result = new Parenting_V19();

            result.Version = ReadVersion(reader, 1, 0x1411ABAB0);
            result.Id = Read_ParentId_V19(reader);
            result.Members = Read_List(reader, Read_ParentId_V19, 1, 0x1411BCDD0);

            return result;
        }

        public class ParentingDataId
        {
            public uint Version { get; internal set; }
            public uint InstanceId { get; internal set; }
            public int ElementType { get; internal set; }
            public uint LinkId { get; internal set; }
        }
        private ParentingDataId Read_ParentingDataId(BinaryReader reader)
        {
            var result = new ParentingDataId();

            result.Version = ReadVersion(reader, 1, 0x1411A6EC0);
            result.InstanceId = reader.ReadUInt32();
            result.ElementType = reader.ReadInt32();
            result.LinkId = reader.ReadUInt32();

            return result;
        }
        public class ParentingData
        {
            public uint Version { get; internal set; }
            public ParentingDataId Id { get; internal set; }
            public List<ParentingDataId> Members { get; internal set; }
        }
        private ParentingData Read_ParentingData(BinaryReader reader)
        {
            var result = new ParentingData();

            result.Version = ReadVersion(reader, 1, 0x1411AE010);
            result.Id = Read_ParentingDataId(reader);
            result.Members = Read_List(reader, Read_ParentingDataId, 1, 0x14119AD90);

            return result;
        }

        public class Folder
        {
            public uint Version { get; internal set; }
            public ParentId_V19 Id { get; internal set; }
            public string Name { get; internal set; }
            public List<ParentId_V19> Members { get; internal set; }
        }
        private Folder Read_Folder(BinaryReader reader)
        {
            var result = new Folder();

            result.Version = ReadVersion(reader, 1, 0x1411ADFF0);
            result.Id = Read_ParentId_V19(reader);
            result.Name = ReadString(reader);
            result.Members = Read_List(reader, Read_ParentId_V19, 1, 0x1411BCDD0);

            return result;
        }

        public class TrackedElementFlag
        {
            public ParentingDataId Data { get; internal set; }
            public uint Value { get; internal set; }
        }
        private TrackedElementFlag Read_TrackedElementFlag(BinaryReader reader)
        {
            var result = new TrackedElementFlag();

            result.Data = Read_ParentingDataId(reader);
            result.Value = reader.ReadUInt32();

            return result;
        }

        public class TrackedElementFlag_V2
        {
            public ParentId_V21 Data { get; internal set; }
            public uint Value { get; internal set; }
        }
        private TrackedElementFlag_V2 Read_TrackedElementFlag_V2(BinaryReader reader)
        {
            var result = new TrackedElementFlag_V2();

            result.Data = Read_ParentId_V21(reader);
            result.Value = reader.ReadUInt32();

            return result;
        }

        public class SceneElementFlag
        {
            public ParentId_V19 Data { get; internal set; }
            public uint Value { get; internal set; }
        }
        private SceneElementFlag Read_SceneElementFlag(BinaryReader reader)
        {
            var result = new SceneElementFlag();

            result.Data = Read_ParentId_V19(reader);
            result.Value = reader.ReadUInt32();

            return result;
        }

        public class EditWorldSessionData
        {
            public uint Version { get; internal set; }
            public ClusterDefinitionResource.ObjectClusterTransform LocalCameraTransform { get; internal set; }
            public List<SceneElementFlag> SceneElementFlags { get; internal set; }
            public List<TrackedElementFlag_V2> TrackedElementFlags_V2 { get; internal set; }
            public List<TrackedElementFlag> TrackedElementFlags { get; internal set; }
        }
        private EditWorldSessionData Read_EditWorldSessionData(BinaryReader reader)
        {
            var result = new EditWorldSessionData();

            result.Version = ReadVersion(reader, 3, 0x1411B0190);
            result.LocalCameraTransform = ClusterReader.Read_ObjectClusterTransform(reader);
            
            if(result.Version < 2)
            {
                result.SceneElementFlags = Read_List(reader, Read_SceneElementFlag, 1, 0x1411C05F0);
            }
            else if(result.Version == 2)
            {
                result.TrackedElementFlags_V2 = Read_List(reader, Read_TrackedElementFlag_V2, 1, 0x1411C05D0);
            }
            else
            {
                result.TrackedElementFlags = Read_List(reader, Read_TrackedElementFlag, 1, 0x1411C05E0);
            }

            return result;
        }

        public class EditWorldSessionDataListItem
        {
            public string Data { get; internal set; }
            public object Value { get; internal set; }
        }
        private EditWorldSessionDataListItem Read_EditWorldSessionDataItem(BinaryReader reader)
        {
            var result = new EditWorldSessionDataListItem();

            result.Data = ReadUUID(reader);
            result.Value = Read_EditWorldSessionData(reader);

            return result;
        }

        public class OfflineResourceLoad
        {
            public uint Version { get; internal set; }
            public string AssetName { get; internal set; }
            public TrackingKey TrackingKey { get; internal set; }
        }
        private OfflineResourceLoad Read_OfflineResourceLoad(BinaryReader reader)
        {
            var result = new OfflineResourceLoad();

            result.Version = ReadVersion(reader, 2, 0x1411ADFE0);
            result.AssetName = ReadString(reader);
            if(result.Version >= 2)
            {
                result.TrackingKey = Read_TrackingKey(reader);
            }

            return result;
        }

        public class ScriptUsingSceneScript
        {
            public uint Version { get; internal set; }
            public int SceneScriptIndex { get; internal set; }
            public uint LinkId { get; internal set; }
        }
        private ScriptUsingSceneScript Read_ScriptUsingSceneScript(BinaryReader reader)
        {
            var result = new ScriptUsingSceneScript();

            result.Version = ReadVersion(reader, 1, 0x1411BA0D0);
            result.SceneScriptIndex = reader.ReadInt32();
            result.LinkId = reader.ReadUInt32();

            return result;
        }

        public class DisplayNameOverride_V10
        {
            public BlueprintResource.V1_InnerL_v4_innerC Key { get; internal set; }
            public string Value { get; internal set; }
        }
        private DisplayNameOverride_V10 Read_DisplayNameOverride_V10(BinaryReader reader)
        {
            var result = new DisplayNameOverride_V10();

            result.Key = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerC(reader);
            result.Value = ReadString(reader);

            return result;
        }

        public class DisplayNameOverride
        {
            public BlueprintResource.V1_InnerL_v4_innerB Key { get; internal set; }
            public string Value { get; internal set; }
        }
        private DisplayNameOverride Read_DisplayNameOverride(BinaryReader reader)
        {
            var result = new DisplayNameOverride();

            result.Key = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerB(reader);
            result.Value = ReadString(reader);

            return result;
        }

        public class HairSubgraph
        {
            public uint Version { get; internal set; }
            public List<string> TintNames { get; internal set; }
            public BlueprintResource.V1_InnerL_v4_innerB RiggedMesh { get; internal set; }
            public BlueprintResource.V1_InnerL_v4_innerC RiggedMesh_V1 { get; internal set; }
        }
        private HairSubgraph Read_HairSubgraph(BinaryReader reader)
        {
            var result = new HairSubgraph();

            result.Version = ReadVersion(reader, 2, 0x1411BA0C0);
            result.TintNames = Read_List(reader, ReadString, 1, 0x14119ADB0);

            if(result.Version >= 2)
            {
                result.RiggedMesh = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerB(reader);
            }
            else
            {
                result.RiggedMesh_V1 = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerC(reader);
            }

            return result;
        }

        public class ResourceId
        {
            public string Data { get; internal set; }
            public string Value { get; internal set; }
        }
        private ResourceId Read_ResourceId(BinaryReader reader)
        {
            var result = new ResourceId();

            result.Data = ReadUUID(reader);
            result.Value = ReadUUID(reader);

            return result;
        }

        public class VectorInput
        {
            public string Data { get; internal set; }
            public List<float> Value { get; internal set; }
        }
        private VectorInput Read_VectorInput(BinaryReader reader)
        {
            var result = new VectorInput();

            result.Data = ReadString(reader);
            result.Value = ReadVectorF(reader, 4);

            return result;
        }

        public class ScalarInput
        {
            public string Data { get; internal set; }
            public float Value { get; internal set; }
        }
        private ScalarInput Read_ScalarInput(BinaryReader reader)
        {
            var result = new ScalarInput();

            result.Data = ReadString(reader);
            result.Value = reader.ReadSingle();

            return result;
        }

        public class ModelInput
        {
            public string Data { get; internal set; }
            public string Value { get; internal set; }
        }
        private ModelInput Read_ModelInput(BinaryReader reader)
        {
            var result = new ModelInput();

            // TODO: Double check these are uuids and not strings
            result.Data = ReadUUID(reader);
            result.Value = ReadUUID(reader);

            return result;
        }

        public class CharacterLookData
        {
            public uint Version { get; internal set; }
            public string ModelMorph { get; internal set; }
            public List<ModelInput> ModelInputs { get; internal set; }
            public List<ScalarInput> ScalarInputs { get; internal set; }
            public List<VectorInput> VectorInputs { get; internal set; }
            public List<ResourceId> ReesourceId { get; internal set; }
            public HairSubgraph HairSubgraph { get; internal set; }
        }
        private CharacterLookData Read_CharacterLookData(BinaryReader reader)
        {
            var result = new CharacterLookData();

            result.Version = ReadVersion(reader, 4, 0x1411A6E90);
            
            if(result.Version < 3)
            {
                result.ModelMorph = ReadUUID(reader);
            }

            if(result.Version < 2)
            {
                result.ModelInputs = Read_List(reader, Read_ModelInput, 1, 0x1411B7BD0);
            }

            result.ScalarInputs = Read_List(reader, Read_ScalarInput, 1, 0x1411B7BE0);
            result.VectorInputs = Read_List(reader, Read_VectorInput, 1, 0x1411B7BF0);
            result.ReesourceId = Read_List(reader, Read_ResourceId, 1, 0x1411B7C00);
            result.HairSubgraph = Read_HairSubgraph(reader);

            return result;
        }

        public class OverrideLock
        {
            public uint Version { get; internal set; }
            public Override_V5_V1 Key { get; internal set; }
            public BlueprintResource.V1_InnerN_inner_inner Value { get; internal set; }
            public object Value_Old { get; internal set; }
            public string PersonaId { get; internal set; }
        }
        private OverrideLock Read_OverrideLock(BinaryReader reader)
        {
            var result = new OverrideLock();

            result.Version = ReadVersion(reader, 2, 0x1411B5890);
            result.Key = Read_Override_V5_V1(reader);
            if(result.Version < 2)
            {
                throw new Exception("TODO: Not yet implemented");
                result.Value_Old = BlueprintReader.ReadSomethingCrazy(reader, 0, 0);
            }
            else
            {
                result.Value = BlueprintReader.Read_BlueprintResource_v1_innerN_inner_inner(reader);
            }

            result.PersonaId = ReadUUID(reader);

            return result;
        }

        public class LicenseOverrideKey
        {
            public uint Value { get; internal set; }
            public BlueprintResource.V1_InnerL_v4_innerB Element { get; internal set; }
            public int PropertyCode { get; internal set; }
        }
        private LicenseOverrideKey Read_LicenseOverrideKey(BinaryReader reader)
        {
            var result = new LicenseOverrideKey();

            result.Value = ReadVersion(reader, 1, 0x1411C4E40);
            result.Element = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerB(reader);
            result.PropertyCode = reader.ReadInt32();

            return result;
        }

        public class LicenseOverride
        {
            public LicenseOverrideKey Key { get; internal set; }
            public BlueprintResource.V1_InnerN_inner_inner Value { get; internal set; }
        }
        private LicenseOverride Read_LicenseOverride(BinaryReader reader)
        {
            var result = new LicenseOverride();

            result.Key = Read_LicenseOverrideKey(reader);
            result.Value = BlueprintReader.Read_BlueprintResource_v1_innerN_inner_inner(reader);

            return result;
        }

        public class LicenseOverrides
        {
            public uint Version { get; internal set; }
            public List<LicenseOverride> Overrides { get; internal set; }
        }
        private LicenseOverrides Read_LicenseOverrides(BinaryReader reader)
        {
            var result = new LicenseOverrides();

            result.Version = ReadVersion(reader, 1, 0x1411A6E70);
            result.Overrides = Read_List(reader, Read_LicenseOverride, 1, 0x1411B7080);

            return result;
        }


        public class Override_V5
        {
            public uint Version { get; internal set; }
            public BlueprintResource.V1_InnerL_v4_innerC Handle { get; internal set; }
            public int PropertyIndex { get; internal set; }
            public int PropertyCode { get; internal set; }
        }
        private Override_V5 Read_Override_V5(BinaryReader reader)
        {
            var result = new Override_V5();

            result.Version = ReadVersion(reader, 1, 0x1411B70A0);
            result.Handle = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerC(reader);

            if (result.Version < 2)
            {
                result.PropertyIndex = reader.ReadInt32();
            }
            else
            {
                result.PropertyCode = reader.ReadInt32();
            }

            return result;
        }

        public class Override_V5_ListItem
        {
            public Override_V5_V1 Data { get; internal set; }
            public BlueprintResource.V1_InnerN_inner_inner Value { get; internal set; }
        }
        private Override_V5_ListItem Read_Override_V5_ListItem(BinaryReader reader)
        {
            var result = new Override_V5_ListItem();

            result.Data = Read_Override_V5_V1(reader);
            result.Value = BlueprintReader.Read_BlueprintResource_v1_innerN_inner_inner(reader);

            return result;
        }

        public class Override_V5_V1
        {
            public uint Version { get; internal set; }
            public BlueprintResource.V1_InnerL_v4_innerC Handle { get; internal set; }
            public int PropertyIndex { get; internal set; }
            public int PropertyCode { get; internal set; }
        }
        private Override_V5_V1 Read_Override_V5_V1(BinaryReader reader)
        {
            var result = new Override_V5_V1();

            var unknownFlag = true;
            if (unknownFlag)
            {
                result.Version = ReadVersion(reader, 2, 0x1411C4E10);
                result.Handle = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerC(reader);

                if (result.Version < 2)
                {
                    result.PropertyIndex = reader.ReadInt32();
                }
                else
                {
                    result.PropertyCode = reader.ReadInt32();
                }
            }
            else
            {
                result.Handle = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerC(reader);
                result.PropertyIndex = reader.ReadInt32();
            }

            return result;
        }

        public class Override_V5_V1_ListItem
        {
            public Override_V5_V1 Data { get; internal set; }
            public object Value { get; internal set; }
        }
        private Override_V5_V1_ListItem Read_Override_V5_V1_ListItem(BinaryReader reader)
        {
            var result = new Override_V5_V1_ListItem();

            throw new Exception("Not implemented");

            result.Data = Read_Override_V5_V1(reader);
            result.Value = BlueprintReader.ReadSomethingCrazy(reader, 0, 0); // TODO;

            return result;
        }

        public class LicenseOverrides_V5
        {
            public uint Version { get; internal set; }
            public List<Override_V5_V1_ListItem> Overrides_V1 { get; internal set; }
            public List<Override_V5_ListItem> Overrides { get; internal set; }
        }
        private LicenseOverrides_V5 Read_LicenseOverrides_V5(BinaryReader reader)
        {
            var result = new LicenseOverrides_V5();

            result.Version = ReadVersion(reader, 3, 0x1411A6E80);
            if (result.Version == 1)
            {
                result.Overrides_V1 = Read_List(reader, Read_Override_V5_V1_ListItem, 1, 0x1411B7090);
            }
            else
            {
                result.Overrides = Read_List(reader, Read_Override_V5_ListItem, 1, 0x1411B70A0);
            }
            return result;
        }

        public class ParamOverride
        {
            public uint Version { get; internal set; }
            public BlueprintResource.V1_InnerR_inner_C Parameter { get; internal set; }
            public BlueprintResource.V1_InnerL_v4_innerC SourceHandle { get; internal set; }
        }
        private ParamOverride Read_ParamOverride(BinaryReader reader)
        {
            var result = new ParamOverride();

            result.Version = ReadVersion(reader, 2, 0x1411B5160);
            result.Parameter = BlueprintReader.Read_BlueprintResource_v1_innerR_inner_C(reader);
            result.SourceHandle = BlueprintReader.Read_BlueprintResource_v1_innerL_v4_innerC(reader);

            return result;
        }

        public class WorldObject
        {
            internal CharacterLookData characterLookData;

            public uint Version { get; internal set; }
            public string Blueprint { get; internal set; }
            public string Name { get; internal set; }
            public List<BlueprintResource.V1_InnerR_inner_C> ParamOverrides { get; internal set; }
            [JsonIgnore]
            public List<ParamOverride> Overrides { get; internal set; }
            public int NumOverrides => Overrides?.Count ?? 0;
            public LicenseOverrides_V5 LicenseOverrides_V5 { get; internal set; }
            public LicenseOverrides LicenseOverrides { get; internal set; }
            public string UUID { get; internal set; }
            public float Scale { get; internal set; }
            public List<OverrideLock> OverrideLocks { get; internal set; }
            public bool characterLookDataUsed { get; internal set; }
            public List<DisplayNameOverride> DisplayNameOverrides { get; internal set; }
            public List<DisplayNameOverride_V10> DisplayNameOverrides_V10 { get; internal set; }
            public bool MoveableFromScript { get; internal set; }
            public List<ScriptUsingSceneScript> ScriptsUsingSceneScript { get; internal set; }
        }
        public WorldObject Read_WorldObject(BinaryReader reader)
        {
            var result = new WorldObject();

            result.Version = ReadVersion(reader, 18, 0x1410E3B50);
            result.Blueprint = ReadUUID(reader);
            result.Name = ReadString(reader);

            if(result.Version < 15)
            {
                if(result.Version < 4)
                {
                    result.ParamOverrides = Read_List(reader, BlueprintReader.Read_BlueprintResource_v1_innerR_inner_C, 1, 0x1411A6190);
                }
                else
                {
                    result.Overrides = Read_List(reader, Read_ParamOverride, 1, 0x1411A6180);
                }
            }

            if (result.Version >= 5 && result.Version <= 17)
            {
                if(result.Version < 13)
                {
                    result.LicenseOverrides_V5 = Read_LicenseOverrides_V5(reader);
                }
                else
                {
                    result.LicenseOverrides = Read_LicenseOverrides(reader);
                }
            }

            if(result.Version >= 2)
            {
                result.UUID = ReadUUID_B(reader);
            }

            if(result.Version >= 3)
            {
                result.Scale = reader.ReadSingle();
            }

            if(result.Version == 7)
            {
                result.OverrideLocks = Read_List(reader, Read_OverrideLock, 1, 0x1411A61A0);
                return result;
            }

            if(result.Version >= 9)
            {
                result.characterLookDataUsed = reader.ReadBoolean();
                if(result.characterLookDataUsed)
                {
                    result.characterLookData = Read_CharacterLookData(reader);
                }
            }

            if(result.Version >= 10)
            {
                if(result.Version >= 12)
                {
                    result.DisplayNameOverrides = Read_List(reader, Read_DisplayNameOverride, 1, 0x1411AA3F0);
                }
                else
                {
                    result.DisplayNameOverrides_V10 = Read_List(reader, Read_DisplayNameOverride_V10, 1, 0x1411AA3E0);
                }
            }

            if(result.Version >= 11)
            {
                result.MoveableFromScript = reader.ReadBoolean();
            }

            if(result.Version >= 16)
            {
                result.ScriptsUsingSceneScript = Read_List(reader, Read_ScriptUsingSceneScript, 1, 0x1411AA400);
            }

            return result;
        }

        public class SceneScriptLicenseSource
        {
            public uint Version { get; internal set; }
            public int Type { get; internal set; }
            public string ResourceId { get; internal set; }
            public string PersonaId { get; internal set; }
            public string Name { get; internal set; }
        }
        private SceneScriptLicenseSource Read_SceneScriptLicenseSource(BinaryReader reader)
        {
            var result = new SceneScriptLicenseSource();

            result.Version = ReadVersion(reader, 1, 0x1411B0180);
            result.Type = reader.ReadInt32();
            
            if(result.Type == 1)
            {
                result.ResourceId = ReadUUID(reader);
            }
            else
            {
                result.PersonaId = ReadUUID(reader);
                result.Name = ReadString(reader);
            }

            return result;
        }

        public class AudioStream
        {
            public uint Version { get; internal set; }
            public string Name { get; internal set; }
            public string Url { get; internal set; }
        }
        private AudioStream Read_AudioStream(BinaryReader reader)
        {
            var result = new AudioStream();

            result.Version = ReadVersion(reader, 1, 0x1411C03E0);
            result.Name = ReadString(reader);
            result.Url = ReadString(reader);

            return result;
        }

        public class AudioStreamListItem
        {
            public int Data { get; internal set; }
            public AudioStream Value { get; internal set; }
        }
        private AudioStreamListItem Read_AudioStreamListItem(BinaryReader reader)
        {
            var result = new AudioStreamListItem();

            result.Data = reader.ReadInt32();
            result.Value = Read_AudioStream(reader);

            return result;
        }

        public class AudioWorldSource
        {
            public uint Version { get; internal set; }
            public string BankResource { get; internal set; }
            public string BackgroundEvent { get; internal set; }
            public string BackgroundSoundResource { get; internal set; }
            public float BackgroundSoundLoudness { get; internal set; }
            public bool UseBackgroundStream { get; internal set; }
            public string BackgroundStreamUrl { get; internal set; }
            public int BackgroundStreamChannel { get; internal set; }
            public List<AudioStreamListItem> AudioStreams { get; internal set; }
        }
        private AudioWorldSource Read_AudioWorldSource(BinaryReader reader)
        {
            var result = new AudioWorldSource();

            result.Version = ReadVersion(reader, 4, 0x1411A0F30);

            if(result.Version < 4)
            {
                result.BankResource = ReadUUID(reader);
                result.BackgroundEvent = ReadUUID_B(reader);
            }

            result.BackgroundSoundResource = ReadUUID(reader);
            result.BackgroundSoundLoudness = reader.ReadSingle();

            if(result.Version >= 2)
            {
                result.UseBackgroundStream = reader.ReadBoolean();

                if(result.Version == 2)
                {
                    result.BackgroundStreamUrl = ReadString(reader);
                    if(result.UseBackgroundStream)
                    {
                        result.BackgroundStreamChannel = 11;
                    }
                }
                else
                {
                    result.BackgroundStreamChannel = reader.ReadInt32();
                }
            }

            if(result.Version >= 3)
            {
                result.AudioStreams = Read_List(reader,Read_AudioStreamListItem, 1, 0x1411AF530);
            }

            return result;
        }

        public class World
        {
            public uint Version { get; internal set; }
            public List<float> WorldUp { get; internal set; }
            public List<float> WorldForward { get; internal set; }
            public List<string> ChunkSources { get; internal set; }
            public string SceneBaseName { get; internal set; }
            public AudioWorldSource AudioWorldSource { get; internal set; }
            public List<BlueprintResource.V1_InnerN> Scripts { get; internal set; }
            public List<WorldObject> WorldObject { get; internal set; }
            public List<string> SceneScripts { get; internal set; }
            public List<SceneScriptLicenseSource> SceneScriptLicenseSources { get; internal set; }
            public string SkyCubeMap { get; internal set; }
            public float SkyBrightness { get; internal set; }
            public List<float> SkyRotation { get; internal set; }
            public List<List<float>> SkyRotationMatrix { get; internal set; }
            public WorldDefinitionResource.AtmosphereData AtmosphereData { get; internal set; }
            public WorldDefinitionResource.ExposureData ExposureData { get; internal set; }
            public WorldDefinitionResource.PostEffectData PostEffectData { get; internal set; }
            public string LightTransportBakeQuality { get; internal set; }
            public int LightTransportBakeQuality_v4 { get; internal set; }
            public WorldDefinitionResource.MediaSurfaceData MediaSurfaceData { get; internal set; }
            public WorldDefinitionResource.BloomData BloomData { get; internal set; }
            public List<OfflineResourceLoad> OfflineResourceLoads { get; internal set; }
            public float GravityMagnitude { get; internal set; }
            public List<EditWorldSessionDataListItem> EditWorldSessionData { get; internal set; }
            public List<Folder> Folders { get; internal set; }
            public bool EnableAvatarAvatarCollisions { get; internal set; }
            public bool AllowFreeCamera { get; internal set; }
            public bool AllowTeleport { get; internal set; }
            public float TeleportRangeMaximum { get; internal set; }
            public List<ParentingData> ParentingData { get; internal set; }
            public List<Parenting_V21> Parenting_V21 { get; internal set; }
            public List<Parenting_V19> Parenting_V19 { get; internal set; }
            public bool AllowBypassSpawnPoint { get; internal set; }
            public bool UseGrid { get; internal set; }
            public List<float> GridSpacing { get; internal set; }
            public WorldDefinitionResource.RuntimeInventorySettings RuntimeInventorySettings { get; internal set; }
        }
        private World Read_WorldSource(BinaryReader reader)
        {
            var result = new World();

            result.Version = ReadVersion(reader, 25, 0x141133CF0);
            result.WorldUp = ReadVectorF(reader, 4);
            result.WorldForward = ReadVectorF(reader, 4);
            result.ChunkSources = Read_List(reader, ReadUUID, 1, 0x1411A03F0);
            result.SceneBaseName = ReadString(reader);
            result.AudioWorldSource = Read_AudioWorldSource(reader);

            if(result.Version < 16)
            {
                result.Scripts = Read_List(reader, BlueprintReader.Read_BlueprintResource_v1_innerN, 1, 0x1411A0410);
            }
            else
            {
                result.WorldObject = Read_List(reader, Read_WorldObject, 1, 0x1411A0400);
            }

            if(result.Version >= 23)
            {
                result.SceneScripts = Read_List(reader, ReadUUID, 1, 0x1411A35F0);
                result.SceneScriptLicenseSources = Read_List(reader, Read_SceneScriptLicenseSource, 1, 0x1411A3600);
            }

            // TODO: No version check
            //if(result.Version < 2)
            //{
            //    result.SkyCubeMap = ReadUUID(reader);
            //}
            //else
            //{
                result.SkyCubeMap = ReadUUID(reader);
            //}

            if(result.Version >= 4)
            {
                result.SkyBrightness = reader.ReadSingle();
            }

            result.SkyRotation = ReadVectorF(reader, 4);
            result.SkyRotationMatrix = Read_RotationMatrix(reader);
            result.AtmosphereData = WorldDefinitionReader.Read_AtmosphereData(reader);
            if(result.Version >= 7)
            {
                result.ExposureData = WorldDefinitionReader.Read_ExposureData(reader);
            }

            if(result.Version >= 12)
            {
                result.PostEffectData = WorldDefinitionReader.Read_PostEffectData(reader);
            }

            if(result.Version < 6)
            {
                if(result.Version == 5)
                {
                    result.LightTransportBakeQuality = ReadString(reader);
                }
                else if(result.Version == 4)
                {
                    result.LightTransportBakeQuality_v4 = reader.ReadInt32();
                }
            }

            if(result.Version >= 3)
            {
                result.MediaSurfaceData = WorldDefinitionReader.Read_MediaSurfaceData(reader);
            }
            if(result.Version >= 8)
            {
                result.BloomData = WorldDefinitionReader.Read_BloomData(reader);
            }

            if(result.Version >= 9 && result.Version <= 14)
            {
                result.OfflineResourceLoads = Read_List(reader, Read_OfflineResourceLoad, 1, 0x1411A0420);
            }

            if(result.Version >= 10)
            {
                result.GravityMagnitude = reader.ReadSingle();
            }

            if(result.Version >= 11)
            {
                result.EditWorldSessionData = Read_List(reader, Read_EditWorldSessionDataItem, 1, 0x1411A3740);
            }

            if(result.Version >= 13 && result.Version <= 18)
            {
                result.Folders = Read_List(reader, Read_Folder, 1, 0x1411A0430);
            }

            if(result.Version >= 14)
            {
                result.EnableAvatarAvatarCollisions = reader.ReadBoolean();
            }

            if(result.Version >= 17)
            {
                result.AllowFreeCamera = reader.ReadBoolean();
                result.AllowTeleport = reader.ReadBoolean();
                result.TeleportRangeMaximum = reader.ReadSingle();
            }

            if(result.Version >= 19)
            {
                if(result.Version >= 21)
                {
                    if(result.Version >= 22)
                    {
                        result.ParentingData = Read_List(reader, Read_ParentingData, 1, 0x1411A0450);
                    }
                    else
                    {
                        result.Parenting_V21 = Read_List(reader, Read_Parenting_V21, 1, 0x1411A0440);
                    }
                }
                else
                {
                    result.Parenting_V19 = Read_List(reader, Read_Parenting_V19, 1, 0x14119E350);
                }
            }

            if(result.Version >= 20)
            {
                result.AllowBypassSpawnPoint = reader.ReadBoolean();
            }

            if(result.Version >= 24)
            {
                result.UseGrid = reader.ReadBoolean();
                result.GridSpacing = ReadVectorF(reader, 4);
            }

            if(result.Version >= 25)
            {
                result.RuntimeInventorySettings = WorldDefinitionReader.Read_RuntimeInventorySettings(reader);
            }

            return result;
        }

        public World Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_WorldSource(reader);
            }
        }

        private BlueprintResource BlueprintReader;
        private WorldDefinitionResource WorldDefinitionReader;
        private ClusterDefinitionResource ClusterReader;
        public WorldSource()
        {
            BlueprintReader = new BlueprintResource();
            BlueprintReader.OverrideVersionMap(this.versionMap, this.componentMap);

            WorldDefinitionReader = new WorldDefinitionResource();
            WorldDefinitionReader.OverrideVersionMap(this.versionMap, this.componentMap);

            ClusterReader = new ClusterDefinitionResource();
            ClusterReader.OverrideVersionMap(this.versionMap, this.componentMap);
        }

        internal override void OverrideVersionMap(Dictionary<ulong, uint> newVersionMap, Dictionary<uint, object> newComponentMap)
        {
            this.versionMap = newVersionMap;
            this.componentMap = newComponentMap;

            BlueprintReader.OverrideVersionMap(newVersionMap, newComponentMap);
            WorldDefinitionReader.OverrideVersionMap(newVersionMap, newComponentMap);
            ClusterReader.OverrideVersionMap(newVersionMap, newComponentMap);
        }
    }
}
