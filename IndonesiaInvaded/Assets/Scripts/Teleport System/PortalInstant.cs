using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInstant : MonoBehaviour
{
   [SerializeField] Transform destination;
   [SerializeField] Animator animator;
   [SerializeField] PlayerMovement player;
   [SerializeField] GameObject playerCamera;
    

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            StartCoroutine(LoadLevel());
        }

    }

    IEnumerator LoadLevel()
    {
        animator.SetTrigger("End");
        player.gameObject.SetActive(false);
        playerCamera.SetActive(false);
        yield return new WaitForSeconds(1);
        player.Teleport(destination.position, destination.rotation);
        player.gameObject.SetActive(true);
        playerCamera.SetActive(true);
        animator.SetTrigger("Start");
    }
}
