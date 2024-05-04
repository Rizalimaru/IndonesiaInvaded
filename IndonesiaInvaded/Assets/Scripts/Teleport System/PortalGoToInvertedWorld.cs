using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalGoToInvertedWorld : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] Animator animator;
    [SerializeField] PlayerDataSaving player;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject ui_ResultGame;

    [Header("Next Stage Button")]
    [SerializeField] Button[] nextStageButton;

    void Start()
    {
        nextStageButton[0].gameObject.SetActive(true);
        nextStageButton[1].gameObject.SetActive(false);
        nextStageButton[2].gameObject.SetActive(false);

    }
    
    public void NextStage()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        Time.timeScale = 1;
        animator.SetTrigger("End");
        player.gameObject.SetActive(false);
        ui_ResultGame.SetActive(false);
        playerCamera.SetActive(false);
        yield return new WaitForSeconds(1);

        player.Teleport(destination.position, destination.rotation);

        player.gameObject.SetActive(true);
        UI_PauseGame.instance.HideResult();
        
        playerCamera.SetActive(true);
        animator.SetTrigger("Start");

        ScoreManager.instance.ResetAllValues();
        UI_ResultGame.instance.ResetAllValues();

        nextStageButton[0].gameObject.SetActive(false);
        nextStageButton[1].gameObject.SetActive(true);
        nextStageButton[2].gameObject.SetActive(false);


    }
}
