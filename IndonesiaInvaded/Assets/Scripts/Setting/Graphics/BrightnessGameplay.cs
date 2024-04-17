using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class BrightnessGameplay : MonoBehaviour
{
    public Slider brightnessSlider;
    public PostProcessProfile brightness;
    public PostProcessLayer layer;

    private const string brightnessLevelKey = "BrightnessLevel";

    AutoExposure exposure;

    void Start()
    {
        brightness.TryGetSettings(out exposure);
        float savedBrightness = PlayerPrefs.GetFloat(brightnessLevelKey, 0.5f); // Default value of 0.5 if not found
        brightnessSlider.value = savedBrightness;
        AdjustBrightness(savedBrightness);
    }

    public void AdjustBrightness(float value)
    {
        if (exposure != null)
        {
            if (value != 0)
            {
                exposure.keyValue.value = value;
            }
            else
            {
                exposure.keyValue.value = 0.05f;
            }

            PlayerPrefs.SetFloat(brightnessLevelKey, value);
            PlayerPrefs.Save();
        }
    }
}
