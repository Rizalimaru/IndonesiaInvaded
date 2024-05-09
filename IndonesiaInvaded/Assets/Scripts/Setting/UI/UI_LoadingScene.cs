using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UI_LoadingScene : MonoBehaviour
{
    public enum SceneName
    {
        Gameplay,
        Level1,
        Level2,
        Level3,
    }
    public static UI_LoadingScene instance;

    public GameObject loadingScreen;
    public GameObject mainMenu;
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

    public void LoadLevelAsync(SceneName level)
    {
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);

        loadingBarFill.value = 0;
        LoadScenesAsync(level);
    }
    IEnumerator LoadScenesAsync(SceneName level)
    {
        List<AsyncOperation> scenes = new List<AsyncOperation>();

        // Load the gameplay scene
        scenes.Add(SceneManager.LoadSceneAsync(SceneName.Gameplay.ToString()));

        // Load the level scene
        scenes.Add(SceneManager.LoadSceneAsync(level.ToString(), LoadSceneMode.Additive));

        // Wait for all scenes to load
        while (scenes.Any(scene => !scene.isDone))
        {
            float progress = scenes.Average(scene => scene.progress);
            loadingBarFill.value = progress;
            yield return null;
        }
    }

    public void LoadScenes()
    {
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);

        loadingBarFill.value = 0;
        StartCoroutine(LoadScenesAsync());
    }

    IEnumerator LoadScenesAsync()
    {

        List<AsyncOperation> scenes = new List<AsyncOperation>();

        scenes.Add(SceneManager.LoadSceneAsync("Gameplay"));
        scenes.Add(SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive));

        foreach (var scene in scenes)
        {
            while (!scene.isDone)
            {
                float progress = 0;
                foreach (var s in scenes)
                {
                    progress += s.progress;
                }
                progress /= scenes.Count;
                loadingBarFill.value = progress;
                yield return null;
            }
        }

        // Tunggu sedikit waktu tambahan sebelum menonaktifkan layar loading
        yield return new WaitForSeconds(1f);

        loadingScreen.SetActive(false);
    }
}
