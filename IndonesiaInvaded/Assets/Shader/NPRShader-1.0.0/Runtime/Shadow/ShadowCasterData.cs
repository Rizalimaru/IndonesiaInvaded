using System;
using System.Collections.Generic;
using UnityEngine;

namespace HSR.NPRShader.Shadow
{
    public struct ShadowCasterData : IComparable<ShadowCasterData>
    {
        internal float Priority;
        public Matrix4x4 ViewMatrix;
        public Matrix4x4 ProjectionMatrix;
        public IReadOnlyList<ShadowRendererData> ShadowRenderers;

        public int CompareTo(ShadowCasterData other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
}
