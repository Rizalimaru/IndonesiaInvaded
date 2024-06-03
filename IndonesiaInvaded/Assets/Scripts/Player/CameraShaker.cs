using UnityEngine;
using Cinemachine;
using System.Collections;
using System;

public class CameraShaker : MonoBehaviour
{   
    public static CameraShaker instance { get; private set; }
    private CinemachineVirtualCamera virtualCamera;
    private float ShakerTimer;

    private void Awake()
    {   
        instance =  this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void CameraShake(float intensity, float timer)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        ShakerTimer = timer;
    }

    private void Update()
    {
        if (ShakerTimer > 0)
        {
            ShakerTimer -= Time.deltaTime;
            if (ShakerTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    public static implicit operator CameraShaker(ThirdPersonCam v)
    {
        throw new NotImplementedException();
    }
}
