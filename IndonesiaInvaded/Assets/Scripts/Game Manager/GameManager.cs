
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{

    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTime = 60f;

    private GameData gameData;
    public List<IDataPersistent> dataPersistenceObjects;
    private FileDataHandler fileDataHandler;
    private string selectedProfileId = "";
    private Coroutine autoSaveCoroutine;
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one Game Manager instance");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }
        
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObject();
        LoadGame();
        
        if (autoSaveCoroutine != null) 
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    public void ChangeSelectedProfile(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        fileDataHandler.Delete(profileId);
        InitializeSelectedProfileId();
        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = fileDataHandler.GetMostRecentlyUpdateProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load(selectedProfileId);

        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if (this.gameData == null)
        {
            Debug.Log("No data was found, creating a new game data object");
            return;
        }

        foreach (IDataPersistent dataPersistentObj in dataPersistenceObjects)
        {
            dataPersistentObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found, creating a new game data object");
            return;
        }
        foreach (IDataPersistent dataPersistentObj in dataPersistenceObjects)
        {
            dataPersistentObj.SaveData(gameData);
        }
        gameData.lastUpdate = System.DateTime.Now.ToBinary();
        fileDataHandler.Save(gameData, selectedProfileId);
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }

    public List<IDataPersistent> FindAllDataPersistenceObject()
    {
        IEnumerable<IDataPersistent> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistent>();

        return new List<IDataPersistent>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfileGameData()
    {
        return fileDataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(autoSaveTime);
            SaveGame();
            Debug.Log("Auto Saved Game");
        }
    }
}

