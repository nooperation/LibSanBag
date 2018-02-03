using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LibSanBag.FileRecordInfo;

namespace LibSanBag
{
    public static class AssetVersions
    {
        /// <summary>
        /// Gets a list of all known resource version hashes for the specified resource type
        /// </summary>
        /// <param name="resourceType">Resource type to get version hash history of</param>
        /// <returns>Collection of version hashes for the specified resource type</returns>
        public static List<string> GetResourceVersions(ResourceType resourceType)
        {
            return AllResourceVersions[resourceType];
        }

        /// <summary>
        /// Gets the resource type for the specified version string
        /// </summary>
        /// <param name="version">Asset version</param>
        /// <returns>Associated recource type on success otherwise ResourceType.Unknown</returns>
        public static ResourceType GetResourceTypeFromVersion(string version)
        {
            foreach (var versions in AllResourceVersions)
            {
                if (versions.Value.Contains(version))
                {
                    return versions.Key;
                }
            }

            return ResourceType.Unknown;
        }

        private static readonly Dictionary<ResourceType, List<string>> AllResourceVersions = new Dictionary<ResourceType, List<string>>
        {
            [ResourceType.AnimationCanonical] = new List<string>
            {
                "88d8f0abbe2893af",
            },
            [ResourceType.AnimationImport] = new List<string>
            {
                "f5c4b15e812473c2",
                "44add6049744d441",
            },
            [ResourceType.GeometryResourceCanonical] = new List<string>
            {
                "51b89e39caab7b79",
            },
            [ResourceType.GeometryResourceImport] = new List<string>
            {
                "5635058fd7bd8c45",
                "608bcc6a85edcb9b",
                "48af51107860b924",
            },
            [ResourceType.hknpShapeImport] = new List<string>
            {
                "08fe8aea3c42c6a4",
            },
            //[ResourceType.] = new List<string>
            //{
            //    "e0ec1dea9efee411",
            //},
            [ResourceType.BlueprintResource] = new List<string>
            {
                "32a23cdd28d3561f",
                "cc197a25a23af4e3",
                "236db40d1d7855d1",
                "230f1609dda95d17",
                "b6aadef4bd599337",
                "8bd8b80cc46c85ec",
                "9947e8633fc915f0",
                "aa5fd02d4db51fbd",
                "567961462978d048",
                "276c9da5b3ccc2c4",
                "b4b585c909cefc03",
                "eae4930c62c804a0",
                "f9d85f400b9d36ed",
                "adf773641950421f",
                "c7b4dd5389690f68",
                "ce515458eecd5f34",
                "4a46f92955a24c0d",
                "4978b1becd18555f",
                "ba47ac07f49278a1",
                "9f1ef083d5a12bac",
                "b8798206b2f9d2d8",
                "97d2098b127c3996",
                "c0004f8618c7dbe7",
                "584238eb4244399a",
                "baf1d4e37d6b6421",
                "96b2da9c481675a2",
                "14aaabb110bfa263",
                "1cda7360aff0efc2",
                "a73bde61f97879ee",
                "d1216ff881936be9",
                "6c2b42bb327ed86d",
                "7213570f1193670b",
                "d2121abc455b1729",
                "19f3fcd88366beaf",
                "d16fafed06ec02ce",
                "20ef767e58ff8152",
                "0ef073206347fd9f",
                "c40d901baa84a2e2",
                "1de6424289130c8b",
                "9717cc839ea4caf5",
                "a9c0febc492c8b80",
                "16360b893e648b33",
                "bb430603ea99ba9b",
                "6de62596c9325ea3",
                "09fe5c917b216ce0",
                "d1270acc15641cbf",
                "f3976b1a9e1fc416",
                "4f6d419cb77506c6",
                "d9df9f296a567d9c",
                "cd67e20dc9024ce8",
                "445c85c1b379def5",
                "ca2ce5ecb73235d2",
            },
            [ResourceType.ClothingSource] = new List<string>
            {
                "d57956a9e8755ea1",
            },
            [ResourceType.ClusterSource] = new List<string>
            {
                "f2f239a307b08dca",
                "ef8f474eaa16ab5e",
                "04c2f16f40e01874",
                "c233f4605d755087",
                "d2474d0b7180e18c",
                "075e77df5480616e",
                "9ae74f0c692873d1",
                "53c5c05f2668afae",
                "942e8b032d43915b",
                "afb39762c90abb6d",
                "c8befc48015add98",
                "ad62a25a271022e3",
                "972fd71aa8496347",
            },
            [ResourceType.ModelMorphResource] = new List<string>
            {
                "a165915d1e69ac08",
                "fa4aa7bc04c023ea",
            },
            [ResourceType.TestResource] = new List<string>
            {
                "30f1f410150fe2ea",
            },
            [ResourceType.WorldChunkSource] = new List<string>
            {
                "2577f8eb78eace35",
                "500de0fde708e6a6",
                "784ccf8a091e11e3",
                "d8bab6e3ca0adad1",
                "b9f2726b161bd121",
                "e48f1f1f874dea54",
                "fd6129f94ada0057",
                "4add5e27f3ed4dff",
                "32b30f02274c2e7f",
                "cd8ec2dab25a163a",
                "a72173e7f8cfce70",
                "edd8ecb0331f1893",
                "fb55e0981d2be78e",
                "c1967c01e706d8cb",
                "e200b4b3053e9015",
                "7c7d03b78e70fed1",
                "ee3d57fb917efce2",
                "1ecb4ea3d1537e52",
                "39848556db10fd5b",
                "224615ec399d53a3",
                "27d29eb9f3144024",
                "c2db8039342da07b",
                "834c4e77fed5ce4b",
                "178a347e2b41552b",
                "f1ac8220b62bf489",
                "fe574bb46e6556cf",
                "f6799e449b62d8b9",
                "867c247c814d7098",
            },
            [ResourceType.WorldSource] = new List<string>
            {
                "a6cced6fccd44c99",
                "5748bfc933c206a7",
                "8bd76b1dc560ff24",
                "726ed5c60c66c08c",
                "9fc809792b20a5a2",
                "a887eca9f519c56e",
                "5ff584b2f60b6364",
                "73835fbc0d15ed5f",
                "db052a125ab7ce3d",
                "a2889beefcc4f0b0",
                "d3620443235de4ea",
                "dcbdbe1093ced7fb",
                "2362dd27b11bd75f",
                "b37fc6de2ca24418",
                "e054ca7db083b2bc",
                "012c6be8da06be8f",
                "f765cd7c1d50c226",
                "30fbc93132ca0ade",
                "0b4054ccfe539f38",
                "64cc0f97184d14c1",
            },
            [ResourceType.LicenseResource] = new List<string>
            {
                "2dd5f090ab6b5468",
                "31ee0708ce0b7879",
                "7aafdf777b01caf5",
                "e4fa764cb431ab8b",
                "b385897104fffda1",
                "431094583d13e854",
                "0d56ce89d133f52b",
                "ebb993b8aecc41b5",
                "fb523207ed241f98",
                "15a48115d981a66a",
                "4d6f1a76ad1172a0",
                "6125b4297b7afd56",
            },
            [ResourceType.ClusterDefinition] = new List<string>
            {
                "47356b09b73b7da3",
                "97cd2435e43058ea",
                "4553f50c35b6f430",
                "9006e0bbe22c3bda",
                "380ef86ad442ee18",
                "03dc13604790c871",
                "a65810c139203dc2",
                "c567a82d6e5afb95",
                "e950c3912b648f3a",
                "03045a87f102cb10",
                "5f4fe63261c0f958",
                "3709eaecce8b6eec",
                "e6aede2aa520fa07",
                "7eaf0c1a452c5fea",
                "be6f65d86c6c6c9a",
                "ec6d447607d4ee09",
                "5401ce1e8755ccc5",
                "b89d4a1489a6fc6c",
                "bf3f022b18a63dfd",
                "6dbeb10a90fcc37b",
                "8b88b41ad1f1da6c",
            },
            [ResourceType.WorldChunkDefinition] = new List<string>
            {
                "9eb043caffc8a44a",
                "13453a1be74e6b37",
                "30b3b045630e5456",
                "57f74be351cd15c1",
                "8e10238dde9a4ce5",
                "a4357a5ff49b0af6",
                "9a6143a7faa98d5d",
                "df148665ced263c8",
                "663c9eef6665a32f",
                "a265095d3395fdf6",
                "3696e23c88445656",
                "961a739d96ab3128",
                "9d08f4015e3bd77f",
                "66fde77ddfa510aa",
                "f61a5e9bdd322e24",
                "118146a43a52335b",
            },
            [ResourceType.WorldDefinition] = new List<string>
            {
                "ef77d68a4b0cfd4d",
                "a9a4ef13fc3b00f9",
                "c1a95aac3eae483f",
                "b1ff2160becac496",
                "bc02539f2587aedc",
                "8b0fd17bb549083d",
                "7ccc0adc03cc2970",
                "359f8ec299e201f7",
                "21baf2708ad836ea",
                "c92fc1d11f8252bb",
                "1455c0b90888edde",
                "cf0aed661ff93453",
                "5d5c681b6114f944",
            },
            [ResourceType.AudioGraphResource] = new List<string>
            {
                "3a334edb7033226c",
            },
            [ResourceType.AudioMaterialResource] = new List<string>
            {
                "7f1f7b85c74d9298",
                "c4a5aa2c387b35f7",
                "667eb286039a558a",
                "9b0b082be71d2673",
                "069a3e3b9f25828f",
            },
            [ResourceType.BankResource] = new List<string>
            {
                "5bfed512939ba7ff",
            },
            [ResourceType.EnvironmentResource] = new List<string>
            {
                "c84526638c38a906",
                "67bca340237ebe96",
            },
            [ResourceType.SoundResource] = new List<string>
            {
                "8510a121d70371a2",
                "ffe353a492e99156",
                "5d4dda35b60493d7",
            },
            [ResourceType.PickResource] = new List<string>
            {
                "4a2f6332eed3dfb0",
            },
            [ResourceType.PickableModelResource] = new List<string>
            {
                "42ae078bdaab63c6",
            },
            [ResourceType.TextureSource] = new List<string>
            {
                "0d049586f771978e",
                "896b078db905e876",
                "c4d25f70c6245bd0",
            },
            [ResourceType.BehaviorProjectData] = new List<string>
            {
                "a72e0b0662cb4408",
            },
            [ResourceType.LuaScriptResource] = new List<string>
            {
                "2487dccddadf7656",
            },
            [ResourceType.SpeechGraphicsAnimationResource] = new List<string>
            {
                "86e523fb88ae256e",
            },
            [ResourceType.SpeechGraphicsEngineResource] = new List<string>
            {
                "0346f2ce5a5f36e1",
            },
            //[ResourceType.] = new List<string>
            //{
            //    "d8345507e1847260",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "c14b1730bf98428d",
            //},
            [ResourceType.ClothingImport] = new List<string>
            {
                "823084584bf5029c",
            },
            [ResourceType.FileResource] = new List<string>
            {
                "78e422aaa1e1ba00",
                "7c29dacd363b945a",
            },
            [ResourceType.HeightmapImport] = new List<string>
            {
                "f196c4728c3bda4c",
            },
            //[ResourceType.] = new List<string>
            //{
            //    "25d507eb26b8eb54",
            //},
            [ResourceType.ScriptImport] = new List<string>
            {
                "ac9fbfc60e051e0d",
                "cb63525ec60d2ea0",
            },
            [ResourceType.SoundImport] = new List<string>
            {
                "6b714f812579233c",
                "187d9530f9fc2ded",
                "8e2a33dd70f661f5",
            },
            //[ResourceType.] = new List<string>
            //{
            //    "34d4c7b2f5d60e9d",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "49d938ebe91ab46b",
            //},
            [ResourceType.TextureImport] = new List<string>
            {
                "26130b88acf15586",
                "2c4aa80ce491b33a",
                "94ec565217ef3ad2",
                "6d5fcb91b158f2f6",
            },
            [ResourceType.BufferResource] = new List<string>
            {
                "d5f5dda636eb2e1a",
            },
            [ResourceType.GeometryResourceResource] = new List<string>
            {
                "581a503da8d3e98a",
            },
            [ResourceType.MaterialResource] = new List<string>
            {
                "40651c5b33a962df",
                "d18b41db83aa2d3d",
                "0d82cdbf668f2669",
            },
            [ResourceType.MeshResource] = new List<string>
            {
                "1248859d98c4fac2",
            },
            [ResourceType.TextureResource] = new List<string>
            {
                "9a8d4bbd19b4cd55",
            },
            [ResourceType.VertexDefinitionResourceResource] = new List<string>
            {
                "6d98a5d596161190",
            },
            //[ResourceType.] = new List<string>
            //{
            //    "051d3c49b715c035",
            //    "ab7bccfc424d3617",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "c166007bb09fd7dd",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "c55dbe97c9562549",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "6e6b3940043320e7",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "65bd023a542c4933",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "9c9957c43919b322",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "79a83b132a1d6be6",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "6b843642d4731f54",
            //},
            //[ResourceType.] = new List<string>
            //{
            //    "44cfe32795d22fb8",
            //},
            [ResourceType.TerrainRuntimeDecalTextureData] = new List<string>
            {
                "9882829ebec65bf7",
            },
            [ResourceType.TerrainRuntimeTextureData] = new List<string>
            {
                "4a5a9ccbb88bc6a8",
                "0d35975a737b7740",
                "79b34fa59b2c7282",
                "024124019cca5f13",
                "bae152f9473bfd9e",
            },
            [ResourceType.TerrainRuntimeData] = new List<string>
            {
                "b725168f4d55f177",
                "c753e74eb6a75d30",
                "1c6a583e69bbbb63",
                "3ad97681339be3f9",
                "f55afe96df07399a",
                "e60417205d24b485",
                "27b4b1c5be5cb7e2",
                "a4309aae24195d9f",
            },
            [ResourceType.TerrainSourceData] = new List<string>
            {
                "642effccc24622a4",
                "eba4ff98ea0b09f2",
                "2cdabee8bc4dfd3a",
                "f7f4bf1d96ae304b",
                "bfcfc0233eeb4828",
                "ec59c258e54ef2e2",
                "7e3ca64894c50744",
                "904b1504d2231ca3",
                "3900df4491028aab",
                "f6559f7acc6b0ffa",
                "5247c0a7aadc4bac",
                "3110ed031da3faf5",
                "c331ed9d8d072201",
                "b2317354d8cbee44",
                "6653d0b621c48f71",
                "dce772d42637897d",
                "6f97b6cf2d949f8e",
                "1cc505341f2922c1",
            },
            [ResourceType.ScriptCompiledBytecodeResource] = new List<string>
            {
                "695aad7e1181dc46",
                "c84707da067146a9",
                "e6ac3244f1076f7b",
            },
            [ResourceType.ScriptMetadataResource] = new List<string>
            {
                "67df52a55a73f7d3",
                "02575c46762a7c3c",
                "d97016058b281211",
                "0b604dc8c94bc188",
                "b8e35358a76fa32a",
                "d75de17df1892f86",
                "0a316ea155e30eda",
            },
            [ResourceType.ScriptResource] = new List<string>
            {
                "8b273ad6e91874c1",
                "152137d932762673",
                "a5e5740caf72f738",
            },
            [ResourceType.ScriptSourceTextResource] = new List<string>
            {
                "4cde67396803610f",
                "6301a7d31aa6f628",
                "dedd8914f8dfe71e",
            },
            [ResourceType.hkaAnimationBindingResource] = new List<string>
            {
                "fced9919cb285f6f",
            },
            [ResourceType.hkaSkeletonResource] = new List<string>
            {
                "b728547fdb2c4522",
            },
            [ResourceType.hkaSkeletonMapperResource] = new List<string>
            {
                "92c58b13b94629aa",
            },
            [ResourceType.hkbBehaviorGraphResource] = new List<string>
            {
                "8a45b4789b7b4bda",
            },
            [ResourceType.hkbCharacterDataResource] = new List<string>
            {
                "e0027325ae4e853f",
            },
            [ResourceType.hkbProjectDataResource] = new List<string>
            {
                "1fd92e78456f6eb8",
            },
            [ResourceType.hknpMaterialResource] = new List<string>
            {
                "161f13afec1887ad",
            },
            [ResourceType.hknpPhysicsSystemDataResource] = new List<string>
            {
                "e06efc4882af72e6",
            },
            [ResourceType.hknpRagdollDataResource] = new List<string>
            {
                "fd48f1a0e5ba9ae7",
            },
            [ResourceType.hknpShapeResource] = new List<string>
            {
                "a11e4b205f3213d8",
            },
            [ResourceType.hkpConstraintDataResource] = new List<string>
            {
                "07d6e3a87b889802",
            },
            [ResourceType.Unknown] = new List<string>(),
        };
    }
}
