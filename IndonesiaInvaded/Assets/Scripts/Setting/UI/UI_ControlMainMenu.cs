using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;


public class UI_ControlMainMenu: MonoBehaviour
{
    public UnityEvent buttonClicked;
    public UnityEvent buttonNoClicked;

    [Header("-------------GameObjects-------------")]
    public GameObject gameObjectMenu;
    public GameObject gameObjectOptions;

    public GameObject gameObjectTitle;

    // Objek page press any key
    public GameObject gameObjectPressAnyKey;

    public GameObject gameObjectMissionSelected;

    [Header("-------------Animation-------------")]
    public Animator buttonAnimator;
    public Animator optionsAnimator;

    public Animator titleGameAnimator;

    public bool titleGameAnimationPlayed = true;

    public Animator pressAnyKeyAnimator;


    public static UI_ControlMainMenu Instance { get; private set; }

    [Header("-------------Canvas Group-------------")]
    [SerializeField] private CanvasGroup UIGroup;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    [Header("-------------Canvas Group Selected Mission-------------")]
    [SerializeField] private CanvasGroup UIGroupMissionSelected;

    [SerializeField] private bool fadeInMissionSelected = false;

    [SerializeField] private bool fadeOutMissionSelected = false;

    [SerializeField] private MainMenu mainMenu;

    private bool anyKeyDownHandled = false;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Destroy(gameObject);
        }

        mainMenu.EnableMenuandAnimationButton();

        
    }


    private void Start()
    {
        
    }

    private void Update()
    {
        if(fadeIn)
        {
            if(UIGroup.alpha < 1)
            {
                UIGroup.alpha += Time.deltaTime * 2;
                if(UIGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
            
        }
        if(fadeOut)
        {
            if(UIGroup.alpha > 0)
            {
                UIGroup.alpha -= Time.deltaTime;
                if(UIGroup.alpha <= 0)
                {
                    fadeOut = false;
                }
            }
        }

        if(fadeInMissionSelected)
        {
            if(UIGroupMissionSelected.alpha < 1)
            {
                UIGroupMissionSelected.alpha += Time.deltaTime * 3;
                if(UIGroupMissionSelected.alpha >= 1)
                {
                    fadeInMissionSelected = false;
                }
            }
            
        }

        if(fadeOutMissionSelected)
        {
            if(UIGroupMissionSelected.alpha > 0)
            {
                UIGroupMissionSelected.alpha -= Time.deltaTime * 3;
                if(UIGroupMissionSelected.alpha <= 0)
                {
                    fadeOutMissionSelected = false;
                }
            }
        }
        // Jika menekan tombol apapun, sembunyikan objek press any key
        if (Input.anyKeyDown)
        {
            if (gameObjectPressAnyKey.activeSelf)
            {
                ShowPressAnyKey();
                if (titleGameAnimationPlayed == true)

                AudioManager._instance.PlaySFX("Button",3);
                {
                    titleGameAnimator.SetTrigger("showbackground");
                    
                    titleGameAnimationPlayed = false;
                }
            }
        }
    }

    // Mendapatkan value bool titleGameAnimationPlayed

    // Mengubah value bool titleGameAnimationPlayed

    public void ButtonClick(){
        buttonClicked.Invoke();
    }

    public void ButtonNoClick(){
        buttonNoClicked.Invoke();
    }
    public void SetTitleGameAnimationPlayed(bool value)
    {
        titleGameAnimationPlayed = value;
    }

    //Pindah scene ke scene yang diinginkan
    public void ChangeSceneLoadGame()
    {
        StartCoroutine(DelayChangeScene());
    }
    IEnumerator DelayChangeScene()
    {
        HideUI();
        yield return new WaitForSeconds(1f);
        UI_AnimatorUI.instance.LoadGameAnimation();
        yield return new WaitForSeconds(1f);      
    }
    public void ShowUI()
    {
        fadeIn = true;
    }

    public void HideUI()
    {
        fadeOut = true;
    }

    // Menampilkan objek press any key

    #region Show and Hide Mission Selected

    public void ShowPressAnyKey()
    {
        if (!anyKeyDownHandled)
        {
            anyKeyDownHandled = true;
            StartCoroutine(DelayPressAnyKey());
        }
    }
    
    IEnumerator DelayPressAnyKey()
    {
        pressAnyKeyAnimator.SetTrigger("FadeIn");
        
        yield return new WaitForSeconds(1.4f);
        gameObjectPressAnyKey.SetActive(false);
        gameObjectMenu.SetActive(true);
        ShowUI();
        
    }

    #endregion PressAnyKey
    public void HideMenu()
    {
        // Mengdelay sebelum menyembunyikan menu
        StartCoroutine(HideMenuDelay());
    }

    public void ShowMenu()
    {
        // Mengdelay sebelum menampilkan menu
        StartCoroutine(ShowMenuDelay());

    
    }
    

    IEnumerator HideMenuDelay()
    {

        optionsAnimator.SetTrigger("FadeInOptions");

        mainMenu.DisableMenuandAnimationButton();
        HideUI();
        
        yield return new WaitForSeconds(0.5f);
        
        if (buttonAnimator != null && buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed"))
        {
            // Jika tombol masih memainkan animasi "Pressed", ganti dengan animasi normal
            buttonAnimator.Play("Normal");
        }


        yield return new WaitForSeconds(0.3f);
        
        
        gameObjectMenu.SetActive(false);
        gameObjectOptions.SetActive(true);
        if (buttonAnimator != null && buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted"))
        {
            // Jika tombol masih memainkan animasi "Pressed", ganti dengan animasi normal
            buttonAnimator.Play("Normal");
        }
    }

    IEnumerator ShowMenuDelay()
    {

        optionsAnimator.SetTrigger("FadeOutOptions");

        mainMenu.EnableMenuandAnimationButton();
        
        yield return new WaitForSeconds(0.5f);
        
        gameObjectMenu.SetActive(true);
        ShowUI();
        gameObjectOptions.SetActive(false);
    }


    // Show mission selected
    public void ShowMissionSelected()
    {
        StartCoroutine(DelayMissionSelected());
    }

    public void HideMissionSelected()
    {
        StartCoroutine(HideMissionSelectedDelay());
    }

    IEnumerator DelayMissionSelected()
    {
        HideUI();
        yield return new WaitForSeconds(0.1f);
        fadeInMissionSelected = true;

        
    
    }
    IEnumerator HideMissionSelectedDelay()
    {
        fadeOutMissionSelected = true;
        yield return new WaitForSeconds(0.9f);
        ShowUI();
    }

    public void HideSelectedMissionInGame()
    {
        fadeOutMissionSelected = true;
    }
    // Exit game
    public void OnAplicationQuit()
    {
        StartCoroutine(DelayExitGame());
        
    }

    IEnumerator DelayExitGame()
    {
        mainMenu.DisableMenuandAnimationButton();
        yield return new WaitForSeconds(0.8f);
        Application.Quit();
    }

    public void OnNewGame(){
        GameManager.instance.NewGame();
    }
    public void OnLoadGame(){
        GameManager.instance.LoadGame();
    }
    public void OnSaveGame(){
        GameManager.instance.SaveGame();
    }
}
