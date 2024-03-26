using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public sealed class GameManager
{
    private bool isGamePaused = false;
    private bool isGameOver = false;

    // Singeleton Instance
    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    // Save Game
    public void SaveGame(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savegame.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // Load Game
    public GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/savegame.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    // Pause Game
     public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

    // Game Over
    public void HandleGameOver()
    {
        isGameOver = true;
        // game over UI
    }

    public bool IsPaused()
    {
        return isGamePaused;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}

[System.Serializable]
public class GameData
{
    // Add fields that you want to save/load here
    public float playerHealth;
    public int experiencePoints;
    public int skillLevel;
    public Vector3 playerPosition;
    public int playerScore;

    public GameData(int score, float health, Vector3 position, int level, int xp)
    {
        playerScore = score;
        playerHealth = health;
        playerPosition = position;
        skillLevel = level;
        experiencePoints = xp;
    }
}
