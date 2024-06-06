using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawningManager : MonoBehaviour
{
    public static SpawningManager instance;

    public GameObject[] wall;

    public GameObject vfxSpawnEnemy;
    public List<Enemy> enemyType = new List<Enemy>();
    public Transform[] spawnPoint;
    public GameObject objectSelf;

    private Collider col;
    private bool isFinished = false;

    private bool isCutSceneTriggered = false;


    private void Awake()
    {
        instance = this;
        col = GetComponent<Collider>();

    }

    private void Start()
    {
        //mencari gameobject dengan tag Portal 

    }

    private void Update()
    {
        int enemyNum = GetCurrentEnemy();
        if (enemyNum == 0 && isFinished == true && !isCutSceneTriggered)
        {
            DissolveWall.instance.DissolveWallFunction();

            AudioManager._instance.TransitionToBackgroundMusic();

            Destroy(objectSelf, 3f);

            //Jika scene yang aktif adalah scene gameple1
            if (SceneManager.GetActiveScene().name == "Gameplay1" || SceneManager.GetActiveScene().name == "Level1")
            {
                EnvironmentCutSceneJakarta.instance.CutSceneJakartaCount();
                // Jika jumlah count cutsceneJakarta sama dengan 1 maka akan memanggil cutscene
                if (EnvironmentCutSceneJakarta.instance.cutSceneJakarta == 6)
                {
                    EnvironmentCutSceneJakarta.instance.CutScenePortal();
                    SkillManager.instance.ResetSkills();
                }
            }

            //Jika scene yang aktif adalah scene gameple2 atau scene gameplay3
            if (SceneManager.GetActiveScene().name == "Gameplay2" || SceneManager.GetActiveScene().name == "Level2")
            {
                EnvironmentCutSceneInvert.instance.CutSceneInvertCount();
                // Jika jumlah count cutsceneInvert sama dengan 1 maka akan memanggil cutscene
                if (EnvironmentCutSceneInvert.instance.cutSceneInvert == 5)
                {
                    EnvironmentCutSceneInvert.instance.CutScenePortal();
                    SkillManager.instance.ResetSkills();
                }
            }

            isCutSceneTriggered = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            DissolveWall.instance.UnDissolveWallFunction();

            AudioManager._instance.TransitionToBattleMusic();

            AudioManager._instance.PlaySFX("Teleport", 0);

            for (int i = 0; i < enemyType.Count; i++)
            {
                SpawnEnemy(enemyType[i], spawnPoint[i].position);

                // tambah vfx hehe
                Vector3 spawnPosition = new Vector3(spawnPoint[i].position.x, spawnPoint[i].position.y + 0.15f, spawnPoint[i].position.z);
                vfxSpawnEnemy = Instantiate(vfxSpawnEnemy, spawnPosition, Quaternion.identity);

                Destroy(vfxSpawnEnemy, 2f);

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

    //spawn vfx spawn enemy



    private void isFinishedEnabler()
    {
        isFinished = true;
    }

    private int GetCurrentEnemy()
    {
        int enemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        return enemy;
    }

    public void ResetSpawning()
    {

        // Reset enemy count
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        // Reset walls
        foreach (GameObject wallObject in wall)
        {
            wallObject.SetActive(false);
        }

        // Reset spawn points
        foreach (Transform spawnPointObject in spawnPoint)
        {
            spawnPointObject.gameObject.SetActive(false);
        }

        // Reset VFX
        foreach (GameObject vfxObject in GameObject.FindGameObjectsWithTag("VFXSpawnEnemy"))
        {
            Destroy(vfxObject);
        }
        col.enabled = true;
        // Reset isFinished flag
        isFinished = false;


        // Reset isCutSceneTriggered flag
        isCutSceneTriggered = false;

    }


}
