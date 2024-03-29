using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerInteractionBase : MonoBehaviour, IInteractable
{
    public GameObject player { get; set; }   
    public bool canInteract { get; set; }

    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update(){
        if(canInteract){
            if(Input.GetKeyDown(KeyCode.E)){
                Interact();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player){
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player){
            canInteract = false;
        }
    }

    public virtual void Interact() {}
}
