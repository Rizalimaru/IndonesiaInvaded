using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInstant : MonoBehaviour
{
   [SerializeField] Transform destination;
   [SerializeField] Animator animator;
   [SerializeField] PlayerMovement player;
    

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent<PlayerMovement>(out var player)){
            StartCoroutine(LoadLevel());
        }

    }

    IEnumerator LoadLevel()
    {
        
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        player.gameObject.SetActive(false);
        player.Teleport(destination.position, destination.rotation);
        player.gameObject.SetActive(true);
        animator.SetTrigger("Start");
    }
}
