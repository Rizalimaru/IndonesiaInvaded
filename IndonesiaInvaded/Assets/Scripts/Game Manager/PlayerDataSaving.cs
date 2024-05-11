using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataSaving : MonoBehaviour
{    
    public static PlayerDataSaving instance;
    Vector2 look;
    internal Vector3 velocity;
    private Transform playerTransform;

    [Header("Animator ReSpawn")]
    public Animator animatorReSpawn;
    

    private void Awake(){
        instance = this;
    }
    private void Start() 
    {
        playerTransform = transform;
    }
    private void Update() 
    {
        if (InputManager.instance.GetExitPressed()) 
        {
            GameManager.instance.SaveGame();
            SceneManager.LoadSceneAsync("MainMenu");
        }

    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        Physics.SyncTransforms();
        look.x = rotation.eulerAngles.y;
        look.y = rotation.eulerAngles.z;
        velocity = Vector3.zero;
    }

    public void ReSpawn()
    {
        Vector3 reSpawnPosition = GameManager.instance.GetLastCheckpointPosition();
        playerTransform.position = reSpawnPosition;

        Debug.Log("Player reSpawned at checkpoint position.");
    }
}
