using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneTimelineManager : MonoBehaviour
{
    public static CutSceneTimelineManager Instance;

    [System.Serializable]
    public class TimelineCutScene
    {
        public string name;
        public PlayableAsset timelineAsset;
    }

    public List<TimelineCutScene> timelineCutScenes;
    private PlayableDirector playableDirector;
    public Camera cutSceneCamera;
    public bool IsCutScenePlaying { get; private set; }
    public delegate void CutSceneFinishedHandler();
    public event CutSceneFinishedHandler OnCutSceneFinished;

    private bool canSkipCutScene;
    private float holdTime;
    public float requiredHoldTime = 2.0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playableDirector = gameObject.AddComponent<PlayableDirector>();
        playableDirector.playOnAwake = false;
        playableDirector.stopped += OnPlayableDirectorStopped;

        if (cutSceneCamera != null)
        {
            cutSceneCamera.enabled = false;
        }
    }

    private void Update()
    {
        if (IsCutScenePlaying && canSkipCutScene)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                holdTime += Time.deltaTime;
                if (holdTime >= requiredHoldTime)
                {
                    SkipCutScene();
                }
            }
            else
            {
                holdTime = 0;
            }
        }
    }

    public void PlayCutScene(string cutSceneName)
    {
        TimelineCutScene cutScene = timelineCutScenes.Find(cs => cs.name == cutSceneName);
        if (cutScene != null)
        {
            if (cutSceneCamera != null)
            {
                cutSceneCamera.enabled = true;
            }
            playableDirector.playableAsset = cutScene.timelineAsset;
            IsCutScenePlaying = true;
            canSkipCutScene = true;
            Cursor.visible = false;
            playableDirector.Play();
        }
        else
        {
            Debug.LogError("Cutscene not found: " + cutSceneName);
        }
    }

    private void SkipCutScene()
    {
        playableDirector.Stop();
        OnPlayableDirectorStopped(playableDirector);  // Manually call the end method
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        TimelineCutScene cutScene = timelineCutScenes.Find(cs => cs.timelineAsset == director.playableAsset);
        if (cutScene != null)
        {
            Debug.Log("Cutscene finished: " + cutScene.name);
        }
        else
        {
            Debug.LogError("Cutscene not found: " + director.playableAsset.name);
        }

        // Disable the cutscene camera after the cutscene ends
        if (cutSceneCamera != null)
        {
            cutSceneCamera.enabled = false;
        }
        IsCutScenePlaying = false;
        canSkipCutScene = false;
        Cursor.visible = true;
        OnCutSceneFinished?.Invoke();
    }
}
