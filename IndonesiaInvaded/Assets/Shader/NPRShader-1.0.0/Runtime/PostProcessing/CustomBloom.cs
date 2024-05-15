using System;
using HSR.NPRShader.Passes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HSR.NPRShader.PostProcessing
{
    [Serializable]
    [VolumeComponentMenuForRenderPipeline("Honkai Star Rail/Bloom", typeof(UniversalRenderPipeline))]
    public class CustomBloom : VolumeComponent, IPostProcessComponent
    {
        public MinFloatParameter Intensity = new(0, 0);
        public MinFloatParameter Threshold = new(0.7f, 0);
        public ColorParameter Tint = new(Color.white, false, false, true);
        public ClampedIntParameter MipDownCount = new(2, 2, 4);
        public BoolParameter CharactersOnly = new(true, BoolParameter.DisplayType.EnumPopup);

        [Header("Blur First RT Size")]

        [DisplayInfo(name = "Width")] public ClampedIntParameter BlurFirstRTWidth = new(310, 100, 500);
        [DisplayInfo(name = "Height")] public ClampedIntParameter BlurFirstRTHeight = new(174, 100, 500);

        [Header("Blur Kernels")]

        public ClampedIntParameter KernelSize1 = new(4, 4, PostProcessPass.BloomMaxKernelSize);
        public ClampedIntParameter KernelSize2 = new(4, 4, PostProcessPass.BloomMaxKernelSize);
        public ClampedIntParameter KernelSize3 = new(6, 4, PostProcessPass.BloomMaxKernelSize);
        public ClampedIntParameter KernelSize4 = new(14, 4, PostProcessPass.BloomMaxKernelSize);

        public CustomBloom()
        {
            displayName = "HSR Bloom";
        }

        public bool IsActive() => Intensity.value > 0;

        public bool IsTileCompatible() => false;
    }
}
