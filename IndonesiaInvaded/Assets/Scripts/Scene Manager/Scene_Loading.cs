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
        GameManager.instance.SaveGame();
        StartCoroutine(MainMenu());
    }
    public void LoadScenes()
    {
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        UI_AnimatorUI.instance.LoadGameAnimation();

        StartCoroutine(LoadScenesAsync());
    } 
    public void LoadScenes2()
    {
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        UI_AnimatorUI.instance.LoadGameAnimation();
        
        StartCoroutine(LoadLevel2());
    } 
    public void LoadScenes3()
    {
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        UI_AnimatorUI.instance.LoadGameAnimation();
        
        StartCoroutine(LoadLevel3());
    } 
    IEnumerator LoadScenesAsync()
    {
        yield return new WaitForSeconds(1f);

        loadingScreen.SetActive(true);
        loadingBarFill.value = 0;

        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        UI_AnimatorUI.instance.LoadGameAnimation();

        yield return new WaitForSeconds(0.5f);
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
        yield return new WaitForSeconds(1f);

        loadingScreen.SetActive(true);
        loadingBarFill.value = 0;

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
        yield return new WaitForSeconds(1f);

        loadingScreen.SetActive(true);
        loadingBarFill.value = 0;
        
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
    IEnumerator MainMenu()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(0);
        animator.SetTrigger("Start");
    }
}
