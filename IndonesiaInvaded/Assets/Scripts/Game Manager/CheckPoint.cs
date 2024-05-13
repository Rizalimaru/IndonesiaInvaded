using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CheckPointManager.instance.AddCheckpoint(this.transform.position);
            Destroy(other.gameObject);
            Debug.Log("Checkpoint : " + this.transform.position);
        }
    }
}
