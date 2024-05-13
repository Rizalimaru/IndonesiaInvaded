using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public Dictionary<string, bool> enemyCollected;
    public List<int> unlockedLevels = new List<int>();
    public GameData()
    {
        enemyCollected = new Dictionary<string, bool>();
        this.unlockedLevels = new List<int>();
    }


}
