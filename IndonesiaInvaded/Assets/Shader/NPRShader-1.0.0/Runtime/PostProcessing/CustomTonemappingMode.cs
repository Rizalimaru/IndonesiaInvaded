using System;
using UnityEngine.Rendering;

namespace HSR.NPRShader.PostProcessing
{
    public enum CustomTonemappingMode
    {
        None = 0,
        ACES = 1,
    }

    [Serializable]
    public sealed class CustomTonemappingModeParameter : VolumeParameter<CustomTonemappingMode>
    {
        public CustomTonemappingModeParameter(CustomTonemappingMode value, bool overrideState = false)
            : base(value, overrideState) { }
    }
}
