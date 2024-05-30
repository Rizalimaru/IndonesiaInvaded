using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentCutSceneJakarta : MonoBehaviour
{

    public static EnvironmentCutSceneJakarta instance;

    public GameObject mainCamera;
    public GameObject cutSceneCamera;

    public GameObject cutSceneCameraPortal;

    public GameObject cutSceneCameraMonas;

    public GameObject[] gameObjectsOff;

    public Animator animasi;

    public Animator animasiPortal;

    [Header("Portal")]

    public GameObject portal;

    public GameObject portalMonas;

    [Header("CutsceneTrigger")]

    public int cutSceneJakarta = 0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        //Jika sedang play coroutine, jika player menekan tombol maka akan muncul tombol skip
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            CameraBack();
        }
        
    }

    public void CameraDelay()
    {
        CameraTrig();

        Invoke("CameraBack", 20);

    }

    private void CameraTrig()
    {
        animasi.SetTrigger("Cutscene");
        ScoreManager.instance.SetTimeUpdating(false);
        mainCamera.SetActive(false);
        cutSceneCamera.SetActive(true);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(false);
        }
    }

    //menambahkan 1 variable cutsceneJakarta
    public void CutSceneJakartaCount()
    {
        cutSceneJakarta++;
    }

    private void CameraBack()
    {
        mainCamera.SetActive(true);
        cutSceneCamera.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
        ScoreManager.instance.SetTimeUpdating(true);

    }

    private void CameraBackPortal()
    {
        mainCamera.SetActive(true);
        cutSceneCameraPortal.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
    }

    private void CameraBackMonas()
    {
        mainCamera.SetActive(true);
        cutSceneCameraMonas.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
    }

    public void CutScenePortal()
    {
        ScoreManager.instance.SetTimeUpdating(false);
        mainCamera.SetActive(false);
        cutSceneCameraPortal.SetActive(true);
        StartCoroutine(PortalDelay());


        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(false);
        }

        Invoke("CameraBackPortal", 2);
    }

    IEnumerator PortalDelay()
    {
        yield return new WaitForSeconds(1);
        portal.SetActive(true);
        yield return new WaitForSeconds(1);
        ScoreManager.instance.SetTimeUpdating(true);
    }

    public void CutSceneMonas()
    {
        ScoreManager.instance.SetTimeUpdating(false);
        mainCamera.SetActive(false);
        cutSceneCameraMonas.SetActive(true);
        StartCoroutine(MonasDelay());

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(false);
        }

        Invoke("CameraBackMonas", 4);
    }

    IEnumerator MonasDelay()
    {
        yield return new WaitForSeconds(2);
        portalMonas.SetActive(true);
        yield return new WaitForSeconds(2);
        ScoreManager.instance.SetTimeUpdating(true);
    }
}

