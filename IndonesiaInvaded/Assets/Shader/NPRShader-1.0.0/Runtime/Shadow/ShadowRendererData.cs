using UnityEngine;

namespace HSR.NPRShader.Shadow
{
    public readonly struct ShadowRendererData
    {
        public readonly Renderer Renderer;
        public readonly Material Material;
        public readonly int SubmeshIndex;
        public readonly int ShaderPass;

        public ShadowRendererData(Renderer renderer, Material material, int submeshIndex, int shaderPass)
        {
            Renderer = renderer;
            Material = material;
            SubmeshIndex = submeshIndex;
            ShaderPass = shaderPass;
        }
    }
}
