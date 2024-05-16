using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public Dictionary<string, bool> enemyCollected;
    public List<int> unlockedLevels = new List<int>();
    public List<int> highScores = new List<int>();
    public List<string> ranks = new List<string>();
    public int score;
    public int enemyDefeats;
    public int bossDefeats;
    public int bonus;
    public float time;
    public int totalScore;
    public int highScore;
    public string rank;

    public GameData()
    {
        enemyCollected = new Dictionary<string, bool>();
        this.unlockedLevels = new List<int>();
        this.highScores = new List<int>();
        this.ranks = new List<string>();
    }

    public int CalculateTotalScore()
    {
        return score + (enemyDefeats * 1000) + (bossDefeats * 5000) + bonus;
    }
    public void UpdateHighScore()
    {
        totalScore = CalculateTotalScore();
        if (totalScore > highScore)
        {
            highScore = totalScore;
        }
    }
    public string GetRank()
    {
        if (totalScore >= 44000)
        {
            return "S"; // S Rank
        }
        else if (totalScore >= 30000 && totalScore <= 43000)
        {
            return "A"; // A Rank
        }
        else if (totalScore >= 20000 && totalScore <= 29999)
        {
            return "B"; // B Rank
        }
        else if (totalScore >= 10000 && totalScore <= 19999)
        {
            return "C"; // C Rank
        }
        else
        {
            return "D"; // D Rank
        }
    }
    public void UpdateRank()
    {
        rank = GetRank();
    }

    public int GetHighScore()
{
    return highScore;
}

public string GetPlayerRank()
{
    return rank;
}


}