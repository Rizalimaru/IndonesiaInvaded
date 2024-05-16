using System.Collections.Generic;
using UnityEngine;

namespace HSR.NPRShader.Editor.Tools
{
    public class FaceMaterialSetter : BaseMaterialSetter
    {
        protected override IReadOnlyDictionary<string, string> SupportedShaderMap => new Dictionary<string, string>()
        {
            ["miHoYo/CRP_Character/CharacterFace"] = "Honkai Star Rail/Character/Face"
        };

        protected override IEnumerable<(string, MaterialInfo.TextureInfo)> ApplyTextures(IReadOnlyDictionary<string, MaterialInfo.TextureInfo> textures)
        {
            yield return ("_MainTex", textures["_MainTex"]);
            yield return ("_FaceMap", textures["_FaceMap"]);
            yield return ("_ExpressionMap", textures["_FaceExpression"]);
        }

        protected override IEnumerable<(string, float)> ApplyFloats(IReadOnlyDictionary<string, float> floats)
        {
            if (floats.TryGetValue("_UseUVChannel2", out float useUV2))
            {
                yield return ("_FaceMapUV2", useUV2);
            }

            yield return ("_EmissionThreshold", floats["_EmissionThreshold"]);
            yield return ("_EmissionIntensity", floats["_EmissionIntensity"]);

            yield return ("_NoseLinePower", floats["_NoseLinePower"]);

            yield return ("_mmBloomIntensity0", floats["_mBloomIntensity0"]);
        }

        protected override IEnumerable<(string, Color)> ApplyColors(IReadOnlyDictionary<string, Color> colors)
        {
            yield return ("_Color", colors["_Color"]);
            yield return ("_ShadowColor", colors["_ShadowColor"]);
            yield return ("_EyeShadowColor", colors["_EyeShadowColor"]);
            yield return ("_EmissionColor", Color.white); // 眼睛高亮
            yield return ("_OutlineColor0", colors["_OutlineColor"]);
            yield return ("_NoseLineColor", colors["_NoseLineColor"]);

            // Texture Scale Offset
            yield return ("_Maps_ST", colors["_MainMaps_ST"]);

            // Expression
            yield return ("_ExCheekColor", colors["_ExCheekColor"]);
            yield return ("_ExShyColor", colors["_ExShyColor"]);
            yield return ("_ExShadowColor", colors["_ExShadowColor"]);
            yield return ("_ExEyeColor", colors["_ExEyeColor"]);
        }
    }
}
