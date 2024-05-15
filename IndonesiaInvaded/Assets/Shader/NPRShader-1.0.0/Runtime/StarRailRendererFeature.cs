using System;
using HSR.NPRShader.Passes;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HSR.NPRShader
{
    [DisallowMultipleRendererFeature("Honkai Star Rail")]
    public class StarRailRendererFeature : ScriptableRendererFeature
    {
#if UNITY_EDITOR
        [UnityEditor.ShaderKeywordFilter.ApplyRulesIfNotGraphicsAPI(GraphicsDeviceType.OpenGLES2)]
        [UnityEditor.ShaderKeywordFilter.SelectIf(true, keywordNames: ShaderKeywordStrings.MainLightShadowScreen)]
        private const bool k_RequiresScreenSpaceShadowsKeyword = true;
#endif

        [NonSerialized] private MainLightPerObjectShadowCasterPass m_MainLightPerObjShadowPass;
        [NonSerialized] private RequestResourcePass m_ForceDepthPrepassPass;
        [NonSerialized] private ScreenSpaceShadowsPass m_ScreenSpaceShadowPass;
        [NonSerialized] private ScreenSpaceShadowsPostPass m_ScreenSpaceShadowPostPass;
        [NonSerialized] private ForwardDrawObjectsPass m_DrawOpaqueForward1Pass;
        [NonSerialized] private ForwardDrawObjectsPass m_DrawOpaqueForward2Pass;
        [NonSerialized] private ForwardDrawObjectsPass m_DrawOpaqueForward3Pass;
        [NonSerialized] private ForwardDrawObjectsPass m_DrawOpaqueOutlinePass;
        [NonSerialized] private ForwardDrawObjectsPass m_DrawTransparentPass;
        [NonSerialized] private PostProcessPass m_PostProcessPass;

        public override void Create()
        {
            m_MainLightPerObjShadowPass = new MainLightPerObjectShadowCasterPass();
            m_ForceDepthPrepassPass = new RequestResourcePass(RenderPassEvent.AfterRenderingGbuffer, ScriptableRenderPassInput.Depth);
            m_ScreenSpaceShadowPass = new ScreenSpaceShadowsPass();
            m_ScreenSpaceShadowPostPass = new ScreenSpaceShadowsPostPass();
            m_DrawOpaqueForward1Pass = new ForwardDrawObjectsPass("DrawStarRailOpaque (1)", true,
                new ShaderTagId("HSRForward1"));
            m_DrawOpaqueForward2Pass = new ForwardDrawObjectsPass("DrawStarRailOpaque (2)", true,
                new ShaderTagId("HSRForward2"));
            m_DrawOpaqueForward3Pass = new ForwardDrawObjectsPass("DrawStarRailOpaque (3)", true,
                new ShaderTagId("HSRForward3"));
            m_DrawOpaqueOutlinePass = new ForwardDrawObjectsPass("DrawStarRailOpaque (Outline)", true,
                new ShaderTagId("HSROutline"));
            m_DrawTransparentPass = new ForwardDrawObjectsPass("DrawStarRailTransparent", false,
                new ShaderTagId("HSRTransparent"), new ShaderTagId("HSROutline"));
            m_PostProcessPass = new PostProcessPass();
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            // AfterRenderingShadows
            renderer.EnqueuePass(m_MainLightPerObjShadowPass);

            // AfterRenderingGbuffer
            renderer.EnqueuePass(m_ForceDepthPrepassPass); // 保证 RimLight、眼睛等需要深度图的效果正常工作
            renderer.EnqueuePass(m_ScreenSpaceShadowPass);

            // AfterRenderingOpaques
            renderer.EnqueuePass(m_ScreenSpaceShadowPostPass);
            renderer.EnqueuePass(m_DrawOpaqueForward1Pass);
            renderer.EnqueuePass(m_DrawOpaqueForward2Pass);
            renderer.EnqueuePass(m_DrawOpaqueForward3Pass);
            renderer.EnqueuePass(m_DrawOpaqueOutlinePass);

            // AfterRenderingTransparents
            renderer.EnqueuePass(m_DrawTransparentPass);

            // BeforeRenderingPostProcessing
            renderer.EnqueuePass(m_PostProcessPass);
        }

        // public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        // {
        //     // PreviewCamera 不会执行这部分代码！！！
        //
        //     base.SetupRenderPasses(renderer, in renderingData);
        // }

        protected override void Dispose(bool disposing)
        {
            m_MainLightPerObjShadowPass.Dispose();
            m_ScreenSpaceShadowPass.Dispose();
            m_PostProcessPass.Dispose();

            base.Dispose(disposing);
        }
    }
}
