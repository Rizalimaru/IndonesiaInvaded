using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public Vector3 playerPosition;
    public Vector3 checkpointPosition;
    public SerializableDictionary<string, bool> monstersCollected;
    public AttributeData playerAttributesData;

    public GameData() 
    {
        playerPosition = Vector3.zero;
        checkpointPosition = Vector3.zero;
        monstersCollected = new SerializableDictionary<string, bool>();
        playerAttributesData = new AttributeData();
    }

    public int GetPercentageComplete() 
    {
        int totalCollected = 0;
        foreach (bool collected in monstersCollected.Values) 
        {
            if (collected) 
            {
                totalCollected++;
            }
        }

        int percentageCompleted = 0;
        if (monstersCollected.Count != 0) 
        {
            percentageCompleted = (totalCollected * 100 / monstersCollected.Count);
        }
        return percentageCompleted;
    }
}
