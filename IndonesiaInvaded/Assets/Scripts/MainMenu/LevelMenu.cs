using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [Header("Scene")]
    public SceneField[] scene;

    [Space(2)]
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Button")]
    [SerializeField] private Button backButton;

    private LevelCheck[] levelChecks;
    private bool isLoadingGame = false;
    

    private void Awake(){
        levelChecks = this.GetComponentsInChildren<LevelCheck>();
    }
    public void LoadLevel1(LevelCheck levelCheck)
    {
        if (isLoadingGame)
        {
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.SaveGame();
            Scene_Loading.instance.LoadScenes();
        }
        else if (levelCheck.hasData)
        {
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.NewGame();
            GameManager.instance.SaveGame();
            Scene_Loading.instance.LoadScenes();
        }
        else{
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.NewGame();
            GameManager.instance.SaveGame();
            Scene_Loading.instance.LoadScenes();
        }
    }
    public void LoadLevel2(LevelCheck levelCheck)
    {
        if (isLoadingGame)
        {
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.SaveGame();
            Scene_Loading.instance.LoadScenes2();
        }
        else if (levelCheck.hasData)
        {
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.NewGame();
            GameManager.instance.SaveGame();
            Scene_Loading.instance.LoadScenes2();
        }
        else{
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.NewGame();
            GameManager.instance.SaveGame();
            Scene_Loading.instance.LoadScenes2();
        }
    }
    public void LoadLevel3(LevelCheck levelCheck)
    {
        if (isLoadingGame)
        {
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.SaveGame();
            Scene_Loading.instance.LoadScenes3();
        }
        else if (levelCheck.hasData)
        {
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.NewGame();
            GameManager.instance.SaveGame();
            Scene_Loading.instance.LoadScenes3();
        }
        else{
            GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
            GameManager.instance.NewGame();
            GameManager.instance.SaveGame();
            Scene_Loading.instance.LoadScenes3();
        }
    }

    public void OnBackClicked()
    {
        StartCoroutine(DelayBack());
    }
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
    IEnumerator DelayBack()
    {
        UI_ControlMainMenu.Instance.HideMissionSelected();

        yield return new WaitForSeconds(0.9f);

        mainMenu.ActivateMenu();
        mainMenu.EnableMenuandAnimationButton();
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("showbackground");
        yield return new WaitForSeconds(0.5f);
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("show");
        
        this.DeactivateMenu();
    }

}
