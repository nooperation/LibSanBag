using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LibSanBag.FileResources
{
    public class BlueprintResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static BlueprintResource Create(string version = "")
        {
            return new BlueprintResource();
        }

        public class AInner
        {
            public uint Version { get; set; }
            public uint VersionB { get; set; }
            public string OriginClusterId { get; internal set; }
            public int LinkId { get; internal set; }
        }
        private AInner Read_BlueprintResource_A_inner(BinaryReader reader)
        {
            var result = new AInner();

            result.Version = ReadVersion(reader, 2, 0x1411B1100);
            result.OriginClusterId = ReadUUID(reader);

            if (result.Version >= 2)
            {
                result.LinkId = reader.ReadInt32();
            }

            return result;
        }

        public class AData
        {
            public uint Version { get; set; }
            public AInner UnknownA { get; internal set; }
            public int Type { get; internal set; }
            public bool Breakable { get; internal set; }
            public uint VersionB { get; internal set; }
            public uint VersionC { get; internal set; }
            public string VersionCString { get; internal set; }
            public string UnknownD { get; internal set; }
        }
        private AData Read_BluepintResource_A(BinaryReader reader)
        {
            var result = new AData();

            result.Version = ReadVersion(reader, 2, 0x1410BB130);
            if (result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.Type = reader.ReadInt32();
            result.Breakable = reader.ReadBoolean();

            if (result.Type == 1)
            {
                // info =
                result.VersionB = ReadVersion(reader, 1, 0x1411ECD30);
            }
            else if (result.Type == 2)
            {
                // info =
                result.VersionC = ReadVersion(reader, 1, 0x1411ECD40);
                result.VersionCString = ReadString(reader);
            }
            else
            {
                // info = 
                result.UnknownD = ReadUUID(reader);
            }

            return result;
        }

        public class BData
        {
            public uint Version { get; set; }
            public uint EntityA { get; set; }
            public uint EntityB { get; set; }
        }
        private BData Read_BluepintResource_B(BinaryReader reader)
        {
            var result = new BData();

            result.Version = ReadVersion(reader, 1, 0x1411D9940);

            result.EntityA = reader.ReadUInt32();
            result.EntityB = reader.ReadUInt32();

            return result;
        }

        public class BlueprintV1Inner
        {
            public uint Version { get; set; }
            public int LinkId { get; internal set; }
        }
        public BlueprintV1Inner Read_BlueprintResource_v1_inner(BinaryReader reader)
        {
            var result = new BlueprintV1Inner();

            result.Version = ReadVersion(reader, 2, 0x1410BB140);
            if(result.Version >= 2)
            {
                result.LinkId = reader.ReadInt32();
            }
            else
            {
                // ControllerSetupData
                ReadComponent(reader, _ => { /* noop */ });
            }

            return result;
        }

        public class BlueprintV1InnerB
        {
            public uint Version { get; set; }
            public uint Start { get; internal set; }
            public uint End { get; internal set; }
        }
        public BlueprintV1InnerB Read_BlueprintResource_v1_innerB(BinaryReader reader)
        {
            var result = new BlueprintV1InnerB();

            result.Version = ReadVersion(reader, 1, 0x1411D9950);
            result.Start = reader.ReadUInt32();
            result.End = reader.ReadUInt32();

            return result;
        }

        public class BlueprintV1
        {
            public List<BlueprintV1Inner> UnknownA { get; internal set; }
            public List<int> UnknownB { get; internal set; }
            public int UnknownD { get; internal set; }
            public List<List<int>> UnknownE { get; internal set; }
            public List<AData> UnknownF { get; internal set; }
            public List<BData> UnknownG { get; internal set; }
            public List<BlueprintV1InnerB> UnknownH { get; internal set; }
            public List<int> UnknownI { get; internal set; }
            public List<V1_InnderD> UnknownJ { get; internal set; }
            public List<V1_InnerE> UnknownK { get; internal set; }
            public List<V1_InnerF> UnknownL { get; internal set; }
            public List<V1_InnerG> UnknownM { get; internal set; }
            public List<V1_InnerH> UnknownN { get; internal set; }
            public List<V1_InnerI> UnknownO { get; internal set; }
            public List<V1_InnerJ> UnknownP { get; internal set; }
            public List<V1_InnerK> UnknownQ { get; internal set; }
            public List<V1_InnerL> UnknownR { get; internal set; }
            public List<V1_InnerM> UnknownS { get; internal set; }
            public List<V1_InnerN> UnknownT { get; internal set; }
            public List<V1_InnerO> UnknownU { get; internal set; }
            public List<V1_InnerP> UnknownV { get; internal set; }
            public string UnknownW { get; internal set; }
            public int UnknownX { get; internal set; }
            public List<V1_InnerQ> UnknownY { get; internal set; }
            public V1_InnerR UnknownZ { get; internal set; }
            public List<string> UnknownAA { get; internal set; }
            public int UnknownAb { get; internal set; }
            public List<V1_UnknownC_Inner> UnknownC { get; internal set; }
        }
        private BlueprintV1 Read_BlueprintResource_v1(BinaryReader reader, uint outerVersion)
        {
            var result = new BlueprintV1();

            result.UnknownA = Read_List(reader, Read_BlueprintResource_v1_inner, 1, 0x1411CD360);
            result.UnknownB = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            result.UnknownC = Read_List(reader, Read_BlueprintResource_v1_UnknownC_inner, 1, 0x1411D9910);
            result.UnknownD = 19;

            if(outerVersion >= 2)
            {
                result.UnknownD = reader.ReadInt32();
            }

            result.UnknownE = new List<List<int>>();
            for (int i = 0; i < result.UnknownD; i++)
            {
                var item = Read_List(reader, n => n.ReadInt32(), 1, 0x1411CD380);
                result.UnknownE.Add(item);
            }

            result.UnknownF = Read_List(reader, Read_BluepintResource_A, 1, 0x1411CD390);
            result.UnknownG = Read_List(reader, Read_BluepintResource_B, 1, 0x1411CD3A0);
            result.UnknownH = Read_List(reader, Read_BlueprintResource_v1_innerB, 1, 0x1411CD3B0);
            result.UnknownI = Read_List(reader, n => n.ReadInt32(), 1, 0x1411CD3C0);
            result.UnknownJ = Read_List(reader, Read_BlueprintResource_v1_innerD, 1, 0x1411D00F0);
            result.UnknownK = Read_List(reader, Read_BlueprintResource_v1_innerE, 1, 0x1411D0110);
            result.UnknownL = Read_List(reader, Read_BlueprintResource_v1_innerF, 1, 0x1411D0120);
            result.UnknownM = Read_List(reader, Read_BlueprintResource_v1_innerG, 1, 0x1411D0130);
            result.UnknownN = Read_List(reader, Read_BlueprintResource_v1_innerH, 1, 0x1411D0140);
            result.UnknownO = Read_List(reader, Read_BlueprintResource_v1_innerI, 1, 0x1411D0150);
            result.UnknownP = Read_List(reader, Read_BlueprintResource_v1_innerJ, 1, 0x1411D0160);
            result.UnknownQ = Read_List(reader, Read_BlueprintResource_v1_innerK, 1, 0x1411D0170);
            result.UnknownR = Read_List(reader, Read_BlueprintResource_v1_innerL, 1, 0x1411D0180);
            result.UnknownS = Read_List(reader, Read_BlueprintResource_v1_innerM, 1, 0x1411D0190);
            result.UnknownT = Read_List(reader, Read_BlueprintResource_v1_innerN, 1, 0x1411A0410);

            if(outerVersion >= 3)
            {
                result.UnknownU = Read_List(reader, Read_BlueprintResource_v1_innerO, 1, 0x1411D01A0);
            }

            result.UnknownV = Read_List(reader, Read_BlueprintResource_v1_innerP, 1, 0x1411CD370);
            result.UnknownW = ReadString(reader);

            result.UnknownX = 11;
            if(outerVersion >= 2)
            {
                result.UnknownX = reader.ReadInt32();
            }

            for (int i = 0; i < result.UnknownX; i++)
            {
                result.UnknownY = Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
            }

            result.UnknownZ = Read_BlueprintResource_v1_innerR(reader);
            result.UnknownAA = Read_List(reader, ReadString, 1, 0x1411CD3D0);
            result.UnknownAb = reader.ReadInt32();
            
            return result;
        }


        public class V1_InnerP_innerA
        {
            public uint Version { get; internal set; }
            public int DynamicFriction { get; internal set; }
            public int StaticFriction { get; internal set; }
            public int Restitution { get; internal set; }
        }
        private V1_InnerP_innerA Read_BlueprintResource_v1_innerP_innerA(BinaryReader reader)
        {
            var result = new V1_InnerP_innerA();

            result.Version = ReadVersion(reader, 1, 0x1412021E0);

            result.DynamicFriction = reader.ReadInt32();
            result.StaticFriction = reader.ReadInt32();
            result.Restitution = reader.ReadInt32();

            return result;
        }

        public class V1_InnerP_inner
        {
            public uint Version { get; internal set; }
            public V1_InnerP_innerA Material { get; internal set; }
            public string Name { get; internal set; }
            public bool IsDynamic { get; internal set; }
            public int MotionType { get; internal set; }
            public bool CanGrab { get; internal set; }
            public bool IsTriggerVolume { get; internal set; }
            public List<float> LinearVelocity { get; internal set; }
            public List<float> AngularVelocity { get; internal set; }
            public float Density { get; internal set; }
            public string AudioMaterialId { get; internal set; }
            public List<ClusterDefinitionResource.AudioResourcePoolSound> AudioResourcePools { get; internal set; }
            public bool CanRide { get; internal set; }
            public int FilterType { get; internal set; }
            public string AudioMaterialTrackingId { get; internal set; }
        }
        private V1_InnerP_inner Read_BlueprintResource_v1_innerP_inner(BinaryReader reader)
        {
            var result = new V1_InnerP_inner();

            result.Version = ReadVersion(reader, 11, 0x1411ECD20);
            result.Material = Read_BlueprintResource_v1_innerP_innerA(reader);
            result.Name = ReadString(reader);

            if (result.Version < 6)
            {
                result.IsDynamic = reader.ReadBoolean();
            }
            else
            {
                result.MotionType = reader.ReadInt32();
                result.CanGrab = reader.ReadBoolean();
            }

            if(result.Version < 8 && result.MotionType == 2)
            {
                // TODO: ??? noop?
            }

            if(result.Version >= 4)
            {
                result.IsTriggerVolume = reader.ReadBoolean();
            }

            result.LinearVelocity = ReadVectorF(reader, 4);
            result.AngularVelocity = ReadVectorF(reader, 4);
            result.Density = reader.ReadSingle();

            if(result.Version >= 2)
            {
                result.AudioMaterialId = ReadUUID(reader);
            }
            if(result.Version >= 9)
            {
                result.AudioResourcePools = ClusterReader.Read_RigidBody_AudioResourcePoolSounds(reader);
            }
            if(result.Version >= 10)
            {
                result.CanRide = reader.ReadBoolean();
            }
            if(result.Version >= 11)
            {
                result.FilterType = reader.ReadInt32();
            }
            if(result.Version - 5 <= 1)
            {
                result.AudioMaterialTrackingId = ReadUUID_B(reader);
            }

            return result;
        }

        public class V1_InnerX
        {
            public uint Version { get; internal set; }
        }
        private V1_InnerX Read_BlueprintResource_v1_innerX(BinaryReader reader)
        {
            var result = new V1_InnerX();

            return result;
        }

        public class V1_InnerW
        {
            public uint Version { get; internal set; }
            public V1_InnerU_inner UnknownA { get; internal set; }
        }
        private V1_InnerW Read_BlueprintResource_v1_innerW(BinaryReader reader)
        {
            var result = new V1_InnerW();

            result.Version = ReadVersion(reader, 1, 0x1410BB100);
            result.UnknownA = Read_BlueprintResource_v1_innerU_inner(reader);

            return result;
        }

        public class V1_InnerV
        {
            public uint Version { get; internal set; }
            public int AttachmentId { get; internal set; }
            public V1_InnerU_inner UnknownA { get; internal set; }
        }
        private V1_InnerV Read_BlueprintResource_v1_innerV(BinaryReader reader)
        {
            var result = new V1_InnerV();

            result.Version = ReadVersion(reader, 1, 0x1410BB0F0);
            result.UnknownA = Read_BlueprintResource_v1_innerU_inner(reader);
            result.AttachmentId = reader.ReadInt32();

            return result;
        }

        public class V1_InnerU_inner
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
        }
        private V1_InnerU_inner Read_BlueprintResource_v1_innerU_inner(BinaryReader reader)
        {
            var result = new V1_InnerU_inner();

            result.Version = ReadVersion(reader, 1, 0x1411DDD80);
            result.UnknownA = Read_BlueprintResource_A_inner(reader);

            return result;
        }

        public class V1_InnerU
        {
            public uint Version { get; internal set; }
           
            public V1_InnerU_inner UnknownA { get; internal set; }
            public int AttachmentId { get; internal set; }
            public bool IsSticky { get; internal set; }
            public bool AimAtCursor { get; internal set; }
            public AInner UnknownE { get; internal set; }
        }
        private V1_InnerU Read_BlueprintResource_v1_innerU(BinaryReader reader)
        {
            var result = new V1_InnerU();

            result.Version = ReadVersion(reader, 5, 0x1410BB0E0);
            if(result.Version >= 3)
            {
                result.UnknownA = Read_BlueprintResource_v1_innerU_inner(reader);
                result.AttachmentId = reader.ReadInt32();

                if (result.Version >= 4)
                {
                    result.IsSticky = reader.ReadBoolean();
                }
                if (result.Version >= 5)
                {
                    result.AimAtCursor = reader.ReadBoolean();
                }
            }
            else
            {
                result.UnknownE = Read_BlueprintResource_A_inner(reader);
                if(result.Version >= 2)
                {
                    result.AttachmentId = reader.ReadInt32();
                }
            }

            return result;
        }

        public class V1_InnerT
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
        }
        private V1_InnerT Read_BlueprintResource_v1_innerT(BinaryReader reader)
        {
            var result = new V1_InnerT();

            result.Version = ReadVersion(reader, 2, 0x1410BB0D0);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            return result;
        }

        public class V1_InnerS
        {
            public uint Version { get; internal set; }
            public V1_InnerL UnknownA { get; internal set; }
            public string ClothSource { get; internal set; }
        }
        private V1_InnerS Read_BlueprintResource_v1_innerS(BinaryReader reader)
        {
            var result = new V1_InnerS();

            result.Version = ReadVersion(reader, 1, 0x1410BB0C0);
            if(result.Version == 1)
            {
                result.UnknownA = Read_BlueprintResource_v1_innerL(reader);
                result.ClothSource = ReadUUID(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_E
        {
            public List<V1_InnerR_inner_A> InstanceDescriptors { get; internal set; }
            public List<V1_InnerR_inner_B> TerminalParametersInfo { get; internal set; }
            public List<int> TerminalParametersInfoIndices { get; internal set; }
            public List<V1_InnerR_inner_C> Parameters { get; internal set; }
            public List<int> ParameterConnectionHandleIndices { get; internal set; }
            public List<long> ElementPropertyAccessorIndices { get; internal set; }
            public List<V1_InnerR_inner_D> Connections { get; internal set; }
            public long InputParameterHandleIndex { get; internal set; }
            public long OutputParameterHandleIndex { get; internal set; }
            public long InstanceDescriptorHandleIndex { get; internal set; }
        }
        private V1_InnerR_inner_E Read_BlueprintResource_v1_innerR_inner_E(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_E();

            result.InstanceDescriptors = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_A, 1, 0x1411DA7B0);
            result.TerminalParametersInfo = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_B, 1, 0x1411DA7C0);
            result.TerminalParametersInfoIndices = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            result.Parameters = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_C, 1, 0x1411A6190);
            result.ParameterConnectionHandleIndices = Read_List(reader, n => n.ReadInt32(), 1, 0x1411DC3F0);

            var unknown = 0; // TODO: Unknown counter
            result.ElementPropertyAccessorIndices = new List<long>();
            for (int i = 0; i < unknown; i++)
            {
                var item = reader.ReadInt64();
                result.ElementPropertyAccessorIndices.Add(item);
            }
            
            result.Connections = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_D, 1, 0x1411DA7D0);

            var unknown2 = 0; // TODO: Unknown counter
            for (var i = 0; i < unknown2; ++i)
            {
                result.InputParameterHandleIndex = reader.ReadInt64();
                result.OutputParameterHandleIndex = reader.ReadInt64();
            }

            var unknown3 = 0; // TODO: Unknown counter
            for (var i = 0; i < unknown3; ++i)
            {
                result.InstanceDescriptorHandleIndex = reader.ReadInt64();
            }

            return result;
        }

        public class V1_InnerR_inner_D
        {
            public uint Version { get; internal set; }
            public int ConnectionInputParameterConnectorIndex { get; internal set; }
            public int ConnectionOutputParameterConnectorIndex { get; internal set; }
            public V1_InnerR_inner_B_inner InputParameterHandle { get; internal set; }
            public V1_InnerR_inner_B_inner OutputParameterHandle { get; internal set; }
        }
        private V1_InnerR_inner_D Read_BlueprintResource_v1_innerR_inner_D(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_D();

            result.Version = ReadVersion(reader, 2, 0x1410BB160);
            result.ConnectionInputParameterConnectorIndex = reader.ReadInt32();
            result.ConnectionOutputParameterConnectorIndex = reader.ReadInt32();

            if(result.Version >= 2)
            {
                result.InputParameterHandle = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
                result.OutputParameterHandle = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_C
        {
            public uint Version { get; internal set; }
            public int ParameterType { get; internal set; }
            public List<V1_InnerR_inner_C_A> InputConnectors { get; internal set; }
            public List<int> OutputConnectorsOld { get; internal set; }
            public List<V1_InnerR_inner_C_A_internal> OutputConnectors { get; internal set; }
            public List<string> InputConnectorNames { get; internal set; }
            public List<string> OutputConnectorNames { get; internal set; }
            public List<V1_InnerR_inner_Ab_internal> ParameterAttributes { get; internal set; }
            public List<List<V1_InnerR_inner_Ab_internal>> UserAttributes { get; internal set; }
            public V1_InnerR_inner_B_inner InstanceDescriptorHandle { get; internal set; }
            public V1_InnerR_inner_B_inner ElementPropertyAccessorHandle { get; internal set; }
        }
        public V1_InnerR_inner_C Read_BlueprintResource_v1_innerR_inner_C(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_C();

            result.Version = ReadVersion(reader, 4, 0x1410BB150);
            result.ParameterType = reader.ReadInt32();
            result.InputConnectors = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_C_A, 1, 0x1411C3B90);

            if(result.Version < 3)
            {
                // TODO: Confirm this, i don't think actually reads anything int he internal loop...
                result.OutputConnectorsOld = Read_List(reader, n => { return -1; }, 1, 0x1411C3BB0);
            }
            else
            {
                result.OutputConnectors = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_C_A_internal, 1, 0x1411C3BA0);
            }

            result.InputConnectorNames = Read_List(reader, ReadString, 1, 0x14119ADB0);
            result.OutputConnectorNames = Read_List(reader, ReadString, 1, 0x14119ADB0);




            if(result.ParameterType == 0x01 ||
                result.ParameterType == 0x06 ||
                result.ParameterType == 0x08 ||
                result.ParameterType == 0x07 ||
                result.ParameterType == 0x0A || 
                
                result.ParameterType == 0x3b ||
                result.ParameterType == 0x7e ||
                result.ParameterType == 0x7f || 
                result.ParameterType == 0x0e || 
                result.ParameterType == 0x80 || 
                result.ParameterType == 0x38
            )
            {
                // nothing
            }
            else if (result.ParameterType == 3)
            {
                var x = reader.ReadInt32();
                var z = ReadUUID(reader);
            }
            else if(result.ParameterType == 0x11)
            {
                var x = reader.ReadByte();
            }
            else if(result.ParameterType == 0x1c)
            {
                var x = reader.ReadByte();
            }
            else if(result.ParameterType == 0x20)
            {
                var y = reader.ReadInt32();
            }
            else if(result.ParameterType == 0x7b)
            {
                var y = reader.ReadBytes(0x14);
            }
            else if(result.ParameterType == 0x74)
            {
                var y = reader.ReadBytes(0x14);
            }
            else if (result.ParameterType == 0x1B)
            {
                var y = ReadVectorF(reader, 4);
            }
            else if(result.ParameterType == 0x15)
            {
                var y = reader.ReadInt32();
            }
            else if(result.ParameterType == 0x16)
            {
                var y = reader.ReadUInt64();
            }
            else if(result.ParameterType == 0x25)
            {
                var y = reader.ReadSingle();
            }
            else if(result.ParameterType == 0x26)
            {
                var xx = ReadVectorF(reader, 4);
            }
            else if(result.ParameterType == 0x75)
            {
                var y = reader.ReadInt32();
            }
            else if(result.ParameterType == 0x18)
            {
                var x = reader.ReadInt32();
                var y = ReadUUID(reader);
            }
            else if(result.ParameterType == 0x72)
            {
                var x = ReadUUID(reader);
                var y = reader.ReadSingle();
                var z = ReadUUID(reader);
                var a  = reader.ReadInt64();
                var b  = reader.ReadInt32();

            }
            else
            {
                Console.WriteLine("Unknown parameter type: {0:x}", result.ParameterType);
               // throw new Exception("Butts");
            }

            if (false)
            {
                /*
                for (int i = 0; i < 0; i++)
                {
                    switch(0)
                    {
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 18:
                        case 19:
                            // todo
                            break;
                        default:
                            break;
                    }
                    switch(0)
                    {
                        case 0:
                            reader.ReadByte();
                            break;
                        case 1:
                        case 8:
                        case 9:
                            // todo
                            break;
                        case 2:
                            reader.ReadUInt32();
                            break;
                        case 3:
                        case 5:
                            reader.ReadSingle();
                            break;
                        case 4:
                            reader.ReadBoolean();
                            break;
                        case 6:
                            ReadVectorF(reader, 4);
                            break;
                        case 7:
                            // todo
                            break;
                        case 15:
                            // todo
                            break;
                        case 16:
                            ReadString(reader);
                            break;
                        case 10:
                            Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
                            break;
                        case 22:
                            // todo
                            break;
                        case 24:
                            // todo
                            break;
                        default:
                            break;
                    }
                }
                */
            }

            if (result.Version >= 3)
            {
                result.ParameterAttributes = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ab_internal, 1, 0x1411C3BC0);
                result.UserAttributes = Read_List(
                    reader, 
                    n => Read_List(n, Read_BlueprintResource_v1_innerR_inner_Ab_internal, 1, 0x1411C3BC0), 
                    1,
                    0x1411C3BD0
                );
            }

            if(result.Version >= 4)
            {
                result.InstanceDescriptorHandle = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
                result.ElementPropertyAccessorHandle = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_C_A_internal
        {
            public uint Version { get; internal set; }
            public List<V1_InnerR_inner_Ab_internal> Attributes { get; internal set; }
            public V1_InnerR_inner_B_inner ConnectionHandle { get; internal set; }
        }
        private V1_InnerR_inner_C_A_internal Read_BlueprintResource_v1_innerR_inner_C_A_internal(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_C_A_internal();

            result.Version = ReadVersion(reader, 3, 0x1411D0CD0);

            if (result.Version >= 2)
            {
                result.Attributes = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ab_internal, 1, 0x1411C3BC0);
            }
            if (result.Version >= 3)
            {
                result.ConnectionHandle = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_C_A
        {
            public uint Version { get; internal set; }
            public V1_InnerR_inner_C_A_internal UnknownA { get; internal set; }
            public int DataType { get; internal set; }
            public byte[] InputRangeByteArray { get; internal set; }
        }
        private V1_InnerR_inner_C_A Read_BlueprintResource_v1_innerR_inner_C_A(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_C_A();

            result.Version = ReadVersion(reader, 2, 0x1411D0CC0);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_v1_innerR_inner_C_A_internal(reader);
            }

            result.DataType = reader.ReadInt32();
            result.InputRangeByteArray = Read_Array(reader);

            return result;
        }

        public class V1_InnerR_inner_B
        {
            public uint Version { get; internal set; }
            public int ElementType { get; internal set; }
            public int PropertyIndex { get; internal set; }
            public int ArrayIndexPropertyIndex { get; internal set; }
            public int ArrayValueType { get; internal set; }
            public V1_InnerR_inner_B_inner ElementHandle { get; internal set; }
        }
        private V1_InnerR_inner_B Read_BlueprintResource_v1_innerR_inner_B(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_B();
            result.ElementType = 0xFFFF;

            result.Version = ReadVersion(reader, 3, 0x1410BB170);
            if(result.Version >= 3)
            {
                // noop
            }
            else
            {
                result.ElementType = reader.ReadInt32();
            }
            result.PropertyIndex = reader.ReadInt32();
            result.ArrayIndexPropertyIndex = reader.ReadInt32();
            result.ArrayValueType = reader.ReadInt32();

            if (result.Version >= 2)
            {
                result.ElementHandle = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_B_inner
        {
            public long Bits { get; internal set; }
            public V1_InnerL_v4_innerC UnknownB { get; internal set; }
        }
        private V1_InnerR_inner_B_inner Read_BlueprintResource_v1_innerR_inner_B_inner(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_B_inner();

            // todo: This also exists in Read_BlueprintResource_v1_innerR_inner_Ab_internal
            if (false /* something confusing */)
            {
                // TODO: This might be possible?
                result.Bits = reader.ReadInt64(); // I don't know
            }
            else
            {
                result.UnknownB = Read_BlueprintResource_v1_innerL_v4_innerC(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_Aa
        {
            public uint Version { get; internal set; }
            public bool IsRequired { get; internal set; }
            public int RangeOffsetFromStateInstance { get; internal set; }
            public V1_InnerR_inner_Ab UnknownC { get; internal set; }
        }
        private V1_InnerR_inner_Aa Read_BlueprintResource_v1_innerR_inner_Aa(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_Aa();

            result.UnknownC = Read_BlueprintResource_v1_innerR_inner_Ab(reader);

            result.Version = ReadVersion(reader, 2, 0x141209060);
            result.IsRequired = reader.ReadBoolean();
            if(result.Version < 2)
            {
                result.RangeOffsetFromStateInstance = reader.ReadInt32();
            }

            return result;
        }

        public class V1_InnerR_inner_Ac
        {
            public V1_InnerR_inner_Ab UnknownA { get; internal set; }
            public uint Version { get; internal set; }
            public int ValueOffsetFromStructInStateInstance { get; internal set; }
        }
        private V1_InnerR_inner_Ac Read_BlueprintResource_v1_innerR_inner_Ac(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_Ac();

            result.UnknownA = Read_BlueprintResource_v1_innerR_inner_Ab(reader);

            result.Version = ReadVersion(reader, 2, 0x141209070);
            if(result.Version < 2)
            {
                result.ValueOffsetFromStructInStateInstance = reader.ReadInt32();
            }

            return result;
        }


        public class V1_InnerR_inner_Ab_internal
        {
            public uint Version { get; internal set; }
            public int Type { get; internal set; }
            public V1_InnerR_inner_B_inner UnknownC { get; internal set; }
            public string UnknownB { get; internal set; }
            public string Name { get; internal set; }
            public int Result_Int { get; internal set; }
            public float Result_Float { get; internal set; }
            public string Result_StringRef { get; internal set; }
            public V1_InnerR_inner_B_inner ElementHandle { get; internal set; }
        }
        private V1_InnerR_inner_Ab_internal Read_BlueprintResource_v1_innerR_inner_Ab_internal(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_Ab_internal();

            result.Version = ReadVersion(reader, 3, 0x1411D0CE0);
            result.Type = reader.ReadInt32();

            /*
             * TODO: REVISIT
             * */
            if(result.Type == 1)
            {
                result.Result_Int = reader.ReadInt32();
            }
            else if(result.Type == 3)
            {
                result.Result_Float = reader.ReadSingle();
            }
            else if(result.Type == 6)
            {
                result.Result_StringRef = ReadString(reader);
            }
            else
            {
                result.ElementHandle = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
            }

            if(result.Version >= 2)
            {
                result.Name = ReadString(reader);
            }

            /*
            if(result.Type - 1 != 0 && result.Type - 2 != 0)
            {
                if(result.Type - 3 == 3)
                {
                    result.UnknownB = ReadString(reader);
                }
                else
                {
                    result.UnknownC = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
                }
            }
            if (result.Version >= 2)
            {
                result.Name = ReadString(reader);
            }
            */

            return result;
        }

        public class V1_InnerR_inner_Ab
        {
            public uint Version { get; internal set; }
            public int Type { get; internal set; }
            public int EditIndex { get; internal set; }
            public int ValueOffsetFromStateInstance { get; internal set; }
            public string Name { get; internal set; }
            public List<V1_InnerR_inner_Ab_internal> Attributes { get; internal set; }
        }
        private V1_InnerR_inner_Ab Read_BlueprintResource_v1_innerR_inner_Ab(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_Ab();

            result.Version = ReadVersion(reader, 4, 0x14120B730);
            result.Type = reader.ReadInt32();
            result.EditIndex = reader.ReadInt32();

            if(result.Version < 4)
            {
                result.ValueOffsetFromStateInstance = reader.ReadInt32();
            }
            if(result.Version >= 2)
            {
                result.Name = ReadString(reader);
            }
            if(result.Version >= 3)
            {
                // TODO: probably broken
                result.Attributes = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ab_internal, 1, 0x1411C3BC0);
            }

            return result;
        }

        public class V1_InnerR_inner_A
        {
            public uint Version { get; internal set; }
            public List<V1_InnerR_inner_Aa> InputConnectorDescriptors { get; internal set; }
            public List<V1_InnerR_inner_Ab> OutputConnectorDescriptors { get; internal set; }
            public List<V1_InnerR_inner_Ac> UserValueDescriptors { get; internal set; }
            public bool RequiresInstanceDescriptor { get; internal set; }
        }
        private V1_InnerR_inner_A Read_BlueprintResource_v1_innerR_inner_A(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_A();

            result.Version = ReadVersion(reader, 1, 0x1410BB180);
            result.InputConnectorDescriptors = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Aa, 1, 0x141201180);
            result.OutputConnectorDescriptors = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ab, 1, 0x141201190);
            result.UserValueDescriptors = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ac, 1, 0x1412011A0);
            result.RequiresInstanceDescriptor = reader.ReadBoolean();

            return result;
        }

        public class V1_InnerR
        {
            public uint Version { get; internal set; }
            public List<V1_InnerR_inner_A> InstanceDescriptors { get; internal set; }
            public List<V1_InnerR_inner_B> TerminalParametersInfo { get; internal set; }
            public List<V1_InnerR_inner_C> Parameters { get; internal set; }
            public List<V1_InnerR_inner_D> Connections { get; internal set; }
            public V1_InnerR_inner_E UnknownE { get; internal set; }
        }

        private V1_InnerR Read_BlueprintResource_v1_innerR(BinaryReader reader)
        {
            var result = new V1_InnerR();

            result.Version = ReadVersion(reader, 2, 0x1411CDF20);
            if(result.Version >= 2)
            {
                result.InstanceDescriptors = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_A, 1, 0x1411DA7B0);
                result.TerminalParametersInfo = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_B, 1, 0x1411DA7C0);
                result.Parameters = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_C, 1, 0x1411A6190); /* todo: incomplete internally */
                result.Connections = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_D, 1, 0x1411DA7D0);
            }
            else
            {
                // todo: incomplete
                result.UnknownE = Read_BlueprintResource_v1_innerR_inner_E(reader);
            }

            return result;
        }

        public class V1_InnerQ
        {
            public uint Version { get; internal set; }
            public int Start { get; internal set; }
            public int End { get; internal set; }
        }
        private V1_InnerQ Read_BlueprintResource_v1_innerQ(BinaryReader reader)
        {
            var result = new V1_InnerQ();

            result.Version = ReadVersion(reader, 1, 0x1411DA900);
            result.Start = reader.ReadInt32();
            result.End = reader.ReadInt32();

            return result;
        }

        public class V1_InnerP
        {
            public uint Version { get; internal set; }
            public string Name { get; internal set; }
            public V1_InnerP_inner Body { get; internal set; }
            public int LinkId { get; internal set; }
        }
        private V1_InnerP Read_BlueprintResource_v1_innerP(BinaryReader reader)
        {
            var result = new V1_InnerP();

            result.Version = ReadVersion(reader, 2, 0x1410BB120);
            result.Name = ReadString(reader);
            result.Body = Read_BlueprintResource_v1_innerP_inner(reader);

            if(result.Version >= 2)
            {
                result.LinkId = reader.ReadInt32();
            }
            
            return result;
        }

        public class V1_InnerO_inner_inner_innerB_inner
        {
            public short UnknownA { get; internal set; }
            public short UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
            public int UnknownE { get; internal set; }
            public int UnknownF { get; internal set; }
            public int UnknownG { get; internal set; }
            public int UnknownH { get; internal set; }
            public int UnknownI { get; internal set; }
            public int UnknownJ { get; internal set; }
            public int UnknownK { get; internal set; }
            public int UnknownL { get; internal set; }
            public int UnknownM { get; internal set; }
            public int UnknownN { get; internal set; }
            public List<float> UnknownO { get; internal set; }
            public List<float> UnknownP { get; internal set; }
            public List<float> UnknownQ { get; internal set; }
        }
        private V1_InnerO_inner_inner_innerB_inner Read_BlueprintResource_v1_innerO_inner_inner_innerB_inner(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerB_inner();

            result.UnknownA = reader.ReadInt16();
            result.UnknownB = reader.ReadInt16();

            result.UnknownC = reader.ReadInt32();
            result.UnknownD = reader.ReadInt32();
            result.UnknownE = reader.ReadInt32();
            result.UnknownF = reader.ReadInt32();
            result.UnknownG = reader.ReadInt32();
            result.UnknownH = reader.ReadInt32();
            result.UnknownI = reader.ReadInt32();
            result.UnknownJ = reader.ReadInt32();
            result.UnknownK = reader.ReadInt32();
            result.UnknownL = reader.ReadInt32();
            result.UnknownM = reader.ReadInt32();
            result.UnknownN = reader.ReadInt32();

            result.UnknownO = ReadVectorF(reader, 4);
            result.UnknownP = ReadVectorF(reader, 4);
            result.UnknownQ = ReadVectorF(reader, 4);
            
            return result;
        }

        public class V1_InnerO_inner_inner_innerB_innerB
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public List<float> UnknownD { get; internal set; }
        }
        private V1_InnerO_inner_inner_innerB_innerB Read_BlueprintResource_v1_innerO_inner_inner_innerB_innerB(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerB_innerB();

            result.Version = ReadVersion(reader, 1, 0x1418EC980);

            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadInt32();
            result.UnknownD = ReadVectorF(reader, 4);

            return result;
        }

        public class V1_InnerO_inner_inner_innerB_innerC
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
        }
        private V1_InnerO_inner_inner_innerB_innerC Read_BlueprintResource_v1_innerO_inner_inner_innerB_innerC(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerB_innerC();

            result.Version = ReadVersion(reader, 1, 0x1418EC990);

            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadInt32();
            result.UnknownD = reader.ReadInt32();

            return result;
        }

        public class V1_InnerO_inner_inner_innerB
        {
            public uint Version { get; internal set; }
            public uint Version2 { get; internal set; }
            public List<float> UnknownA { get; internal set; }
            public List<V1_InnerO_inner_inner_innerB_inner> UnknownB { get; internal set; }
            public uint Version3 { get; internal set; }
            public List<List<float>> SpawnLocations { get; internal set; }
            public List<V1_InnerO_inner_inner_innerB_inner> UnknownC { get; internal set; }
            public uint Version3_2 { get; internal set; }
            public List<List<float>> SpawnLocations_2 { get; internal set; }
            public List<V1_InnerO_inner_inner_innerB_inner> UnknownC_2 { get; internal set; }
            public List<V1_InnerO_inner_inner_innerB_innerB> UnknownD { get; internal set; }
            public List<V1_InnerO_inner_inner_innerB_innerC> UnknownE { get; internal set; }
        }
        private V1_InnerO_inner_inner_innerB Read_BlueprintResource_v1_innerO_inner_inner_innerB(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerB();

            result.Version = ReadVersion(reader, 3, 0x1418EAE00);
            result.Version2 = ReadVersion(reader, 1, 0x1418EBA90);
            result.UnknownA = Read_List(
                reader,
                n => n.ReadSingle(),
                1,
                0x141222920
            );
            result.UnknownB = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerB_inner, 1, 0x1418EC970);
            result.Version3 = ReadVersion(reader, 1, 0x1418EBAA0);
            result.SpawnLocations = Read_List(
                reader,
                n => ReadVectorF(n, 4),
                1,
                0x1416E9700
            );
            result.UnknownC = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerB_inner, 1, 0x1418EC970);

            if(result.Version >= 2)
            {
                result.Version3_2 = ReadVersion(reader, 1, 0x1418EBAA0);
                result.SpawnLocations_2 = Read_List(
                    reader,
                    n => ReadVectorF(n, 4),
                    1,
                    0x1416E9700
                );
                result.UnknownC_2 = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerB_inner, 1, 0x1418EC970);
            }

            if(result.Version >= 3)
            {
                result.UnknownD = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerB_innerB, 1, 0x1418EBAD0);
                result.UnknownE = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerB_innerC, 1, 0x1418EBAE0);
            }

            return result;
        }

        public class V1_InnerO_inner_inner_innerC
        {
            public uint Version { get; internal set; }
            public AABB UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public ButtzComponentCData UnknownC { get; internal set; }
        }
        private V1_InnerO_inner_inner_innerC Read_BlueprintResource_v1_innerO_inner_inner_innerC(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerC();

            result.Version = ReadVersion(reader, 1, 0x1418EBAB0);
            result.UnknownA = Read_AABB(reader);
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = ReadComponent(reader, ButtzComponentC);

            return result;
        }

        public class ButtzComponentCData
        {
            public uint Version { get; internal set; }
            public List<uint> UnknownA { get; internal set; }
            public List<List<float>> UnknownB { get; internal set; }
        }
        private ButtzComponentCData ButtzComponentC(BinaryReader reader)
        {
            var result = new ButtzComponentCData();

            result.Version = ReadVersion(reader, 1, 0x1418ED1D0);
            result.UnknownA = Read_List(reader, n => n.ReadUInt32(), 1, 0x1411A28D0);
            result.UnknownB = Read_List(
                reader,
                n => ReadVectorF(n, 4),
                1,
                0x1416E9700
            );

            return result;
        }

        public class V1_InnerO_inner_inner_innerD
        {
            public uint Version { get; internal set; }
            public AABB UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
        }
        private V1_InnerO_inner_inner_innerD Read_BlueprintResource_v1_innerO_inner_inner_innerD(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerD();

            result.Version = ReadVersion(reader, 1, 0x1418EBAC0);
            result.UnknownA = Read_AABB(reader);
            result.UnknownB = reader.ReadInt32();

            return result;
        }





        public class V1_InnerO_inner_inner_innerB_Component_A
        {
        }
        private V1_InnerO_inner_inner_innerB_Component_A Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_A(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerB_Component_A();

            return result;
        }

        public class V1_InnerO_inner_inner_innerB_Component_BX
        {
            public uint Version { get; internal set; }
            public string UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public string UnknownC { get; internal set; }
        }
        private V1_InnerO_inner_inner_innerB_Component_BX Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_BX(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerB_Component_BX();

            result.Version = ReadVersion(reader, 1, 0x14115CC80);
            result.UnknownA = ReadString(reader);
            result.UnknownB = ReadString(reader);
            result.UnknownC = ReadString(reader);

            return result;
        }

        public class V1_InnerO_inner_inner_innerB_Component_B
        {
            public uint Version { get; internal set; }
            public V1_InnerO_inner_inner_innerB_Component_BX UnknownA { get; internal set; }
            public V1_InnerO_inner_inner_innerB_Component_BX UnknownB { get; internal set; }
            public V1_InnerO_inner_inner_innerB_Component_BX UnknownC { get; internal set; }
            public bool UnknownD { get; internal set; }
            public List<float> UnknownE { get; internal set; }
        }
        private V1_InnerO_inner_inner_innerB_Component_B Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_B(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerB_Component_B();

            result.Version = ReadVersion(reader, 3, 0x1418ED1B0);
            result.UnknownA = Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_BX(reader);
            result.UnknownB = Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_BX(reader);
            result.UnknownC = Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_BX(reader);

            if(result.Version >= 2)
            {
                result.UnknownD = reader.ReadBoolean();
            }
            if(result.Version >= 3)
            {
                result.UnknownE = ReadVectorF(reader, 4);
            }

            return result;
        }

        public class V1_InnerO_inner_inner_innerB_Component
        {
            public uint Version { get; internal set; }
            public string UnknownA { get; internal set; }
            public List<V1_InnerO_inner_inner_innerB_Component_A> UnknownB { get; internal set; }
            public List<V1_InnerO_inner_inner_innerB_Component_A> UnknownC { get; internal set; }
            public V1_InnerO_inner_inner_innerB_Component_BX UnknownD { get; internal set; }
            public V1_InnerO_inner_inner_innerB_Component_BX UnknownE { get; internal set; }
            public List<float> UnknownF { get; internal set; }
            public List<float> UnknownG { get; internal set; }
            public List<float> UnknownH { get; internal set; }
            public V1_InnerO_inner_inner_innerB_Component_BX UnknownI { get; internal set; }
            public bool UnknownJ { get; internal set; }
            public int UnknownK { get; internal set; }
        }
        private V1_InnerO_inner_inner_innerB_Component Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_innerB_Component();

            result.Version = ReadVersion(reader, 6, 0x1418EC440);
            result.UnknownA = ReadString(reader);

            result.UnknownB = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_A, 1, 0x1418ED1A0);
            if(result.Version >= 2)
            {
                result.UnknownC = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_A, 1, 0x1418ED1A0);
            }

            result.UnknownD = Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_BX(reader);
            result.UnknownE = Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_BX(reader);

            result.UnknownF = ReadVectorF(reader, 4);
            if (result.Version < 3)
            {
                result.UnknownG = ReadVectorF(reader, 4);
            }
            if (result.Version >= 2) 
            { 
                result.UnknownH = ReadVectorF(reader, 4);
            }
            if(result.Version >= 4)
            {
                result.UnknownI = Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component_BX(reader);
            }
            if(result.Version >= 5)
            {
                result.UnknownJ = reader.ReadBoolean();
            }
            if(result.Version >= 6)
            {
                result.UnknownK = reader.ReadInt32();
            }

            return result;
        }

        public class V1_InnerO_inner_inner_inner
        {
            public uint Version { get; internal set; }
            public short UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
            public int UnknownE { get; internal set; }
            public int UnknownF { get; internal set; }
            public int UnknownG { get; internal set; }
            public int UnknownH { get; internal set; }
            public int UnknownI { get; internal set; }
            public int UnknownJ { get; internal set; }
            public List<float> UnknownK { get; internal set; }
            public List<float> UnknownL { get; internal set; }
        }
        private V1_InnerO_inner_inner_inner Read_BlueprintResource_v1_innerO_inner_inner_inner(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner_inner();

            result.Version = ReadVersion(reader, 1, 0x1418ED1C0);

            result.UnknownA = reader.ReadInt16();
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadInt32();
            result.UnknownD = reader.ReadInt32();
            result.UnknownE = reader.ReadInt32();
            result.UnknownF = reader.ReadInt32();
            result.UnknownG = reader.ReadInt32();
            result.UnknownH = reader.ReadInt32();
            result.UnknownI = reader.ReadInt32();
            result.UnknownJ = reader.ReadInt32();

            result.UnknownK = ReadVectorF(reader, 4); // 16byte var
            result.UnknownL = ReadVectorF(reader, 4); // 16byte var

            return result;
        }

        public class V1_InnerO_inner_inner
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public List<float> UnknownB { get; internal set; }
            public uint VerisonB { get; internal set; }
            public uint VersionC { get; internal set; }
            public List<float> UnknownC { get; internal set; }
            public List<V1_InnerO_inner_inner_inner> UnknownD { get; internal set; }
            public AABB UnknownE { get; internal set; }
            public V1_InnerO_inner_inner_innerB UnknownF { get; internal set; }
            public V1_InnerO_inner_inner_innerB_Component TerrainMaterial { get; internal set; }
            public List<float> UnknownG { get; internal set; }
            public int UnknownH { get; internal set; }
            public int UnknownI { get; internal set; }
            public string HavokCompressedMeshShape { get; internal set; }
            public List<V1_InnerO_inner_inner_innerC> UnknownJ { get; internal set; }
            public List<V1_InnerO_inner_inner_innerD> UnknownK { get; internal set; }
        }
        private V1_InnerO_inner_inner Read_BlueprintResource_v1_innerO_inner_inner(BinaryReader reader)
        {
            var result = new V1_InnerO_inner_inner();

            result.Version = ReadVersion(reader, 7, 0x1418E8130);
            if(result.Version >= 3)
            {
                result.UnknownA = reader.ReadInt32();
            }
            result.UnknownB = ReadVectorF(reader, 4); // unused? 16byte var
            result.VerisonB = ReadVersion(reader, 2, 0x1418EADF0);
            result.VersionC = ReadVersion(reader, 1, 0x1418EBA80);
            result.UnknownC = Read_List(
                reader,
                n => n.ReadSingle(),
                1,
                0x141222920
            );

            result.UnknownD = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_inner, 1, 0x1418EC960);

            if(result.VerisonB >= 2)
            {
                result.UnknownE = Read_AABB(reader);
            }

            result.UnknownF = Read_BlueprintResource_v1_innerO_inner_inner_innerB(reader);

            if(result.Version >= 2)
            {
                result.TerrainMaterial = ReadComponent(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerB_Component);
            }

            if(result.Version >= 4)
            {
                result.UnknownG = ReadVectorF(reader, 4);
            }

            if(result.Version >= 5)
            {
                result.UnknownH = reader.ReadInt32();

                // TODO: Revisit the conditional logic with UnknownH here...

                if(result.Version < 6 && result.UnknownH == 0)
                {
                    result.UnknownI = reader.ReadInt32(); // skip 4 bytes
                }
                else if(result.UnknownH != 0)
                {
                    result.HavokCompressedMeshShape = ReadString(reader); // Guessing, maybe read_array?
                }

                if(result.Version >= 7)
                {
                    result.UnknownJ = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerC, 1, 0x1418EAE30);
                    result.UnknownK = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner_innerD, 1, 0x1418EAE40);
                }
            }

            return result;
        }

        public class V1_InnerO_inner
        {
            public uint Version { get; internal set; }
            public List<V1_InnerO_inner_inner> UnknownA { get; internal set; }
            public List<V1_InnerO_inner_inner> UnknownB { get; internal set; }
        }
        private V1_InnerO_inner Read_BlueprintResource_v1_innerO_inner(BinaryReader reader)
        {
            var result = new V1_InnerO_inner();

            result.Version = ReadVersion(reader, 2, 0x1418E2BF0);
            if(result.Version < 2)
            {
                result.UnknownA = Read_List(reader, Read_BlueprintResource_v1_innerO_inner_inner, 2, 0x1418E2C00);
            }
            else
            {
                result.UnknownB = Read_List(reader, n => ReadComponent(n, Read_BlueprintResource_v1_innerO_inner_inner), 1, 0x1418E7DE0);
            }

            return result;
        }

        public class V1_InnerO
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public V1_InnerO_inner UnknownC { get; internal set; }
            public string TerainSourceDataRef { get; internal set; }
            public int UnknownD { get; internal set; }
            public List<float> UnknownE { get; internal set; }
            public int UnknownF { get; internal set; }
            public int UnknownG { get; internal set; }
            public int UnknownH { get; internal set; }
            public string UnknownI { get; internal set; }
        }
        private V1_InnerO Read_BlueprintResource_v1_innerO(BinaryReader reader)
        {
            var result = new V1_InnerO();

            result.Version = ReadVersion(reader, 5, 0x1410BB0B0);
            if(result.Version >= 3)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.UnknownB = ReadString(reader);

            if(result.Version < 5)
            {
                result.UnknownC = Read_BlueprintResource_v1_innerO_inner(reader);
            }
            else
            {
                result.TerainSourceDataRef = ReadUUID(reader);
            }

            result.UnknownD = reader.ReadInt32();
            result.UnknownE = ReadVectorF(reader, 4); // 16bytes

            if(result.Version >= 2)
            {
                result.UnknownF = reader.ReadInt32();
            }

            result.UnknownG = reader.ReadInt32();
            result.UnknownH = reader.ReadInt32();

            if(result.Version >= 4)
            {
                result.UnknownI = ReadUUID(reader);
            }

            return result;
        }

        public class UnknownCase15Data
        {
            public uint Version { get; internal set; }
            public uint InstanceId { get; internal set; }
            public uint ElementType { get; internal set; }
            public uint LinkId { get; internal set; }
        }
        private UnknownCase15Data Read_UnknownCase15(BinaryReader reader)
        {
            var result = new UnknownCase15Data();

            result.Version = ReadVersion(reader, 1, 0x1411A6EC0);
            result.InstanceId = reader.ReadUInt32();
            result.ElementType = reader.ReadUInt32();
            result.LinkId = reader.ReadUInt32();

            return result;
        }

        public class UnknownCase5Data
        {
            public int Subtype { get; internal set; }
            public List<V1_InnerN_inner_inner> Array { get; internal set; }
            public List<uint> ArrayTrackingTags { get; internal set; }
        }
        private UnknownCase5Data Read_UnknownCase5(BinaryReader reader, ulong version)
        {
            var result = new UnknownCase5Data();

            result.Subtype = reader.ReadInt32();
            result.Array = Read_List(reader, Read_BlueprintResource_v1_innerN_inner_inner, 1, 0x1411B25B0);
            if (version >= 6)
            {
                result.ArrayTrackingTags = Read_List(reader, n => n.ReadUInt32(), 1, 0x1411A28D0);
            }

            return result;
        }

        public class UnknownCase7Data
        {
            public int TrackingTag { get; internal set; }
            public string Id { get; internal set; }
        }
        private UnknownCase7Data Read_UnknownCase7(BinaryReader reader, ulong version)
        {
            var result = new UnknownCase7Data();

            result.Id = ReadUUID(reader);
            if (version == 3)
            {
                result.TrackingTag = reader.ReadInt32();
            }

            return result;
        }

        public class UnknownCase9Data_inner
        {
            public string Data { get; internal set; }
            public V1_InnerN_inner_inner Value { get; internal set; }
        }
        private UnknownCase9Data_inner Read_UnknownCase9_inner(BinaryReader reader)
        {
            var result = new UnknownCase9Data_inner();

            result.Data = ReadString(reader);
            result.Value = Read_BlueprintResource_v1_innerN_inner_inner(reader);

            return result;

        }
        public class UnknownCase9Data
        {
            public int Subtype { get; internal set; }
            public List<UnknownCase9Data_inner> StringMap { get; internal set; }
        }
        private UnknownCase9Data Read_UnknownCase9(BinaryReader reader)
        {
            var result = new UnknownCase9Data();

            result.Subtype = reader.ReadInt32();
            result.StringMap = Read_List(reader, Read_UnknownCase9_inner, 1, 0x1411B25C0);

            return result;
        }

        public class UnknownCase13Data
        {
            public int Subtype { get; internal set; }
            public List<V1_InnerN_inner_inner> Array { get; internal set; }
        }
        private UnknownCase13Data Read_UnknownCase13(BinaryReader reader)
        {
            var result = new UnknownCase13Data();

            result.Subtype = reader.ReadInt32();
            result.Array = Read_List(reader, Read_BlueprintResource_v1_innerN_inner_inner, 1, 0x1411B25B0);

            return result;

        }
        public object ReadSomethingCrazy(BinaryReader reader, int val, ulong version)
        {
            switch (val)
            {
                case 0:
                    return null;
                case 1: // int64
                    return reader.ReadInt64();
                case 2: // double
                    return reader.ReadDouble();
                case 3: // vector4
                    return ReadVectorF(reader, 4);
                case 4: // mTransform
                    return ClusterReader.Read_ObjectClusterTransform(reader);
                case 5: // subtype
                    return Read_UnknownCase5(reader, version);
                case 6: // string
                    return ReadString(reader);
                case 7: // resource id
                    return Read_UnknownCase7(reader, version);
                case 8: // ElementHandle
                    return Read_BlueprintResource_v1_innerL_v4_innerC(reader);
                case 9: // subtype
                    return Read_UnknownCase9(reader);
                case 10:
                case 11:
                case 12:
                case 13:
                    return Read_UnknownCase13(reader);
                case 14: // TrackedElementId
                    return Read_BlueprintResource_v1_innerL_v4_innerB(reader);
                case 15: // TrackedMemberId
                    return Read_UnknownCase15(reader);
                default:
                    return null;
            }

            /*
            0x1418DF53E  0x1418D9AA4
            0x1418DF48B  0x1418D9ABC
            0x1418DF48B  0x1418D9AD1
            0x1418DF4CC  0x1418D9AE6
            0x1418DF56C  0x1418D9B44
            0x1418DF377  0x1418D995D
            0x1418DF554  0x1418D9B29
            0x1418DF57A  0x1418D9B64
            0x1418DF4FC  0x1418D9B01
            0x1418DF61B  0x1418D9F0A
            0x1418DF441  0x1418D9A62
            0x1418DF441  0x1418D9A62
            0x1418DF441  0x1418D9A62
            0x1418DF441  0x1418D9A62
            0x1418DF665  0x1418D9F48
            0x1418DF676  0x1418D9F60
            */
        }

        private int ConvertTypeValue(uint typeValue)
        {
            typeValue = typeValue & 0xFF000000;

            switch (typeValue)
            {
                case 0x8000000:
                    return 13;
                case 0x4000000:
                    return 11;
                case 0x5000000:
                    return 3;
                case 0x6000000:
                    return 4;
                case 0x2000000:
                    return 6;
                case 0x80000000:
                    return 5;
                case 0x81000000:
                    return 9;
                case 0x1000000:
                    return 7;
                case 0xC000000:
                    return 15;
                case 0x9000000:
                    return 12;
                case 0xA000000:
                    return 10;
                case 0xB000000:
                    return 14;
                case 0x40000000:
                    return 1;
                case 0x41000000:
                    return 2;
                default:
                    return 0;
            }
        }

        public class V1_InnerN_inner_inner
        {
            public uint Version { get; internal set; }
            public uint Type { get; internal set; }
            public object Data { get; internal set; }
        }
        public V1_InnerN_inner_inner Read_BlueprintResource_v1_innerN_inner_inner(BinaryReader reader)
        {
            var result = new V1_InnerN_inner_inner();

            result.Version = ReadVersion(reader, 6, 0x1411A3E00);
            result.Type = reader.ReadUInt32();

            var realType = ConvertTypeValue(result.Type);
            result.Data = ReadSomethingCrazy(reader, realType, result.Version);

            return result;
        }

        public class V1_InnerN_inner
        {
            public uint Version { get; internal set; }
            public V1_InnerN_inner_inner Value { get; internal set; }
            public V1_InnerN_inner_inner Attributes { get; internal set; }
            public string MetadataName { get; internal set; }
            public string DisplayName { get; internal set; }
        }
        private V1_InnerN_inner Read_BlueprintResource_v1_innerN_inner(BinaryReader reader)
        {
            var result = new V1_InnerN_inner();

            result.Version = ReadVersion(reader, 2, 0x1411CCEA0);
            result.Value = Read_BlueprintResource_v1_innerN_inner_inner(reader);
            result.Attributes = Read_BlueprintResource_v1_innerN_inner_inner(reader);
            result.MetadataName = ReadString(reader);
            result.DisplayName = ReadString(reader);

            return result;
        }


        public class V1_InnerN_innerB
        {
            public AInner UnknownX { get; internal set; }
            public string Name { get; internal set; }
            public bool HasScriptMetadataResource { get; internal set; }
            public string ScriptMetadataResource { get; internal set; }
            public int ParamCount { get; internal set; }
            public List<ClusterDefinitionResource.ScriptParameter> Parameters { get; internal set; }
            public int ScriptNodeIndex { get; internal set; }
            public List<int> ParameterNodeIndices { get; internal set; }
            public string MetadataTrackingId { get; internal set; }
            public List<string> ParameterTrackingIds { get; internal set; }
        }
        private V1_InnerN_innerB Read_BlueprintResource_v1_innerN_innerB(BinaryReader reader, uint parent_version)
        {
            var result = new V1_InnerN_innerB();
            result.HasScriptMetadataResource = true;

            if(parent_version >= 5)
            {
                result.UnknownX = Read_BlueprintResource_A_inner(reader);
            }

            result.Name = ReadString(reader);
            if(parent_version >= 2)
            {
                result.HasScriptMetadataResource = reader.ReadBoolean();
            }

            if(result.HasScriptMetadataResource)
            {
                result.ScriptMetadataResource = ReadUUID(reader);
            }

            if (parent_version != 3)
            {
                result.ParamCount = reader.ReadInt32();
                result.Parameters = new List<ClusterDefinitionResource.ScriptParameter>();
                for (int i = 0; i < result.ParamCount; i++)
                {
                    var parameter = ClusterReader.Read_ScriptComponent_parameter(reader);
                    result.Parameters.Add(parameter);
                }
            }

            if(parent_version >= 3)
            {
                result.ScriptNodeIndex = reader.ReadInt32();
                result.ParameterNodeIndices = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            }

            if (parent_version >= 6 && parent_version <= 7)
            {
                result.MetadataTrackingId = ReadUUID_B(reader);
            }

            if(parent_version == 7)
            {
                result.ParameterTrackingIds = Read_List(reader, ReadUUID_B, 1, 0x1411C1050);
            }

            return result;
        }

        public class V1_InnerN
        {
            public uint Version { get; internal set; }
            public AInner UnknownX { get; internal set; }
            public string Name { get; internal set; }
            public string ScriptMetadataResource { get; internal set; }
            public List<V1_InnerN_inner> ParameterData { get; internal set; }
            public int ScriptType { get; internal set; }
            public V1_InnerN_innerB UnknownF { get; internal set; }
        }
        public V1_InnerN Read_BlueprintResource_v1_innerN(BinaryReader reader)
        {
            var result = new V1_InnerN();

            result.Version = ReadVersion(reader, 12, 0x1410BB0A0);
            if(result.Version >= 9)
            {
                result.UnknownX = Read_BlueprintResource_A_inner(reader);
                result.Name = ReadString(reader);
                result.ScriptMetadataResource = ReadUUID(reader);
                result.ParameterData = Read_List(reader, Read_BlueprintResource_v1_innerN_inner, 1, 0x1411BF180);

                if(result.Version >= 10)
                {
                    result.ScriptType = reader.ReadInt32();
                }
            }
            else
            {
                result.UnknownF = Read_BlueprintResource_v1_innerN_innerB(reader, result.Version);
            }

            return result;
        }

        public class V1_InnerM
        {
            public uint Version { get; internal set; }
            public bool HasBankResource { get; internal set; }
            public bool HasSoundResource { get; internal set; }
            public bool HasHandleCollection { get; internal set; }
            public string BankResource { get; internal set; }
            public string Event { get; internal set; }
            public string SoundResource { get; internal set; }
            public float SoundLoudness { get; internal set; }
            public string StreamTag { get; internal set; }
            public float StreamLoudness { get; internal set; }
            public bool UseWebStream { get; internal set; }
            public string WebStreamUrl { get; internal set; }
            public int StreamChannel { get; internal set; }
            public uint ElementType { get; internal set; }
            public long ElementIndex { get; internal set; }
            public int UnknownO { get; internal set; }
            public AInner UnknownX { get; internal set; }
            public int ShapeType { get; internal set; }
            public List<float> BoxShape { get; internal set; }
            public float SphereShapeRadius { get; internal set; }
        }
        private V1_InnerM Read_BlueprintResource_v1_innerM(BinaryReader reader)
        {
            var result = new V1_InnerM();

            result.Version = ReadVersion(reader, 7, 0x1410BB090);
            if(result.Version >= 4)
            {
                result.UnknownX = Read_BlueprintResource_A_inner(reader);
            }

            result.HasBankResource = result.Version < 7;
            result.HasSoundResource = true;
            result.HasHandleCollection = true;

            if (result.Version >= 2)
            {
                if(result.Version < 7)
                {
                    result.HasBankResource = reader.ReadBoolean();
                }

                result.HasSoundResource = reader.ReadBoolean();

                if(result.Version < 3)
                {
                    result.HasHandleCollection = reader.ReadBoolean();
                }
            }

            if(result.HasBankResource)
            {
                result.BankResource = ReadUUID(reader);
            }

            if(result.Version < 7)
            {
                result.Event = ReadUUID_B(reader);
            }

            if(result.HasSoundResource)
            {
                result.SoundResource = ReadUUID(reader);
            }
            else
            {
                // noop
            }

            result.SoundLoudness = reader.ReadSingle();

            if(result.Version < 7)
            {
                result.StreamTag = ReadString(reader);
                result.StreamLoudness = reader.ReadSingle();
            }

            if(result.Version >= 5)
            {
                result.UseWebStream = reader.ReadBoolean();
                if(result.Version < 6)
                {
                    result.WebStreamUrl = ReadString(reader);
                }
                else
                {
                    result.StreamChannel = reader.ReadInt32();
                }
            }

            if (!result.HasHandleCollection)
            {
                if (result.Version < 3)
                {
                    return result;
                }
            }
            else 
            {
                if (result.Version < 3)
                {
                    result.ElementType = reader.ReadUInt32();
                    result.ElementIndex = reader.ReadInt64();
                    return result;
                }
            }

            result.ShapeType = reader.ReadInt32();
            result.BoxShape = ReadVectorF(reader, 4);
            result.SphereShapeRadius = reader.ReadSingle();

            return result;
        }

        public class V1_InnerL_v4_innerC
        {
            public uint Version { get; internal set; }
            public long Value { get; internal set; }
        }
        public V1_InnerL_v4_innerC Read_BlueprintResource_v1_innerL_v4_innerC(BinaryReader reader)
        {
            var result = new V1_InnerL_v4_innerC();

            result.Version = ReadVersion(reader, 2, 0x1411B5140);
            result.Value = reader.ReadInt64();

            if(result.Value != -1 && result.Version < 2)
            {
                // NOOP
            }

            return result;
        }

        public class V1_InnerL_v4_innerB
        {
            public uint Version { get; internal set; }
            public int ElementType { get; internal set; }
            public uint LinkId { get; internal set; }
        }
        public V1_InnerL_v4_innerB Read_BlueprintResource_v1_innerL_v4_innerB(BinaryReader reader)
        {
            var result = new V1_InnerL_v4_innerB();

            result.Version = ReadVersion(reader, 1, 0x1411B5150);
            result.ElementType = reader.ReadInt32();
            result.LinkId = reader.ReadUInt32();

            return result;
        }

        public class V1_InnerL_v4_inner
        {
            public uint Version { get; internal set; }
            public float Weight { get; internal set; }
            public int Joint { get; internal set; }
        }
        private V1_InnerL_v4_inner Read_BlueprintResource_v1_innerL_v4_inner(BinaryReader reader)
        {
            var result = new V1_InnerL_v4_inner();

            result.Version = ReadVersion(reader, 1, 0x14121B550);
            result.Weight = reader.ReadSingle();
            result.Joint = reader.ReadInt32();

            return result;
        }

        public class V1_InnerL_v4
        {
            public uint Version { get; internal set; }
            public float Uv0 { get; internal set; }
            public float Uv1 { get; internal set; }
            public int TriangleIndex { get; internal set; }
            public List<V1_InnerL_v4_inner> Weights { get; internal set; }
            public ClusterDefinitionResource.OffsetTransform AttachmentTransform { get; internal set; }
            public V1_InnerL_v4_innerB EntityElementId { get; internal set; }
            public V1_InnerL_v4_innerC TransformHandle { get; internal set; }
        }
        private V1_InnerL_v4 Read_BlueprintResource_v1_innerL_v4(BinaryReader reader)
        {
            var result = new V1_InnerL_v4();

            result.Version = ReadVersion(reader, 2, 0x141202200);
            result.Uv0 = reader.ReadSingle();
            result.Uv1 = reader.ReadSingle();
            result.TriangleIndex = reader.ReadInt32();

            result.Weights = Read_List(reader, Read_BlueprintResource_v1_innerL_v4_inner, 1, 0x14120B090);
            result.AttachmentTransform = ClusterReader.Read_AnimationComponent_OffsetTransform(reader);

            if(result.Version >= 2)
            {
                result.EntityElementId = Read_BlueprintResource_v1_innerL_v4_innerB(reader);
            }
            else
            {
                result.TransformHandle = Read_BlueprintResource_v1_innerL_v4_innerC(reader);
            }

            return result;
        }

        public class V1_InnerL
        {
            public uint Version { get; internal set; }
            public bool ShadowCaster { get; internal set; }
            public string Name { get; internal set; }
            public List<Transform> MeshBindingsOld { get; internal set; }
            public List<ClusterDefinitionResource.OffsetTransform> PoseOld { get; internal set; }
            public string ModelDefinition { get; internal set; }
            public float Scale { get; internal set; }
            public V1_InnerK_Inner UnknownG { get; internal set; }
            public List<V1_InnerL_v4> AttachmentPoints { get; internal set; }
            public List<Transform> MeshBindings { get; internal set; }
            public List<ClusterDefinitionResource.OffsetTransform> Pose { get; internal set; }
            public string MorphSkeleton { get; internal set; }
        }
        private V1_InnerL Read_BlueprintResource_v1_innerL(BinaryReader reader)
        {
            var result = new V1_InnerL();

            result.Version = ReadVersion(reader, 6, 0x1410BB080);

            if(result.Version == 1)
            {
                result.ModelDefinition = ReadUUID(reader);
                result.ShadowCaster = reader.ReadBoolean();
                result.Name = ReadString(reader);
                result.MeshBindingsOld = Read_List(reader, Read_Transform, 1, 0x1411F3EA0);
                result.PoseOld = Read_List(reader, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0);
            }
            else if(result.Version == 2)
            {
                result.ModelDefinition = ReadUUID(reader);
                result.ShadowCaster = reader.ReadBoolean();
                result.Name = ReadString(reader);
                result.MeshBindingsOld = Read_List(reader, Read_Transform, 1, 0x1411F3EA0);
                result.PoseOld = Read_List(reader, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0);
                result.Scale = reader.ReadSingle();
            }
            else if(result.Version == 3)
            {
                result.UnknownG = Read_BlueprintResource_v1_innerK_Inner(reader);
                result.MeshBindingsOld = Read_List(reader, Read_Transform, 1, 0x1411F3EA0);
                result.PoseOld = Read_List(reader, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0);
            }
            else if(result.Version == 4)
            {
                result.UnknownG = Read_BlueprintResource_v1_innerK_Inner(reader);
                result.MeshBindingsOld = Read_List(reader, Read_Transform, 1, 0x1411F3EA0);
                result.PoseOld = Read_List(reader, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0);
                result.AttachmentPoints = Read_List(reader, Read_BlueprintResource_v1_innerL_v4, 1, 0x1411F2790);
            }
            else if (result.Version >= 5)
            {
                result.UnknownG = Read_BlueprintResource_v1_innerK_Inner(reader); // second time around, something bad happens here...
                result.MeshBindings = ReadComponent(reader, n => Read_List(n, Read_Transform, 1, 0x1411F3EA0));
                result.Pose = ReadComponent(reader, n => Read_List(n, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0));
                result.AttachmentPoints = Read_List(reader, Read_BlueprintResource_v1_innerL_v4, 1, 0x1411F2790);
            }

            if (result.Version >= 6)
            {
                result.MorphSkeleton = ReadUUID(reader);
            }

            return result;
        }
        private bool x = false;
        
        public class V1_InnerK_Inner_Inner_A_innerE
        {
            public string Data { get; internal set; }
            public uint Version { get; internal set; }
            public bool Value { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_innerE Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerE(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_innerE();

            result.Data = ReadString(reader);
            result.Version = ReadVersion(reader, 1, 0x141220A80);
            result.Value = reader.ReadBoolean();

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_innerD_X
        {
            public uint Version { get; internal set; }
            public List<float> Value { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_innerD_X Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerD_X(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_innerD_X();

            result.Version = ReadVersion(reader, 1, 0x14121E220);
            result.Value = ReadVectorF(reader, 4);

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_innerD
        {
            public string Data { get; internal set; }
            public uint Version { get; internal set; }
            public V1_InnerK_Inner_Inner_A_innerD_X Value { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_innerD Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerD(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_innerD();

            result.Data = ReadString(reader); // might be readarray?
            result.Version = ReadVersion(reader, 1, 0x14121E230);
            result.Value = Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerD_X(reader);

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_innerC
        {
            public string Data { get; internal set; }
            public uint Version { get; internal set; }
            public V1_InnerK_Inner_Inner_A_innerD_X Value { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_innerC Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerC(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_innerC();

            result.Data = ReadString(reader); // might be readarray?
            result.Value = Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerD_X(reader);

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_innerB
        {
            public uint Version { get; internal set; }
            public int Value { get; internal set; }
            public string Data { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_innerB Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerB(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_innerB();

            result.Data = ReadString(reader);
            result.Version = ReadVersion(reader, 1, 0x14121E210);
            result.Value = reader.ReadInt32();

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_inner_X
        {
            public uint Version { get; internal set; }
            public string Texture { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_inner_X Read_BlueprintResource_v1_innerK_Inner_Inner_A_inner_X(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_inner_X();

            result.Version = ReadVersion(reader, 1, 0x141223110);
            result.Texture = ReadUUID(reader);

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_inner
        {
            public string Unknown0 { get; internal set; }
            public uint Version { get; internal set; }
            public V1_InnerK_Inner_Inner_A_inner_X TextureField { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_inner Read_BlueprintResource_v1_innerK_Inner_Inner_A_inner(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_inner();

            result.Unknown0 = ReadString(reader);

            result.Version = ReadVersion(reader, 1, 0x14121E200);
            result.TextureField = Read_BlueprintResource_v1_innerK_Inner_Inner_A_inner_X(reader);

            return result;
        }

        public class V1_InnerK_Inner_Inner_A
        {
            public uint Version { get; internal set; }
            public string Name { get; internal set; }
            public string MaterialType { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_inner> Texture2DFields { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_inner> TextureCubeFields { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_innerB> RangedFloatFields { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_innerC> RangedVectorFields { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_innerD> ColorFields { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_innerE> BoolFields { get; internal set; }
            public string MaterialResource { get; internal set; }
            public int Flags { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A Read_BlueprintResource_v1_innerK_Inner_Inner_A(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A();

            result.Version = ReadVersion(reader, 5, 0x141206320);
            result.Name = ReadString(reader);
            result.MaterialType = ReadString(reader);
            result.Texture2DFields = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_inner, 1, 0x141211AE0);

            if (result.Version < 4)
            {
                result.TextureCubeFields = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_inner, 1, 0x1418EB7C0);
            }

            result.RangedFloatFields = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerB, 1, 0x141211B00);
            result.RangedVectorFields = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerC, 1, 0x141211B10);
            result.ColorFields = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerD, 1, 0x141211B20);

            if(result.Version >= 5)
            {
                result.BoolFields = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerE, 1, 0x141217100);
            }

            result.MaterialResource = ReadUUID(reader);
            result.Flags = reader.ReadInt32();

            if(result.Version < 2)
            {
                // NOOP
            }

            if(result.Version < 3)
            {
                // VideoScreen...?
                // NOOP
            }

            return result;
        }



        public class V1_InnerK_Inner_Inner_B_InnerA
        {
            public uint Version { get; internal set; }
            public int IndexStart { get; internal set; }
            public int IndexCount { get; internal set; }
            public float ErrorLower { get; internal set; }
            public float ErrorUpper { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_B_InnerA Read_BlueprintResource_v1_innerK_Inner_Inner_B_InnerA(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_B_InnerA();

            result.Version = ReadVersion(reader, 1, 0x141222930);
            result.IndexStart = reader.ReadInt32();
            result.IndexCount = reader.ReadInt32();
            result.ErrorLower = reader.ReadSingle();
            result.ErrorUpper = reader.ReadSingle();

            return result;
        }

        public class V1_InnerK_Inner_Inner_B
        {
            public uint Version { get; internal set; }
            public List<V1_InnerK_Inner_Inner_B_InnerA> Lods { get; internal set; }
            public uint IndexStart { get; internal set; }
            public uint IndexCount { get; internal set; }
            public uint Flags { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_B Read_BlueprintResource_v1_innerK_Inner_Inner_B(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_B();

            result.Version = ReadVersion(reader, 3, 0x14120E1A0);
            if(result.Version >= 3)
            {
                result.Lods = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_B_InnerA, 1, 0x14121CBB0);
            }
            else
            {
                result.IndexStart = reader.ReadUInt32();
                result.IndexCount = reader.ReadUInt32();
            }

            if(result.Version >= 2)
            {
                result.Flags = reader.ReadUInt32();
            }

            return result;
        }

        public class V1_InnerK_Inner_Inner_C
        {
            public uint Version { get; internal set; }
            public V1_InnerK_Inner_Inner_A Material { get; internal set; }
            public uint IndexCount { get; internal set; }
            public uint IndexStart { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_C Read_BlueprintResource_v1_innerK_Inner_Inner_C(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_C();

            result.Version = ReadVersion(reader, 1, 0x1418E77B0);
            result.Material = Read_BlueprintResource_v1_innerK_Inner_Inner_A(reader);
            result.IndexCount = reader.ReadUInt32();
            result.IndexStart = reader.ReadUInt32();

            return result;
        }

        public class V1_InnerK_Inner_Inner
        {
            public uint Version { get; internal set; }
            public string PickResource { get; internal set; }
            public string Geometry { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A> Materials { get; internal set; }
            public List<V1_InnerK_Inner_Inner_B> Sections { get; internal set; }
            public List<V1_InnerK_Inner_Inner_C> Parts { get; internal set; }
            public string ModelResource { get; internal set; }
            public string Morphs { get; internal set; }
            public string BlendShape { get; internal set; }
        }
        private V1_InnerK_Inner_Inner Read_BlueprintResource_v1_innerK_Inner_Inner(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner();

            result.Version = ReadVersion(reader, 5, 0x1411FB8B0);
            result.PickResource = ReadUUID(reader);
            result.Geometry = ReadUUID(reader);

            if(result.Version >= 3)
            {
                result.Materials = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A, 1, 0x141205450);
                result.Sections = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_B, 1, 0x141205460);
            }
            else
            {
                result.Parts = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_C, 1, 0x1418E9D50);
            }

            if(result.Version < 4)
            {
                result.ModelResource = ReadUUID(reader);
            }

            if(result.Version >= 2)
            {
                if (result.Version < 5)
                {
                    result.Morphs = ReadUUID(reader);
                }
                else
                {
                    result.BlendShape = ReadUUID(reader);
                }
            }

            return result;
        }

        public class V1_InnerK_Inner
        {
            public uint Version { get; internal set; }
            public string ModelDefinitionId { get; internal set; }
            public V1_InnerK_Inner_Inner Model { get; internal set; }
            public bool ShadowCaster { get; internal set; }
            public string Name { get; internal set; }
            public float Scale { get; internal set; }
            public bool IsAttachment { get; internal set; }
            public List<string> Tags { get; internal set; }
            public int LayerId { get; internal set; }
            public float MaxRenderDistance { get; internal set; }
            public bool IsScriptable { get; internal set; }
            public bool IsInitiallyVisible { get; internal set; }
            public AInner Schematic { get; internal set; }
        }
        private V1_InnerK_Inner Read_BlueprintResource_v1_innerK_Inner(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner();

            result.Version = ReadVersion(reader, 8, 0x1411DD9A0);

            if(result.Version >= 4)
            {
                result.Schematic = Read_BlueprintResource_A_inner(reader);
            }

            if (result.Version == 1)
            {
                result.ModelDefinitionId = ReadUUID(reader);
            }
            else
            {
                result.Model = Read_BlueprintResource_v1_innerK_Inner_Inner(reader);
            }

            if(result.Version < 5)
            {
                result.ShadowCaster = reader.ReadBoolean();
            }

            result.Name = ReadString(reader);
            result.Scale = reader.ReadSingle();

            if(result.Version >= 3 && result.Version < 5)
            {
                result.IsAttachment = reader.ReadBoolean();
            }
            else if(result.Version >= 5)
            {
                result.Tags = Read_List(reader, ReadString, 1, 0x14119ADB0);
                result.LayerId = reader.ReadInt32();
            }

            if (result.Version >= 6)
            {
               result.MaxRenderDistance = reader.ReadSingle();
            }
            if (result.Version >= 7)
            {
               result.IsScriptable = reader.ReadBoolean();
            }
            if (result.Version >= 8)
            {
               result.IsInitiallyVisible = reader.ReadBoolean();
            }

            return result;
        }


        public class V1_InnerK
        {
            public uint Version { get; internal set; }
            public string ModelDefinition { get; internal set; }
            public byte ShadowCaster { get; internal set; }
            public string Name { get; internal set; }
            public float Scale { get; internal set; }
            public V1_InnerK_Inner MeshSchematic { get; internal set; }
        }
        private V1_InnerK Read_BlueprintResource_v1_innerK(BinaryReader reader)
        {
            var result = new V1_InnerK();

            result.Version = ReadVersion(reader, 2, 0x1410BB070);
            if(result.Version == 1)
            {
                result.ModelDefinition = ReadUUID(reader);
                result.ShadowCaster = reader.ReadByte(); // unused?
                result.Name = ReadString(reader);
                result.Scale = reader.ReadSingle();
            }
            else if(result.Version >= 2)
            {
                result.MeshSchematic = Read_BlueprintResource_v1_innerK_Inner(reader);
            }

            return result;
        }

        public class V1_InnerJ
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public int LightType { get; internal set; }
            public string Name { get; internal set; }
            public List<float> Color { get; internal set; }
            public float Intensity { get; internal set; }
            public float Range { get; internal set; }
            public bool IsShadowCaster { get; internal set; }
            public float SpotAngle { get; internal set; }
            public float SpotAngularFalloff { get; internal set; }
            public float SpotNearClip { get; internal set; }
            public bool Scriptable { get; internal set; }
            public bool EnableDynamicShadows { get; internal set; }
        }
        private V1_InnerJ Read_BlueprintResource_v1_innerJ(BinaryReader reader)
        {
            var result = new V1_InnerJ();

            result.Version = ReadVersion(reader, 5, 0x1410BB060);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.LightType = reader.ReadInt32();
            result.Name = ReadString(reader);
            result.Color = ReadVectorF(reader, 4);

            if(result.Version < 3)
            {
                // noop
            }

            result.Intensity = reader.ReadSingle();
            result.Range = reader.ReadSingle();
            result.IsShadowCaster = reader.ReadBoolean();

            result.SpotAngle = reader.ReadSingle();
            result.SpotAngularFalloff = reader.ReadSingle();
            result.SpotNearClip = reader.ReadSingle();

            if (result.Version >= 4)
            {
                result.Scriptable = reader.ReadBoolean();
            }
            if(result.Version >= 5)
            {
                result.EnableDynamicShadows = reader.ReadBoolean();
            }

            return result;
        }

        public class V1_InnerI
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public float DiagonalFovRadians { get; internal set; }
            public float NearPlane { get; internal set; }
            public float FarPlane { get; internal set; }
            public bool Perspective { get; internal set; }
        }
        private V1_InnerI Read_BlueprintResource_v1_innerI(BinaryReader reader)
        {
            var result = new V1_InnerI();

            result.Version = ReadVersion(reader, 2, 0x1410BB050);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.DiagonalFovRadians = reader.ReadSingle();
            result.NearPlane = reader.ReadSingle();
            result.FarPlane = reader.ReadSingle();
            result.Perspective = reader.ReadBoolean();

            return result;
        }

        public class V1_InnerH
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public bool IsInitialSpawnPoint { get; internal set; }
        }
        private V1_InnerH Read_BlueprintResource_v1_innerH(BinaryReader reader)
        {
            var result = new V1_InnerH();

            result.Version = ReadVersion(reader, 4, 0x1410BB040);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            if(result.Version >= 3)
            {
                result.IsInitialSpawnPoint = reader.ReadBoolean();
            }

            return result;
        }

        public class V1_InnerG
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public string SpeechAlgorithmsId { get; internal set; }
            public string SpeechCharacterId { get; internal set; }
            public bool HandsRigged { get; internal set; }
        }
        private V1_InnerG Read_BlueprintResource_v1_innerG(BinaryReader reader)
        {
            var result = new V1_InnerG();

            result.Version = ReadVersion(reader, 3, 0x1410BB030);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.SpeechAlgorithmsId = ReadUUID(reader);
            result.SpeechCharacterId = ReadUUID(reader);

            if(result.Version >= 3)
            {
                result.HandsRigged = reader.ReadBoolean();
            }

            return result;
        }

        public class V1_InnerF_Inner
        {
            public uint Version { get; set; }
            public byte Flags { get; set; }
        }
        private V1_InnerF_Inner Read_BlueprintResource_v1_innerF_Inner(BinaryReader reader)
        {
            var result = new V1_InnerF_Inner();

            result.Version = ReadVersion(reader, 1, 0x1411F3E90);
            result.Flags = reader.ReadByte();

            return result;
        }

        public class V1_InnerF
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public bool HasShapeResource { get; internal set; }
            public bool HasShapeModelerResourceRef { get; internal set; }
            public int ShapeType { get; internal set; }
            public List<float> BoxExtents { get; internal set; }
            public float SphereRadius { get; internal set; }
            public string ShapeId { get; internal set; }
            public string SourceResourceRefId { get; internal set; }
            public float ShapeScale { get; internal set; }
            public bool HasCollision { get; internal set; }
            public int AudioLinkCount { get; internal set; }
            public V1_InnerF_Inner ProhibitInvalidEnumVisibility { get; internal set; }
        }
        private V1_InnerF Read_BlueprintResource_v1_innerF(BinaryReader reader)
        {
            var result = new V1_InnerF();

            result.Version = ReadVersion(reader, 7, 0x1410BB020);
            if(result.Version >= 6)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.HasShapeResource = true;
            result.HasShapeModelerResourceRef = true;

            if(result.Version >= 4)
            {
                result.HasShapeResource = reader.ReadBoolean(); 
                result.HasShapeModelerResourceRef = reader.ReadBoolean(); 
            }

            result.ShapeType = reader.ReadInt32();
            result.BoxExtents = ReadVectorF(reader, 4);
            result.SphereRadius = reader.ReadSingle();

            if(result.HasShapeResource)
            {
                result.ShapeId = ReadUUID(reader);
            }
            if(result.HasShapeModelerResourceRef)
            {
                result.SourceResourceRefId = ReadUUID(reader);
            }

            result.ShapeScale = reader.ReadSingle();

            if(result.Version >= 2)
            {
                result.HasCollision = reader.ReadBoolean();
            }

            if(result.Version >= 3 && result.Version <= 4)
            {
                result.AudioLinkCount = reader.ReadInt32();
            }

            if(result.Version >= 7)
            {
                result.ProhibitInvalidEnumVisibility = Read_BlueprintResource_v1_innerF_Inner(reader);
            }

            return result;
        }

        public class V1_InnerE
        {
            public uint Version { get; internal set; }
            public AInner Data { get; internal set; }
        }
        private V1_InnerE Read_BlueprintResource_v1_innerE(BinaryReader reader)
        {
            var result = new V1_InnerE();

            result.Version = ReadVersion(reader, 2, 0x1410BB010);
            if(result.Version >= 2)
            {
                result.Data = Read_BlueprintResource_A_inner(reader);
            }

            return result;
        }

        public class V1_InnderD
        {
            public uint Version { get; set; }
            public AInner UnknownA { get; internal set; }
            public bool HasProjectData { get; internal set; }
            public bool HasProxyShape { get; internal set; }
            public bool HasSkeleton { get; internal set; }
            public bool HasAnimBinding { get; internal set; }
            public string ProjectDataId { get; internal set; }
            public string ProxyShapeId { get; internal set; }
            public string SkeletonId { get; internal set; }
            public string AnimationBindingId { get; internal set; }
            public int CreationMode { get; internal set; }
            public bool ClientOnly { get; internal set; }
            public float PlaybackSpeed { get; internal set; }
            public float StartTime { get; internal set; }
            public int PlaybackMode { get; internal set; }
            public bool BeginOnLoad { get; internal set; }
            public List<V1InnerD_V6> AnimationData { get; internal set; }
            public List<V1InnerD_V7> AnimationOverrides { get; internal set; }
            public List<V1InnerD_V8> AnimationSkeletonMappers { get; internal set; }
            public List<ClusterDefinitionResource.OffsetTransform> SgOffsetTransformsMap { get; internal set; }
        }

        private V1_InnderD Read_BlueprintResource_v1_innerD(BinaryReader reader)
        {
            var result = new V1_InnderD();

            result.Version = ReadVersion(reader, 10, 0x1410BB000);
            if(result.Version >= 3)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.HasProjectData = true;
            result.HasProxyShape = true;
            result.HasSkeleton = true;
            result.HasAnimBinding = true;

            if (result.Version >= 2)
            {
                result.HasProjectData = reader.ReadBoolean();
                result.HasProxyShape = reader.ReadBoolean();
                result.HasSkeleton = reader.ReadBoolean();
                result.HasAnimBinding = reader.ReadBoolean();
            }

            if (result.HasProjectData)
            {
                result.ProjectDataId = ReadUUID(reader);
            }

            if(result.HasProxyShape)
            {
                result.ProxyShapeId = ReadUUID(reader);
            }

            if (result.HasSkeleton)
            {
                result.SkeletonId = ReadUUID(reader);
            }

            if (result.HasAnimBinding)
            {
                result.AnimationBindingId = ReadUUID(reader);
            }

            result.CreationMode = reader.ReadInt32();
            result.ClientOnly = reader.ReadBoolean();

            if(result.Version >= 4)
            {
                result.PlaybackSpeed = reader.ReadSingle();
                result.StartTime = reader.ReadSingle();
            }

            if (result.Version >= 5)
            {
                result.PlaybackMode = reader.ReadInt32();
                result.BeginOnLoad = reader.ReadBoolean();
            }

            if(result.Version >= 6)
            {
                result.AnimationData = Read_List(reader, Read_BlueprintResource_v1_innerD_V6, 1, 0x1411F0410);
            }

            if(result.Version >= 7)
            {
                result.AnimationOverrides = Read_List(reader, Read_BlueprintResource_v1_innerD_V7, 1, 0x1411F80B0);
            }

            if(result.Version >= 8)
            {
                result.AnimationSkeletonMappers = Read_List(reader, Read_BlueprintResource_v1_innerD_V8, 1, 0x1411F80C0);
            }

            if(result.Version == 9)
            {
                result.SgOffsetTransformsMap = ClusterReader.Read_AnimationComponent_OffsetTransformsMap(reader);
            }

            return result;
        }

        public class V1InnerD_V8
        {
            public uint Version { get; internal set; }
            public string SkeletonMapperId { get; internal set; }
        }
        private V1InnerD_V8 Read_BlueprintResource_v1_innerD_V8(BinaryReader reader)
        {
            var result = new V1InnerD_V8();

            result.Version = ReadVersion(reader, 1, 0x1411F80C0);
            result.SkeletonMapperId = ReadUUID(reader);

            return result;
        }

        public class V1InnerD_V7
        {
            public uint Version { get; internal set; }
            public string AnimationBindingId { get; internal set; }
            public string AnimationOverrideName { get; internal set; }
            public string AnimationName { get; internal set; }
            public string AnimationClusterResourceId { get; internal set; }
            public string AnimationSkeletonName { get; internal set; }
        }
        private V1InnerD_V7 Read_BlueprintResource_v1_innerD_V7(BinaryReader reader)
        {
            var result = new V1InnerD_V7();

            result.Version = ReadVersion(reader, 2, 0x141205330);
            result.AnimationBindingId = ReadUUID(reader);
            result.AnimationOverrideName = ReadString(reader);
            result.AnimationName = ReadString(reader);
            result.AnimationClusterResourceId = ReadUUID(reader);

            if(result.Version >= 2)
            {
                result.AnimationSkeletonName = ReadString(reader);
            }

            return result;
        }

        public class V1InnerD_V6
        {
            public uint Version { get; set; }
            public string AnimationBindingId { get; internal set; }
            public string AnimationName { get; internal set; }
            public float PlaybackSpeed { get; internal set; }
            public int PlaybackMode { get; internal set; }
            public int AnimationBindingRemapIndex { get; internal set; }
            public string AnimationClusterResourceId { get; internal set; }
        }
        private V1InnerD_V6 Read_BlueprintResource_v1_innerD_V6(BinaryReader reader)
        {
            var result = new V1InnerD_V6();

            result.Version = ReadVersion(reader, 3, 0x1412021F0);
            result.AnimationBindingId = ReadUUID(reader);
            result.AnimationName = ReadString(reader);
            result.PlaybackSpeed = reader.ReadSingle();
            result.PlaybackMode = reader.ReadInt32();

            if(result.Version == 2)
            {
                result.AnimationBindingRemapIndex = reader.ReadInt32();
                result.AnimationClusterResourceId = ReadUUID(reader);
            }

            return result;
        }

        public class V1_UnknownC_Inner
        {
            public uint Version { get; set; }
            public uint ParentIndex { get; internal set; }
            public uint Flags { get; internal set; }
            public Transform LocalTransform { get; internal set; }
            public int ElementType { get; internal set; }
            public List<float> UserRotationValues { get; internal set; }
            public uint LinkId { get; internal set; }
        }

        private V1_UnknownC_Inner Read_BlueprintResource_v1_UnknownC_inner(BinaryReader reader)
        {
            var result = new V1_UnknownC_Inner();

            result.Version = ReadVersion(reader, 4, 0x1410BB110);
            result.ParentIndex = reader.ReadUInt32();
            result.Flags = reader.ReadUInt32();
            result.LocalTransform = Read_Transform(reader);
            result.ElementType = reader.ReadInt32();

            if (result.Version >= 3)
            {
                result.UserRotationValues = ReadVectorF(reader, 4);
            }

            if (result.Version >= 4)
            {
                result.LinkId = reader.ReadUInt32();
            }

            return result;
        }


        public class BP_Inner1
        {
            public uint Version { get; set; }
            public uint Start { get; internal set; }
            public uint End { get; internal set; }
        }
        private BP_Inner1 Read_BlueprintResource_inner1(BinaryReader reader)
        {
            var result = new BP_Inner1();

            result.Version = ReadVersion(reader, 1, 0x1411ECD10);
            result.Start = reader.ReadUInt32();
            result.End = reader.ReadUInt32();

            return result;
        }

        public class SomethingA
        {
            public List<V1_InnerQ> OldData { get; internal set; }
            public List<BP_Inner1> NewData { get; internal set; }
        }
        private SomethingA Read_BlueprintResource_Something(BinaryReader reader, uint version)
        {
            var result = new SomethingA();

            if (version < 6)
            {
                result.OldData = Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
            }
            else
            {
                result.NewData = Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
            }

            return result;
        }


        public class Blueprint
        {
            public uint Version { get; set; }
            public BlueprintV1 OldData { get; internal set; }
            public List<BlueprintV1Inner> Objects { get; internal set; }
            public List<int> ObjectIndices { get; internal set; }
            public List<V1_InnerP> EntitySchematics { get; internal set; }
            public string Name { get; internal set; }
            public List<V1_UnknownC_Inner> TransformGraphEntries { get; internal set; }
            public uint TransformSchematicCount { get; internal set; }
            public List<List<int>> TransformSchematics { get; internal set; }
            public uint TransformNonSchematicCount { get; internal set; }
            public List<List<int>> TransformNonSchematics { get; internal set; }
            public List<AData> JointSchematics { get; internal set; }
            public List<BData> JointGraphEntries { get; internal set; }
            public List<BlueprintV1InnerB> EntityJoinRangesOldList { get; internal set; }
            public List<BP_Inner1> EntityJoinRangesArray { get; internal set; }
            public List<int> EntityJointIndices { get; internal set; }
            public long SchematicBitField { get; internal set; }
            public List<V1_InnderD> AnimData { get; internal set; }
            public SomethingA AnimIndices { get; internal set; }
            public List<V1_InnerE> PoseData { get; internal set; }
            public SomethingA PoseIndices { get; internal set; }
            public List<V1_InnerF> RBodData { get; internal set; }
            public SomethingA RBodIndices { get; internal set; }
            public List<V1_InnerG> CharData { get; internal set; }
            public SomethingA CharIndices { get; internal set; }
            public List<V1_InnerH> SptData { get; internal set; }
            public SomethingA SptIndices { get; internal set; }
            public List<V1_InnerI> CamData { get; internal set; }
            public SomethingA CamIndices { get; internal set; }
            public List<V1_InnerJ> LitData { get; internal set; }
            public SomethingA LitIndices { get; internal set; }
            public List<V1_InnerK> SMshData { get; internal set; }
            public SomethingA SMshIndices { get; internal set; }
            public List<V1_InnerL> RMeshData { get; internal set; }
            public SomethingA RMeshIndices { get; internal set; }
            public List<V1_InnerM> AudData { get; internal set; }
            public SomethingA AudIndices { get; internal set; }
            public List<V1_InnerN> ScrData { get; internal set; }
            public SomethingA ScrIndices { get; internal set; }
            public List<V1_InnerO> TeraData { get; internal set; }
            public SomethingA TeraIndices { get; internal set; }
            public List<V1_InnerS> CloData { get; internal set; }
            public SomethingA CloIndices { get; internal set; }
            public List<V1_InnerT> IKBdData { get; internal set; }
            public SomethingA IKBdIndices { get; internal set; }
            public List<V1_InnerU> GrbpData { get; internal set; }
            public SomethingA GrbpIndices { get; internal set; }
            public List<V1_InnerV> SitpData { get; internal set; }
            public SomethingA SitpIndices { get; internal set; }
            public List<V1_InnerW> SebpData { get; internal set; }
            public SomethingA SebpIndices { get; internal set; }
            public V1_InnerR EditParameterGraph { get; internal set; }
            public List<string> CustomCapabilities { get; internal set; }
            public float Scale { get; internal set; }
            public int HighestLinkId { get; internal set; }
        }
        private Blueprint Read_BlueprintResource(BinaryReader reader)
        {
            var result = new Blueprint();

            result.Version = ReadVersion(reader, 7, 0x1411A5660);
            if(result.Version < 4)
            {
                result.OldData = Read_BlueprintResource_v1(reader, result.Version);
                return result;
            }

            result.Objects = Read_List(reader, Read_BlueprintResource_v1_inner, 1, 0x1411CD360);
            result.ObjectIndices = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            result.EntitySchematics = Read_List(reader, Read_BlueprintResource_v1_innerP, 1, 0x1411CD370);
            result.Name = ReadString(reader);
            result.TransformGraphEntries = Read_List(reader, Read_BlueprintResource_v1_UnknownC_inner, 1, 0x1411D9910);

            result.TransformSchematicCount = reader.ReadUInt32();
            result.TransformSchematics = new List<List<int>>();
            for (int i = 0; i < result.TransformSchematicCount; i++)
            {
                var item = Read_List(reader, n => n.ReadInt32(), 1, 0x1411CD380);
                result.TransformSchematics.Add(item);
            }

            result.TransformNonSchematicCount = reader.ReadUInt32();
            result.TransformNonSchematics = new List<List<int>>();
            for (int i = 0; i < result.TransformNonSchematicCount; i++)
            {
                var item = Read_List(reader, n => n.ReadInt32(), 1, 0x1411CD380);
                result.TransformNonSchematics.Add(item);
            }

            result.JointSchematics = Read_List(reader, Read_BluepintResource_A, 1, 0x1411CD390);
            result.JointGraphEntries = Read_List(reader, Read_BluepintResource_B, 1, 0x1411CD3A0);
            
            // entityJoinRanges =
            if(result.Version < 6)
            {
                result.EntityJoinRangesOldList = Read_List(reader, Read_BlueprintResource_v1_innerB, 1, 0x1411CD3B0);
                result.EntityJointIndices = Read_List(reader, n => n.ReadInt32(), 1, 0x1411CD3C0);
            }
            else
            {
                result.EntityJoinRangesArray = Read_List(reader, Read_BlueprintResource_inner1 , 1, 0x1411D9920);
                result.EntityJointIndices = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            }

            result.SchematicBitField = reader.ReadInt64();
            if((result.SchematicBitField & (1 << 0)) != 0)
            {
                result.AnimData = Read_List(reader, Read_BlueprintResource_v1_innerD, 1, 0x1411D00F0);
                result.AnimIndices = Read_BlueprintResource_Something(reader, result.Version);
            }
            
            if((result.SchematicBitField & (1 << 1)) != 0)
            {
                result.PoseData = Read_List(reader, Read_BlueprintResource_v1_innerE, 1, 0x1411D0110);
                result.PoseIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 2)) != 0)
            {
                result.RBodData = Read_List(reader, Read_BlueprintResource_v1_innerF, 1, 0x1411D0120);
                result.RBodIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 3)) != 0)
            {
                result.CharData = Read_List(reader, Read_BlueprintResource_v1_innerG, 1, 0x1411D0130);
                result.CharIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 4)) != 0)
            {
                result.SptData = Read_List(reader, Read_BlueprintResource_v1_innerH, 1, 0x1411D0140); 
                result.SptIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 5)) != 0)
            {
                result.CamData = Read_List(reader, Read_BlueprintResource_v1_innerI, 1, 0x1411D0150);
                result.CamIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 6)) != 0)
            {
                result.LitData = Read_List(reader, Read_BlueprintResource_v1_innerJ, 1, 0x1411D0160);
                result.LitIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 7)) != 0)
            {
                result.SMshData = Read_List(reader, Read_BlueprintResource_v1_innerK, 1, 0x1411D0170);
                result.SMshIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 8)) != 0)
            {
                result.RMeshData = Read_List(reader, Read_BlueprintResource_v1_innerL, 1, 0x1411D0180);
                result.RMeshIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 9)) != 0)
            {
                result.AudData = Read_List(reader, Read_BlueprintResource_v1_innerM, 1, 0x1411D0190);
                result.AudIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 10)) != 0)
            {
                result.ScrData = Read_List(reader, Read_BlueprintResource_v1_innerN, 1, 0x1411A0410);
                result.ScrIndices = Read_BlueprintResource_Something(reader, result.Version);
            }
            if ((result.SchematicBitField & (1 << 11)) != 0)
            {
                result.TeraData = Read_List(reader, Read_BlueprintResource_v1_innerO, 1, 0x1411D01A0); // no longer used?
                result.TeraIndices = Read_BlueprintResource_Something(reader, result.Version);
            }
            if ((result.SchematicBitField & (1 << 12)) != 0)
            {
                result.CloData = Read_List(reader, Read_BlueprintResource_v1_innerS, 1, 0x1411D01B0);
                result.CloIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if ((result.SchematicBitField & (1 << 13)) != 0)
            {
                result.IKBdData = Read_List(reader, Read_BlueprintResource_v1_innerT, 1, 0x1411D01C0);
                result.IKBdIndices = Read_BlueprintResource_Something(reader, result.Version);
            }
            if ((result.SchematicBitField & (1 << 14)) != 0)
            {
                result.GrbpData = Read_List(reader, Read_BlueprintResource_v1_innerU, 1, 0x1411D01D0);
                result.GrbpIndices = Read_BlueprintResource_Something(reader, result.Version);
            } 
            if ((result.SchematicBitField & (1 << 15)) != 0)
            {
                result.SitpData = Read_List(reader, Read_BlueprintResource_v1_innerV, 1, 0x1411D01E0);
                result.SitpIndices = Read_BlueprintResource_Something(reader, result.Version);
            }  
            if ((result.SchematicBitField & (1 << 16)) != 0)
            {
                result.SebpData = Read_List(reader, Read_BlueprintResource_v1_innerW, 1, 0x1411D01F0);
                result.SebpIndices = Read_BlueprintResource_Something(reader, result.Version);
            }

            if(result.Version < 7)
            {
                result.EditParameterGraph = Read_BlueprintResource_v1_innerR(reader);
            }

            result.CustomCapabilities = Read_List(reader, ReadString, 1, 0x1411CD3D0);
            result.Scale = reader.ReadSingle();

            if(result.Version >= 7)
            {
                result.HighestLinkId = reader.ReadInt32();
            }

            return result;
        }

        private ClusterDefinitionResource ClusterReader { get; set; }

        public Blueprint Resource { get; set; }
        public string Id { get; private set; }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            ClusterReader = new ClusterDefinitionResource();
            ClusterReader.OverrideVersionMap(this.versionMap, this.componentMap);

            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_BlueprintResource(reader);
            }
        }
    }
}
