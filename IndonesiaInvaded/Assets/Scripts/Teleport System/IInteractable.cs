using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    GameObject player { get; set; }
    bool canInteract { get; set; }
    void Interact();
}
