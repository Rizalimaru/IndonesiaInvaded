using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_ControlMainMenu: MonoBehaviour
{
    public GameObject gameObjectMenu;
    public GameObject gameObjectOptions;
    public Animator buttonAnimator;
    public Animator optionsAnimator;
    
    

    public static UI_ControlMainMenu Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //Pindah scene ke scene yang diinginkan
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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

        optionsAnimator.SetTrigger("FadeOut");
        
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
