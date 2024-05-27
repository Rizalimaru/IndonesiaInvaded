using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutsceneCamera: MonoBehaviour
{
    [Header("Transform Camera")]
    public GameObject cameraEnt;
    public GameObject cameraPly;

    [Header("Animasi Trigger")]
    public Animator animasi;

    private void Awake()
    {
        GameObject bos = GameObject.Find("Boss");
        animasi = bos.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CameraTrig();
        gameObject.SetActive(false);
        Invoke("CameraBack", 3);

    }

    private void CameraTrig()
    {
        cameraEnt.SetActive(true);
        cameraPly.SetActive(false);
    }

    private void CameraBack()
    {
        cameraEnt.SetActive(false);
        cameraPly.SetActive(true);
    }
}