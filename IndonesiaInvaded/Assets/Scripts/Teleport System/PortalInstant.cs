using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInstant : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] Animator animator;
    [SerializeField] PlayerDataSaving player;
    public GameObject playerCamera;
    // public GameObject mainCamera;
    // public GameObject cutSceneCamera;
    // public GameObject uiGame;
    // public bool hasCutScenePlayed = false;

    // private void Start()
    // {
    //     mainCamera.SetActive(true);
    //     playerCamera.SetActive(true);
    //     player.gameObject.SetActive(true);
    //     uiGame.SetActive(true);
    //     cutSceneCamera.SetActive(false);
    // }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
        // if (other.CompareTag("Player") && hasCutScenePlayed == false)
        // {
            // hasCutScenePlayed = true;
            // playerCamera.SetActive(false);
            // mainCamera.SetActive(false);
            // cutSceneCamera.SetActive(true);

            // Invoke("SwitchCam", 4);
            // player.Teleport(destination.position, destination.rotation);
            StartCoroutine(LoadLevel());
        }

    }

    IEnumerator LoadLevel()
    {
        animator.SetTrigger("End");
        // hasCutScenePlayed = true;
        ObjectDisable();

        AudioManager.Instance.StopBackgroundMusicWithTransition("Game", 1f);

        yield return new WaitForSeconds(0.5f);

        player.Teleport(destination.position, destination.rotation);

        // cutSceneCamera.SetActive(true);

        animator.SetTrigger("Start");

        // mainCamera.SetActive(false);
        // Invoke("SwitchCam", 5);
        
        GameManager.instance.SaveGame();
        AudioManager.Instance.PlayBackgroundMusicWithTransition("GameJakarta", 0, 1f);
    }

    private void ObjectDisable()
    {
        player.gameObject.SetActive(false);
        playerCamera.SetActive(false);
        // uiGame.SetActive(false);
    }
    // void SwitchCam()
    // {
    //     mainCamera.SetActive(true);
    //     playerCamera.SetActive(true);
    //     player.gameObject.SetActive(true);
    //     uiGame.SetActive(true);
    //     cutSceneCamera.SetActive(false);
    // }
}
