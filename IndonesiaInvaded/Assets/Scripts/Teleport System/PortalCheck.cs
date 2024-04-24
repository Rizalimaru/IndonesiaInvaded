using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCheck : MonoBehaviour
{
    
    public SceneField SceneToLoad;
    public string exitName;
    public Animator animator;
    public static bool loadPortal;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            loadPortal = true;
            PlayerPrefs.SetString("LastExitName", exitName);
            StartCoroutine(NextLevel());
        }

    }

    IEnumerator NextLevel()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneToLoad);
        animator.SetTrigger("Start");
    }
}
