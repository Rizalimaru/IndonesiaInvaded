using UnityEngine;

public class Checkpoint : MonoBehaviour
{
//     public Vector3 position;
//     public Quaternion rotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SaveGame();
            Debug.Log("Checkpoint Saved Game");

        }
    }
    // public void Initialize()
    // {
    //     // Initialize checkpoint data
    //     position = Vector3.zero;
    //     rotation = Quaternion.identity;
    // }

    // public bool IsReached(Vector3 playerPosition)
    // {
    //     // Check if player has reached the checkpoint
    //     return Vector3.Distance(playerPosition, position) < 0.1f;
    // }

}
