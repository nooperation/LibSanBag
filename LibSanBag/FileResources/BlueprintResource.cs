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
            public string UnknownA { get; internal set; }
            public int UnknownC { get; internal set; }
        }
        private AInner Read_BlueprintResource_A_inner(BinaryReader reader)
        {
            var result = new AInner();

            result.Version = ReadVersion(reader, 2, 0x1411B1100);
            result.UnknownA = ReadUUID(reader);

            if (result.Version >= 2)
            {
                result.UnknownC = reader.ReadInt32();
            }

            return result;
        }

        public class AData
        {
            public uint Version { get; set; }
            public AInner UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public byte UnknownC { get; internal set; }
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

            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadByte();

            if (result.UnknownB == 1)
            {
                result.VersionB = ReadVersion(reader, 1, 0x1411ECD30);
            }
            else if (result.UnknownB == 2)
            {
                result.VersionC = ReadVersion(reader, 1, 0x1411ECD40);
                result.VersionCString = ReadString(reader);
            }
            else
            {
                result.UnknownD = ReadUUID(reader);
            }

            return result;
        }

        public class BData
        {
            public uint Version { get; set; }
            public int UnknownA { get; set; }
            public int UnknownB { get; set; }
        }
        private BData Read_BluepintResource_B(BinaryReader reader)
        {
            var result = new BData();

            result.Version = ReadVersion(reader, 1, 0x1411D9940);

            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();

            return result;
        }

        public class BlueprintV1Inner
        {
            public uint Version { get; set; }
            public int UnknownA { get; internal set; }
        }
        public BlueprintV1Inner Read_BlueprintResource_v1_inner(BinaryReader reader)
        {
            var result = new BlueprintV1Inner();

            result.Version = ReadVersion(reader, 2, 0x1410BB140);
            if(result.Version >= 2)
            {
                result.UnknownA = reader.ReadInt32();
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
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
        }
        public BlueprintV1InnerB Read_BlueprintResource_v1_innerB(BinaryReader reader)
        {
            var result = new BlueprintV1InnerB();

            result.Version = ReadVersion(reader, 1, 0x1411D9950);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();

            return result;
        }

        public class BlueprintV1
        {
            public List<BlueprintV1Inner> UnknownA { get; internal set; }
            public List<int> UnknownB { get; internal set; }
            public V1_UnknownC UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
            public List<BlueprintV1Inner> UnknownE { get; internal set; }
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
        }
        private BlueprintV1 Read_BlueprintResource_v1(BinaryReader reader, uint outerVersion)
        {
            var result = new BlueprintV1();

            result.UnknownA = Read_List(reader, Read_BlueprintResource_v1_inner, 1, 0x1411CD360);
            result.UnknownB = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            result.UnknownC = Read_BlueprintResource_v1_UnknownC(reader);
            result.UnknownD = 19;

            if(outerVersion >= 2)
            {
                result.UnknownD = reader.ReadInt32();
            }

            result.UnknownE = new List<BlueprintV1Inner>();
            for (int i = 0; i < result.UnknownD; i++)
            {
                var item = Read_BlueprintResource_v1_inner(reader);
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
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
        }
        private V1_InnerP_innerA Read_BlueprintResource_v1_innerP_innerA(BinaryReader reader)
        {
            var result = new V1_InnerP_innerA();

            result.Version = ReadVersion(reader, 1, 0x1412021E0);

            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadInt32();

            return result;
        }

        public class V1_InnerP_inner
        {
            public uint Version { get; internal set; }
            public V1_InnerP_innerA UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public bool UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
            public bool UnknownE { get; internal set; }
            public bool UnknownF { get; internal set; }
            public List<float> UnknownG { get; internal set; }
            public List<float> UnknownH { get; internal set; }
            public int UnknownI { get; internal set; }
            public string UnknownJ { get; internal set; }
            public List<ClusterDefinitionResource.AudioResourcePoolSound> UnknownK { get; internal set; }
            public bool UnknownL { get; internal set; }
            public int UnknownM { get; internal set; }
            public string UnknownN { get; internal set; }
        }
        private V1_InnerP_inner Read_BlueprintResource_v1_innerP_inner(BinaryReader reader)
        {
            var result = new V1_InnerP_inner();

            result.Version = ReadVersion(reader, 11, 0x1411ECD20);
            result.UnknownA = Read_BlueprintResource_v1_innerP_innerA(reader);
            result.UnknownB = ReadString(reader);

            if (result.Version < 6)
            {
                result.UnknownC = reader.ReadBoolean();
            }
            else
            {
                result.UnknownD = reader.ReadInt32();
                result.UnknownE = reader.ReadBoolean();
            }

            if(result.Version < 8 && result.UnknownD == 2)
            {
                // TODO: ??? noop?
            }

            if(result.Version >= 4)
            {
                result.UnknownF = reader.ReadBoolean();
            }

            result.UnknownG = ReadVectorF(reader, 4);
            result.UnknownH = ReadVectorF(reader, 4);
            result.UnknownI = reader.ReadInt32();

            if(result.Version >= 2)
            {
                result.UnknownJ = ReadUUID(reader);
            }
            if(result.Version >= 9)
            {
                result.UnknownK = ClusterReader.Read_RigidBody_AudioResourcePoolSounds(reader);
            }
            if(result.Version >= 10)
            {
                result.UnknownL = reader.ReadBoolean();
            }
            if(result.Version >= 11)
            {
                result.UnknownM = reader.ReadInt32();
            }

            if(result.Version - 5 <= 1)
            {
                result.UnknownN = ReadUUID_B(reader);
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
            public AInner UnknownA { get; internal set; }
        }
        private V1_InnerW Read_BlueprintResource_v1_innerW(BinaryReader reader)
        {
            var result = new V1_InnerW();

            result.Version = ReadVersion(reader, 1, 0x1410BB100);

            result.Version = ReadVersion(reader, 1, 0x1411DDD80);
            result.UnknownA = Read_BlueprintResource_A_inner(reader);

            return result;
        }

        public class V1_InnerV
        {
            public uint Version { get; internal set; }
            public int UnknownB { get; internal set; }
            public V1_InnerU_inner UnknownA { get; internal set; }
        }
        private V1_InnerV Read_BlueprintResource_v1_innerV(BinaryReader reader)
        {
            var result = new V1_InnerV();

            result.Version = ReadVersion(reader, 1, 0x1410BB0F0);
            result.UnknownA = Read_BlueprintResource_v1_innerU_inner(reader);
            result.UnknownB = reader.ReadInt32();

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
            public int UnknownB { get; internal set; }
            public bool UnknownC { get; internal set; }
            public bool UnknownD { get; internal set; }
            public AInner UnknownE { get; internal set; }
            public int UnknownF { get; internal set; }
        }
        private V1_InnerU Read_BlueprintResource_v1_innerU(BinaryReader reader)
        {
            var result = new V1_InnerU();

            result.Version = ReadVersion(reader, 5, 0x1410BB0E0);
            if(result.Version >= 3)
            {
                result.UnknownA = Read_BlueprintResource_v1_innerU_inner(reader);

                result.UnknownB = reader.ReadInt32();
                result.UnknownC = reader.ReadBoolean();
                result.UnknownD = reader.ReadBoolean();
            }
            else
            {
                result.UnknownE = Read_BlueprintResource_A_inner(reader);
                result.UnknownF = reader.ReadInt32();
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
            public List<V1_InnerR_inner_A> UnknownA { get; internal set; }
            public List<V1_InnerR_inner_B> UnknownB { get; internal set; }
            public List<int> UnknownC { get; internal set; }
            public List<V1_InnerR_inner_C> UnknownD { get; internal set; }
            public List<int> UnknownE { get; internal set; }
            public List<long> UnknownF { get; internal set; }
            public List<V1_InnerR_inner_D> UnknownG { get; internal set; }
        }
        private V1_InnerR_inner_E Read_BlueprintResource_v1_innerR_inner_E(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_E();

            result.UnknownA = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_A, 1, 0x1411DA7B0);
            result.UnknownB = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_B, 1, 0x1411DA7C0);
            result.UnknownC = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            result.UnknownD = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_C, 1, 0x1411A6190);
            result.UnknownE = Read_List(reader, n => n.ReadInt32(), 1, 0x1411DC3F0);

            var unknown = 0;
            result.UnknownF = new List<long>();
            for (int i = 0; i < unknown; i++)
            {
                var item = reader.ReadInt64();
                result.UnknownF.Add(item);
            }
            
            result.UnknownG = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_D, 1, 0x1411DA7D0);

            var unknown2 = 0; // TODO: Unknown counter
            for (var i = 0; i < unknown2; ++i)
            {
                reader.ReadInt64();
                reader.ReadInt64();
            }

            var unknown3 = 0; // TODO: Unknown counter
            for (var i = 0; i < unknown3; ++i)
            {
                reader.ReadInt64();
            }

            return result;
        }

        public class V1_InnerR_inner_D
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public V1_InnerR_inner_B_inner UnknownC { get; internal set; }
            public V1_InnerR_inner_B_inner UnknownD { get; internal set; }
        }
        private V1_InnerR_inner_D Read_BlueprintResource_v1_innerR_inner_D(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_D();

            result.Version = ReadVersion(reader, 2, 0x1410BB160);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();

            if(result.Version >= 2)
            {
                result.UnknownC = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
                result.UnknownD = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_C
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public List<V1_InnerR_inner_C_A> UnknownB { get; internal set; }
            public List<int> UnknownC { get; internal set; }
            public List<V1_InnerR_inner_C_A_internal> UnknownD { get; internal set; }
            public List<string> UnknownE { get; internal set; }
            public List<string> UnknownF { get; internal set; }
            public List<V1_InnerR_inner_Ab_internal> UnknownG { get; internal set; }
            public List<List<V1_InnerR_inner_Ab_internal>> UnknownH { get; internal set; }
            public V1_InnerR_inner_B_inner UnknownI { get; internal set; }
            public V1_InnerR_inner_B_inner UnknownJ { get; internal set; }
        }
        private V1_InnerR_inner_C Read_BlueprintResource_v1_innerR_inner_C(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_C();

            result.Version = ReadVersion(reader, 4, 0x1410BB150);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_C_A, 1, 0x1411C3B90);

            if(result.Version < 3)
            {
                result.UnknownC = Read_List(reader, n => n.ReadInt32(), 1, 0x1411C3BB0);
            }
            else
            {
                result.UnknownD = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_C_A_internal, 1, 0x1411C3BA0);
            }

            result.UnknownE = Read_List(reader, ReadString, 1, 0x14119ADB0);
            result.UnknownF = Read_List(reader, ReadString, 1, 0x14119ADB0);

            if(false)
            {
                // todo: hell no. not doing this
            }

            if(result.Version >= 3)
            {
                result.UnknownG = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ab_internal, 1, 0x1411C3BC0);
                result.UnknownH = Read_List(
                    reader, 
                    n => Read_List(n, Read_BlueprintResource_v1_innerR_inner_Ab_internal, 1, 0x1411C3BC0), 
                    1,
                    0x1411C3BD0
                );
            }

            if(result.Version >= 4)
            {
                result.UnknownI = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
                result.UnknownJ = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_C_A_internal
        {
            public uint Version { get; internal set; }
            public List<V1_InnerR_inner_Ab_internal> UnknownA { get; internal set; }
            public V1_InnerR_inner_B_inner UnknownB { get; internal set; }
        }
        private V1_InnerR_inner_C_A_internal Read_BlueprintResource_v1_innerR_inner_C_A_internal(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_C_A_internal();

            result.Version = ReadVersion(reader, 3, 0x1411D0CD0);

            if (result.Version >= 2)
            {
                result.UnknownA = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ab_internal, 1, 0x1411C3BC0);
            }
            if (result.Version >= 3)
            {
                result.UnknownB = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_C_A
        {
            public uint Version { get; internal set; }
            public uint Version2 { get; internal set; }
            public V1_InnerR_inner_C_A_internal UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public byte[] UnknownC { get; internal set; }
        }
        private V1_InnerR_inner_C_A Read_BlueprintResource_v1_innerR_inner_C_A(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_C_A();

            result.Version = ReadVersion(reader, 2, 0x1411D0CC0);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_v1_innerR_inner_C_A_internal(reader);
            }

            result.UnknownB = reader.ReadInt32();
            result.UnknownC = Read_Array(reader);

            return result;
        }

        public class V1_InnerR_inner_B
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
            public V1_InnerR_inner_B_inner UnknownE { get; internal set; }
        }
        private V1_InnerR_inner_B Read_BlueprintResource_v1_innerR_inner_B(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_B();
            result.UnknownA = 0xFFFF;

            result.Version = ReadVersion(reader, 3, 0x1410BB170);
            if(result.Version >= 3)
            {
                // noop
            }
            else
            {
                result.UnknownA = reader.ReadInt32();
            }
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadInt32();
            result.UnknownD = reader.ReadInt32();

            if (result.Version >= 2)
            {
                result.UnknownE = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
            }

            return result;
        }

        public class V1_InnerR_inner_B_inner
        {
            public long UnknownA { get; internal set; }
            public V1_InnerL_v4_innerC UnknownB { get; internal set; }
        }
        private V1_InnerR_inner_B_inner Read_BlueprintResource_v1_innerR_inner_B_inner(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_B_inner();

            // todo: This also exists in Read_BlueprintResource_v1_innerR_inner_Ab_internal
            if (false /* something confusing */)
            {
                // TODO: This might be possible?
                result.UnknownA = reader.ReadInt64(); // I don't know
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
            public bool UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
        }
        private V1_InnerR_inner_Aa Read_BlueprintResource_v1_innerR_inner_Aa(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_Aa();

            result.Version = ReadVersion(reader, 2, 0x141209060);
            result.UnknownA = reader.ReadBoolean();
            if(result.Version < 2)
            {
                result.UnknownB = reader.ReadInt32();
            }

            return result;
        }

        public class V1_InnerR_inner_Ac
        {
            public V1_InnerR_inner_Ab UnknownA { get; internal set; }
            public uint Version { get; internal set; }
            public int UnknownB { get; internal set; }
        }
        private V1_InnerR_inner_Ac Read_BlueprintResource_v1_innerR_inner_Ac(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_Ac();

            result.UnknownA = Read_BlueprintResource_v1_innerR_inner_Ab(reader);
            result.Version = ReadVersion(reader, 2, 0x141209070);

            if(result.Version < 2)
            {
                result.UnknownB = reader.ReadInt32();
            }

            return result;
        }


        public class V1_InnerR_inner_Ab_internal
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public V1_InnerR_inner_B_inner UnknownC { get; internal set; }
        }
        private V1_InnerR_inner_Ab_internal Read_BlueprintResource_v1_innerR_inner_Ab_internal(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_Ab_internal();

            result.Version = ReadVersion(reader, 3, 0x1411D0CE0);
            result.UnknownA = reader.ReadInt32();

            if(result.UnknownA - 1 != 0 && result.UnknownA - 2 != 0)
            {
                if(result.UnknownA - 2 == 3)
                {
                    result.UnknownB = ReadString(reader);
                }
                else
                {
                    result.UnknownC = Read_BlueprintResource_v1_innerR_inner_B_inner(reader);
                }
            }

            return result;
        }

        public class V1_InnerR_inner_Ab
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public string UnknownD { get; internal set; }
            public List<V1_InnerR_inner_Ab_internal> UnknownE { get; internal set; }
        }
        private V1_InnerR_inner_Ab Read_BlueprintResource_v1_innerR_inner_Ab(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_Ab();

            result.Version = ReadVersion(reader, 4, 0x14120B730);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();

            if(result.Version < 4)
            {
                result.UnknownC = reader.ReadInt32();
            }
            if(result.Version >= 2)
            {
                result.UnknownD = ReadString(reader);
            }
            if(result.Version >= 3)
            {
                // TODO: probably broken
                result.UnknownE = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ab_internal, 1, 0x1411C3BC0);
            }

            return result;
        }

        public class V1_InnerR_inner_A
        {
            public uint Version { get; internal set; }
            public List<V1_InnerR_inner_Aa> UnknownA { get; internal set; }
            public List<V1_InnerR_inner_Ab> UnknownB { get; internal set; }
            public List<V1_InnerR_inner_Ac> UnknownC { get; internal set; }
            public bool UnknownD { get; internal set; }
        }
        private V1_InnerR_inner_A Read_BlueprintResource_v1_innerR_inner_A(BinaryReader reader)
        {
            var result = new V1_InnerR_inner_A();

            result.Version = ReadVersion(reader, 1, 0x1410BB180);
            result.UnknownA = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Aa, 1, 0x141201180);
            result.UnknownB = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ab, 1, 0x141201190);
            result.UnknownC = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_Ac, 1, 0x1412011A0);
            result.UnknownD = reader.ReadBoolean();

            return result;
        }

        public class V1_InnerR
        {
            public uint Version { get; internal set; }
            public List<V1_InnerR_inner_A> UnknownA { get; internal set; }
            public List<V1_InnerR_inner_B> UnknownB { get; internal set; }
            public List<V1_InnerR_inner_C> UnknownC { get; internal set; }
            public List<V1_InnerR_inner_D> UnknownD { get; internal set; }
            public V1_InnerR_inner_E UnknownE { get; internal set; }
        }

        private V1_InnerR Read_BlueprintResource_v1_innerR(BinaryReader reader)
        {
            var result = new V1_InnerR();

            result.Version = ReadVersion(reader, 2, 0x1411CDF20);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_A, 1, 0x1411DA7B0);
                result.UnknownB = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_B, 1, 0x1411DA7C0);
                result.UnknownC = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_C, 1, 0x1411A6190); /* todo: incomplete internally */
                result.UnknownD = Read_List(reader, Read_BlueprintResource_v1_innerR_inner_D, 1, 0x1411DA7D0);
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
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
        }
        private V1_InnerQ Read_BlueprintResource_v1_innerQ(BinaryReader reader)
        {
            var result = new V1_InnerQ();

            result.Version = ReadVersion(reader, 1, 0x1411DA900);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();

            return result;
        }

        public class V1_InnerP
        {
            public uint Version { get; internal set; }
            public string UnknownA { get; internal set; }
            public V1_InnerP_inner UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
        }
        private V1_InnerP Read_BlueprintResource_v1_innerP(BinaryReader reader)
        {
            var result = new V1_InnerP();

            result.Version = ReadVersion(reader, 2, 0x1410BB120);
            result.UnknownA = ReadString(reader);
            result.UnknownB = Read_BlueprintResource_v1_innerP_inner(reader);

            if(result.Version >= 2)
            {
                result.UnknownC = reader.ReadInt32();
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


        public class V1_InnerN_inner_inner
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
        }
        private V1_InnerN_inner_inner Read_BlueprintResource_v1_innerN_inner_inner(BinaryReader reader)
        {
            var result = new V1_InnerN_inner_inner();

            result.Version = ReadVersion(reader, 6, 0x1411A3E00);
            result.UnknownA = reader.ReadInt32();

            return result;
        }

        public class V1_InnerN_inner
        {
            public uint Version { get; internal set; }
            public V1_InnerN_inner_inner UnknownA { get; internal set; }
            public V1_InnerN_inner_inner UnknownB { get; internal set; }
            public string UnknownC { get; internal set; }
            public string UnknownD { get; internal set; }
        }
        private V1_InnerN_inner Read_BlueprintResource_v1_innerN_inner(BinaryReader reader)
        {
            var result = new V1_InnerN_inner();

            result.Version = ReadVersion(reader, 2, 0x1411CCEA0);
            result.UnknownA = Read_BlueprintResource_v1_innerN_inner_inner(reader);
            result.UnknownB = Read_BlueprintResource_v1_innerN_inner_inner(reader);
            result.UnknownC = ReadString(reader);
            result.UnknownD = ReadString(reader);

            return result;
        }


        public class V1_InnerN_innerB
        {
            public AInner UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public bool UnknownC { get; internal set; }
            public string UnknownD { get; internal set; }
            public int UnknownE { get; internal set; }
            public List<ClusterDefinitionResource.ScriptParameter> UnknownF { get; internal set; }
            public int UnknownG { get; internal set; }
            public List<int> UnknownH { get; internal set; }
            public string UnknownI { get; internal set; }
            public List<string> UnknownJ { get; internal set; }
        }
        private V1_InnerN_innerB Read_BlueprintResource_v1_innerN_innerB(BinaryReader reader, uint parent_version)
        {
            var result = new V1_InnerN_innerB();
            var local_UnknownC = true;

            if(parent_version >= 5)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.UnknownB = ReadString(reader);
            if(parent_version >= 2)
            {
                result.UnknownC = reader.ReadBoolean();
                local_UnknownC = !result.UnknownC;
            }

            if(local_UnknownC)
            {
                result.UnknownD = ReadUUID(reader);
            }

            if (parent_version != 3)
            {
                result.UnknownE = reader.ReadInt32();
                result.UnknownF = new List<ClusterDefinitionResource.ScriptParameter>();
                for (int i = 0; i < result.UnknownE; i++)
                {
                    var item = ClusterReader.Read_ScriptComponent_parameter(reader);
                    result.UnknownF.Add(item);
                }
            }

            if(parent_version >= 3)
            {
                result.UnknownG = reader.ReadInt32();
                result.UnknownH = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            }

            if(parent_version <= 7) // - 6 <= 1
            {
                result.UnknownI = ReadUUID_B(reader);
            }

            if(parent_version == 7)
            {
                result.UnknownJ = Read_List(reader, ReadUUID_B, 1, 0x1411C1050);
            }

            return result;
        }

        public class V1_InnerN
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public string UnknownC { get; internal set; }
            public List<V1_InnerN_inner> UnknownD { get; internal set; }
            public int UnknownE { get; internal set; }
            public V1_InnerN_innerB UnknownF { get; internal set; }
        }
        private V1_InnerN Read_BlueprintResource_v1_innerN(BinaryReader reader)
        {
            var result = new V1_InnerN();

            result.Version = ReadVersion(reader, 12, 0x1410BB0A0);
            if(result.Version >= 9)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
                result.UnknownB = ReadString(reader);
                result.UnknownC = ReadUUID(reader);
                result.UnknownD = Read_List(reader, Read_BlueprintResource_v1_innerN_inner, 1, 0x1411BF180);

                if(result.Version >= 10)
                {
                    result.UnknownE = reader.ReadInt32();
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
            public bool UnknownA { get; internal set; }
            public bool UnknownB { get; internal set; }
            public bool UnknownC { get; internal set; }
            public string UnknownD { get; internal set; }
            public string UnknownE { get; internal set; }
            public string UnknownF { get; internal set; }
            public int UnknownG { get; internal set; }
            public string UnknownH { get; internal set; }
            public int UnknownI { get; internal set; }
            public bool UnknownJ { get; internal set; }
            public string UnknownK { get; internal set; }
            public int UnknownL { get; internal set; }
            public int UnknownM { get; internal set; }
            public long UnknownN { get; internal set; }
            public int UnknownO { get; internal set; }
        }
        private V1_InnerM Read_BlueprintResource_v1_innerM(BinaryReader reader)
        {
            var result = new V1_InnerM();

            var local_UnknownA = result.Version < 7;
            var local_UnknownB = true;
            var local_UnknownC = true;

            result.Version = ReadVersion(reader, 7, 0x1410BB090);
            if(result.Version >= 4)
            {
                Read_BlueprintResource_A_inner(reader);
            }

            if(result.Version >= 2)
            {
                if(result.Version < 7)
                {
                    result.UnknownA = reader.ReadBoolean();
                    local_UnknownA = !result.UnknownA;
                }

                result.UnknownB = reader.ReadBoolean();
                local_UnknownB = result.UnknownB;

                if(result.Version < 3)
                {
                    result.UnknownC = reader.ReadBoolean();
                    local_UnknownC = !result.UnknownC;
                }
            }

            if(local_UnknownA)
            {
                result.UnknownD = ReadUUID(reader);
            }

            if(result.Version < 7)
            {
                result.UnknownE = ReadUUID_B(reader);
            }

            if(local_UnknownB)
            {
                result.UnknownF = ReadUUID(reader);
            }
            else
            {
                // noop
            }

            result.UnknownG = reader.ReadInt32();

            if(result.Version < 7)
            {
                result.UnknownH = ReadString(reader);
                result.UnknownI = reader.ReadInt32();
            }

            if(result.Version >= 5)
            {
                result.UnknownJ = reader.ReadBoolean();
                if(result.Version < 6)
                {
                    result.UnknownK = ReadString(reader);
                }
                else
                {
                    result.UnknownL = reader.ReadInt32();
                }
            }

            if (!local_UnknownC)
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
                    result.UnknownM = reader.ReadInt32();
                    result.UnknownN = reader.ReadInt64();
                }
            }

            result.UnknownO = reader.ReadInt32();

            return result;
        }

        public class V1_InnerL_v4_innerC
        {
            public uint Version { get; internal set; }
            public long UnknownA { get; internal set; }
        }
        private V1_InnerL_v4_innerC Read_BlueprintResource_v1_innerL_v4_innerC(BinaryReader reader)
        {
            var result = new V1_InnerL_v4_innerC();

            result.Version = ReadVersion(reader, 2, 0x1411B5140);
            result.UnknownA = reader.ReadInt64();

            if(result.UnknownA != -1 && result.Version < 2)
            {
                // NOOP
            }

            return result;
        }

        public class V1_InnerL_v4_innerB
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
        }
        private V1_InnerL_v4_innerB Read_BlueprintResource_v1_innerL_v4_innerB(BinaryReader reader)
        {
            var result = new V1_InnerL_v4_innerB();

            result.Version = ReadVersion(reader, 1, 0x1411B5150);
            result.UnknownA = reader.ReadInt32();

            return result;
        }

        public class V1_InnerL_v4_inner
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
        }
        private V1_InnerL_v4_inner Read_BlueprintResource_v1_innerL_v4_inner(BinaryReader reader)
        {
            var result = new V1_InnerL_v4_inner();

            result.Version = ReadVersion(reader, 1, 0x14121B550);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();

            return result;
        }

        public class V1_InnerL_v4
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public List<V1_InnerL_v4_inner> UnknownD { get; internal set; }
            public ClusterDefinitionResource.OffsetTransform UnknownE { get; internal set; }
            public V1_InnerL_v4_innerB UnknownF { get; internal set; }
            public V1_InnerL_v4_innerC UnknownG { get; internal set; }
        }
        private V1_InnerL_v4 Read_BlueprintResource_v1_innerL_v4(BinaryReader reader)
        {
            var result = new V1_InnerL_v4();

            result.Version = ReadVersion(reader, 2, 0x141202200);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadInt32();

            result.UnknownD = Read_List(reader, Read_BlueprintResource_v1_innerL_v4_inner, 1, 0x14120B090);
            result.UnknownE = ClusterReader.Read_AnimationComponent_OffsetTransform(reader);

            if(result.Version >= 2)
            {
                result.UnknownF = Read_BlueprintResource_v1_innerL_v4_innerB(reader);
            }
            else
            {
                result.UnknownG = Read_BlueprintResource_v1_innerL_v4_innerC(reader);
            }

            return result;
        }

        public class V1_InnerL
        {
            public uint Version { get; internal set; }
            public byte UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public List<Transform> UnknownC { get; internal set; }
            public List<ClusterDefinitionResource.OffsetTransform> UnknownD { get; internal set; }
            public string UnknownE { get; internal set; }
            public int UnknownF { get; internal set; }
            public V1_InnerK_Inner UnknownG { get; internal set; }
            public List<V1_InnerL_v4> UnknownH { get; internal set; }
            public V1_InnerK_Inner UnknownI { get; internal set; }
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
                result.UnknownA = reader.ReadByte(); // unused?
                result.UnknownB = ReadString(reader);
                result.UnknownC = Read_List(reader, Read_Transform, 1, 0x1411F3EA0);
                result.UnknownD = Read_List(reader, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0);
            }
            else if(result.Version == 2)
            {
                result.UnknownE = ReadUUID(reader);

                result.UnknownA = reader.ReadByte(); // unused?
                result.UnknownB = ReadString(reader);
                result.UnknownC = Read_List(reader, Read_Transform, 1, 0x1411F3EA0);
                result.UnknownD = Read_List(reader, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0);

                result.UnknownF = reader.ReadInt32();
            }
            else if(result.Version == 3)
            {
                result.UnknownG = Read_BlueprintResource_v1_innerK_Inner(reader);
                result.UnknownC = Read_List(reader, Read_Transform, 1, 0x1411F3EA0);
                result.UnknownD = Read_List(reader, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0);
            }
            else if(result.Version == 4)
            {
                result.UnknownG = Read_BlueprintResource_v1_innerK_Inner(reader);
                result.UnknownC = Read_List(reader, Read_Transform, 1, 0x1411F3EA0);
                result.UnknownD = Read_List(reader, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0);

                result.UnknownH = Read_List(reader, Read_BlueprintResource_v1_innerL_v4, 1, 0x1411F2790);
            }
            else if (result.Version >= 5)
            {
                result.UnknownI = Read_BlueprintResource_v1_innerK_Inner(reader);
                result.MeshBindings = ReadComponent(reader, n => Read_List(n, Read_Transform, 1, 0x1411F3EA0));
                result.Pose = ReadComponent(reader, n => Read_List(n, ClusterReader.Read_AnimationComponent_OffsetTransform, 1, 0x1411F3EB0));
            
                result.UnknownH = Read_List(reader, Read_BlueprintResource_v1_innerL_v4, 1, 0x1411F2790);
            }

            if (result.Version >= 6)
            {
                result.MorphSkeleton = ReadUUID(reader);
            }

            return result;
        }
        
        public class V1_InnerK_Inner_Inner_A_innerE
        {
            public string UnknownA { get; internal set; }
            public uint Version { get; internal set; }
            public bool UnknownB { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_innerE Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerE(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_innerE();

            result.UnknownA = ReadString(reader); // might be readarray?
            result.Version = ReadVersion(reader, 1, 0x141220A80);
            result.UnknownB = reader.ReadBoolean();

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_innerD
        {
            public string UnknownA { get; internal set; }
            public uint Version { get; internal set; }
            public uint VersionB { get; internal set; }
            public List<float> UnknownB { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_innerD Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerD(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_innerD();

            result.UnknownA = ReadString(reader); // might be readarray?
            result.Version = ReadVersion(reader, 1, 0x14121E230);
            result.VersionB = ReadVersion(reader, 1, 0x14121E220);
            result.UnknownB = ReadVectorF(reader, 4);

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_innerC
        {
            public string UnknownA { get; internal set; }
            public uint Version { get; internal set; }
            public List<float> UnknownB { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_innerC Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerC(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_innerC();

            result.UnknownA = ReadString(reader); // might be readarray?
            result.Version = ReadVersion(reader, 1, 0x14121E220);
            result.UnknownB = ReadVectorF(reader, 4);

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_innerB
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public string Unknown0 { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_innerB Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerB(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_innerB();

            result.Unknown0 = ReadString(reader);
            result.Version = ReadVersion(reader, 1, 0x14121E210);
            result.UnknownA = reader.ReadInt32();

            return result;
        }

        public class V1_InnerK_Inner_Inner_A_inner
        {
            public uint Version { get; internal set; }
            public uint VersionB { get; internal set; }
            public string Texture { get; internal set; }
            public string Unknown0 { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A_inner Read_BlueprintResource_v1_innerK_Inner_Inner_A_inner(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A_inner();

            result.Unknown0 = ReadString(reader);
            result.Version = ReadVersion(reader, 1, 0x14121E200);
            result.VersionB = ReadVersion(reader, 1, 0x141223110);
            result.Texture = ReadUUID(reader);

            return result;
        }

        public class V1_InnerK_Inner_Inner_A
        {
            public uint Version { get; internal set; }
            public string UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_inner> UnknownC { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_inner> UnknownD { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_innerB> UnknownE { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_innerC> UnknownF { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_innerD> UnknownG { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A_innerE> UnknownH { get; internal set; }
            public string UnknownI { get; internal set; }
            public int UnknownJ { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_A Read_BlueprintResource_v1_innerK_Inner_Inner_A(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_A();

            result.Version = ReadVersion(reader, 5, 0x141206320);
            result.UnknownA = ReadString(reader);
            result.UnknownB = ReadString(reader);
            result.UnknownC = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_inner, 1, 0x141211AE0);

            if (result.Version < 4)
            {
                result.UnknownD = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_inner, 1, 0x1418EB7C0);
            }

            result.UnknownE = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerB, 1, 0x141211B00);
            result.UnknownF = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerC, 1, 0x141211B10);
            result.UnknownG = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerD, 1, 0x141211B20);

            if(result.Version >= 5)
            {
                result.UnknownH = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A_innerE, 1, 0x141217100);
            }

            result.UnknownI = ReadUUID(reader);
            result.UnknownJ = reader.ReadInt32();

            if(result.Version < 2)
            {
                // NOOP
            }

            if(result.Version < 3)
            {
                // NOOP
            }

            return result;
        }



        public class V1_InnerK_Inner_Inner_B_InnerA
        {
            public uint Version { get; internal set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_B_InnerA Read_BlueprintResource_v1_innerK_Inner_Inner_B_InnerA(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_B_InnerA();

            result.Version = ReadVersion(reader, 1, 0x141222930);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadInt32();
            result.UnknownD = reader.ReadInt32();

            return result;
        }

        public class V1_InnerK_Inner_Inner_B
        {
            public uint Version { get; internal set; }
            public List<V1_InnerK_Inner_Inner_B_InnerA> UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_B Read_BlueprintResource_v1_innerK_Inner_Inner_B(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_B();

            result.Version = ReadVersion(reader, 3, 0x14120E1A0);
            if(result.Version >= 3)
            {
                result.UnknownA = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_B_InnerA, 1, 0x14121CBB0);
            }
            else
            {
                result.UnknownB = reader.ReadInt32();
                result.UnknownC = reader.ReadInt32();
            }

            if(result.Version >= 2)
            {
                result.UnknownD = reader.ReadInt32();
            }

            return result;
        }

        public class V1_InnerK_Inner_Inner_C
        {
            public uint Version { get; internal set; }
            public V1_InnerK_Inner_Inner_A UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
        }
        private V1_InnerK_Inner_Inner_C Read_BlueprintResource_v1_innerK_Inner_Inner_C(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner_C();

            result.Version = ReadVersion(reader, 1, 0x1418E77B0);
            result.UnknownA = Read_BlueprintResource_v1_innerK_Inner_Inner_A(reader);
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadInt32();

            return result;
        }

        public class V1_InnerK_Inner_Inner
        {
            public uint Version { get; internal set; }
            public string PickResource { get; internal set; }
            public string UnknownB { get; internal set; }
            public List<V1_InnerK_Inner_Inner_A> UnknownC { get; internal set; }
            public List<V1_InnerK_Inner_Inner_B> UnknownD { get; internal set; }
            public List<V1_InnerK_Inner_Inner_C> UnknownE { get; internal set; }
            public string UnknownF { get; internal set; }
            public string Morphs { get; internal set; }
            public string BlendShape { get; internal set; }
        }
        private V1_InnerK_Inner_Inner Read_BlueprintResource_v1_innerK_Inner_Inner(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner_Inner();

            result.Version = ReadVersion(reader, 5, 0x1411FB8B0);
            result.PickResource = ReadUUID(reader);
            result.UnknownB = ReadUUID(reader);

            if(result.Version >= 3)
            {
                result.UnknownC = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_A, 1, 0x141205450);
                result.UnknownD = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_B, 1, 0x141205460);
            }
            else
            {
                result.UnknownE = Read_List(reader, Read_BlueprintResource_v1_innerK_Inner_Inner_C, 1, 0x1418E9D50);
            }

            if(result.Version < 4)
            {
                result.UnknownF = ReadUUID(reader);
            }

            if(result.Version >= 2)
            {
                if(result.Version < 5)
                {
                    result.Morphs = ReadUUID(reader);
                }

                result.BlendShape = ReadUUID(reader);
            }

            return result;
        }

        public class V1_InnerK_Inner
        {
            public uint Version { get; internal set; }
            public string UnknownA { get; internal set; }
            public V1_InnerK_Inner_Inner UnknownB { get; internal set; }
            public byte UnknownC { get; internal set; }
            public string UnknownD { get; internal set; }
            public int UnknownE { get; internal set; }
            public bool UnknownF { get; internal set; }
            public List<string> UnknownG { get; internal set; }
            public int UnknownH { get; internal set; }
            public int UnknownI { get; internal set; }
            public bool UnknownJ { get; internal set; }
            public bool UnknownK { get; internal set; }
            public AInner UnkonwnL { get; internal set; }
        }
        private V1_InnerK_Inner Read_BlueprintResource_v1_innerK_Inner(BinaryReader reader)
        {
            var result = new V1_InnerK_Inner();

            result.Version = ReadVersion(reader, 8, 0x1411DD9A0);

            if(result.Version >= 4)
            {
                result.UnkonwnL = Read_BlueprintResource_A_inner(reader);
            }

            if(result.Version == 1)
            {
                result.UnknownA = ReadUUID(reader);
            }

            result.UnknownB = Read_BlueprintResource_v1_innerK_Inner_Inner(reader);

            if(result.Version < 5)
            {
                result.UnknownC = reader.ReadByte(); // unused?
            }

            result.UnknownD = ReadString(reader);
            result.UnknownE = reader.ReadInt32();

            if(result.Version >= 3)
            {
                if(result.Version < 5)
                {
                    result.UnknownF = reader.ReadBoolean();
                }

                result.UnknownG = Read_List(reader, ReadString, 1, 0x14119ADB0);

                if(result.Version >= 5)
                {
                   result.UnknownH = reader.ReadInt32();
                }
            }

            if (result.Version >= 6)
            {
               result.UnknownI = reader.ReadInt32();
            }
            if (result.Version >= 7)
            {
               result.UnknownJ = reader.ReadBoolean();
            }
            if (result.Version >= 8)
            {
               result.UnknownK = reader.ReadBoolean();
            }

            return result;
        }


        public class V1_InnerK
        {
            public uint Version { get; internal set; }
            public string ModelDefinition { get; internal set; }
            public byte UnknownB { get; internal set; }
            public string Name { get; internal set; }
            public int UnknownD { get; internal set; }
            public V1_InnerK_Inner UnknownV2 { get; internal set; }
        }
        private V1_InnerK Read_BlueprintResource_v1_innerK(BinaryReader reader)
        {
            var result = new V1_InnerK();

            result.Version = ReadVersion(reader, 2, 0x1410BB070);
            if(result.Version == 1)
            {
                result.ModelDefinition = ReadUUID(reader);
                result.UnknownB = reader.ReadByte(); // unused?
                result.Name = ReadString(reader);
                result.UnknownD = reader.ReadInt32();
            }
            else if(result.Version >= 2)
            {
                result.UnknownV2 = Read_BlueprintResource_v1_innerK_Inner(reader);
            }

            return result;
        }

        public class V1_InnerJ
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public string UnknownC { get; internal set; }
            public List<float> UnknownD { get; internal set; }
            public int UnknownE { get; internal set; }
            public int UnknownF { get; internal set; }
            public bool UnknownG { get; internal set; }
            public int UnknownH { get; internal set; }
            public int UnknownI { get; internal set; }
            public int UnknownJ { get; internal set; }
            public bool UnknownK { get; internal set; }
            public bool UnknownL { get; internal set; }
        }
        private V1_InnerJ Read_BlueprintResource_v1_innerJ(BinaryReader reader)
        {
            var result = new V1_InnerJ();

            result.Version = ReadVersion(reader, 5, 0x1410BB060);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.UnknownB = reader.ReadInt32();
            result.UnknownC = ReadString(reader);
            result.UnknownD = ReadVectorF(reader, 4);

            if(result.Version < 3)
            {
                // noop
            }

            result.UnknownE = reader.ReadInt32();
            result.UnknownF = reader.ReadInt32();
            result.UnknownG = reader.ReadBoolean();

            result.UnknownH = reader.ReadInt32();
            result.UnknownI = reader.ReadInt32();
            result.UnknownJ = reader.ReadInt32();

            if (result.Version >= 4)
            {
                result.UnknownK = reader.ReadBoolean();
            }
            if(result.Version >= 5)
            {
                result.UnknownL = reader.ReadBoolean();
            }

            return result;
        }

        public class V1_InnerI
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
            public bool UnknownE { get; internal set; }
        }
        private V1_InnerI Read_BlueprintResource_v1_innerI(BinaryReader reader)
        {
            var result = new V1_InnerI();

            result.Version = ReadVersion(reader, 2, 0x1410BB050);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.UnknownB = reader.ReadInt32();
            result.UnknownC = reader.ReadInt32();
            result.UnknownD = reader.ReadInt32();
            result.UnknownE = reader.ReadBoolean();

            return result;
        }

        public class V1_InnerH
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public bool UnknownB { get; internal set; }
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
                result.UnknownB = reader.ReadBoolean();
            }

            return result;
        }

        public class V1_InnerG
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public string UnknownC { get; internal set; }
            public bool UnknownD { get; internal set; }
        }
        private V1_InnerG Read_BlueprintResource_v1_innerG(BinaryReader reader)
        {
            var result = new V1_InnerG();

            result.Version = ReadVersion(reader, 3, 0x1410BB030);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.UnknownB = ReadUUID(reader);
            result.UnknownC = ReadUUID(reader);

            if(result.Version >= 3)
            {
                result.UnknownD = reader.ReadBoolean();
            }

            return result;
        }

        public class V1_InnerF_Inner
        {
            public uint Version { get; set; }
            public byte UnknownA { get; set; }
        }
        private V1_InnerF_Inner Read_BlueprintResource_v1_innerF_Inner(BinaryReader reader)
        {
            var result = new V1_InnerF_Inner();

            result.Version = ReadVersion(reader, 1, 0x1411F3E90);
            result.UnknownA = reader.ReadByte();

            return result;
        }

        public class V1_InnerF
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
            public bool UnknownB { get; internal set; }
            public bool UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
            public List<float> UnknownE { get; internal set; }
            public int UnknownF { get; internal set; }
            public string UnknownG { get; internal set; }
            public string UnknownH { get; internal set; }
            public int UnknownI { get; internal set; }
            public byte UnknownJ { get; internal set; }
            public int UnknownK { get; internal set; }
            public V1_InnerF_Inner UnknownL { get; internal set; }
        }
        private V1_InnerF Read_BlueprintResource_v1_innerF(BinaryReader reader)
        {
            var result = new V1_InnerF();

            result.Version = ReadVersion(reader, 7, 0x1410BB020);
            if(result.Version >= 6)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.UnknownB = true;
            result.UnknownC = true;

            if(result.Version >= 4)
            {
                result.UnknownB = reader.ReadBoolean(); 
                result.UnknownC = reader.ReadBoolean(); 
            }

            result.UnknownD = reader.ReadInt32();
            result.UnknownE = ReadVectorF(reader, 4);
            result.UnknownF = reader.ReadInt32();

            if(result.UnknownB)
            {
                result.UnknownG = ReadUUID(reader);
            }
            if(result.UnknownC)
            {
                result.UnknownH = ReadUUID(reader);
            }

            result.UnknownI = reader.ReadInt32();

            if(result.Version >= 2)
            {
                result.UnknownJ = reader.ReadByte();
            }

            if(result.Version <= 4) // -3 <= 1
            {
                result.UnknownK = reader.ReadInt32(); // unused, skip 4 bytes
            }

            if(result.Version >= 7)
            {
                result.UnknownL = Read_BlueprintResource_v1_innerF_Inner(reader);
            }

            return result;
        }

        public class V1_InnerE
        {
            public uint Version { get; internal set; }
            public AInner UnknownA { get; internal set; }
        }
        private V1_InnerE Read_BlueprintResource_v1_innerE(BinaryReader reader)
        {
            var result = new V1_InnerE();

            result.Version = ReadVersion(reader, 2, 0x1410BB010);
            if(result.Version >= 2)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            return result;
        }

        public class V1_InnderD
        {
            public uint Version { get; set; }
            public AInner UnknownA { get; internal set; }
            public byte UnknownB { get; internal set; }
            public byte UnknownC { get; internal set; }
            public byte UnknownD { get; internal set; }
            public byte UnknownE { get; internal set; }
            public string UnknownB_ID { get; internal set; }
            public string UnknownC_ID { get; internal set; }
            public string UnknownD_ID { get; internal set; }
            public string UnknownE_ID { get; internal set; }
            public int UnknownF { get; internal set; }
            public byte UnknownG { get; internal set; }
            public int UnknownH { get; internal set; }
            public int UnknownI { get; internal set; }
            public int UnknownJ { get; internal set; }
            public byte UnknownK { get; internal set; }
            public List<V1InnerD_V6> UnknownL { get; internal set; }
            public List<V1InnerD_V7> UnknownM { get; internal set; }
            public List<V1InnerD_V8> UnknownN { get; internal set; }
            public List<ClusterDefinitionResource.OffsetTransform> UnknownO { get; internal set; }
        }

        private V1_InnderD Read_BlueprintResource_v1_innerD(BinaryReader reader)
        {
            var result = new V1_InnderD();

            result.Version = ReadVersion(reader, 10, 0x1410BB000);
            if(result.Version >= 3)
            {
                result.UnknownA = Read_BlueprintResource_A_inner(reader);
            }

            result.UnknownB = 1;
            result.UnknownC = 1;
            result.UnknownD = 1;
            result.UnknownE = 1;
            if (result.Version >= 2)
            {
                result.UnknownB = reader.ReadByte();
                result.UnknownC = reader.ReadByte();
                result.UnknownD = reader.ReadByte();
                result.UnknownE = reader.ReadByte();
            }

            if(result.UnknownB != 0)
            {
                result.UnknownB_ID = ReadUUID(reader);
            }

            if(result.UnknownC != 0)
            {
                result.UnknownC_ID = ReadUUID(reader);
            }

            if (result.UnknownD != 0)
            {
                result.UnknownD_ID = ReadUUID(reader);
            }

            if (result.UnknownE != 0)
            {
                result.UnknownE_ID = ReadUUID(reader);
            }

            result.UnknownF = reader.ReadInt32();
            result.UnknownG = reader.ReadByte();

            if(result.Version >= 4)
            {
                result.UnknownH = reader.ReadInt32();
                result.UnknownI = reader.ReadInt32();
            }

            if (result.Version >= 5)
            {
                result.UnknownJ = reader.ReadInt32();
                result.UnknownK = reader.ReadByte();
            }

            if(result.Version >= 6)
            {
                result.UnknownL = Read_List(reader, Read_BlueprintResource_v1_innerD_V6, 1, 0x1411F0410);
            }

            if(result.Version >= 7)
            {
                result.UnknownM = Read_List(reader, Read_BlueprintResource_v1_innerD_V7, 1, 0x1411F80B0);
            }

            if(result.Version >= 8)
            {
                result.UnknownN = Read_List(reader, Read_BlueprintResource_v1_innerD_V8, 1, 0x1411F80C0);
            }

            if(result.Version == 9)
            {
                result.UnknownO = ClusterReader.Read_AnimationComponent_OffsetTransformsMap(reader);
            }

            return result;
        }

        public class V1InnerD_V8
        {
            public uint Version { get; internal set; }
            public string Id { get; internal set; }
        }
        private V1InnerD_V8 Read_BlueprintResource_v1_innerD_V8(BinaryReader reader)
        {
            var result = new V1InnerD_V8();

            result.Version = ReadVersion(reader, 1, 0x1411F80C0);
            result.Id = ReadUUID(reader);

            return result;
        }

        public class V1InnerD_V7
        {
            public uint Version { get; internal set; }
            public string UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public string UnknownC { get; internal set; }
            public string UnknownD { get; internal set; }
            public string UnknownE { get; internal set; }
        }
        private V1InnerD_V7 Read_BlueprintResource_v1_innerD_V7(BinaryReader reader)
        {
            var result = new V1InnerD_V7();

            result.Version = ReadVersion(reader, 2, 0x141205330);
            result.UnknownA = ReadUUID(reader);
            result.UnknownB = ReadString(reader);
            result.UnknownC = ReadString(reader);
            result.UnknownD = ReadUUID(reader);

            if(result.Version >= 2)
            {
                result.UnknownE = ReadString(reader);
            }

            return result;
        }

        public class V1InnerD_V6
        {
            public uint Version { get; set; }
            public string UnknownA { get; internal set; }
            public string UnknownB { get; internal set; }
            public int UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
            public int UnknownE { get; internal set; }
            public string UnknownF { get; internal set; }
        }
        private V1InnerD_V6 Read_BlueprintResource_v1_innerD_V6(BinaryReader reader)
        {
            var result = new V1InnerD_V6();

            result.Version = ReadVersion(reader, 3, 0x1412021F0);
            result.UnknownA = ReadUUID(reader);
            result.UnknownB = ReadString(reader);
            result.UnknownC = reader.ReadInt32();
            result.UnknownD = reader.ReadInt32();

            if(result.Version == 2)
            {
                result.UnknownE = reader.ReadInt32(); // unused, 4 bytes?
                result.UnknownF = ReadUUID(reader);
            }

            return result;
        }

        public class V1_UnknownC_Inner
        {
            public uint Version { get; set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
            public Transform UnknownC { get; internal set; }
            public int UnknownD { get; internal set; }
            public int UnknownF { get; internal set; }
        }

        private V1_UnknownC_Inner Read_BlueprintResource_v1_UnknownC_inner(BinaryReader reader)
        {
            var result = new V1_UnknownC_Inner();

            result.Version = ReadVersion(reader, 4, 0x1410BB110);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();
            result.UnknownC = Read_Transform(reader);
            result.UnknownD = reader.ReadInt32();

            if (result.Version >= 3)
            {
                // m128
                reader.ReadUInt64();
                reader.ReadUInt64();
            }

            if (result.Version >= 4)
            {
                result.UnknownF = reader.ReadInt32();
            }

            return result;
        }

        public class V1_UnknownC
        {
            public uint Version { get; set; }
            public int UnknownA { get; internal set; }
            public List<V1_UnknownC_Inner> UnknownB { get; internal set; }
        }
        private V1_UnknownC Read_BlueprintResource_v1_UnknownC(BinaryReader reader)
        {
            var result = new V1_UnknownC();

            result.Version = ReadVersion(reader, 1, 0x1411D9910);

            result.UnknownA = reader.ReadInt32();

            result.UnknownB = new List<V1_UnknownC_Inner>();
            for (int i = 0; i < result.UnknownA; i++)
            {
                var item = Read_BlueprintResource_v1_UnknownC_inner(reader);
                result.UnknownB.Add(item);
            }

            return result;
        }

        public class BP_Inner1
        {
            public uint Version { get; set; }
            public int UnknownA { get; internal set; }
            public int UnknownB { get; internal set; }
        }
        private BP_Inner1 Read_BlueprintResource_inner1(BinaryReader reader)
        {
            var result = new BP_Inner1();

            result.Version = ReadVersion(reader, 1, 0x1411ECD10);
            result.UnknownA = reader.ReadInt32();
            result.UnknownB = reader.ReadInt32();

            return result;
        }

        public class Blueprint
        {
            public uint Version { get; set; }
            public BlueprintV1 OldData { get; internal set; }
            public List<BlueprintV1Inner> UnknownA { get; internal set; }
            public List<int> UnknownB { get; internal set; }
            public List<V1_InnerP> UnknownC { get; internal set; }
            public string UnknownD { get; internal set; }
            public V1_UnknownC UnknownE { get; internal set; }
            public uint UnknownF { get; internal set; }
            public List<List<int>> UnknownFList { get; internal set; }
            public uint UnknownG { get; internal set; }
            public List<List<int>> UnknownGList { get; internal set; }
            public List<AData> UnknownH { get; internal set; }
            public List<BData> UnknownI { get; internal set; }
            public List<BlueprintV1InnerB> UnknownJ { get; internal set; }
            public List<int> UnknownK { get; internal set; }
            public List<BP_Inner1> UnknownL { get; internal set; }
            public List<int> UnknownM { get; internal set; }
            public long UnknownN { get; internal set; }
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

            result.UnknownA = Read_List(reader, Read_BlueprintResource_v1_inner, 1, 0x1411CD360);
            result.UnknownB = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            result.UnknownC = Read_List(reader, Read_BlueprintResource_v1_innerP, 1, 0x1411CD370);
            result.UnknownD = ReadString(reader);
            result.UnknownE = Read_BlueprintResource_v1_UnknownC(reader);

            result.UnknownF = reader.ReadUInt32();
            result.UnknownFList = new List<List<int>>();
            for (int i = 0; i < result.UnknownF; i++)
            {
                var item = Read_List(reader, n => n.ReadInt32(), 1, 0x1411CD380);
                result.UnknownFList.Add(item);
            }

            result.UnknownG = reader.ReadUInt32();
            result.UnknownGList = new List<List<int>>();
            for (int i = 0; i < result.UnknownG; i++)
            {
                var item = Read_List(reader, n => n.ReadInt32(), 1, 0x1411CD380);
                result.UnknownGList.Add(item);
            }

            result.UnknownH = Read_List(reader, Read_BluepintResource_A, 1, 0x1411CD390);
            result.UnknownI = Read_List(reader, Read_BluepintResource_B, 1, 0x1411CD3A0);

            if(result.Version < 6)
            {
                result.UnknownJ = Read_List(reader, Read_BlueprintResource_v1_innerB, 1, 0x1411CD3B0);
                result.UnknownK = Read_List(reader, n => n.ReadInt32(), 1, 0x1411CD3C0);
            }
            else
            {
                result.UnknownL = Read_List(reader, Read_BlueprintResource_inner1 , 1, 0x1411D9920);
                result.UnknownM = Read_List(reader, n => n.ReadInt32(), 1, 0x1411BF150);
            }

            result.UnknownN = reader.ReadInt64();
            if((result.UnknownN & (1 << 0)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerD, 1, 0x1411D00F0);

                if(result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }
            
            if((result.UnknownN & (1 << 1)) != 0)
            {
                Read_BlueprintResource_v1_innerE(reader);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if ((result.UnknownN & (1 << 2)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerF, 1, 0x1411D0120);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if ((result.UnknownN & (1 << 3)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerG, 1, 0x1411D0130);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }


            if ((result.UnknownN & (1 << 4)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerH, 1, 0x1411D0140);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if ((result.UnknownN & (1 << 5)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerI, 1, 0x1411D0150);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if ((result.UnknownN & (1 << 6)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerJ, 1, 0x1411D0160);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if ((result.UnknownN & (1 << 7)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerK, 1, 0x1411D0170);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if ((result.UnknownN & (1 << 8)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerL, 1, 0x1411D0180);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if ((result.UnknownN & (1 << 9)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerM, 1, 0x1411D0190);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if ((result.UnknownN & (1 << 10)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerN, 1, 0x1411A0410);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }
            if ((result.UnknownN & (1 << 11)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerO, 1, 0x1411D01A0);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }
            if ((result.UnknownN & (1 << 12)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerS, 1, 0x1411D01B0);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if ((result.UnknownN & (1 << 13)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerT, 1, 0x1411D01C0);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }
            if ((result.UnknownN & (1 << 14)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerU, 1, 0x1411D01D0);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            } 
            if ((result.UnknownN & (1 << 15)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerV, 1, 0x1411D01E0);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }  
            if ((result.UnknownN & (1 << 16)) != 0)
            {
                Read_List(reader, Read_BlueprintResource_v1_innerW, 1, 0x1411D01F0);

                if (result.Version < 6)
                {
                    Read_List(reader, Read_BlueprintResource_v1_innerQ, 1, 0x1411D0100);
                }
                else
                {
                    Read_List(reader, Read_BlueprintResource_inner1, 1, 0x1411D9920);
                }
            }

            if(result.Version < 7)
            {
                Read_BlueprintResource_v1_innerR(reader);
            }
            Read_List(reader, ReadString, 1, 0x1411CD3D0);
            reader.ReadInt32();

            if(result.Version >= 7)
            {
                reader.ReadInt32();
            }

            return result;
        }

        private ClusterDefinitionResource ClusterReader { get; set; }

        public Blueprint Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            ClusterReader = new ClusterDefinitionResource();
            ClusterReader.OverrideVersionMap(this.versionMap);

            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_BlueprintResource(reader);
            }
        }
    }
}
