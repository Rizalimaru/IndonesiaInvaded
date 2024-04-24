using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public GameObject destroyed;
    public GameObject hpOrbPrefab;
    public GameObject spOrbPrefab;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionRadius = 3f;

    // Persentase drop orb
    [SerializeField] private int emptyChance = 50;
    [SerializeField] private int hpOrbChance = 25;
    [SerializeField] private int spOrbChance = 25;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        // Instantiate destroyed object
        Instantiate(destroyed, transform.position, Quaternion.identity);

        // Random number to determine drop type
        int dropType = Random.Range(1, 101);

        // Determine drop based on chances
        if (dropType <= emptyChance)
        {
            // Do nothing, the box remains empty
        }
        else if (dropType <= emptyChance + hpOrbChance)
        {
            // Drop HP Orb
            Instantiate(hpOrbPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // Drop SP Orb
            Instantiate(spOrbPrefab, transform.position, Quaternion.identity);
        }

        // Apply explosion force
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Destroy the box
        Destroy(gameObject);
    }
}
