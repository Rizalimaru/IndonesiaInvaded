using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInstant : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] Animator animator;
    [SerializeField] PlayerDataSaving player;
    public GameObject playerCamera;
   
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
   
            StartCoroutine(LoadLevel());
        }

    }

    IEnumerator LoadLevel()
    {
        animator.SetTrigger("End");
        player.gameObject.SetActive(false);
        playerCamera.SetActive(false);
        AudioManager.Instance.StopBackgroundMusicWithTransition("Game", 1f);
        yield return new WaitForSeconds(0.5f);
        player.Teleport(destination.position, destination.rotation);
        player.gameObject.SetActive(true);
        playerCamera.SetActive(true);
        animator.SetTrigger("Start");
        GameManager.instance.SaveGame();
        AudioManager.Instance.PlayBackgroundMusicWithTransition("GameJakarta", 0, 1f);
    }
}
