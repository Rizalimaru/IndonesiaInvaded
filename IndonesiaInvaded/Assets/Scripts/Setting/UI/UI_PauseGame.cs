using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_PauseGame : MonoBehaviour
{

    // Singleton instance
    public static UI_PauseGame instance;
    
    private AudioManager audioManagerInstance;

    public static bool GameIsPaused = false;

    private bool isGameOver = false;

    private bool isLoadMainMenu = false;


    private bool isResultScreenShown = false; // Check if the result screen is shown
    // Lock cursor when the game is not paused
    private bool isCursorLocked = true;

    [Header("-----------------------Events-----------------------")]
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;

    [Header("-----------------------GameObjects-----------------------")]

    public GameObject gameObjectPause;
    public GameObject gameObjectUI;
    public GameObject gameObjectOptions;
    public GameObject gameResult;
    public GameObject gameOver;

    [Header("-----------------------Player-----------------------")]
    public GameObject playerCamera;

    [Header("-----------------------Panels-----------------------")]
    public GameObject[] panelOptions;

    [Header("-----------------------Animation-----------------------")]

    public Animator pauseAnimator;
    public Animator optionsAnimatorGame;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of UI_PauseGame found!");
            // Destroy(gameObject);
        }
        else
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Lock cursor initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioManagerInstance = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && !isResultScreenShown && !isLoadMainMenu) // Check if the result screen is not shown
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
                        Resume(); // Resume the game if options are not active

                        Debug.Log("Game resumed");
                    }
                }
                else
                {
                    Pause(); // Pause the game if not paused
                }
            }

            if (PlayerAttribut.instance.currentHealth <= 0)
            {
                GameOver();
                isGameOver = true; // Set isGameOver to true after calling GameOver()
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
        AudioSetting.instance.PlayPauseSoundEffect();
        pauseAnimator.SetTrigger("pausein");
        gameObjectPause.SetActive(true);
        gameObjectUI.SetActive(false);
        playerCamera.SetActive(false);
        gameResult.SetActive(false);

        Time.timeScale = 0f;
        GameIsPaused = true;

        isCursorLocked = false; // Unlock cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GamePaused.Invoke(); // Invoke pause event

        audioManagerInstance.PauseSoundEffectGroup("AttackPlayer");

        Debug.Log("Game paused");
    }

    public void GameOver()
    {

        gameOver.SetActive(true);
        gameObjectUI.SetActive(false);
        playerCamera.SetActive(false);
        gameResult.SetActive(false);

        Time.timeScale = 0f;
        GameIsPaused = true;
        isCursorLocked = false; // Unlock cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GamePaused.Invoke();

        audioManagerInstance.PauseSoundEffectGroup("AttackPlayer");



    }

    public void ResetGameOver(){
        gameOver.SetActive(false);
        gameObjectUI.SetActive(true);
        playerCamera.SetActive(true);
        gameResult.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        isCursorLocked = true; // Lock cursor when unpaused
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameResumed.Invoke(); // Invoke resume event

        audioManagerInstance.ResumeSoundEffectGroup("AttackPlayer");
    }

    public void ShowResult()
    {

        gameResult.SetActive(true);
        gameObjectUI.SetActive(false);
        playerCamera.SetActive(false);
        gameOver.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        isResultScreenShown = true; // Set isResultScreenShown to true when showing the result screen
        isCursorLocked = false; // Unlock cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GamePaused.Invoke();


        audioManagerInstance.PauseSoundEffectGroup("AttackPlayer"); 


    }

    public void HideResult()
    {
        gameResult.SetActive(false);
        gameObjectUI.SetActive(true);
        playerCamera.SetActive(true);
        gameOver.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        isResultScreenShown = false; // Set isResultScreenShown to true when showing the result screen
        isCursorLocked = true; // Lock cursor when unpaused
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameResumed.Invoke(); // Invoke resume event

        audioManagerInstance.ResumeSoundEffectGroup("AttackPlayer");
    }

    public void Resume()
    {
        pauseAnimator.SetTrigger("pauseout");
        gameObjectPause.SetActive(false);
        gameObjectUI.SetActive(true);
        playerCamera.SetActive(true);
        gameResult.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        isCursorLocked = true; // Lock cursor when unpaused
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioManagerInstance.ResumeSoundEffectGroup("AttackPlayer");

        GameResumed.Invoke(); // Invoke resume event
    }

    public void LoadMenu()
    {
        Scene_Loading.instance.LoadMainMenu();
        Time.timeScale = 1f;
        gameObjectPause.SetActive(false);
        gameObjectUI.SetActive(false);
        playerCamera.SetActive(true);
        gameResult.SetActive(false);
        gameOver.SetActive(false);
        GameIsPaused = false;
        isLoadMainMenu = true; // Set isLoadMainMenu to true after calling LoadMenu()
        audioManagerInstance.StopAllBackgroundMusic();
        audioManagerInstance.ResumeSoundEffectGroup("AttackPlayer");
    }


    public void ShowOptions()
    {
        gameObjectOptions.SetActive(true);
        optionsAnimatorGame.SetTrigger("FadeInOptions");
    }

    public void HideOptions()
    {
        optionsAnimatorGame.SetTrigger("FadeOutOptions");
        StartCoroutine(HideOptionsCoroutine());
    }

    private IEnumerator HideOptionsCoroutine()
    {   
        yield return new WaitForSecondsRealtime(0.5f);
        gameObjectOptions.SetActive(false);
    }

    // Update cursor state based on pause status
    // private void LateUpdate()
    // {
    //     if (GameIsPaused && !isCursorLocked)
    //     {
    //         Cursor.lockState = CursorLockMode.None;
    //         Cursor.visible = true; // Show cursor during pause
    //     }
    //     else if (!GameIsPaused && isCursorLocked)
    //     {
    //         Cursor.lockState = CursorLockMode.Locked;
    //         Cursor.visible = false;
    //     }
    // }
}
