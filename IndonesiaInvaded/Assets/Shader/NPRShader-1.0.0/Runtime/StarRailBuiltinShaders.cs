using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HSR.NPRShader
{
    public static class StarRailBuiltinShaders
    {
        public const string BloomShader = "Hidden/Honkai Star Rail/Post Processing/Bloom";
        public const string UberPostShader = "Hidden/Honkai Star Rail/Post Processing/UberPost";
        public const string ScreenSpaceShadowsShader = "Hidden/Honkai Star Rail/Shadow/ScreenSpaceShadows";

        public static IEnumerable<Shader> Walk()
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static;
            foreach (FieldInfo field in typeof(StarRailBuiltinShaders).GetFields(bindingFlags))
            {
                string shaderName = (string)field.GetRawConstantValue();
                yield return Shader.Find(shaderName);
            }
        }
    }
}
