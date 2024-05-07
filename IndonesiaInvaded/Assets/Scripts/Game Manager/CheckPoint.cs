using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.UpdateCheckpoint(checkpointIndex);
            Debug.Log("Checkpoint Reached: " + checkpointIndex);
        }
    }
}
