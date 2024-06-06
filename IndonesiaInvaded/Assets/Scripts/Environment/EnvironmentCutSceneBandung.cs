using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentCutSceneBandung : MonoBehaviour
{

    public static EnvironmentCutSceneBandung instance { get; private set; }

    [Header("Camera")]
    public GameObject mainCamera;
    public GameObject cutSceneCameraPortal;

    [Header("GameObject")]

    public GameObject[] gameObjectsOff;

    [Header("Portal")]

    public GameObject portal;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CameraDelay()
    {
        Invoke("CameraBack", 20);
    }

    private void CameraBackPortal()
    {
        SetCursorVisibility(false);
        mainCamera.SetActive(true);
        cutSceneCameraPortal.SetActive(false);

        foreach (GameObject go in gameObjectsOff)
        {
            go.SetActive(true);
        }
    }

    public void CutScenePortal()
    {
        SkillManager.instance.ResetSkills();
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

    
    private void SetCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

}

