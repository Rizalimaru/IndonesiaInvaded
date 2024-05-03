using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public long lastUpdated;
    public Vector3 playerPosition;
    public PlayerAttribut playerAttribut;
    public SerializableDictionary<string, bool> enemy;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        playerPosition = Vector3.zero;
        enemy = new SerializableDictionary<string, bool>();
        playerAttribut = new PlayerAttribut();
    }

    public int GetPercentageComplete()
    {
        int totalCollected = 0;
        foreach (bool collected in enemy.Values)
        {
            if (collected)
            {
                totalCollected++;
            }
        }

        int percentageCompleted = -1;
        if (enemy.Count != 0)
        {
            percentageCompleted = (totalCollected * 100 / enemy.Count);
        }
        return percentageCompleted;
    }

    public string GetNameLevel()
    {
        if(enemy == null){

        }
        return "";
    }

}
