using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutsceneCamera: MonoBehaviour
{

    public static BossCutsceneCamera instance { get; private set; }
    
    [Header("Transform Camera")]
    public GameObject cameraEnt;
    public GameObject cameraPly;

    [Header("Object Off")]

    public GameObject[] objectOff;

    private void Awake()
    {
        GameObject bos = GameObject.Find("Boss");
        instance = this;
    }
    /**
    private void OnTriggerEnter(Collider other)
    {
        TriggerSpawnBoss();
    }
    **/
    public void TriggerSpawnBoss()
    {
        Invoke("CameraTrig", 0.1f);
        gameObject.SetActive(false);
        Invoke("CameraBack", 3);
    }

    private void CameraTrig()
    {
        foreach (GameObject obj in objectOff)
        {
            obj.SetActive(false);
        }
        cameraEnt.SetActive(true);
        cameraPly.SetActive(false);
    }

    private void CameraBack()
    {
        foreach (GameObject obj in objectOff)
        {
            obj.SetActive(true);
        }
        cameraEnt.SetActive(false);
        cameraPly.SetActive(true);
    }
}