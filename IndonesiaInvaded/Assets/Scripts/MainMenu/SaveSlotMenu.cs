using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;
    
    [Header("Menu Button")]
    [SerializeField] private Button backButton;

    [Header("Scene Load Data")]
    [SerializeField] private SceneField sceneField;
    
    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

   

    public void OnSaveClicked(SaveSlot saveSlot)
    {
        DisableMenuButton();
        GameManager.instance.ChangeSelectedProfile(saveSlot.GetProfileId());
        if (!isLoadingGame)
        {
            GameManager.instance.NewGame();
        }
        SceneManager.LoadSceneAsync(sceneField);
    }

    
     public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        // UI_ControlMainMenu.Instance.HideMissionSelected();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {

        this.gameObject.SetActive(true);
        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profileGameData = GameManager.instance.GetAllProfileGameData();

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
}
