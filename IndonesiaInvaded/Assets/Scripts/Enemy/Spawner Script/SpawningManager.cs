using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public static SpawningManager instance;

    public GameObject[] wall;
    public List<Enemy> enemyType = new List<Enemy>();
    public Transform[] spawnPoint;
    public GameObject objectSelf;

    private Collider col;
    private bool isFinished = false;

    private bool isCutSceneTriggered = false;


    private void Start()
    {
        //mencari gameobject dengan tag Portal 
        
    }

    private void Update()
    {
        int enemyNum = GetCurrentEnemy();
        if(enemyNum == 0 && isFinished == true && !isCutSceneTriggered)
        {
            DissolveWall.instance.DissolveWallFunction();

            EnvironmentCutSceneJakarta.instance.CutSceneJakartaCount();

            AudioManager._instance.TransitionToBackgroundMusic();

            
            Destroy(objectSelf,3f);

            isCutSceneTriggered = true;

            if (EnvironmentCutSceneJakarta.instance.cutSceneJakarta == 6)
            {
                EnvironmentCutSceneJakarta.instance.CutScenePortal();
            }
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

            AudioManager._instance.TransitionToBattleMusic();

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
