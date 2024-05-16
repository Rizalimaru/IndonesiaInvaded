using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HSR.NPRShader.Passes
{
    public class RequestResourcePass : ScriptableRenderPass
    {
        public RequestResourcePass(RenderPassEvent evt, ScriptableRenderPassInput passInput)
        {
            renderPassEvent = evt;
            ConfigureInput(passInput);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) { }
    }
}
