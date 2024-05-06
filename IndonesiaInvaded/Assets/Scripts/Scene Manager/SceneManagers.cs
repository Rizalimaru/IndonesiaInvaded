using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public static SceneManagers instance;

    public List<Level> levels = new List<Level>();
    public List<Menus> menus = new List<Menus>();
    public int CurrentLevelIndex=1;
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

    public void LoadLevelWithIndex(int index)
    {
        
        if (index <= levels.Count)
        {
            //Load Gameplay scene for the level
            SceneManager.LoadSceneAsync("Gameplay" + index.ToString());
            //Load first part of the level in additive mode
            SceneManager.LoadSceneAsync("Level" + index.ToString(), LoadSceneMode.Additive);
        }
        //reset the index if we have no more levels
        else CurrentLevelIndex =1;
    }

    public void NextLevel()
    {
        UI_PauseGame.instance.HideResult();
        CurrentLevelIndex++;
        LoadLevelWithIndex(CurrentLevelIndex);
        GameManager.instance.SaveGame();
    }
    public void RestartLevel()
    {
        LoadLevelWithIndex(CurrentLevelIndex);
        GameManager.instance.LoadGame();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(menus[(int)Type.Main_Menu].sceneName);
    }
}