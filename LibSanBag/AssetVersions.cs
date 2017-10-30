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
            return ResourceVersions[resourceType];
        }

        private static Dictionary<ResourceType, List<string>> ResourceVersions = new Dictionary<ResourceType, List<string>>
        {
            [ResourceType.AnimationCanonical] = new List<string>
            {
                "88D8F0ABBE2893AF",
            },
            [ResourceType.AnimationImport] = new List<string>
            {
                "44ADD6049744D441",
            },
            [ResourceType.GeometryResourceCanonical] = new List<string>
            {
                "51B89E39CAAB7B79",
            },
            [ResourceType.GeometryResourceImport] = new List<string>
            {
                "608BCC6A85EDCB9B",
                "48AF51107860B924",
            },
            [ResourceType.hknpShapeImport] = new List<string>
            {
                "08FE8AEA3C42C6A4",
            },
            [ResourceType.BlueprintResource] = new List<string>
            {
                "EAE4930C62C804A0",
                "F9D85F400B9D36ED",
                "ADF773641950421F",
                "C7B4DD5389690F68",
                "CE515458EECD5F34",
                "4A46F92955A24C0D",
                "4978B1BECD18555F",
                "BA47AC07F49278A1",
                "9F1EF083D5A12BAC",
                "B8798206B2F9D2D8",
                "97D2098B127C3996",
                "C0004F8618C7DBE7",
                "584238EB4244399A",
                "BAF1D4E37D6B6421",
                "96B2DA9C481675A2",
                "14AAABB110BFA263",
                "1CDA7360AFF0EFC2",
                "A73BDE61F97879EE",
                "D1216FF881936BE9",
                "6C2B42BB327ED86D",
                "7213570F1193670B",
                "D2121ABC455B1729",
                "19F3FCD88366BEAF",
                "D16FAFED06EC02CE",
                "20EF767E58FF8152",
                "0EF073206347FD9F",
                "C40D901BAA84A2E2",
                "1DE6424289130C8B",
                "9717CC839EA4CAF5",
                "A9C0FEBC492C8B80",
                "16360B893E648B33",
                "BB430603EA99BA9B",
                "6DE62596C9325EA3",
                "09FE5C917B216CE0",
                "D1270ACC15641CBF",
                "F3976B1A9E1FC416",
                "4F6D419CB77506C6",
                "D9DF9F296A567D9C",
                "CD67E20DC9024CE8",
                "445C85C1B379DEF5",
                "CA2CE5ECB73235D2",
            },
            [ResourceType.ClusterSource] = new List<string>
            {
                "EF8F474EAA16AB5E",
                "04C2F16F40E01874",
                "C233F4605D755087",
                "D2474D0B7180E18C",
                "075E77DF5480616E",
                "9AE74F0C692873D1",
                "53C5C05F2668AFAE",
                "942E8B032D43915B",
                "AFB39762C90ABB6D",
                "C8BEFC48015ADD98",
                "AD62A25A271022E3",
                "972FD71AA8496347"
            },
            [ResourceType.ModelMorphResource] = new List<string>()
            {
                "FA4AA7BC04C023EA",
            },
            [ResourceType.TestResource] = new List<string>
            {
                "30F1F410150FE2EA",
            },
            [ResourceType.WorldChunkSource] = new List<string>
            {
                "500DE0FDE708E6A6",
                "784CCF8A091E11E3",
                "D8BAB6E3CA0ADAD1",
                "B9F2726B161BD121",
                "E48F1F1F874DEA54",
                "FD6129F94ADA0057",
                "4ADD5E27F3ED4DFF",
                "32B30F02274C2E7F",
                "CD8EC2DAB25A163A",
                "A72173E7F8CFCE70",
                "EDD8ECB0331F1893",
                "FB55E0981D2BE78E",
                "C1967C01E706D8CB",
                "E200B4B3053E9015",
                "7C7D03B78E70FED1",
                "EE3D57FB917EFCE2",
                "1ECB4EA3D1537E52",
                "39848556DB10FD5B",
                "224615EC399D53A3",
                "27D29EB9F3144024",
                "C2DB8039342DA07B",
                "834C4E77FED5CE4B",
                "178A347E2B41552B",
                "F1AC8220B62BF489",
                "FE574BB46E6556CF",
                "F6799E449B62D8B9",
                "867C247C814D7098",
            },
            [ResourceType.WorldSource] = new List<string>
            {
                "9FC809792B20A5A2",
                "A887ECA9F519C56E",
                "5FF584B2F60B6364",
                "73835FBC0D15ED5F",
                "DB052A125AB7CE3D",
                "A2889BEEFCC4F0B0",
                "D3620443235DE4EA",
                "DCBDBE1093CED7FB",
                "2362DD27B11BD75F",
                "B37FC6DE2CA24418",
                "E054CA7DB083B2BC",
                "012C6BE8DA06BE8F",
                "F765CD7C1D50C226",
                "30FBC93132CA0ADE",
                "0B4054CCFE539F38",
                "64CC0F97184D14C1",
            },
            [ResourceType.LicenseResource] = new List<string>
            {
                "0D56CE89D133F52B",
                "EBB993B8AECC41B5",
                "FB523207ED241F98",
                "15A48115D981A66A",
                "4D6F1A76AD1172A0",
                "6125B4297B7AFD56",
            },
            [ResourceType.ClusterDefinition] = new List<string>
            {
                "C567A82D6E5AFB95",
                "E950C3912B648F3A",
                "03045A87F102CB10",
                "5F4FE63261C0F958",
                "3709EAECCE8B6EEC",
                "E6AEDE2AA520FA07",
                "7EAF0C1A452C5FEA",
                "BE6F65D86C6C6C9A",
                "EC6D447607D4EE09",
                "5401CE1E8755CCC5",
                "B89D4A1489A6FC6C",
                "BF3F022B18A63DFD",
                "6DBEB10A90FCC37B",
                "8B88B41AD1F1DA6C",
            },
            [ResourceType.WorldChunkDefinition] = new List<string>
            {
                "9EB043CAFFC8A44A",
                "13453A1BE74E6B37",
                "30B3B045630E5456",
                "57F74BE351CD15C1",
                "8E10238DDE9A4CE5",
                "A4357A5FF49B0AF6",
                "9A6143A7FAA98D5D",
                "DF148665CED263C8",
                "663C9EEF6665A32F",
                "A265095D3395FDF6",
                "3696E23C88445656",
                "961A739D96AB3128",
                "9D08F4015E3BD77F",
                "66FDE77DDFA510AA",
                "F61A5E9BDD322E24",
                "118146A43A52335B",
            },
            [ResourceType.WorldDefinition] = new List<string>
            {
                "B1FF2160BECAC496",
                "BC02539F2587AEDC",
                "8B0FD17BB549083D",
                "7CCC0ADC03CC2970",
                "359F8EC299E201F7",
                "21BAF2708AD836EA",
                "C92FC1D11F8252BB",
                "1455C0B90888EDDE",
                "CF0AED661FF93453",
                "5D5C681B6114F944",
            },
            [ResourceType.AudioGraphResource] = new List<string>
            {
                "3A334EDB7033226C",
            },
            [ResourceType.AudioMaterialResource] = new List<string>
            {
                "667EB286039A558A",
                "9B0B082BE71D2673",
                "069A3E3B9F25828F",
            },
            [ResourceType.BankResource] = new List<string>
            {
                "5BFED512939BA7FF",
            },
            [ResourceType.EnvironmentResource] = new List<string>
            {
                "C84526638C38A906",
                "67BCA340237EBE96",
            },
            [ResourceType.SoundResource] = new List<string>
            {
                "8510A121D70371A2",
                "FFE353A492E99156",
                "5D4DDA35B60493D7",
            },
            [ResourceType.PickResource] = new List<string>
            {
                "4A2F6332EED3DFB0",
            },
            [ResourceType.PickableModelResource] = new List<string>
            {
                "42AE078BDAAB63C6",
            },
            [ResourceType.TextureSource] = new List<string>
            {
                "0D049586F771978E",
                "896B078DB905E876",
                "C4D25F70C6245BD0",
            },
            [ResourceType.BehaviorProjectData] = new List<string>
            {
                "A72E0B0662CB4408",
            },
            [ResourceType.LuaScriptResource] = new List<string>
            {
                "2487DCCDDADF7656",
            },
            [ResourceType.SpeechGraphicsAnimationResource] = new List<string>
            {
                "86E523FB88AE256E",
            },
            [ResourceType.SpeechGraphicsEngineResource] = new List<string>
            {
                "0346F2CE5A5F36E1",
            },
            [ResourceType.FileResource] = new List<string>
            {
                "78E422AAA1E1BA00",
                "7C29DACD363B945A",
            },
            [ResourceType.HeightmapImport] = new List<string>
            {
                "F196C4728C3BDA4C",
            },
            //[ResourceType.] = new List<string>()
            //{
            //     "AC9FBFC60E051E0D",
            //     "CB63525EC60D2EA0",
            //},
            [ResourceType.SoundImport] = new List<string>()
            {
                "6B714F812579233C",
                "187D9530F9FC2DED",
                "8E2A33DD70F661F5",
            },
            [ResourceType.TextureImport] = new List<string>
            {
                "26130B88ACF15586",
                "2C4AA80CE491B33A",
                "94EC565217EF3AD2",
                "6D5FCB91B158F2F6",
            },
            [ResourceType.BufferResource] = new List<string>
            {
                "D5F5DDA636EB2E1A",
            },
            [ResourceType.GeometryResourceResource] = new List<string>
            {
                "581A503DA8D3E98A",
            },
            [ResourceType.MaterialResource] = new List<string>
            {
                "40651C5B33A962DF",
                "D18B41DB83AA2D3D",
                "0D82CDBF668F2669",
            },
            [ResourceType.MeshResource] = new List<string>
            {
                "1248859D98C4FAC2",
            },
            [ResourceType.TextureResource] = new List<string>
            {
                "9A8D4BBD19B4CD55",
            },
            [ResourceType.VertexDefinitionResourceResource] = new List<string>
            {
                "6D98A5D596161190",
            },
            //[ResourceType.] = new List<string>()
            //{
            //     "051D3C49B715C035",
            //     "AB7BCCFC424D3617",
            //}
            //[ResourceType.] = new List<string>()
            //{
            //     "C166007BB09FD7DD",
            //},
            //[ResourceType.] = new List<string>()
            //{
            //     "C55DBE97C9562549",
            //},
            //[ResourceType.] = new List<string>()
            //{
            //     "6E6B3940043320E7",
            //},
            //[ResourceType.] = new List<string>()
            //{
            //     "65BD023A542C4933",
            //},
            //[ResourceType.] = new List<string>()
            //{
            //     "9C9957C43919B322",
            //},
            //[ResourceType.] = new List<string>()
            //{
            //     "79A83B132A1D6BE6",
            //},
            //[ResourceType.] = new List<string>()
            //{
            //     "6B843642D4731F54",
            //},
            //[ResourceType.] = new List<string>()
            //{
            //     "44CFE32795D22FB8",
            //},
            [ResourceType.TerrainRuntimeDecalTextureData] = new List<string>
            {
                "9882829EBEC65BF7",
            },
            [ResourceType.TerrainRuntimeTextureData] = new List<string>
            {
                "4A5A9CCBB88BC6A8",
                "0D35975A737B7740",
                "79B34FA59B2C7282",
                "024124019CCA5F13",
                "BAE152F9473BFD9E",
            },
            [ResourceType.TerrainRuntimeData] = new List<string>
            {
                "B725168F4D55F177",
                "C753E74EB6A75D30",
                "1C6A583E69BBBB63",
                "3AD97681339BE3F9",
                "F55AFE96DF07399A",
                "E60417205D24B485",
                "27B4B1C5BE5CB7E2",
                "A4309AAE24195D9F",
            },
            [ResourceType.TerrainSourceData] = new List<string>
            {
                "2CDABEE8BC4DFD3A",
                "F7F4BF1D96AE304B",
                "BFCFC0233EEB4828",
                "EC59C258E54EF2E2",
                "7E3CA64894C50744",
                "904B1504D2231CA3",
                "3900DF4491028AAB",
                "F6559F7ACC6B0FFA",
                "5247C0A7AADC4BAC",
                "3110ED031DA3FAF5",
                "C331ED9D8D072201",
                "B2317354D8CBEE44",
                "6653D0B621C48F71",
                "DCE772D42637897D",
                "6F97B6CF2D949F8E",
                "1CC505341F2922C1",
            },
            [ResourceType.ScriptCompiledBytecodeResource] = new List<string>
            {
                "C84707DA067146A9",
                "E6AC3244F1076F7B",
            },
            [ResourceType.ScriptMetadataResource] = new List<string>
            {
                "02575C46762A7C3C",
                "D97016058B281211",
                "0B604DC8C94BC188",
                "B8E35358A76FA32A",
                "D75DE17DF1892F86",
                "0A316EA155E30EDA",
            },
            [ResourceType.ScriptResource] = new List<string>
            {
                "152137D932762673",
                "A5E5740CAF72F738",
            },
            [ResourceType.hkaAnimationBindingResource] = new List<string>
            {
                "FCED9919CB285F6F",
            },
            [ResourceType.hkaSkeletonResource] = new List<string>
            {
                "B728547FDB2C4522",
            },
            [ResourceType.ScriptSourceTextResource] = new List<string>
            {
                "6301A7D31AA6F628",
                "DEDD8914F8DFE71E",
            },
            [ResourceType.hknpMaterialResource] = new List<string>
            {
                "161F13AFEC1887AD",
            },
            [ResourceType.hknpPhysicsSystemDataResource] = new List<string>
            {
                "E06EFC4882AF72E6",
            },
            [ResourceType.hkaSkeletonMapperResource] = new List<string>
            {
                "92C58B13B94629AA",
            },
            [ResourceType.hkbBehaviorGraphResource] = new List<string>
            {
                "8A45B4789B7B4BDA",
            },
            [ResourceType.hkbCharacterDataResource] = new List<string>
            {
                "E0027325AE4E853F",
            },
            [ResourceType.hkbProjectDataResource] = new List<string>
            {
                "1FD92E78456F6EB8",
            },
            [ResourceType.hknpRagdollDataResource] = new List<string>
            {
                "FD48F1A0E5BA9AE7",
            },
            [ResourceType.hknpShapeResource] = new List<string>
            {
                "A11E4B205F3213D8",
            },
            [ResourceType.hkpConstraintDataResource] = new List<string>
            {
                "07D6E3A87B889802",
            },
        };
    }
}
