using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_Loading : MonoBehaviour
{
    public static Scene_Loading instance;

    [Header("Scene Animator")]
    [SerializeField] Animator animator;

    [Header("UI Animator")]
    public GameObject loadingScreen;
    public Slider loadingBarFill;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadScene("Mainmenu"));
    }
    public void LoadSceneByName(string gameplayScene, string levelScene, string bgMusic)
    {
        loadingScreen.SetActive(true);
        loadingBarFill.value = 0;
        StartCoroutine(LoadScenesAsync(new List<string> { gameplayScene, levelScene }, bgMusic));
    }
    private IEnumerator LoadScenesAsync(List<string> scenesToLoad, string bgMusic)
    {
        AudioManager.Instance.StopAllBackgroundMusic();
        List<AsyncOperation> scenes = new List<AsyncOperation>();

        foreach (var sceneName in scenesToLoad)
        {
            scenes.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
        }

        while (!AllScenesLoaded(scenes))
        {
            loadingBarFill.value = CalculateProgress(scenes);
            yield return null;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.UnloadSceneAsync(0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.UnloadSceneAsync(2);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.UnloadSceneAsync(4);
        }

        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(false);
        ScoreManager.instance.ResetAllValues();
        AudioManager.Instance.PlayBackgroundMusicWithTransition(bgMusic, 0, 1f);
    }

    private IEnumerator LoadScene(string sceneName)
    {
        AudioManager.Instance.StopAllBackgroundMusic();
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1f);

        loadingScreen.SetActive(true);
        loadingBarFill.value = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);

        while (!scene.isDone)
        {
            loadingBarFill.value = scene.progress;
            yield return null;
        }

        loadingScreen.SetActive(false);
        animator.SetTrigger("Start");
    }

    private bool AllScenesLoaded(List<AsyncOperation> scenes)
    {
        foreach (var scene in scenes)
        {
            if (!scene.isDone)
                return false;
        }
        return true;
    }

    private float CalculateProgress(List<AsyncOperation> scenes)
    {
        float progress = 0;
        foreach (var scene in scenes)
        {
            progress += scene.progress;
        }
        return progress / scenes.Count;
    }

}
