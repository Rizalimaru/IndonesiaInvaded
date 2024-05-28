using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotMenu : MonoBehaviour
{
    public static SaveSlotMenu instance;

    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Button")]
    [SerializeField] private Button backButton;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBarFill;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        instance = this;
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }


    public void OnSaveClicked(SaveSlot saveSlot)
    {
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        StartCoroutine(DelaySave(saveSlot));
    }

    IEnumerator DelaySave(SaveSlot saveSlot)
    {
        DisableMenuButton();

        // Menyembunyikan UI Mission Selected dan memulai animasi load game
        UI_ControlMainMenu.Instance.HideSelectedMissionInGame();
        UI_AnimatorUI.instance.LoadGameAnimation();
        yield return new WaitForSeconds(0.5f);

        if (isLoadingGame)
        {
            // GameManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            SaveGameandLoadScene();
        }
        else if (saveSlot.hasData)
        {
            // GameManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            GameManager.instance.NewGame();
            SaveGameandLoadScene();
        }
        else
        {
            // GameManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            GameManager.instance.NewGame();
            SaveGameandLoadScene();
        }
    }

    public void SaveGameandLoadScene()
    {
        
        GameManager.instance.SaveGame();
        // Scene_Loading.instance.LoadScenes();

    }

    public void OnBackClicked()
    {
        StartCoroutine(DelayBack());
    }

    public void ActivateMenu(bool isLoadingGame)
    {

        this.gameObject.SetActive(true);
        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profileGameData = GameManager.instance.GetAllProfilesGameData();
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profileGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);

            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButton()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }

    IEnumerator DelayBack()
    {
        UI_ControlMainMenu.Instance.HideMissionSelected();

        yield return new WaitForSeconds(0.9f);

        // Mengaktifkan Main Menu dan Interactable Button
        mainMenu.ActivateMenu();
        mainMenu.EnableMenuandAnimationButton();
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("showbackground");
        yield return new WaitForSeconds(0.5f);
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("show");
        
        this.DeactivateMenu();
    }
}
