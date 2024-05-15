using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HSR.NPRShader.PostProcessing
{
    [Serializable]
    [VolumeComponentMenuForRenderPipeline("Honkai Star Rail/Tonemapping", typeof(UniversalRenderPipeline))]
    public class CustomTonemapping : VolumeComponent, IPostProcessComponent
    {
        public CustomTonemappingModeParameter Mode = new(CustomTonemappingMode.None);

        [Header("ACES Parameters")]

        [DisplayInfo(name = "Param A")] public FloatParameter ACESParamA = new(2.80f);
        [DisplayInfo(name = "Param B")] public FloatParameter ACESParamB = new(0.40f);
        [DisplayInfo(name = "Param C")] public FloatParameter ACESParamC = new(2.10f);
        [DisplayInfo(name = "Param D")] public FloatParameter ACESParamD = new(0.50f);
        [DisplayInfo(name = "Param E")] public FloatParameter ACESParamE = new(1.50f);

        public CustomTonemapping()
        {
            displayName = "HSR Tonemapping";
        }

        public bool IsActive() => Mode.value != CustomTonemappingMode.None;

        // 返回 true 的原因，请参考 Native Render Pass 的内容
        public bool IsTileCompatible() => true;
    }
}
