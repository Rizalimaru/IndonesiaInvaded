using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenuV2 mainMenu;
    private List<string> scenesToUnload = new List<string>();

    public void LoadLevel1(LevelCheck levelCheck)
    {
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();
        Scene_Loading.instance.LoadSceneByName("Gameplay1", "Level1", "GameJakarta");
    }

    public void LoadLevel2(LevelCheck levelCheck)
    {
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();

        if (SceneManager.GetActiveScene().name == "Gameplay1")
        {
            SceneManager.UnloadSceneAsync("Gameplay1");
        }

        Scene_Loading.instance.LoadSceneByName("Gameplay2", "Level2", "GameInvert");


    }

    public void LoadLevel3(LevelCheck levelCheck)
    {
        GameManager.instance.ChangeSelectedProfileId(levelCheck.GetProfileId());
        GameManager.instance.NewGame();
        GameManager.instance.SaveGame();

        if (SceneManager.GetActiveScene().name == "Gameplay2")
        {
            SceneManager.UnloadSceneAsync("Gameplay2");
        }

        Scene_Loading.instance.LoadSceneByName("Gameplay3", "Level3", "GameBandung");

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
