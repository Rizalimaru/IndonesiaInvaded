using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private LevelMenu saveSlotsMenu;
    
    [Header("Button UI")]
    public Button newGameButton;
    public Button loadButton;
    public Button optionsButton;
    public Button exitButton;

    [Header("Title Game")]
    // animasi title game
    public Animator titleGameAnimator;

    private void Start(){
        DisableButtonsDependingOnData();
    }

    private void DisableButtonsDependingOnData() 
    {
        if (!GameManager.instance.HasGameData()) 
        {
            loadButton.interactable = false;
        }
    }
    public void NewGame(){
        DisableMenuandAnimationButton();
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        Scene_Loading.instance.LoadScenes();
    }
    public void OnNewGameClicked()
    {
        //UI_ControlMainMenu.Instance.ChangeSceneLoadGame();
        StartCoroutine(DelayNewGame());
    }
    IEnumerator DelayNewGame()
    {
        UI_ControlMainMenu.Instance.HideUI();
        DisableMenuandAnimationButton();
        
        
        yield return new WaitForSeconds(0.9f);
        titleGameAnimator.SetTrigger("hide");

        UI_ControlMainMenu.Instance.ShowMissionSelected();
        
        // saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }
    public void OnContinueClicked()
    {
        DisableMenuandAnimationButton();
        GameManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("Gameplay1");
        SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive);
    }
    public void OnLoadGameClicked()
    {
        StartCoroutine(DelayLoadGame());
    }
    IEnumerator DelayLoadGame()
    {
        UI_ControlMainMenu.Instance.HideUI();

        DisableMenuandAnimationButton();

        yield return new WaitForSeconds(0.9f);
        titleGameAnimator.SetTrigger("hide");
        

        UI_ControlMainMenu.Instance.ShowMissionSelected();
        // saveSlotsMenu.ActivateMenu(true);
        // this.DeactivateMenu();
    }
    public void DisableMenuandAnimationButton()
    {
        newGameButton.interactable = false;
        loadButton.interactable = false;
        optionsButton.interactable = false;
        exitButton.interactable = false;
    }
    public void EnableMenuandAnimationButton()
    {
        newGameButton.interactable = true;
        loadButton.interactable = true; 
        optionsButton.interactable = true;
        exitButton.interactable = true;
    }
    public void ActivateMenu(){
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }
    public void DeactivateMenu(){
        this.gameObject.SetActive(false);
    }
}
