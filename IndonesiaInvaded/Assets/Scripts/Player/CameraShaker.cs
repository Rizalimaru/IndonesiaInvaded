using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    // Durasi dan intensitas getaran
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 1f;

    // Reference ke Cinemachine Virtual Camera
    public CinemachineVirtualCamera virtualCamera;

    // Method untuk memicu getaran pada kamera
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShakeCamera();
        }
    }
    public void ShakeCamera()
    {
        // Membuat instance NoiseSettings
        var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Mengatur durasi dan intensitas getaran
        noise.m_AmplitudeGain = shakeIntensity;
        noise.m_FrequencyGain = shakeDuration;

        // Menjalankan coroutine untuk menghentikan getaran setelah durasi tertentu
        StartCoroutine(StopShaking());
    }

    // Coroutine untuk menghentikan getaran setelah durasi tertentu
    private IEnumerator StopShaking()
    {
        yield return new WaitForSeconds(shakeDuration);
        
        // Menghentikan getaran dengan mengatur intensitas dan frekuensi menjadi 0
        var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
