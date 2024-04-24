using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMainMenuManager : MonoBehaviour
{
    public static SceneMainMenuManager instance;
    [SerializeField] Animator animator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(Mainmenu());
    }

    IEnumerator Mainmenu()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(0);
        animator.SetTrigger("Start");
    }

    public void LoadGame()
    {
        StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(1);
        yield return new WaitForSeconds(1);
        animator.SetTrigger("Start");

        
    }
    
        
}
