using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SaveGame();
            Debug.Log("Checkpoint Saved Game");

        }
    }

}
