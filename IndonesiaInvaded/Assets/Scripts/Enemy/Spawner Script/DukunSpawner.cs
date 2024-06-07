using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DukunSpawner : MonoBehaviour
{
    public GameObject obj;
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
            SpawnEnemy(obj);

            BossCutsceneCamera.instance.TriggerSpawnBoss();

            Debug.Log("Spawning Dukun");

            col.enabled = false;
        }
    }

    private void SpawnEnemy(GameObject bossToSpawn)
    {
        Boss boss = bossToSpawn.GetComponent<Boss>();

        bossToSpawn.gameObject.SetActive(true);

        boss.agent.enabled = true;
    }
}
