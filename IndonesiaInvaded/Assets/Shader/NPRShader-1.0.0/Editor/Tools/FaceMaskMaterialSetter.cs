using System.Collections.Generic;
using UnityEngine;

namespace HSR.NPRShader.Editor.Tools
{
    public class FaceMaskMaterialSetter : BaseMaterialSetter
    {
        protected override IReadOnlyDictionary<string, string> SupportedShaderMap => new Dictionary<string, string>()
        {
            ["miHoYo/CRP_Character/Character Stencil Clear"] = "Honkai Star Rail/Character/FaceMask"
        };

        protected override IEnumerable<(string, Color)> ApplyColors(IReadOnlyDictionary<string, Color> colors)
        {
            yield return ("_Color", colors["_Color"]);
        }
    }
}
