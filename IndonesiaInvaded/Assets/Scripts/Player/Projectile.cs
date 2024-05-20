using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public bool isShooting = false;
    private Vector3 targetDirection;

    void Update()
    {
        if (isShooting)
        {
            transform.position += targetDirection * speed * Time.deltaTime;
        }
    }

    public void Shoot()
    {
        GameObject enemy = FindClosestEnemy();
        if (enemy != null)
        {
            targetDirection = (enemy.transform.position - transform.position).normalized;
            isShooting = true;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Ranged Kena!!!");
            Destroy(gameObject);
        }
    }
}
