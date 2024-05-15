using System.Reflection;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HSR.NPRShader.Editor.Build
{
    internal class ShaderStripper : IShaderVariantStripper, IShaderVariantStripperScope
    {
        public bool active
        {
            get
            {
                UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
                if (asset == null)
                {
                    return false;
                }

                ScriptableRendererData[] rendererDataList = GetRendererDataList(asset);
                if (rendererDataList == null)
                {
                    return false;
                }

                foreach (ScriptableRendererData rendererData in rendererDataList)
                {
                    foreach (ScriptableRendererFeature rendererFeature in rendererData.rendererFeatures)
                    {
                        if (rendererFeature is StarRailRendererFeature && rendererFeature.isActive)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private static ScriptableRendererData[] GetRendererDataList(UniversalRenderPipelineAsset asset)
        {
            const string FieldName = "m_RendererDataList";
            const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance;
            FieldInfo field = asset.GetType().GetField(FieldName, Flags);
            return field.GetValue(asset) as ScriptableRendererData[];
        }

        private LocalKeyword m_MainLightShadowsScreen;

        public bool CanRemoveVariant(Shader shader, ShaderSnippetData shaderVariant, ShaderCompilerData shaderCompilerData)
        {
            // 强制保留 _MAIN_LIGHT_SHADOWS_SCREEN
            if (shaderCompilerData.shaderKeywordSet.IsEnabled(m_MainLightShadowsScreen))
            {
                return false;
            }

            // 剩下的靠 URP 默认的 Stripper
            // https://github.com/Unity-Technologies/Graphics/blob/master/Packages/com.unity.render-pipelines.universal/Editor/ShaderScriptableStripper.cs
            return true;
        }

        public void BeforeShaderStripping(Shader shader)
        {
            m_MainLightShadowsScreen = shader.keywordSpace.FindKeyword(ShaderKeywordStrings.MainLightShadowScreen);
        }

        public void AfterShaderStripping(Shader shader) { }
    }
}
