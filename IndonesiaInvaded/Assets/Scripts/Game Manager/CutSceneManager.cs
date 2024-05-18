using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance;

    [System.Serializable]
    public class CutScene
    {
        public string name;
        public VideoClip videoClip;
    }

    public List<CutScene> cutScenes;
    private VideoPlayer videoPlayer;
    public Camera cutSceneCamera;
    public bool IsCutScenePlaying { get; private set; }
    public delegate void CutSceneFinishedHandler();
    public event CutSceneFinishedHandler OnCutSceneFinished;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCamera = cutSceneCamera;
        videoPlayer.loopPointReached += OnVideoFinished;

        if (cutSceneCamera != null)
        {
            cutSceneCamera.enabled = false;
        }
    }

    public void PlayCutScene(string cutSceneName)
    {
        CutScene cutScene = cutScenes.Find(cs => cs.name == cutSceneName);
        if (cutScene != null)
        {
            if (cutSceneCamera != null)
            {
                cutSceneCamera.enabled = true;
            }
            videoPlayer.clip = cutScene.videoClip;
            IsCutScenePlaying = true;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogError("Cutcene not found: " + cutSceneName);
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        CutScene cutScene = cutScenes.Find(cs => cs.videoClip == vp.clip);
        if (cutScene != null)
        {
            Debug.Log("Cutscene finished: " + cutScene.name);
        }
        else
        {
            Debug.LogError("Cutscene not found: " + vp.clip.name);
        }

        // Disable the cutscene camera after the cutscene ends
        if (cutSceneCamera != null)
        {
            cutSceneCamera.enabled = false;
        }
        IsCutScenePlaying = false;
        OnCutSceneFinished?.Invoke();
    }
}
