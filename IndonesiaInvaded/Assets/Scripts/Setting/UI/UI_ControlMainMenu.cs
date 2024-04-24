using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UI_ControlMainMenu: MonoBehaviour
{
    public GameObject gameObjectMenu;
    public GameObject gameObjectOptions;

    // Objek page press any key
    public GameObject gameObjectPressAnyKey;
    public Animator buttonAnimator;
    public Animator optionsAnimator;

    public Animator pressAnyKeyAnimator;


    public static UI_ControlMainMenu Instance { get; private set; }

    [SerializeField] private CanvasGroup UIGroup;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    private void Awake()
    {
        Instance = this;
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
        // Jika menekan tombol apapun, sembunyikan objek press any key
        if (Input.anyKeyDown)
        {
            if (gameObjectPressAnyKey.activeSelf)
            {
                ShowPressAnyKey();
                

            }
            
        }


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
        SceneMainMenuManager.instance.LoadGame();       

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

    public void ShowPressAnyKey()
    {
        StartCoroutine(DelayPressAnyKey());
    }
    
    IEnumerator DelayPressAnyKey()
    {
        pressAnyKeyAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1.4f);
        gameObjectPressAnyKey.SetActive(false);
        gameObjectMenu.SetActive(true);
        ShowUI();
        
    }




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
        
        yield return new WaitForSeconds(0.5f);
        
        gameObjectMenu.SetActive(true);
        gameObjectOptions.SetActive(false);
    }

    // Exit game
    public void ExitGame()
    {
        Application.Quit();
    }
}
