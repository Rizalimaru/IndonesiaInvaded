using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public static SpawningManager instance;

    public GameObject[] wall;
    public Boss bossObject;
    public Transform spawnPoint;
    public GameObject objectSelf;

    private Collider col;
    private bool isFinished = false;

    private void Update()
    {
        bool isBossDead= CheckIfBossDead();
        if (isBossDead == true && isFinished == true)
        {
            DissolveWall.instance.DissolveWallFunction();
            Destroy(objectSelf, 2f);
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

            SpawnEnemy(bossObject, spawnPoint.position);

            // Sekaligus mengaktifkan wall tanpa harus menyebutkan satu per satu
            for (int i = 0; i < wall.Length; i++)
            {
                wall[i].SetActive(true);
            }

            // ObjectiveManager.instance.StartObjective() --> Ganti yang ini mas Vikrie

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

    private void SpawnEnemy(Boss bossToSpawn, Vector3 spawnPos)
    {
        Boss boss = bossToSpawn.GetComponent<Boss>();

        Instantiate(boss, spawnPos, Quaternion.identity);

        boss.agent.enabled = true;

    }

    private void isFinishedEnabler()
    {
        isFinished = true;
    }

    private bool CheckIfBossDead()
    {
        int numCheck = GameObject.FindGameObjectsWithTag("Boss").Length;
        if(numCheck == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
