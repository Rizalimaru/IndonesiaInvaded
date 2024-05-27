using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float detectionRadius = 50f; // Define the detection radius
    public bool isShooting = false;
    private Vector3 targetDirection;
    private Transform spawnerTransform;
    private bool followSpawner = false;

    public float spinSpeed = 360f; // Degrees per second

    void Update()
    {
        if (isShooting)
        {
            transform.position += targetDirection * speed * Time.deltaTime;
            transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime); // Spin the projectile
        }
        else if (followSpawner && spawnerTransform != null)
        {
            transform.position = spawnerTransform.position;
            FaceClosestEnemy();
        }
    }

    public void FollowSpawner(Transform spawner)
    {
        spawnerTransform = spawner;
        followSpawner = true;
    }

    public void Shoot()
    {
        GameObject enemy = FindClosestEnemy();
        if (enemy != null)
        {
            targetDirection = (enemy.transform.position - transform.position).normalized;
            isShooting = true;
            followSpawner = false; // Stop following the spawner when shooting
        }
    }

    GameObject FindClosestEnemy()
    {
        // Find all game objects with tags "Enemy" and "Boss"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");

        // Combine both arrays into one list
        List<GameObject> allTargets = new List<GameObject>();
        allTargets.AddRange(enemies);
        allTargets.AddRange(bosses);

        GameObject closest = null;
        float minDistance = detectionRadius;
        Vector3 currentPos = transform.position;

        // Iterate through all targets to find the closest one
        foreach (GameObject target in allTargets)
        {
            float distance = Vector3.Distance(target.transform.position, currentPos);
            if (distance < minDistance)
            {
                closest = target;
                minDistance = distance;
            }
        }

        return closest;
    }


    void FaceClosestEnemy()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            Vector3 directionToEnemy = (closestEnemy.transform.position - transform.position).normalized;
            RotateTowardsEnemy(directionToEnemy);
        }
    }

    void RotateTowardsEnemy(Vector3 directionToEnemy)
    {
        // Calculate the angle to rotate the projectile's x-axis towards the enemy
        float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") | collision.gameObject.CompareTag("Boss"))
        {
            Debug.Log("Ranged Kena!!!");
            Destroy(gameObject);
        }
    }
}
