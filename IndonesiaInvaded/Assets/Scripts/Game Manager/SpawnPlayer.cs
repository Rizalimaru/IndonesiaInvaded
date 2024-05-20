using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public static SpawnPlayer instance;
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private void Awake()
    {
        instance = this;
    }
    public void Spawn()
    {
        if (playerPrefab != null && spawnPoint != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Player Prefab or Spawn Point not set!");
        }
    }
}
