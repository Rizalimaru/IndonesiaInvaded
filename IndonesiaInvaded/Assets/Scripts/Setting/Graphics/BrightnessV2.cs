using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BrightnessV2 : MonoBehaviour
{
    //variabel directional light
    public Light directionalLight;
    public Slider brightnessSlider;
    //private ColorAdjustments colorAdjustments;
    private const string brightnessLevelKey = "BrightnessLevel";

    void Start()
    {

        if (directionalLight == null)
        {
            // Jika directional light tidak diatur, tidak akan ada print error
            Debug.LogError("Directional light is not set.");
        }

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
        // Atur nilai kecerahan
        directionalLight.intensity = Remap(value, 0.2f, 1.5f, 0.2f, 1.5f);
    }
    
    // Metode untuk mengubah rentang nilai
    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return from2 + (value - from1) / (to1 - from1) * (to2 - from2);
    }
}
