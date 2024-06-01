using System.Collections;
using UnityEngine;

public class EnvironmentCutSceneJakarta : MonoBehaviour
{
    public static EnvironmentCutSceneJakarta instance;

    [Header("Camera")]
    public GameObject mainCamera;
    public GameObject playerCamera;
    public GameObject cutSceneCamera;
    public GameObject cutSceneCameraPortal;
    public GameObject cutSceneCameraMonas;
    public GameObject cutSceneBoss;

    [Header("GameObject and Animator")]
    public GameObject[] gameObjectsOff;
    public Animator animasi;
    public Animator animasiPortal;
    public Animator animator;

    [Header("Portal")]
    public GameObject portal;
    public GameObject portalMonas;

    [Header("CutsceneTrigger")]
    public int cutSceneJakarta = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            RestoreMainCamera();
        }
    }

    public void CameraDelay()
    {
        TriggerCamera();
        Invoke(nameof(RestoreMainCamera), 20);
    }

    private void TriggerCamera()
    {
        ScoreManager.instance.SetTimeUpdating(false);
        SetCameraState(mainCamera, false);
        SetCameraState(cutSceneCamera, true);
        animasi.SetTrigger("Cutscene");
        SetGameObjectsActive(false);
    }

    public void CutSceneJakartaCount()
    {
        cutSceneJakarta++;
    }

    private void RestoreMainCamera()
    {
        SetCameraState(mainCamera, true);
        SetCameraState(cutSceneCamera, false);
        SetGameObjectsActive(true);
        ScoreManager.instance.SetTimeUpdating(true);
    }

    private void RestorePortalCamera()
    {
        SetCameraState(mainCamera, true);
        SetCameraState(cutSceneCameraPortal, false);
        SetGameObjectsActive(true);
    }

    private void RestoreMonasCamera()
    {
        SetCameraState(mainCamera, true);
        SetCameraState(cutSceneCameraMonas, false);
        SetGameObjectsActive(true);
    }

    public void PlayCutScenePortal()
    {
        ScoreManager.instance.SetTimeUpdating(false);
        SetCameraState(mainCamera, false);
        SetCameraState(cutSceneCameraPortal, true);
        StartCoroutine(PortalDelay());
        SetGameObjectsActive(false);
        Invoke(nameof(RestorePortalCamera), 2);
    }

    private IEnumerator PortalDelay()
    {
        yield return new WaitForSeconds(1);
        portal.SetActive(true);
        yield return new WaitForSeconds(1);
        ScoreManager.instance.SetTimeUpdating(true);
    }

    public void PlayCutSceneMonas()
    {
        ScoreManager.instance.SetTimeUpdating(false);
        SetCameraState(mainCamera, false);
        SetCameraState(playerCamera, true);
        SetCameraState(cutSceneCameraMonas, true);
        StartCoroutine(MonasDelay());
        SetGameObjectsActive(false);
    }

    private IEnumerator MonasDelay()
    {
        yield return new WaitForSeconds(2);
        portalMonas.SetActive(true);
        yield return new WaitForSeconds(2);
        ScoreManager.instance.SetTimeUpdating(true);
    }

    public void PlayCutSceneBeforePortal()
    {
        animator.SetTrigger("End");
        ScoreManager.instance.SetTimeUpdating(false);
        SetCameraState(mainCamera, false);
        SetCameraState(playerCamera, false);
        SetCameraState(cutSceneBoss, true);
        SetGameObjectsActive(false);
        animator.SetTrigger("Start");
        Invoke(nameof(PlayCutSceneMonas), 26f);
        ScoreManager.instance.SetTimeUpdating(true);
    }

    private void SetCameraState(GameObject camera, bool state)
    {
        if (camera != null)
        {
            camera.SetActive(state);
        }
    }

    private void SetGameObjectsActive(bool state)
    {
        foreach (GameObject go in gameObjectsOff)
        {
            if (go != null)
            {
                go.SetActive(state);
            }
        }
    }
}
