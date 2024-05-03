using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_LoadingScene : MonoBehaviour
{
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

    public void LoadScenes()
    {
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);

        loadingBarFill.value = 0;
        StartCoroutine(LoadScenesAsync());
    }

    IEnumerator LoadScenesAsync()
    {
        AudioManager.instance.StopBackgroundMusicWithTransition("MainMenu", 1f);
        List<AsyncOperation> scenes = new List<AsyncOperation>();



        // Sesuaikan indeks scene dengan indeks scene yang ingin Anda muat
        scenes.Add(SceneManager.LoadSceneAsync("Gameplay"));
        scenes.Add(SceneManager.LoadSceneAsync("BlockoutJakarta", LoadSceneMode.Additive));


        // Tunggu hingga semua scene dimuat
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
