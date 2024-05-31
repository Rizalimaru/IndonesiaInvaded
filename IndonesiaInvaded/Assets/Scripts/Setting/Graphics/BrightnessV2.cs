using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BrightnessV2 : MonoBehaviour
{
    public VolumeProfile volumeProfile;
    public Slider brightnessSlider;
    private ColorAdjustments colorAdjustments;
    private const string brightnessLevelKey = "BrightnessLevel";

    void Start()
    {
        if (!volumeProfile.TryGet(out colorAdjustments))
        {
            colorAdjustments = volumeProfile.Add<ColorAdjustments>();
        }
        // Aktifkan efek Color Adjustments
        colorAdjustments.active = true;

        float savedBrightness = PlayerPrefs.GetFloat(brightnessLevelKey, 0f);

        // Atur nilai slider berdasarkan nilai yang disimpan sebelumnya
        brightnessSlider.value = savedBrightness;

        ChangeBrightness(savedBrightness);

        // Tambahkan listener untuk slider
        brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
    }

    void ChangeBrightness(float value)
    {
        // Atur nilai kecerahan
        SetBrightness(value);

        // Simpan nilai kecerahan
        PlayerPrefs.SetFloat(brightnessLevelKey, value);
        PlayerPrefs.Save();
    }

    void SetBrightness(float value)
    {
        // Konversi nilai slider ke dalam rentang [-1, 1]
        float adjustedValue = Remap(value, brightnessSlider.minValue, brightnessSlider.maxValue, -1.5f, 0.5f);

        // Atur nilai kecerahan
        colorAdjustments.postExposure.value = adjustedValue;
    }
    
    // Metode untuk mengubah rentang nilai
    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return from2 + (value - from1) / (to1 - from1) * (to2 - from2);
    }
}
