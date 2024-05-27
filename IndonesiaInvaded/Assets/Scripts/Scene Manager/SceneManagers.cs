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
    public int CurrentLevelIndex = 1;
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
            GameManager.instance.SaveGame();
        }
        //reset the index if we have no more levels
        else CurrentLevelIndex = 1;
    }


    public void NextLevel()
    {
        GameManager.instance.SaveGame();
        UI_PauseGame.instance.HideResult();
        CurrentLevelIndex++;
        LoadLevelWithIndex(CurrentLevelIndex);
        ScoreManager.instance.ResetAllValues();
        GameManager.instance.SaveGame();
    }
    public void RestartLevel()
    {
        GameManager.instance.LoadGame();
        LoadLevelWithIndex(CurrentLevelIndex);

    }

    public void LoadMainMenu()
    {
        GameManager.instance.SaveGame();
        UI_PauseGame.instance.HideResult();
        SceneManager.LoadSceneAsync(menus[(int)Type.Main_Menu].sceneName);
        GameManager.instance.SaveGame();
    }

    public void Creditscene()
    {
        AudioManager.Instance.StopAllBackgroundMusic();
        GameManager.instance.SaveGame();
        UI_PauseGame.instance.HideResult();
        SceneManager.LoadScene("CreditScene");
        ScoreManager.instance.ResetAllValues();
        GameManager.instance.SaveGame();

        AudioManager.Instance.PlayBackgroundMusicWithTransition("CreditScene",0, 1f);


    }
}