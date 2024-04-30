using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public int numEnemySpawn;
    public float spawnDelay;
    public List<Enemy> enemyPrefabs = new List<Enemy>();
    public SpawnMethod spawnMethod = SpawnMethod.RoundRobin;
    private NavMeshTriangulation triangulation;


    private Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(enemyPrefabs[i], numEnemySpawn));
        }
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);

        int spawnedEnemies = 0;

        while(spawnedEnemies < numEnemySpawn)
        {
            if(spawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(spawnedEnemies);
            }
            else if(spawnMethod == SpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }

            spawnedEnemies++;

            yield return wait;
        }
    }

    private void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int spawnIndex = spawnedEnemies % enemyPrefabs.Count;

        DoSpawnEnemy(spawnIndex);
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, enemyPrefabs.Count));
    }

    private void DoSpawnEnemy(int spawnIndex)
    {
        PoolableObject poolableObject = EnemyObjectPools[spawnIndex].GetObject();

        if(poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

            int vertexIndex = Random.Range(0, triangulation.vertices.Length);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, -1))
            {
                enemy.Agent.Warp(hit.position);
                enemy.target = player;
                enemy.stateManager.StartAgent();
                enemy.Agent.enabled = true;
            }
            else
            {
                Debug.LogError($"Error in putting prefab in navmesh");
            }

        }
        else
        {
            Debug.LogError($"There is error in DoSpawnEnemy");
        }
    }

    public enum SpawnMethod
    {
        RoundRobin,
        Random
    }
}
