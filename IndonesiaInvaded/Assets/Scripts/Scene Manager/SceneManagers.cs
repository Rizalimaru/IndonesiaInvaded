using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public static SceneManagers instance;

    [Header("Scenes To Load")]
    public SceneField newGame;
    public SceneField MainMenu;

    [Space(10)]

     [Header("UI Elements")]
    public GameObject completeLevelUI;
    public GameObject deadGame;
    public GameObject pauseMenu;


    bool isPaused = false;
    bool hasGameEnded = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void NewGame(){
        SceneManager.LoadScene(newGame);
    }
    public void CompleteLevel()
    {
        if (hasGameEnded == false)
        {
            hasGameEnded = true;
            Debug.Log("Level Complete");
            completeLevelUI.SetActive(true);
        }
    }

    public void EndGame()
    {
        if (hasGameEnded == false)
        {
            hasGameEnded = true;
            Debug.Log("Game Over");
            deadGame.SetActive(true);
            Invoke("RestartGame", 2f);
        }
    }

    public void RestartGame()
    {
        completeLevelUI.SetActive(false);
        deadGame.SetActive(false);
        hasGameEnded = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            }
        }
    }

}