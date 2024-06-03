using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameComplete : MonoBehaviour
{
    [Header("Animator Fade In/Out")]
    [SerializeField] Animator animator;

    [Header("Level Number")]
    [SerializeField] int levelNumber;

    [Header("Game Object To Disable")]
    [SerializeField] PlayerDataSaving player;
    [SerializeField] GameObject playerCamera;

    private GameData gameData;

    private void Start()
    {
        if (gameData == null)
        {
            gameData = new GameData();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
        if (SceneManager.GetActiveScene().name == "Gameplay1")
        {

            EnvironmentCutSceneJakarta.instance.CutSceneAfterPortal();
            yield return new WaitForSeconds(9);
            UI_ResultGame.instance.ShowResult();
            UI_PauseGame.instance.ShowResult();

            player.gameObject.SetActive(true);
            playerCamera.SetActive(true);

            animator.SetTrigger("Start");
            GameManager.instance.SaveGame();
        }
        else
        {
            UI_ResultGame.instance.ShowResult();
            UI_PauseGame.instance.ShowResult();

            player.gameObject.SetActive(true);
            playerCamera.SetActive(true);

            animator.SetTrigger("Start");
            GameManager.instance.SaveGame();
        }

    }


}
