using UnityEngine;

public class AttackPrefabScript : MonoBehaviour
{

    public float force;
    public float decayTime;
    public Rigidbody rb;
    public Collider col;

    public objectTag tagOption;

    public enum objectTag
    {
        Enemy,
        Player
    }

    void Awake()
    {

        rb = GetComponent<Rigidbody>();
        col = col.GetComponent<Collider>();

        rb.AddForce(transform.forward * force);

        GameObject.Destroy(gameObject, decayTime);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;

        if (other.CompareTag("Player"))
        {
            GameObject.Destroy(gameObject);
        }
        
    }

    
}
