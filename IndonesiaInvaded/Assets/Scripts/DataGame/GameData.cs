using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public long lastUpdated;
    public Vector3 playerPosition;
    public Checkpoint checkpoint;
    public PlayerAttribut playerAttribut;
    public SerializableDictionary<string, bool> enemy;
    
    public GameData() 
    {
        playerPosition = Vector3.zero;
        enemy = new SerializableDictionary<string, bool>();
        playerAttribut = new PlayerAttribut();
        checkpoint  = null;
        lastUpdated = DateTime.Now.Ticks;
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

}
