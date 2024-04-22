
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Space(10)]

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;


    public List<IDataPersistent> dataPersistenceObjects;
    private FileDataHandler fileDataHandler;
    private string selectedProfileId = "";

    private GameData gameData;
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
            this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            this.selectedProfileId = fileDataHandler.GetMostRecentlyUpdateProfileId();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObject();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void ChangeSelectedProfile(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load(selectedProfileId);

        if(this.gameData == null && initializeDataIfNull)
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
        foreach(IDataPersistent dataPersistentObj in dataPersistenceObjects)
        {
            dataPersistentObj.SaveData(ref gameData);
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
        IEnumerable<IDataPersistent> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistent>();

        return new List<IDataPersistent>(dataPersistenceObjects);
    }
    
    public bool HasGameData(){
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfileGameData()
    {
        return fileDataHandler.LoadAllProfiles();
    }

}

