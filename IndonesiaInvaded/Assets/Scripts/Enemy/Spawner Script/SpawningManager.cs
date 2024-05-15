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


    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.P))
       {
           SpawnEnemy(enemyType[0], spawnPoint[0].position);
       }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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
            ObjectiveManager.instance.StartObjective();

            // Sekaligus mengaktifkan wall tanpa harus menyebutkan satu per satu
            for (int i = 0; i < wall.Length; i++)
            {
                wall[i].SetActive(true);
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

    //hide the wall
    public void HideWall()
    {
        for (int i = 0; i < wall.Length; i++)
        {
            wall[i].SetActive(false);
        }
    }
    
}
