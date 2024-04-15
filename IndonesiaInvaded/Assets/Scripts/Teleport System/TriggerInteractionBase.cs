using UnityEngine;
public class TriggerInteractionBase : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TeleportTrigger portalTrigger = GetComponent<TeleportTrigger>();
            if (portalTrigger != null)
            {
                portalTrigger.Interact();
            }
        }
    }
}
