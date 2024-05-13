using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckPointManager.instance.SetCheckPoint(transform.position);
            Destroy(gameObject);
        }
    }
}
