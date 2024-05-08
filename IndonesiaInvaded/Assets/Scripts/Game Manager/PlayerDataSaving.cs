using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataSaving : MonoBehaviour, IDataPersistent
{    
    Vector2 look;
    internal Vector3 velocity;
    public void LoadData(GameData data) 
    {
            this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data) 
    {
            data.playerPosition = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("dead")){
            Debug.Log("dead");
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
}
