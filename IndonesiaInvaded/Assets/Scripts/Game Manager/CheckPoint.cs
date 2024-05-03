using UnityEngine;

public class Checkpoint : MonoBehaviour,IDataPersistent
{
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SaveGame();
            Debug.Log("Checkpoint Saved Game");

        }
    }

    public void LoadData(GameData data){
        player.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data){
        data.playerPosition = player.transform.position;  

    }

}
