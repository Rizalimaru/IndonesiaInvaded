using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenuV2 mainMenu;

    [Header("Menu Button")]
    [SerializeField] private Button backButton;

    [Header("CutScene Name")]
    [SerializeField] List<string> cutSceneName;
    [SerializeField] GameObject[] uiMainMenu;
    private int nextSceneIndex;

    private void Start()
    {
        // CutSceneManager.Instance.OnCutSceneFinished += OnCutSceneFinished;
    }

    public void LoadLevel1(LevelCheck levelCheck)
    {
        // nextSceneIndex = 1;
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();
        Scene_Loading.instance.LoadScenes();
        // PlayCutSceneBeforeLevel(cutSceneName[0]);
    }

    public void LoadLevel2(LevelCheck levelCheck)
    {
        // nextSceneIndex = 2;
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();
        Scene_Loading.instance.LoadScenes2();
        // PlayCutSceneBeforeLevel(cutSceneName[1]);
    }

    public void LoadLevel3(LevelCheck levelCheck)
    {
        // nextSceneIndex = 3;
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();
        Scene_Loading.instance.LoadScenes3();
        // PlayCutSceneBeforeLevel(cutSceneName[2]);
    }

    private void PlayCutSceneBeforeLevel(string cutSceneName)
    {
        CutSceneManager.Instance.PlayCutScene(cutSceneName);
        for (int i = 0; i < uiMainMenu.Length; i++)
        {
            uiMainMenu[i].SetActive(false);
        }
    }

    private void OnCutSceneFinished()
    {
        switch (nextSceneIndex)
        {
            case 1:
                Scene_Loading.instance.LoadScenes();
                break;
            case 2:
                Scene_Loading.instance.LoadScenes2();
                break;
            case 3:
                Scene_Loading.instance.LoadScenes3();
                break;
            default:
                Debug.LogError("Invalid scene index");
                break;
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
        mainMenu.EnableMenuAndAnimationButton();

        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("showbackground");

        yield return new WaitForSeconds(0.5f);
        
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("show");

        this.DeactivateMenu();
    }
}
