#ifndef _CHAR_DEPTH_ONLY_INCLUDED
#define _CHAR_DEPTH_ONLY_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "CharRenderingHelpers.hlsl"

struct CharDepthOnlyAttributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float2 uv1          : TEXCOORD0;
    float2 uv2          : TEXCOORD1;
};

struct CharDepthOnlyVaryings
{
    float4 positionHCS  : SV_POSITION;
    float3 normalWS     : NORMAL;
    float4 uv           : TEXCOORD0;
};

CharDepthOnlyVaryings CharDepthOnlyVertex(CharDepthOnlyAttributes i, float4 mapST)
{
    CharDepthOnlyVaryings o;

    o.positionHCS = TransformObjectToHClip(i.positionOS.xyz);
    o.normalWS = TransformObjectToWorldNormal(i.normalOS);
    o.uv = CombineAndTransformDualFaceUV(i.uv1, i.uv2, mapST);

    return o;
}

float4 CharDepthOnlyFragment(CharDepthOnlyVaryings i)
{
    return i.positionHCS.z;
}

#endif
