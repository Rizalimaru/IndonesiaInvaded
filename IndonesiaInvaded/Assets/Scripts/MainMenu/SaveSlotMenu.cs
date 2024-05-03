using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
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
        yield return new WaitForSeconds(0.5f);

        if (isLoadingGame)
        {
            GameManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            SaveGameandLoadScene();
            loadingScreen.SetActive(true);
            while (!scenesToLoad.All(op => op.isDone))
            {
                float progress = Mathf.Clamp01(scenesToLoad.Sum(op => op.progress) / (0.9f * scenesToLoad.Count));
                loadingBarFill.value = progress;

                yield return null;
            }
        }
        else if (saveSlot.hasData)
        {
            GameManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            GameManager.instance.NewGame();
            SaveGameandLoadScene();
            loadingScreen.SetActive(true);
            while (!scenesToLoad.All(op => op.isDone))
            {
                float progress = Mathf.Clamp01(scenesToLoad.Sum(op => op.progress) / (0.9f * scenesToLoad.Count));
                loadingBarFill.value = progress;

                yield return null;
            }
        }
        else
        {
            GameManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            GameManager.instance.NewGame();
            SaveGameandLoadScene();
            loadingScreen.SetActive(true);
            while (!scenesToLoad.All(op => op.isDone))
            {
                float progress = Mathf.Clamp01(scenesToLoad.Sum(op => op.progress) / (0.9f * scenesToLoad.Count));
                loadingBarFill.value = progress;

                yield return null;
            }
        }
    }

    private void SaveGameandLoadScene()
    {
        GameManager.instance.SaveGame();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Gameplay"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("BlockoutJakarta", LoadSceneMode.Additive));


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
        mainMenu.titleGameAnimator.SetTrigger("show");
        mainMenu.ActivateMenu();
        mainMenu.EnableMenuandAnimationButton();

        this.DeactivateMenu();


    }

    public void ActivateMenu(bool isLoadingGame)
    {

        this.gameObject.SetActive(true);
        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profileGameData = GameManager.instance.GetAllProfilesGameData();

        backButton.interactable = true;

        GameObject firstSelected = backButton.gameObject;
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
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
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
