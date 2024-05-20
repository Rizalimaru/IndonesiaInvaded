using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Material skyboxMaterial; // Tambahkan ini untuk menerima material skybox
    public float rotationSpeed;

    void Start()
    {
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
        }
    }

    void Update()
    {
        if (RenderSettings.skybox != null)
        {
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
        }
    }
}
