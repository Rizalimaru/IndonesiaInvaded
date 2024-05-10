using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public SceneField[] scene;
    [Space(2)]
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Button")]
    [SerializeField] private Button backButton;

    public void LoadLevel1()
    {
        Scene_Loading.instance.LoadScenes();
    }
    public void LoadLevel2()
    {
        Scene_Loading.instance.LoadScenes2();
    }
    public void LoadLevel3()
    {
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
        mainMenu.EnableMenuandAnimationButton();
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("showbackground");
        yield return new WaitForSeconds(0.5f);
        UI_ControlMainMenu.Instance.titleGameAnimator.SetTrigger("show");
        
        this.DeactivateMenu();
    }

}
