using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float detectionRadius = 50f; // Define the detection radius
    public bool isShooting = false;
    private Vector3 targetDirection;
    private Transform spawnerTransform;
    private bool followSpawner = false;

    void Update()
    {
        if (isShooting)
        {
            transform.position += targetDirection * speed * Time.deltaTime;
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
            RotateTowardsEnemy(targetDirection);
            isShooting = true;
            followSpawner = false; // Stop following the spawner when shooting
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = detectionRadius;
        Vector3 currentPos = transform.position;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, currentPos);
            if (distance < minDistance)
            {
                closest = enemy;
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
        float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Ranged Kena!!!");
            Destroy(gameObject);
        }
    }
}
