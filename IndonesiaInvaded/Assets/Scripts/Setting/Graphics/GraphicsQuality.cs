using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class GraphicsQuality : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown fpsDropdown;

    //public Slider brightnessSlider;
    //public TMP_Text brightnessValueText;

    //private const string brightnessLevelKey = "BrightnessLevel";
    public const string qualityDropdownIndexKey = "qualityDropdown";
    public const string resolutionDropdownIndexKey = "resolutionDropdown";
    public const string fpsDropdownIndexKey = "fpsDropdown";

    // Membuat enum untuk menyimpan fps

    public enum fpslimits
    {
        FPS30 = 30,
        FPS45 = 45,
        FPS60 = 60,
        FPS120 = 120
    }
    void Start()
    {   
        // Menyimpan nilai kualitas pada dropdown quality ke dalam PlayerPrefs dengan key QualityDropdownIndexKey
        qualityDropdown.value = PlayerPrefs.GetInt(qualityDropdownIndexKey, 0);

        // Menyimpan nilai resolusi pada dropdown resolution ke dalam PlayerPrefs dengan key ResolutionDropdownIndexKey
        resolutionDropdown.value = PlayerPrefs.GetInt(resolutionDropdownIndexKey, 0);

        // Menyimpan nilai fps pada dropdown fps ke dalam PlayerPrefs dengan key FPSDropdownIndexKey
        fpsDropdown.value = PlayerPrefs.GetInt(fpsDropdownIndexKey, 0);

         /* Load nilai kecerahan dari PlayerPrefs saat memulai permainan
        float brightnessLevel = PlayerPrefs.GetFloat(brightnessLevelKey, 1f);
        SetBrightness(brightnessLevel);

        // Set nilai slider berdasarkan nilai kecerahan yang disimpan
        brightnessSlider.value = brightnessLevel;*/
    }

    public void SetFPS(int index)
    {
        switch (index)
        {
            case 0:
                SetFPSLimit(fpslimits.FPS30);
                break;
            case 1:
                SetFPSLimit(fpslimits.FPS45);
                break;
            case 2:
                SetFPSLimit(fpslimits.FPS60);
                break;
            default:
                SetFPSLimit(fpslimits.FPS30);
                break;
        }

        PlayerPrefs.SetInt(fpsDropdownIndexKey, index);
        PlayerPrefs.Save();
    }

    void SetFPSLimit(fpslimits limit)
    {
        Application.targetFrameRate = (int)limit;
    }

    public void SetQualityLevelDropdown(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt(qualityDropdownIndexKey, index);
        PlayerPrefs.Save();
        Debug.Log("Quality Level: " + index);
    }

    public void SetResolution(int index)
    {
        bool isFullScreen = true;

        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, isFullScreen);
                Debug.Log("Resolution: 1920x1080");
                break;
            case 1:
                isFullScreen = false;
                Screen.SetResolution(1920,1080, isFullScreen);
                Debug.Log("Resolution: 1920x1080 , windowed mode");
                break;
            case 2:
                Screen.SetResolution(1280, 720, isFullScreen);
                Debug.Log("Resolution: 1280x720");

                break;
            case 3:
                isFullScreen = false;
                Screen.SetResolution(1280, 720, isFullScreen);
                Debug.Log("Resolution: 1280x720, windowed mode");


                break;
        }

        PlayerPrefs.SetInt(resolutionDropdownIndexKey, index);
        PlayerPrefs.Save();
    }

    // public void SetBrightness(float value)
    // {

    //     Light[] lights = FindObjectsOfType<Light>();
    //     foreach (Light light in lights)
    //     {
    //         light.intensity = value;

    //         // Jika ada light yang menggunakan mode Realtime, atur mode tersebut ke Baked
    //         if (light.lightmapBakeType == LightmapBakeType.Realtime)
    //         {
    //             light.lightmapBakeType = LightmapBakeType.Baked;
    //         }

    //         // Jika ada light yang menggunakan mode Mixed, atur mode tersebut ke Baked
    //         if (light.lightmapBakeType == LightmapBakeType.Mixed)
    //         {
    //             light.lightmapBakeType = LightmapBakeType.Baked;
    //         }
    //     }

    //     // Tampilkan nilai kecerahan pada teks
    //     brightnessValueText.text = value.ToString("0.0");

    //     // Simpan nilai kecerahan ke PlayerPrefs
    //     PlayerPrefs.SetFloat(brightnessLevelKey, value);
    //     PlayerPrefs.Save();
    // }



}
