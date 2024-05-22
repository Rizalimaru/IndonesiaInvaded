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
        loadingScreen.SetActive(true);
        // Stop audio bgm mainmenu sebelum play game 

        loadingBarFill.value = 0;
        
        StartCoroutine(LoadScenesAsync());
    } 
    public void LoadScenes2()
    {
        loadingScreen.SetActive(true);
        // Stop audio bgm mainmenu sebelum play game 

        loadingBarFill.value = 0;
        StartCoroutine(LoadLevel2());
    } 
    public void LoadScenes3()
    {
        loadingScreen.SetActive(true);
        // Stop audio bgm mainmenu sebelum play game 

        loadingBarFill.value = 0;
        StartCoroutine(LoadLevel3());
    } 
    IEnumerator LoadScenesAsync()
    {
        AudioManager.Instance.StopAllBackgroundMusic();
        List<AsyncOperation> scenes = new List<AsyncOperation>();

        // Sesuaikan indeks scene dengan indeks scene yang ingin Anda muat
        
        scenes.Add(SceneManager.LoadSceneAsync("Gameplay1" ));
        scenes.Add(SceneManager.LoadSceneAsync("Level1",LoadSceneMode.Additive));


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


        ScoreManager.instance.ResetAllValues();
        AudioManager.Instance.PlayBackgroundMusicWithTransition("GameJakarta",0, 1f);
    }
    IEnumerator LoadLevel2()
    {

        AudioManager.Instance.StopAllBackgroundMusic();

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
        ScoreManager.instance.ResetAllValues();

        AudioManager.Instance.PlayBackgroundMusicWithTransition("GameInvert",0, 1f);
    }
    IEnumerator LoadLevel3()
    {
        AudioManager.Instance.StopAllBackgroundMusic();
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


        ScoreManager.instance.ResetAllValues();

        AudioManager.Instance.PlayBackgroundMusicWithTransition("GameBandung",0, 1f);
    }
    IEnumerator MainMenu()
    {

        AudioManager.Instance.StopAllBackgroundMusic();
        List<AsyncOperation> scenes = new List<AsyncOperation>();
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(true);

        loadingBarFill.value = 0;
        scenes.Add(SceneManager.LoadSceneAsync(0));

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

        loadingScreen.SetActive(false);
        animator.SetTrigger("Start");
    }
}
