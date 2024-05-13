using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public int currentCheckpointIndex;
    public int maxLevelNumber;
    public int levelNumber;
    public Dictionary<string, bool> enemyCollected;
    public List<int> unlockedLevels = new List<int>();
    public GameData()
    {
        this.currentCheckpointIndex = 0;
        this.maxLevelNumber = 0;
        this.levelNumber = 0;
        enemyCollected = new Dictionary<string, bool>();
        this.unlockedLevels = new List<int>();
    }


}
