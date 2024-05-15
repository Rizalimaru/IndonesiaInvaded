using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public Dictionary<string, bool> enemyCollected;
    public List<int> unlockedLevels = new List<int>();
    public int score;
    public int enemyDefeats;
    public int bossDefeats;
    public int bonus;
    public float time;
    public int totalScore;
    public int highScore;

    public GameData()
    {
        enemyCollected = new Dictionary<string, bool>();
        this.unlockedLevels = new List<int>();
    }

    public int CalculateTotalScore(int score, int enemyDefeats, int bossDefeats, int bonus)
    {
        int totalScore = score + (enemyDefeats * 1000) + (bossDefeats * 5000) + bonus;
        return totalScore;
    }


}
