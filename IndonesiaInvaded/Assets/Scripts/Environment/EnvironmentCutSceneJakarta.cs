using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCutSceneJakarta : MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject cutSceneCamera;

    public GameObject[] gameObjectsOff;

    public Animator animasi;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CameraDelay());

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CameraDelay()
    {
        yield return new WaitForSeconds(2);

        CameraTrig();

        Invoke("CameraBack", 20);

    }

    private void CameraTrig()
    {
        animasi.SetTrigger("Cutscene");
        mainCamera.SetActive(false);
        cutSceneCamera.SetActive(true);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(false);
        }
    }

    private void CameraBack()
    {
        mainCamera.SetActive(true);
        cutSceneCamera.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
    }
}

