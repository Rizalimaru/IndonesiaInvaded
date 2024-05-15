using UnityEngine;

public class RangedBullet : MonoBehaviour
{

    public float force;
    public float decayTime;
    public Rigidbody rb;
    public Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = col.GetComponent<Collider>();

        Invoke("enableCollider", 0.6f);
        rb.AddForce(transform.forward * force);

        GameObject.Destroy(gameObject, decayTime);
    }

    public void enableCollider()
    {
        col.enabled = true;
    }

    void Update()
    {
        
    }
}
