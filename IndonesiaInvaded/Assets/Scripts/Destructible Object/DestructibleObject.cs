using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public GameObject destroyed;
    private Combat combat;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionRadius = 3f;

    [SerializeField] private int hpAmount = 200; // Jumlah HP yang akan ditambahkan
    [SerializeField] private int spAmount = 50; // Jumlah SP yang akan ditambahkan
    [SerializeField] private GameObject healVFXPrefab; // Prefab untuk VFX heal

    private void Start()
    {
        combat = Combat.instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;

        if (other.gameObject.CompareTag("Sword") && combat.isAttacking)
        {
            // Add score when player destroys the object
            ScoreManager.instance.AddScore(500);
            AudioManager._instance.PlaySFX("DestructibleObject", 0);

            HealPlayerAndPlayVFX(other);

            DestroyObject();
        }
    }

    private void HealPlayerAndPlayVFX(Collider swordCollider)
    {
        // Assuming the player's parent object is the one wielding the sword
        GameObject player = swordCollider.transform.root.gameObject;
        PlayerAttribut playerAttributes = player.GetComponent<PlayerAttribut>();

        if (playerAttributes != null)
        {
            AudioManager._instance.PlaySFX("DestructibleObject", 1);
            playerAttributes.RegenHPOrb(hpAmount);
            playerAttributes.RegenSPOrb(spAmount);
            Debug.Log("Player received HP and SP directly from destructible object.");

            // Instantiate VFX at player's position
            if (healVFXPrefab != null)
            {
                Instantiate(healVFXPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    private void DestroyObject()
    {
        // Instantiate destroyed object
        Instantiate(destroyed, transform.position, Quaternion.identity);

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
