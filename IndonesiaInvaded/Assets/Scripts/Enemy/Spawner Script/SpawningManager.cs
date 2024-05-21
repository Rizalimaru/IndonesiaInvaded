using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public static SpawningManager instance;

    public GameObject[] wall;
    public Transform targetPlayer;
    public List<Enemy> enemyType = new List<Enemy>();
    public Transform[] spawnPoint;
    public GameObject objectSelf;

    private Collider col;
    private bool isFinished = false;

    private void Update()
    {
        int enemyNum = GetCurrentEnemy();
        if(enemyNum == 0 && isFinished == true)
        {
            DissolveWall.instance.DissolveWallFunction();
            Destroy(objectSelf,2f);
        }
    }

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            DissolveWall.instance.UnDissolveWallFunction();
            
            Debug.Log("Spawning Enemy");
            for (int i = 0; i < enemyType.Count; i++)
            {
                SpawnEnemy(enemyType[i], spawnPoint[i].position);
            }
            

            // Sekaligus mengaktifkan wall tanpa harus menyebutkan satu per satu
            for (int i = 0; i < wall.Length; i++)
            {
                wall[i].SetActive(true);
            }

            ObjectiveManager.instance.StartObjective();

            Invoke("isFinishedEnabler", 2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            col.enabled = false;
        }
    }

    private void SpawnEnemy(Enemy enemyToSpawn, Vector3 spawnPos)
    {
        Enemy enemy = enemyToSpawn.GetComponent<Enemy>();

        Instantiate(enemy, spawnPos, Quaternion.identity);

        enemy.Agent.enabled = true;

    }

    private void isFinishedEnabler()
    {
        isFinished = true;
    }

    private int GetCurrentEnemy()
    {
        int enemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        return enemy;
    }
}
