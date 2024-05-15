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
    private LevelCheck[] levelChecks;
    private bool isLoadingGame = false;


    private void Awake()
    {
        instance = this;
        levelChecks = this.GetComponentsInChildren<LevelCheck>();
    }

    private void Start()
    {
        CheckLevel1Completion();
    }
    public void NewGame(LevelCheck levelCheck)
    {
        if (isLoadingGame)
        {
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.SaveGame();
            StartCoroutine(DelayNewGame());
        }
        else if (levelCheck.hasData)
        {
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.NewGame();
            GameManager.instance.SaveGame();
            StartCoroutine(DelayNewGame());
        }
        else{
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.NewGame();
            GameManager.instance.SaveGame();
            StartCoroutine(DelayNewGame());
        }

    }

    IEnumerator DelayNewGame()
    {

        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        DisableMenuAndAnimationButton();
        UI_ControlMainMenu.Instance.HideUI();
        yield return new WaitForSeconds(1f);
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);

        Scene_Loading.instance.LoadScenes();
        yield return new WaitForSeconds(0.9f);
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
        yield return new WaitForSeconds(0.9f);
        titleGameAnimator.SetTrigger("hide");
        mission.SetActive(true);
        UI_ControlMainMenu.Instance.ShowMissionSelected();
    }
    public void DisableMenuAndAnimationButton()
    {
        newGameButton.interactable = false;
        loadButton.interactable = false;
        optionsButton.interactable = false;
        exitButton.interactable = false;
    }
    public void EnableMenuAndAnimationButton()
    {
        newGameButton.interactable = true;
        loadButton.interactable = true;
        optionsButton.interactable = true;
        exitButton.interactable = true;
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

        yield return new WaitForSeconds(0.9f);

        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("showbackground");
        yield return new WaitForSeconds(0.5f);
        ActivateMenu();
        EnableMenuAndAnimationButton();
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
