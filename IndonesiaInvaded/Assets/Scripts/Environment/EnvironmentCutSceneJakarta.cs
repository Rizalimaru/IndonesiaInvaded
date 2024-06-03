using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentCutSceneJakarta : MonoBehaviour
{

    public static EnvironmentCutSceneJakarta instance;

    [Header("Camera")]
    public GameObject mainCamera;
    public GameObject cutSceneCamera;
    public GameObject cutSceneCameraPortal;
    public GameObject cutSceneCameraMonas;
    public GameObject cutSceneAfterPortal;
    public GameObject cutSceneBeforePortal;

    [Header("GameObject and Animator")]

    public GameObject[] gameObjectsOff;

    public Animator animasi;

    public Animator animasiPortal;

    [Header("Portal")]

    public GameObject portal;

    public GameObject portalMonas;

    [Header("CutsceneTrigger")]

    public int cutSceneJakarta = 0;
    // Start is called before the first frame update

    private void Awake()
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
        SetCursorVisibility(false);
        ScoreManager.instance.SetTimeUpdating(false);
        mainCamera.SetActive(false);
        cutSceneCamera.SetActive(true);
        animasi.SetTrigger("Cutscene");

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
        SetCursorVisibility(true);
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
        SetCursorVisibility(true);
        mainCamera.SetActive(true);
        cutSceneCameraPortal.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
    }

    private void CameraBackMonas()
    {
        SetCursorVisibility(true);
        mainCamera.SetActive(true);
        cutSceneCameraMonas.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
    }

    public void CutScenePortal()
    {
        SetCursorVisibility(false);
        ScoreManager.instance.SetTimeUpdating(false);
        mainCamera.SetActive(false);
        cutSceneCameraPortal.SetActive(true);
        StartCoroutine(PortalDelay());

        SkillManager.instance.ResetSkills();


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
        SetCursorVisibility(false);
        ScoreManager.instance.SetTimeUpdating(false);
        SkillManager.instance.ResetSkills();
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

    public void CutSceneAfterPortal()
    {
        SetCursorVisibility(false);
        ScoreManager.instance.SetTimeUpdating(false);
        mainCamera.SetActive(false);
        cutSceneAfterPortal.SetActive(true);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(false);
        }

        Invoke("CameraBackAfterPortal", 9);
    }
    private void CameraBackAfterPortal()
    {
        SetCursorVisibility(true);
        mainCamera.SetActive(true);
        cutSceneAfterPortal.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
    }

    private void CameraBackBeforePortal()
    {
        SetCursorVisibility(true);
        mainCamera.SetActive(true);
        cutSceneBeforePortal.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
    }

    public void CutSceneBeforePortal()
    {
        SetCursorVisibility(false);
        ScoreManager.instance.SetTimeUpdating(false);
        mainCamera.SetActive(false);
        cutSceneBeforePortal.SetActive(true);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(false);
        }

        Invoke("CameraBackBeforePortal", 26);
    }

    public void CutSceneBoss()
    {
        StartCoroutine(BossDelay());
    }

    IEnumerator BossDelay()
    {
        CutSceneBeforePortal();
        yield return new WaitForSeconds(26);
        CutSceneMonas();
    }

    private void SetCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

}

