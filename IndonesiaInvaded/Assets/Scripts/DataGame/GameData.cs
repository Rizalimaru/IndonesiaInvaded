using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public int score;
    public int enemyDefeats;
    public int bossDefeats;
    public int bonus;
    public float time;
    public int totalScore;
    public int highScore;
    public string rank;
    public List<int> unlockedLevels = new List<int>();
    public Dictionary<string, GameData> playerData;
    public Dictionary<string, bool> enemyCollected;
    public GameData()
    {
        enemyCollected = new Dictionary<string, bool>();
        this.unlockedLevels = new List<int>();
        playerData = new Dictionary<string, GameData>();

    }

    public int CalculateTotalScore()
    {
        return score + (enemyDefeats * 1000) + (bossDefeats * 5000) + bonus;
    }
    public int UpdateHighScore()
    {
        totalScore = CalculateTotalScore();
        if (totalScore > highScore)
        {
            highScore = totalScore;
        }
        return highScore;
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
    public string UpdateRank()
    {
        rank = GetRank();

        return rank;
    }


    public void SavePlayerData(string profileId)
    {
        playerData[profileId] = this;
    }
    public GameData GetPlayerData(string profileId)
    {
        return playerData.ContainsKey(profileId) ? playerData[profileId] : null;
    }


    public string GetHighScore(string profileId)
    {
        GameData data = GetPlayerData(profileId);
        return data != null ? data.highScore.ToString() : "N/A";
    }

    public string GetPlayerRank(string profileId)
    {
        GameData data = GetPlayerData(profileId);
        return data != null ? data.rank : "N/A";
    }

}