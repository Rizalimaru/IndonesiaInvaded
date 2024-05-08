using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{

    public Transform targetPlayer;
    public List<Enemy> enemyType = new List<Enemy>();
    public Transform[] spawnPoint;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spawning Enemy");
            for (int i = 0; i < enemyType.Count; i++)
            {
                SpawnEnemy(enemyType[i], spawnPoint[i].position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void SpawnEnemy(Enemy enemyToSpawn, Vector3 spawnPos)
    {
        Enemy enemy = enemyToSpawn.GetComponent<Enemy>();

        Instantiate(enemy, spawnPos, Quaternion.identity);

        enemy.Agent.enabled = true;
    }
}
