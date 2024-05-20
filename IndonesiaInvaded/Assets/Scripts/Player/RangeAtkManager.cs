using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAtkManager : MonoBehaviour
{
    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;
    public GameObject spawner4;

    public GameObject projectile;

    KeyCode rangedAtkKey = KeyCode.Mouse1;
    KeyCode fireKey = KeyCode.Mouse0;

    // References to the current projectiles
    private GameObject currentProjectile1;
    private GameObject currentProjectile2;
    private GameObject currentProjectile3;
    private GameObject currentProjectile4;

    private float detectionRadius;

    private bool rangeAtkAktif;

    void Update()
    {
        detectionRadius = projectile.GetComponent<Projectile>().detectionRadius;
        rangeAtkAktif = Input.GetKey(rangedAtkKey);

        if (rangeAtkAktif)
        {
            StartCoroutine(spawnerManager());
        }
        else
        {
            DestroyProjectiles();
        }

        if (Input.GetKeyDown(fireKey))
        {
            ShootProjectile();
        }
    }

    IEnumerator spawnerManager()
    {
        // Check and spawn projectiles only if the current ones are destroyed
        if (currentProjectile1 == null)
        {
            currentProjectile1 = Instantiate(projectile, spawner1.transform.position, spawner1.transform.rotation);
            currentProjectile1.GetComponent<Projectile>().FollowSpawner(spawner1.transform);
        }
        yield return new WaitForSeconds(0.1f);

        if (currentProjectile2 == null)
        {
            currentProjectile2 = Instantiate(projectile, spawner2.transform.position, spawner2.transform.rotation);
            currentProjectile2.GetComponent<Projectile>().FollowSpawner(spawner2.transform);
        }
        yield return new WaitForSeconds(0.1f);

        if (currentProjectile3 == null)
        {
            currentProjectile3 = Instantiate(projectile, spawner3.transform.position, spawner3.transform.rotation);
            currentProjectile3.GetComponent<Projectile>().FollowSpawner(spawner3.transform);
        }
        yield return new WaitForSeconds(0.1f);

        if (currentProjectile4 == null)
        {
            currentProjectile4 = Instantiate(projectile, spawner4.transform.position, spawner4.transform.rotation);
            currentProjectile4.GetComponent<Projectile>().FollowSpawner(spawner4.transform);
        }
    }

    void DestroyProjectiles()
    {
        if (currentProjectile1 != null)
        {
            Destroy(currentProjectile1);
            currentProjectile1 = null;
        }
        if (currentProjectile2 != null)
        {
            Destroy(currentProjectile2);
            currentProjectile2 = null;
        }
        if (currentProjectile3 != null)
        {
            Destroy(currentProjectile3);
            currentProjectile3 = null;
        }
        if (currentProjectile4 != null)
        {
            Destroy(currentProjectile4);
            currentProjectile4 = null;
        }
    }

    void ShootProjectile()
    {
        if (currentProjectile1 != null && !currentProjectile1.GetComponent<Projectile>().isShooting)
        {
            currentProjectile1.GetComponent<Projectile>().Shoot();
            return;
        }
        if (currentProjectile2 != null && !currentProjectile2.GetComponent<Projectile>().isShooting)
        {
            currentProjectile2.GetComponent<Projectile>().Shoot();
            return;
        }
        if (currentProjectile3 != null && !currentProjectile3.GetComponent<Projectile>().isShooting)
        {
            currentProjectile3.GetComponent<Projectile>().Shoot();
            return;
        }
        if (currentProjectile4 != null && !currentProjectile4.GetComponent<Projectile>().isShooting)
        {
            currentProjectile4.GetComponent<Projectile>().Shoot();
            return;
        }
    }
}
