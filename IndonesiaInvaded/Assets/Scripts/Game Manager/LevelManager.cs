using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IDataPersistence
{
    public static LevelManager instance;

    [Header("Unlock Level")]
    public List<int> unlockedLevels = new List<int>();
    public int maxLevelNumber = 4;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Level Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    public void UnlockLevel(int levelNumber)
    {
        if (!unlockedLevels.Contains(levelNumber))
        {
            unlockedLevels.Add(levelNumber);
        }
    }

    public bool IsLevelUnlocked(int levelNumber)
    {
        return unlockedLevels.Contains(levelNumber);
    }

    public void OnCompleteLevel(int levelNumber)
    {
        if (!IsLevelUnlocked(levelNumber))
        {
            UnlockLevel(levelNumber);
        }

        if (levelNumber < maxLevelNumber)
        {
            UnlockLevel(levelNumber + 1);
        }

        GameManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        this.unlockedLevels = data.unlockedLevels;
    }

    public void SaveData(GameData data)
    {
        data.unlockedLevels = this.unlockedLevels;
    }
}
