using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMainMenuManager : MonoBehaviour
{
    public static SceneMainMenuManager instance;
    [Header("Scene Animator")]
    [SerializeField] Animator animator;
    [Header("UI Animator")]
    public GameObject loadingScreen;
    public Slider loadingBarFill;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        GameManager.instance.SaveGame();
        StartCoroutine(Mainmenu());
    }

    IEnumerator Mainmenu()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(0);
        animator.SetTrigger("Start");
    }

    public void LoadGame()
    {
        StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(1);
        yield return new WaitForSeconds(1);
        animator.SetTrigger("Start");

        
    }
    
    public void LoadScenes()
    {
        loadingScreen.SetActive(true);

        loadingBarFill.value = 0;
        
        StartCoroutine(LoadScenesAsync());
    } 
    public void LoadScenes2()
    {
        loadingScreen.SetActive(true);

        loadingBarFill.value = 0;
        
        StartCoroutine(LoadLevel2());
    } 
    public void LoadScenes3()
    {
        loadingScreen.SetActive(true);

        loadingBarFill.value = 0;
        
        StartCoroutine(LoadLevel3());
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
        yield return new WaitForSeconds(1f);

        loadingScreen.SetActive(false);
    }
    IEnumerator LoadLevel2()
    {
        List<AsyncOperation> scenes = new List<AsyncOperation>();

        // Sesuaikan indeks scene dengan indeks scene yang ingin Anda muat
        scenes.Add(SceneManager.LoadSceneAsync("Gameplay2"));
        scenes.Add(SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive));


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
    IEnumerator LoadLevel3()
    {
        List<AsyncOperation> scenes = new List<AsyncOperation>();

        // Sesuaikan indeks scene dengan indeks scene yang ingin Anda muat
        scenes.Add(SceneManager.LoadSceneAsync("Gameplay3"));
        scenes.Add(SceneManager.LoadSceneAsync("Level3", LoadSceneMode.Additive));


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
