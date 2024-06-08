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

    private float delayGameOver;

    private Animator animator;


    private bool isResultScreenShown = false; // Check if the result screen is shown
    // Lock cursor when the game is not paused
    private bool isCursorLocked = true;

    [Header("-----------------------Events-----------------------")]
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;

    [Header("-----------------------GameObjects-----------------------")]

    public GameObject gameObjectPause;
    public GameObject[] gameObjectUI;
    public GameObject gameObjectOptions;
    public GameObject gameResult;
    public GameObject[] HPBarOndelOndel;
    public GameObject[] HPBarDukun;

    [Header("-----------------------GameOver-----------------------")]
    public GameObject gameOver;
    public GameObject player;

    [Header("-----------------------Player-----------------------")]
    public GameObject playerCamera;

    [Header("-----------------------Panels-----------------------")]
    public GameObject[] panelOptions;

    [Header("-----------------------Animation-----------------------")]

    public Animator pauseAnimator;
    public Animator optionsAnimatorGame;

    private void Awake()
    {
        if (instance != null)
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
        animator = player.GetComponent<Animator>();
        // Lock cursor initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioManagerInstance = AudioManager.Instance;

        ScoreManager.instance.ResetAllValues();
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
                    }
                }
                else
                {
                    Pause(); // Pause the game if not paused
                }
            }

        EndGame();
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

    public void ActiveBossHPBarOndel()
    {
        for (int i = 0; i < HPBarOndelOndel.Length; i++)
        {
            HPBarOndelOndel[i].SetActive(true);
        }
    }

    public void DisableBossHPBarOndel()
    {
        for (int i = 0; i < HPBarOndelOndel.Length; i++)
        {
            HPBarOndelOndel[i].SetActive(false);
        }
    }


    public void Pause()
    {
        AudioSetting.instance.PlayPauseSoundEffect();
        pauseAnimator.SetTrigger("pausein");
        gameObjectPause.SetActive(true);
        for (int i = 0; i < gameObjectUI.Length; i++)
        {
            gameObjectUI[i].SetActive(false);
        }
        playerCamera.SetActive(false);
        gameResult.SetActive(false);

        Time.timeScale = 0f;
        GameIsPaused = true;

        isCursorLocked = false; // Unlock cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GamePaused.Invoke(); // Invoke pause event

        audioManagerInstance.PauseSFX();
    }

    public void GameOver()
    {
        // player.SetActive(false);

        GameIsPaused = true;
        Time.timeScale = 0f;
        isCursorLocked = false; // Unlock cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        gameOver.SetActive(true);
        playerCamera.SetActive(false);

        // GamePaused.Invoke();

        audioManagerInstance.PauseSFX();
    }

    public void ResetGameOver()
    {   
        animator.SetBool("Death", false);
        PlayerMovement.instance.enabled = true;
        ThirdPersonCam.instance.GetBisaRotasi = true;
        Combat.instance.enabled = true;
        delayGameOver = 0f;
        isGameOver = false;
        GameIsPaused = false;
        Time.timeScale = 1f;
        isCursorLocked = true; // Lock cursor when unpaused
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameOver.SetActive(false);
        playerCamera.SetActive(true);

        // GameResumed.Invoke(); // Invoke resume event
        audioManagerInstance.ResumeSFX();
    }

    public void ShowResult()
    {
        gameResult.SetActive(true);
        for (int i = 0; i < gameObjectUI.Length; i++)
        {
            gameObjectUI[i].SetActive(false);
        }
        playerCamera.SetActive(false);
        gameOver.SetActive(false);

        Time.timeScale = 0f;
        GameIsPaused = true;
        isResultScreenShown = true; // Set isResultScreenShown to true when showing the result screen
        isCursorLocked = false; // Unlock cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GamePaused.Invoke();

        audioManagerInstance.PauseSFX();
    }

    public void HideResult()
    {
        gameResult.SetActive(false);
        for (int i = 0; i < gameObjectUI.Length; i++)
        {
            gameObjectUI[i].SetActive(true);
        }
        playerCamera.SetActive(true);
        gameOver.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        isResultScreenShown = false; // Set isResultScreenShown to true when showing the result screen

        GameResumed.Invoke(); // Invoke resume event

        audioManagerInstance.ResumeSFX();
    }

    public void CreditScene()
    {
        gameResult.SetActive(false);
        for (int i = 0; i < gameObjectUI.Length; i++)
        {
            gameObjectUI[i].SetActive(true);
        }
        playerCamera.SetActive(true);
        gameOver.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        isResultScreenShown = false; // Set isResultScreenShown to true when showing the result screen
        //isCursorLocked = true; // Lock cursor when unpaused
        GameResumed.Invoke(); // Invoke resume event

        audioManagerInstance.ResumeSFX();
    }

    public void Resume()
    {
        pauseAnimator.SetTrigger("pauseout");
        gameObjectPause.SetActive(false);
        for (int i = 0; i < gameObjectUI.Length; i++)
        {
            gameObjectUI[i].SetActive(true);
        }
        playerCamera.SetActive(true);
        gameResult.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        isCursorLocked = true; // Lock cursor when unpaused
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioManagerInstance.ResumeSFX();

        GameResumed.Invoke(); // Invoke resume event
    }

    public void LoadMenu()
    {
        Scene_Loading.instance.LoadMainMenu();
        Time.timeScale = 1f;
        gameObjectPause.SetActive(false);
        for (int i = 0; i < gameObjectUI.Length; i++)
        {
            gameObjectUI[i].SetActive(true);
        }
        playerCamera.SetActive(true);
        gameResult.SetActive(false);
        gameOver.SetActive(false);
 
        //GameIsPaused = false;
        isLoadMainMenu = true; // Set isLoadMainMenu to true after calling LoadMenu()
        audioManagerInstance.StopAllBackgroundMusic();
        audioManagerInstance.ResumeSFX();
        audioManagerInstance.StopSpesificSFX();



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

    public void EndGame()
    {
        if (PlayerAttribut.instance.currentHealth <= 0)
        {
            delayGameOver += Time.deltaTime;

            if (delayGameOver >= 5f)
            {
                isGameOver = true;
                GameOver();
            }
        }
    }

}
