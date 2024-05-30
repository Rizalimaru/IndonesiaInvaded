using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCutSceneInvert : MonoBehaviour
{

    public static EnvironmentCutSceneInvert instance;
    public GameObject mainCamera;
    public GameObject cutSceneCamera;

    public GameObject cutSceneCameraPortal;
    public GameObject[] gameObjectsOff;

    [Header("---------------Animator--------------")]
    public Animator animasi;
    public Animator animasiPortal;

    [Header("---------------Portal---------------")]

    public GameObject portal;

    [Header("CutsceneTrigger")]

    public int cutSceneInvert = 0;
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

    public IEnumerator CameraCutScene()
    {
        yield return new WaitForSeconds(0);

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

    public void CameraBack()
    {
        mainCamera.SetActive(true);
        cutSceneCamera.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
        ScoreManager.instance.SetTimeUpdating(true);

    }

    public void CutScenePortal(){
        animasiPortal.SetTrigger("Portal");
        ScoreManager.instance.SetTimeUpdating(false);
        mainCamera.SetActive(false);
        cutSceneCameraPortal.SetActive(true);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(false);
        }
    }

    private void CameraBackPortal()
    {
        animasi.SetTrigger("CutsceneBack");
        ScoreManager.instance.SetTimeUpdating(true);
        mainCamera.SetActive(true);
        cutSceneCamera.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
    }
}
