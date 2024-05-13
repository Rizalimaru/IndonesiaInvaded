using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComplete : MonoBehaviour
{
    [Header("Animator Fade In/Out")]
    [SerializeField] Animator animator;

    [Header("Level Number")]
    [SerializeField] int levelNumber;
    [Header("Game Object To Disable")]
    [SerializeField] PlayerDataSaving player;
    [SerializeField] GameObject playerCamera;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            LevelManager.instance.OnCompleteLevel(levelNumber);
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel()
    {
        animator.SetTrigger("End");
        player.gameObject.SetActive(false);
        playerCamera.SetActive(false);
        yield return new WaitForSeconds(1);
        UI_ResultGame.instance.ShowResult();
        UI_PauseGame.instance.ShowResult();
        player.gameObject.SetActive(true);
        playerCamera.SetActive(true);
        animator.SetTrigger("Start");
        GameManager.instance.SaveGame();
    }

    
}
