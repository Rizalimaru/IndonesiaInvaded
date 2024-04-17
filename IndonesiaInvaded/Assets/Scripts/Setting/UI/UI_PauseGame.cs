using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_PauseGame : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject gameObjectPause;

    [Header("Options GameObject")]
    public GameObject gameObjectOptions;
    public Animator optionsAnimator;
    public GameObject[] panelOptions;

    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
{
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (GameIsPaused)
        {
            if (gameObjectOptions.activeSelf)
            {
                HideOptions();
            }
            else
            {
                Resume();
            }
        }
        else
        {
            Pause();
        }
    }
}


    private void ShowPanel(int index)
    {
        for (int i = 0; i < panelOptions.Length; i++)
        {
            panelOptions[i].SetActive(i == index);
        }
    }

    public void ShowController()
    {
        ShowPanel(0);
    }

    public void ShowDisplay()
    {
        ShowPanel(1);
    }

    public void ShowGraphics()
    {
        ShowPanel(2);
    }

    public void ShowAudio()
    {
        ShowPanel(3);
    }



    public void Pause()
    {

        gameObjectPause.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        Debug.Log("Game paused");
    }

    public void Resume()
    {
        // Memainkan animasi FadeOut
        gameObjectPause.SetActive(false);
        Time.timeScale = 1f;

        // Menandakan bahwa permainan sedang dalam kondisi berjalan
        GameIsPaused = false;

    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowOptions()
    {
        gameObjectOptions.SetActive(true);
        
    }
    public void HideOptions()
    {
        gameObjectOptions.SetActive(false);
    }



}