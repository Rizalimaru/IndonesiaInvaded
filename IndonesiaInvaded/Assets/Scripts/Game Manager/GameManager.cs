using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

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
    
    private GameData gameData;
    private List<IDataPersistent> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileId = "";
    static int s_CurrentEpisode = -1;
    static int s_CurrentLevel = -1;
    public static GameManager instance { get; private set; }

    private void Awake() 
    {
        if (instance != null) 
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence) 
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

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
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void ChangeSelectedProfileId(string newProfileId) 
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

    public void DeleteProfileData(string profileId) 
    {
        dataHandler.Delete(profileId);
        InitializeSelectedProfileId();
        LoadGame();
    }

    private void InitializeSelectedProfileId() 
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
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
        if (disableDataPersistence) 
        {
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData == null && initializeDataIfNull) 
        {
            NewGame();
        }

        if (this.gameData == null) 
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        foreach (IDataPersistent dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistence) 
        {
            return;
        }

        if (this.gameData == null) 
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        foreach (IDataPersistent dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(gameData);
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<IDataPersistent> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistent> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistent>();

        return new List<IDataPersistent>(dataPersistenceObjects);
    }

    public bool HasGameData() 
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData() 
    {
        return dataHandler.LoadAllProfiles();
    }


    public void NextLevel()
    {
#if UNITY_EDITOR
        //in editor if we didn't found the current episode or level, mean we are playing a test scene not part of the
        //game database list, so calling next level is the same as restarting level
        if (s_CurrentEpisode < 0 || s_CurrentLevel < 0)
        {
            var asyncOp = EditorSceneManager.LoadSceneAsyncInPlayMode(EditorSceneManager.GetActiveScene().path, new LoadSceneParameters(LoadSceneMode.Single));
            return;
        }
#endif
        
        
        s_CurrentLevel += 1;

        if (GameDatabase.Instance.episodes[s_CurrentEpisode].scenes.Length <= s_CurrentLevel)
        {
            s_CurrentLevel = 0;
            s_CurrentEpisode += 1;
        }

        if (s_CurrentEpisode >= GameDatabase.Instance.episodes.Length)
            s_CurrentEpisode = 0;

#if UNITY_EDITOR
        var op = EditorSceneManager.LoadSceneAsyncInPlayMode(GameDatabase.Instance.episodes[s_CurrentEpisode].scenes[s_CurrentLevel], new LoadSceneParameters(LoadSceneMode.Single));
#else
        SceneManager.LoadScene(GameDatabase.Instance.episodes[s_CurrentEpisode].scenes[s_CurrentLevel]);
#endif
    }
}
