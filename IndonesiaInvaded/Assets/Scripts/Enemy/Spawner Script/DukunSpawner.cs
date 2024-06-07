using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DukunSpawner : MonoBehaviour
{

    public Transform sp;
    public Boss obj;
    private Collider col;

    private BossCutsceneCamera cam;

    private void Awake()
    {
        col = GetComponent<Collider>();
        cam = FindObjectOfType<BossCutsceneCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossCutsceneCamera.instance.TriggerSpawnBoss();

            Debug.Log("Spawning Dukun");

            SpawnEnemy(obj, sp.position);

            col.enabled = false;
        }
    }

    private void SpawnEnemy(Boss bossToSpawn, Vector3 spawnPos)
    {
        Boss boss = bossToSpawn.GetComponent<Boss>();

        Instantiate(boss, spawnPos, boss.transform.rotation);

        boss.agent.enabled = true;

    }
}
