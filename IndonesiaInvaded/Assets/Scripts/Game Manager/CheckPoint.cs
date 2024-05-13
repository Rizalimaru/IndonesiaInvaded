using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        GameManager.instance.SetLastCheckpoint(transform.position);
        Debug.Log("Checkpoint activated!");
    }
}
