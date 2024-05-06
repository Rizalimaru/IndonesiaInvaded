using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public GameObject destroyed;
    private ObjectiveManager objectiveManager;
    public GameObject hpOrbPrefab;
    public GameObject spOrbPrefab;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionRadius = 3f;


    // Persentase drop orb
    [SerializeField] private int emptyChance = 50;
    [SerializeField] private int hpOrbChance = 25;
    [SerializeField] private int spOrbChance = 25;
    private void Start()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>(); // Mengubah pencarian kelas
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Add score when player destroys the object
            ScoreManager.instance.AddScore(500);

            DestroyObject();
            if (objectiveManager != null) // Mengubah nama variabel
            {
                objectiveManager.DestroyBox(); // Mengubah nama metode
            }
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
            Debug.Log("Dropping HP Orb!");
            GameObject hpOrb = Instantiate(hpOrbPrefab, transform.position, Quaternion.identity);
            Debug.Log("HP Orb position: " + hpOrb.transform.position);
        }
        else
        {
            // Drop SP Orb
            Debug.Log("Dropping SP Orb!");
            GameObject spOrb = Instantiate(spOrbPrefab, transform.position, Quaternion.identity);
            Debug.Log("SP Orb position: " + spOrb.transform.position);
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
