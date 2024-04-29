using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public long lastUpdate;
    public List<string> passedScenes;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> monsterKilled;
    

    public GameData()
    {
        playerPosition = Vector3.zero;
        monsterKilled = new SerializableDictionary<string, bool>();
    }
    public int GetPercentageCompelete(){
        int totalCollected = 0;
        foreach (bool collected in monsterKilled.Values)
        {
            if(collected){
                totalCollected++;
            }
        }
        int percentageComplete = -1;
        if(monsterKilled.Count != 0){
            percentageComplete = (totalCollected * 100)  / monsterKilled.Count;
        }

        return percentageComplete;
    }
}
