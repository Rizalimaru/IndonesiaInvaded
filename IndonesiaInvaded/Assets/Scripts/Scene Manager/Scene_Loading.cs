using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_Loading : MonoBehaviour
{
    public static Scene_Loading instance;

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

    public void LoadScenes()
    {
        loadingScreen.SetActive(true);

        loadingBarFill.value = 0;
        
        StartCoroutine(LoadScenesAsync());
    } 

    IEnumerator LoadScenesAsync()
    {
        List<AsyncOperation> scenes = new List<AsyncOperation>();

        // Sesuaikan indeks scene dengan indeks scene yang ingin Anda muat
        scenes.Add(SceneManager.LoadSceneAsync("Gameplay1"));
        scenes.Add(SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive));


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

        loadingScreen.SetActive(false);
    }

}
