using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataSaving : MonoBehaviour, IDataPersistent
{    
    public Transform player;
    Vector2 look;
    internal Vector3 velocity;
    

    public void LoadData(GameData data) 
    {
        player.position = data.playerPosition;
    }
    public void SaveData(GameData data) 
    {
         data.playerPosition = player.position;
    }

    public void Update(){
        if(InputManager.instance.GetExitPressed())
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

    private  void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("dead")){
            Respawn();
        }
    }

    private void Respawn() 
    {
        
    }
}
