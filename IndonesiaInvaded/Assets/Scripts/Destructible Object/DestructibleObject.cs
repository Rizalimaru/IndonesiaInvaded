using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public GameObject destroyed;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionRadius = 3f;
    

    private void OnMouseDown()
    {
        DestroyObject();
    }

    private void DestroyObject()
    {
       Instantiate(destroyed, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        Destroy(gameObject);
    }
}
