Shader "Honkai Star Rail/Character/Body (Transparent)"
{
    Properties
    {
        [KeywordEnum(Game, MMD)] _Model("Model Type", Float) = 0
        _ModelScale("Model Scale", Float) = 1

        [HeaderFoldout(Shader Options)]
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 0                       // 默认 Off
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendColor("Src Blend (RGB)", Float) = 5  // 默认 SrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlendColor("Dst Blend (RGB)", Float) = 10 // 默认 OneMinusSrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendAlpha("Src Blend (A)", Float) = 0    // 默认 Zero
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlendAlpha("Dst Blend (A)", Float) = 0    // 默认 Zero
        [Header(Debug)] [Space(5)]
        [Toggle] _SingleMaterial("Show Part", Float) = 0
        [If(_SINGLEMATERIAL_ON)] [Indent] [IntRange] _SingleMaterialID("Material Index", Range(-1, 7)) = -1

        [HeaderFoldout(Maps)]
        [SingleLineTextureNoScaleOffset(_Color)] _MainTex("Albedo", 2D) = "white" {}
        [HideInInspector] _Color("Color", Color) = (1, 1, 1, 1)
        [SingleLineTextureNoScaleOffset] _LightMap("Light Map", 2D) = "white" {}
        [TextureScaleOffset] _Maps_ST("Maps Scale Offset", Vector) = (1, 1, 0, 0)
        [Header(Overrides)] [Space(5)]
        [If(_MODEL_GAME)] _BackColor("Back Face Color", Color) = (1, 1, 1, 1)
        [If(_MODEL_GAME)] [Toggle] _BackFaceUV2("Back Face Use UV2", Float) = 0

        [HeaderFoldout(Diffuse)]
        [RampTexture] _RampMapCool("Ramp (Cool)", 2D) = "white" {}
        [RampTexture] _RampMapWarm("Ramp (Warm)", 2D) = "white" {}
        _RampCoolWarmLerpFactor("Cool / Warm", Range(0, 1)) = 1

        [HeaderFoldout(Specular)]
        [HSRMaterialIDFoldout] _SpecularColor("Color", Float) = 0
        [HSRMaterialIDProperty(_SpecularColor, 0)] _SpecularColor0("Specular Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_SpecularColor, 31)] _SpecularColor1("Specular Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_SpecularColor, 63)] _SpecularColor2("Specular Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_SpecularColor, 95)] _SpecularColor3("Specular Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_SpecularColor, 127)] _SpecularColor4("Specular Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_SpecularColor, 159)] _SpecularColor5("Specular Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_SpecularColor, 192)] _SpecularColor6("Specular Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_SpecularColor, 223)] _SpecularColor7("Specular Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDFoldout] _SpecularMetallic("Metallic", Float) = 0
        [HSRMaterialIDProperty(_SpecularMetallic, 0)] _SpecularMetallic0("Specular Metallic", Range(0, 1)) = 0
        [HSRMaterialIDProperty(_SpecularMetallic, 31)] _SpecularMetallic1("Specular Metallic", Range(0, 1)) = 0
        [HSRMaterialIDProperty(_SpecularMetallic, 63)] _SpecularMetallic2("Specular Metallic", Range(0, 1)) = 0
        [HSRMaterialIDProperty(_SpecularMetallic, 95)] _SpecularMetallic3("Specular Metallic", Range(0, 1)) = 0
        [HSRMaterialIDProperty(_SpecularMetallic, 127)] _SpecularMetallic4("Specular Metallic", Range(0, 1)) = 1 // 一般情况下是金属
        [HSRMaterialIDProperty(_SpecularMetallic, 159)] _SpecularMetallic5("Specular Metallic", Range(0, 1)) = 0
        [HSRMaterialIDProperty(_SpecularMetallic, 192)] _SpecularMetallic6("Specular Metallic", Range(0, 1)) = 0
        [HSRMaterialIDProperty(_SpecularMetallic, 223)] _SpecularMetallic7("Specular Metallic", Range(0, 1)) = 0
        [HSRMaterialIDFoldout] _SpecularShininess("Shininess", Float) = 0
        [HSRMaterialIDProperty(_SpecularShininess, 0)] _SpecularShininess0("Specular Shininess", Range(0.1, 500)) = 10
        [HSRMaterialIDProperty(_SpecularShininess, 31)] _SpecularShininess1("Specular Shininess", Range(0.1, 500)) = 10
        [HSRMaterialIDProperty(_SpecularShininess, 63)] _SpecularShininess2("Specular Shininess", Range(0.1, 500)) = 10
        [HSRMaterialIDProperty(_SpecularShininess, 95)] _SpecularShininess3("Specular Shininess", Range(0.1, 500)) = 10
        [HSRMaterialIDProperty(_SpecularShininess, 127)] _SpecularShininess4("Specular Shininess", Range(0.1, 500)) = 10
        [HSRMaterialIDProperty(_SpecularShininess, 159)] _SpecularShininess5("Specular Shininess", Range(0.1, 500)) = 10
        [HSRMaterialIDProperty(_SpecularShininess, 192)] _SpecularShininess6("Specular Shininess", Range(0.1, 500)) = 10
        [HSRMaterialIDProperty(_SpecularShininess, 223)] _SpecularShininess7("Specular Shininess", Range(0.1, 500)) = 10
        [HSRMaterialIDFoldout] _SpecularIntensity("Intensity", Float) = 0
        [HSRMaterialIDProperty(_SpecularIntensity, 0)] _SpecularIntensity0("Specular Intensity", Range(0, 100)) = 1
        [HSRMaterialIDProperty(_SpecularIntensity, 31)] _SpecularIntensity1("Specular Intensity", Range(0, 100)) = 1
        [HSRMaterialIDProperty(_SpecularIntensity, 63)] _SpecularIntensity2("Specular Intensity", Range(0, 100)) = 1
        [HSRMaterialIDProperty(_SpecularIntensity, 95)] _SpecularIntensity3("Specular Intensity", Range(0, 100)) = 1
        [HSRMaterialIDProperty(_SpecularIntensity, 127)] _SpecularIntensity4("Specular Intensity", Range(0, 100)) = 1
        [HSRMaterialIDProperty(_SpecularIntensity, 159)] _SpecularIntensity5("Specular Intensity", Range(0, 100)) = 1
        [HSRMaterialIDProperty(_SpecularIntensity, 192)] _SpecularIntensity6("Specular Intensity", Range(0, 100)) = 1
        [HSRMaterialIDProperty(_SpecularIntensity, 223)] _SpecularIntensity7("Specular Intensity", Range(0, 100)) = 1
        [HSRMaterialIDFoldout] _SpecularEdgeSoftness("Edge Softness", Float) = 0
        [HSRMaterialIDProperty(_SpecularEdgeSoftness, 0)] _SpecularEdgeSoftness0("Specular Edge Softness", Range(0, 1)) = 0.1
        [HSRMaterialIDProperty(_SpecularEdgeSoftness, 31)] _SpecularEdgeSoftness1("Specular Edge Softness", Range(0, 1)) = 0.1
        [HSRMaterialIDProperty(_SpecularEdgeSoftness, 63)] _SpecularEdgeSoftness2("Specular Edge Softness", Range(0, 1)) = 0.1
        [HSRMaterialIDProperty(_SpecularEdgeSoftness, 95)] _SpecularEdgeSoftness3("Specular Edge Softness", Range(0, 1)) = 0.1
        [HSRMaterialIDProperty(_SpecularEdgeSoftness, 127)] _SpecularEdgeSoftness4("Specular Edge Softness", Range(0, 1)) = 0.1
        [HSRMaterialIDProperty(_SpecularEdgeSoftness, 159)] _SpecularEdgeSoftness5("Specular Edge Softness", Range(0, 1)) = 0.1
        [HSRMaterialIDProperty(_SpecularEdgeSoftness, 192)] _SpecularEdgeSoftness6("Specular Edge Softness", Range(0, 1)) = 0.1
        [HSRMaterialIDProperty(_SpecularEdgeSoftness, 223)] _SpecularEdgeSoftness7("Specular Edge Softness", Range(0, 1)) = 0.1

        [HeaderFoldout(Emission, Use Albedo.a as emission map)]
        _EmissionColor("Color", Color) = (1, 1, 1, 1)
        _EmissionThreshold("Threshold", Range(0, 1)) = 1
        _EmissionIntensity("Intensity", Float) = 0

        [HeaderFoldout(Bloom)]
        [HSRMaterialIDFoldout] _BloomIntensity("Intensity", Float) = 0
        [HSRMaterialIDProperty(_BloomIntensity, 0)] _mmBloomIntensity0("Bloom Intensity", Float) = 0
        [HSRMaterialIDProperty(_BloomIntensity, 31)] _mmBloomIntensity1("Bloom Intensity", Float) = 0
        [HSRMaterialIDProperty(_BloomIntensity, 63)] _mmBloomIntensity2("Bloom Intensity", Float) = 0
        [HSRMaterialIDProperty(_BloomIntensity, 95)] _mmBloomIntensity3("Bloom Intensity", Float) = 0
        [HSRMaterialIDProperty(_BloomIntensity, 127)] _mmBloomIntensity4("Bloom Intensity", Float) = 0
        [HSRMaterialIDProperty(_BloomIntensity, 159)] _mmBloomIntensity5("Bloom Intensity", Float) = 0
        [HSRMaterialIDProperty(_BloomIntensity, 192)] _mmBloomIntensity6("Bloom Intensity", Float) = 0
        [HSRMaterialIDProperty(_BloomIntensity, 223)] _mmBloomIntensity7("Bloom Intensity", Float) = 0
        [HSRMaterialIDFoldout] _BloomColor("Color", Float) = 0
        [HSRMaterialIDProperty(_BloomColor, 0)] _BloomColor0("Bloom Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_BloomColor, 31)] _BloomColor1("Bloom Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_BloomColor, 63)] _BloomColor2("Bloom Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_BloomColor, 95)] _BloomColor3("Bloom Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_BloomColor, 127)] _BloomColor4("Bloom Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_BloomColor, 159)] _BloomColor5("Bloom Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_BloomColor, 192)] _BloomColor6("Bloom Color", Color) = (1, 1, 1, 1)
        [HSRMaterialIDProperty(_BloomColor, 223)] _BloomColor7("Bloom Color", Color) = (1, 1, 1, 1)

        [HeaderFoldout(Outline)]
        [KeywordEnum(Tangent, Normal)] _OutlineNormal("Normal Source", Float) = 0
        _OutlineWidth("Width", Range(0, 4)) = 1
        _OutlineZOffset("Z Offset", Float) = 0
        [HSRMaterialIDFoldout] _OutlineColor("Color", Float) = 0
        [HSRMaterialIDProperty(_OutlineColor, 0)] _OutlineColor0("Outline Color", Color) = (0, 0, 0, 1)
        [HSRMaterialIDProperty(_OutlineColor, 31)] _OutlineColor1("Outline Color", Color) = (0, 0, 0, 1)
        [HSRMaterialIDProperty(_OutlineColor, 63)] _OutlineColor2("Outline Color", Color) = (0, 0, 0, 1)
        [HSRMaterialIDProperty(_OutlineColor, 95)] _OutlineColor3("Outline Color", Color) = (0, 0, 0, 1)
        [HSRMaterialIDProperty(_OutlineColor, 127)] _OutlineColor4("Outline Color", Color) = (0, 0, 0, 1)
        [HSRMaterialIDProperty(_OutlineColor, 159)] _OutlineColor5("Outline Color", Color) = (0, 0, 0, 1)
        [HSRMaterialIDProperty(_OutlineColor, 192)] _OutlineColor6("Outline Color", Color) = (0, 0, 0, 1)
        [HSRMaterialIDProperty(_OutlineColor, 223)] _OutlineColor7("Outline Color", Color) = (0, 0, 0, 1)

        [HeaderFoldout(Dither)]
        _DitherAlpha("Alpha", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "ComplexLit" // Packages/com.unity.render-pipelines.universal/Runtime/Passes/GBufferPass.cs: Fill GBuffer, but skip lighting pass for ComplexLit
            "Queue" = "Transparent"
        }

        Pass
        {
            Name "BodyTransparent"

            Tags
            {
                "LightMode" = "HSRTransparent"
            }

            // 透明部分和角色的 Stencil
            Stencil
            {
                Ref 5
                WriteMask 5  // 透明和角色位
                Comp Always
                Pass Replace // 写入透明和角色位
                Fail Keep
            }

            Cull [_Cull]
            ZWrite Off

            Blend 0 [_SrcBlendColor] [_DstBlendColor], [_SrcBlendAlpha] [_DstBlendAlpha]

            ColorMask RGBA 0

            HLSLPROGRAM

            #pragma vertex BodyVertex
            #pragma fragment BodyColorFragment

            #pragma shader_feature_local _MODEL_GAME _MODEL_MMD
            // #pragma shader_feature_local_fragment _ _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _SINGLEMATERIAL_ON
            #pragma shader_feature_local_fragment _ _BACKFACEUV2_ON

            #pragma multi_compile_fog

            #pragma multi_compile _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ _LIGHT_LAYERS
            #pragma multi_compile _ _FORWARD_PLUS

            #define CHAR_BODY_SHADER_TRANSPARENT
            #include "CharBodyCore.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "BodyOutline"

            Tags
            {
                "LightMode" = "HSROutline"
            }

            Stencil
            {
                Ref 5
                ReadMask 4    // 透明位
                WriteMask 1   // 角色位
                Comp NotEqual // 不透明部分
                Pass Replace  // 写入角色位
                Fail Keep
            }

            Cull Front
            ZTest LEqual
            ZWrite Off

            Blend 0 [_SrcBlendColor] [_DstBlendColor], [_SrcBlendAlpha] [_DstBlendAlpha]

            ColorMask RGBA 0

            HLSLPROGRAM

            #pragma vertex BodyOutlineVertex
            #pragma fragment BodyOutlineFragment

            #pragma shader_feature_local _MODEL_GAME _MODEL_MMD
            // #pragma shader_feature_local_fragment _ _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _SINGLEMATERIAL_ON
            #pragma shader_feature_local_fragment _ _BACKFACEUV2_ON

            #pragma shader_feature_local_vertex _OUTLINENORMAL_TANGENT _OUTLINENORMAL_NORMAL

            #pragma multi_compile_fog

            #define CHAR_BODY_SHADER_TRANSPARENT
            #include "CharBodyCore.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "BodyShadow"

            Tags
            {
                "LightMode" = "HSRPerObjectShadowCaster"
            }

            Cull [_Cull]
            ZWrite On // 写入 Shadow Map
            ZTest LEqual

            ColorMask 0

            HLSLPROGRAM

            #pragma target 2.0

            #pragma vertex BodyShadowVertex
            #pragma fragment BodyShadowFragment

            #pragma shader_feature_local _MODEL_GAME _MODEL_MMD
            // #pragma shader_feature_local_fragment _ _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _BACKFACEUV2_ON

            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #define CHAR_BODY_SHADER_TRANSPARENT
            #include "CharBodyCore.hlsl"

            ENDHLSL
        }

        // No Depth
        // No Motion Vectors
    }

    CustomEditor "StaloSRPShaderGUI"
    Fallback Off
}
