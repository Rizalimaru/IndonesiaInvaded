using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMangement : MonoBehaviour
{
    [Header("Scenes To Load")]
    [SerializeField] private SceneField persistentGamePlay;
    [SerializeField] private SceneField newGame;
    [SerializeField] private SceneField options;
    [SerializeField] private SceneField continueGame;
    [SerializeField] private SceneField selectLevel;
    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public void NewGame(){
        scenesToLoad.Add(SceneManager.LoadSceneAsync(persistentGamePlay));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(newGame, LoadSceneMode.Additive));
    }

    public void ContinueGame(){
        scenesToLoad.Add(SceneManager.LoadSceneAsync(persistentGamePlay));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(continueGame, LoadSceneMode.Additive));
    }

    public void OptionsGame(){
        scenesToLoad.Add(SceneManager.LoadSceneAsync(persistentGamePlay));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(options, LoadSceneMode.Additive));
    }

    public void ExitGame(){
        Application.Quit();
    }
}
