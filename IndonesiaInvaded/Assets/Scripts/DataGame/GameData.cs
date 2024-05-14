using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public Dictionary<string, bool> enemyCollected;
    public int highScore;
    public int totalScore;
    public List<int> unlockedLevels = new List<int>();
    public GameData()
    {
        this.lastUpdated = DateTime.Now.Ticks;
        this.highScore = 0;
        this.totalScore = 0;
        enemyCollected = new Dictionary<string, bool>();
        this.unlockedLevels = new List<int>();
    }

    public int GetHighScore()
    {
        highScore = Math.Max(highScore, totalScore);
        return highScore;
    }
}
