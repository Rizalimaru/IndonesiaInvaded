using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComplete : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int levelNumber;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.OnCompleteLevel(levelNumber);
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        UI_ResultGame.instance.ShowResult();
        UI_PauseGame.instance.ShowResult();
        animator.SetTrigger("Start");
        GameManager.instance.SaveGame();
    }

    
}
