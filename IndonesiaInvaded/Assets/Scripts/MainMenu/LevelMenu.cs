using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour, IDataPersistence
{
    [Header("Scene")]
    public SceneField[] scene;

    [Space(2)]
    [Header("Menu Navigation")]
    [SerializeField] private MainMenuV2 mainMenu;

    [Header("Menu Button")]
    [SerializeField] private Button backButton;

    public Button[] buttons;
    private GameData data;
    private void Awake(){
        data = new GameData();
    }

    public void LoadLevel1(LevelCheck levelCheck)
    {
        GameManager.instance.SavePlayerData(levelCheck.GetProfileId(), data);
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();
        Scene_Loading.instance.LoadScenes();
    }
    public void LoadLevel2(LevelCheck levelCheck)
    {
        GameManager.instance.SavePlayerData(levelCheck.GetProfileId(), data);
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();
        Scene_Loading.instance.LoadScenes2();
    }
    public void LoadLevel3(LevelCheck levelCheck)
    {
        GameManager.instance.SavePlayerData(levelCheck.GetProfileId(), data);
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();
        Scene_Loading.instance.LoadScenes3();
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

    public void SaveData(GameData data)
    {
        
    }

    public void LoadData(GameData data)
    {

    }

}
