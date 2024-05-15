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
    public string rank;

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

    public int GetHighScore(){
        return highScore;
    }

    public void UpdateHighScore()
    {
        totalScore = CalculateTotalScore(score, enemyDefeats, bossDefeats, bonus);
        if (totalScore > highScore)
        {
            highScore = totalScore;
        }
        else{
            GetHighScore();
        }
    }

    public string GetRank()
    {
        if (highScore >= 44000)
        {
            return "S"; // S Rank
        }
        else if (highScore >= 30000)
        {
            return "A"; // A Rank
        }
        else if (highScore >= 20000)
        {
            return "B"; // B Rank
        }
        else if (highScore >= 10000)
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
}