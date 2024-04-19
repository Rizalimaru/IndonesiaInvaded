using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_PauseGame : MonoBehaviour
{
    //public UnityEvent GamePaused;
    //public UnityEvent GameResumed;

    public static bool GameIsPaused = false;

    public GameObject gameObjectPause;
    public GameObject gameObjectOptions;

    public GameObject playerCamera;
    public GameObject[] panelOptions;

    // Lock cursor when the game is not paused
    private bool isCursorLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        // Lock cursor initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        playerCamera.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        isCursorLocked = false; // Unlock cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //GamePaused.Invoke(); // Invoke pause event

        Debug.Log("Game paused");
    }

    public void Resume()
    {
        gameObjectPause.SetActive(false);
        playerCamera.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
        isCursorLocked = true; // Lock cursor when unpaused
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //GameResumed.Invoke(); // Invoke resume event
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
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

    // Update cursor state based on pause status
    private void LateUpdate()
    {
        if (GameIsPaused && !isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; // Show cursor during pause
        }
        else if (!GameIsPaused && isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
