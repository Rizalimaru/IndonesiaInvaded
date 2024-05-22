using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordVFXSpawnerManager : MonoBehaviour
{
    public GameObject hitVFX;

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
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
