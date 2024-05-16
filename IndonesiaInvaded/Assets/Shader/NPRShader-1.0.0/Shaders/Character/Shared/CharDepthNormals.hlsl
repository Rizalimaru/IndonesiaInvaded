#ifndef _CHAR_DEPTH_NORMALS_INCLUDED
#define _CHAR_DEPTH_NORMALS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "CharRenderingHelpers.hlsl"

struct CharDepthNormalsAttributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float2 uv1          : TEXCOORD0;
    float2 uv2          : TEXCOORD1;
};

struct CharDepthNormalsVaryings
{
    float4 positionHCS  : SV_POSITION;
    float3 normalWS     : NORMAL;
    float4 uv           : TEXCOORD0;
};

CharDepthNormalsVaryings CharDepthNormalsVertex(CharDepthNormalsAttributes i, float4 mapST)
{
    CharDepthNormalsVaryings o;

    o.positionHCS = TransformObjectToHClip(i.positionOS.xyz);
    o.normalWS = TransformObjectToWorldNormal(i.normalOS);
    o.uv = CombineAndTransformDualFaceUV(i.uv1, i.uv2, mapST);

    return o;
}

float4 CharDepthNormalsFragment(CharDepthNormalsVaryings i)
{
    float3 normalWS = NormalizeNormalPerPixel(i.normalWS);
    return float4(normalWS, 0);
}

#endif
