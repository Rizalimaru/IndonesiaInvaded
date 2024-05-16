using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

namespace HSR.NPRShader.Passes
{
    public class ForwardDrawObjectsPass : DrawObjectsPass
    {
        public ForwardDrawObjectsPass(string profilerTag, bool isOpaque, params ShaderTagId[] shaderTagIds)
            : this(profilerTag, isOpaque,
                // 放在最后绘制，这样就不需要清理被挡住的角色的 Stencil
                isOpaque ? RenderPassEvent.AfterRenderingOpaques : RenderPassEvent.AfterRenderingTransparents,
                shaderTagIds) { }

        public ForwardDrawObjectsPass(string profilerTag, bool isOpaque, RenderPassEvent evt, params ShaderTagId[] shaderTagIds)
            : this(profilerTag, isOpaque, -1, evt, shaderTagIds) { }

        public ForwardDrawObjectsPass(string profilerTag, bool isOpaque, LayerMask layerMask, RenderPassEvent evt, params ShaderTagId[] shaderTagIds)
            : base(profilerTag, shaderTagIds, isOpaque, evt,
                isOpaque ? RenderQueueRange.opaque : RenderQueueRange.transparent,
                layerMask, new StencilState(), 0) { }
    }
}
