using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuV2 : MonoBehaviour
{
    public static MainMenuV2 instance;
    [Header("Button UI")]
    public Button newGameButton;
    public Button loadButton;
    public Button optionsButton;
    public Button exitButton;

    [Header("Title Game")]
    public Animator titleGameAnimator;
    public Animator UIAnimator;

    [Header("UI Menu")]
    public GameObject mission;
    public GameObject ui;

    [Header("Menu Navigation")]
    [SerializeField] private LevelMenu levelMenu;
    private LevelCheck[] levelChecks;
    private GameData data;
    [Header("Cutscene")]
    public string newGameCutSceneName = "";
    public GameObject[] uiMainMenu;


    private void Awake()
    {
        instance = this;
        data = new GameData();
        levelChecks = this.GetComponentsInChildren<LevelCheck>();
    }
    

    private void Start()
    {
        CheckLevel1Completion();
        CutSceneManager.Instance.OnCutSceneFinished += OnCutSceneFinished;
    }
    public void NewGame(LevelCheck levelCheck)
    {
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();
        StartCoroutine(DelayNewGame());

    }

    IEnumerator DelayNewGame()
    {

        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        DisableMenuAndAnimationButton();
        UI_ControlMainMenu.Instance.HideUI();

        yield return new WaitForSeconds(1f);
        
        CutSceneManager.Instance.PlayCutScene(newGameCutSceneName);
        for (int i = 0; i < uiMainMenu.Length; i++)
        {
            uiMainMenu[i].SetActive(false);
        }
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("FadeOut");
        
        yield return new WaitForSeconds(1f);
    }
    private void OnCutSceneFinished()
    {
        DeactivateMenu();
        Scene_Loading.instance.LoadScenes();
        AudioManager.Instance.PlayBackgroundMusicWithTransition("Game", 0, 1f);
    }

    public void LoadGame()
    {
        StartCoroutine(DelayLoadGame());
    }

    IEnumerator DelayLoadGame()
    {
        DisableMenuAndAnimationButton();
        UI_ControlMainMenu.Instance.HideUI();

        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("hidebackground");

        yield return new WaitForSeconds(0.9f);

        mission.SetActive(true);
        UI_ControlMainMenu.Instance.ShowMissionSelected();
    }
    public void DisableMenuAndAnimationButton()
    {
        newGameButton.interactable = false;
        optionsButton.interactable = false;
        exitButton.interactable = false;
        loadButton.interactable = false;
    }
    public void EnableMenuAndAnimationButton()
    {
        newGameButton.interactable = true;
        optionsButton.interactable = true;
        exitButton.interactable = true;
        CheckLevel1Completion();
    }
    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
    public void BackButton()
    {
        StartCoroutine(DelayBack());
    }
    IEnumerator DelayBack()
    {
        UI_ControlMainMenu.Instance.HideMissionSelected();
        EnableMenuAndAnimationButton();

        yield return new WaitForSeconds(0.9f);

        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("showbackground");
        yield return new WaitForSeconds(0.5f);
        ActivateMenu();
        
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("show");
        mission.SetActive(false);
    }
    void CheckLevel1Completion()
    {
        if (LevelManager.instance.IsLevelUnlocked(1))
        {
            loadButton.interactable = true;
        }
        else
        {
            loadButton.interactable = false;           
        }
    }

   
}
