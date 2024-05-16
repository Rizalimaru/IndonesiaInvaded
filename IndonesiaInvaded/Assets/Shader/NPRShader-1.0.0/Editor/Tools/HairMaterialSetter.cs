using System.Collections.Generic;
using UnityEngine;

namespace HSR.NPRShader.Editor.Tools
{
    public class HairMaterialSetter : BaseMaterialSetter
    {
        protected override IReadOnlyDictionary<string, string> SupportedShaderMap => new Dictionary<string, string>()
        {
            ["miHoYo/CRP_Character/CharacterHair"] = "Honkai Star Rail/Character/Hair"
        };

        protected override IEnumerable<(string, MaterialInfo.TextureInfo)> ApplyTextures(IReadOnlyDictionary<string, MaterialInfo.TextureInfo> textures)
        {
            yield return ("_MainTex", textures["_MainTex"]);
            yield return ("_LightMap", textures["_LightMap"]);
            yield return ("_RampMapWarm", textures["_DiffuseRampMultiTex"]);
            yield return ("_RampMapCool", textures["_DiffuseCoolRampMultiTex"]);
        }

        protected override IEnumerable<(string, float)> ApplyFloats(IReadOnlyDictionary<string, float> floats)
        {
            yield return ("_Cull", floats["_CullMode"]);

            yield return ("_AlphaTest", floats["_EnableAlphaCutoff"]);
            yield return ("_AlphaTestThreshold", floats["_AlphaCutoff"]);

            yield return ("_EmissionThreshold", floats["_EmissionThreshold"]);
            yield return ("_EmissionIntensity", floats["_EmissionIntensity"]);

            yield return ("_mmBloomIntensity0", floats["_mBloomIntensity0"]);
        }

        protected override IEnumerable<(string, Color)> ApplyColors(IReadOnlyDictionary<string, Color> colors)
        {
            yield return ("_Color", colors["_Color"]);
            yield return ("_BackColor", colors["_BackColor"]);
            yield return ("_SpecularColor0", colors["_SpecularColor0"]);
            yield return ("_RimColor0", colors["_RimColor0"]);
            yield return ("_OutlineColor0", colors["_OutlineColor0"]);

            // Texture Scale Offset
            yield return ("_Maps_ST", colors["_MainMaps_ST"]);
        }
    }
}
