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

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBarFill;
    
    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }


    public void OnSaveClicked(SaveSlot saveSlot)
    {
        StartCoroutine(DelaySave(saveSlot));
    }

    IEnumerator DelaySave(SaveSlot saveSlot)
    {
        DisableMenuButton();

        // Menyembunyikan UI Mission Selected dan memulai animasi load game
        UI_ControlMainMenu.Instance.HideSelectedMissionInGame();
        UI_AnimatorUI.instance.LoadGameAnimation();
        
        yield return new WaitForSeconds(1f);
        
        GameManager.instance.ChangeSelectedProfile(saveSlot.GetProfileId());
        if (!isLoadingGame)
        {
            GameManager.instance.NewGame();
        }


        // Menambahkan Progress Loading Screen dan Scene Load
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneField);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBarFill.value = progress;

            yield return null;
        }
    }

    public void OnBackClicked()
    {
        StartCoroutine(DelayBack());
    }

    IEnumerator DelayBack()
    {
        UI_ControlMainMenu.Instance.HideMissionSelected();

        yield return new WaitForSeconds(0.9f);

        // Mengaktifkan Main Menu dan Interactable Button
        mainMenu.ActivateMenu();
        mainMenu.EnableMenuandAnimationButton();

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
