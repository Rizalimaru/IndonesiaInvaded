using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    public static CutSceneController instance;
    public string cutSceneName = "";

    private void Awake()
    {
        instance = this;
    }
    public void CutScene()
    {
        if (CutSceneManager.Instance != null)
        {
            CutSceneManager.Instance.PlayCutScene(cutSceneName);
        }
        else
        {
            Debug.LogError("CutSceneManager instance is not available.");
        }
    }

}
