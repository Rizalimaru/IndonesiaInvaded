using UnityEngine;

public class SwordVFXSpawnerManager : MonoBehaviour
{
    public GameObject hitVFX;

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.collider.CompareTag("Sword"))
        {
            spawnVfxhit();
        }
    }

    void spawnVfxhit()
    {
        GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        Destroy(vfx, .5f);
    }
}
