using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataSaving : MonoBehaviour, IDataPersistent
{    
    Vector2 look;
    internal Vector3 velocity;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void LoadData(GameData data) 
    {
        if (data != null)
        {
            this.transform.position = data.playerPosition;
        }
        else
        {
            Debug.LogWarning("No saved data found for player position.");
        }
    }

    public void SaveData(GameData data) 
    {
        if (data != null && gameManager != null && gameManager.GetGameData().currentCheckpointIndex != 0)
        {
            data.playerPosition = this.transform.position;
        }
        else
        {
            Debug.LogWarning("No GameData instance provided for saving player position, or checkpoint not reached yet.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("dead")){
            Respawn();
        }
    }

    private void Respawn() 
    {
        if (gameManager != null && gameManager.HasGameData() && gameManager.GetGameData().currentCheckpointIndex != 0)
        {
            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.GetComponent<Checkpoint>().checkpointIndex == gameManager.GetGameData().currentCheckpointIndex)
                {
                    Vector3 respawnPosition = checkpoint.transform.position;
                    transform.position = respawnPosition;
                    return;
                }
            }
        }
        transform.position = Vector3.zero;
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        Physics.SyncTransforms();
        look.x = rotation.eulerAngles.y;
        look.y = rotation.eulerAngles.z;
        velocity = Vector3.zero;
    }
}
