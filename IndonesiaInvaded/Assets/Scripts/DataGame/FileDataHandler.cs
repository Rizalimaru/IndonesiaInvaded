using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "kelompok8";
    private readonly string backupExtension = ".bak";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(String profileId, bool allowRestoreFromBackup = true)
    {
        if(profileId == null){
            return null;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        GameData loadedData = null;
        
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if(useEncryption){
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                if (allowRestoreFromBackup) 
                {
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        loadedData = Load(profileId, false);
                    }
                }
                else 
                {
                    Debug.LogError("Error occured when trying to load file at path: " + fullPath  + " and backup did not work.\n" + e);
                }
            }

        }
        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        if(profileId == null){
            return;
        }
        
        string fullPath = Path.Combine(dataDirPath,profileId, dataFileName);
        string backupFilePath = fullPath + backupExtension;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if(useEncryption){
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

           GameData verifiedGameData = Load(profileId);
            if (verifiedGameData != null) 
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            else 
            {
                throw new Exception("Save file could not be verified and backup could not be created.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving data: " + fullPath + "\n " + e);
        }
    }

    public void Delete(string profileId) 
    {
        if (profileId == null) 
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try 
        {
            if (File.Exists(fullPath)) 
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else 
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }
        }
        catch (Exception e) 
        {
            Debug.LogError("Failed to delete profile data for profileId: " 
                + profileId + " at path: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles(){
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();

        foreach(DirectoryInfo dirInfo in dirInfos){
            string profileId = dirInfo.Name;
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if(!File.Exists(fullPath)){
                Debug.LogWarning("No data found for profile: " + profileId);
                continue;
            }

            GameData profileData = Load(profileId);
            if(profileData != null){
                profileDictionary.Add(profileId, profileData);
            }
            else{
                Debug.LogError("Failed to load data for profile: " + profileId);
            
            }
        }
        return profileDictionary;
    }

    public string GetMostRecentlyUpdateProfileId(){
        string mostRecentlyProfileId = null;
        Dictionary<string, GameData> profileGameData = LoadAllProfiles();
        foreach(KeyValuePair<string, GameData> pair in profileGameData){
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if(gameData == null){
                continue;
            }

            if(mostRecentlyProfileId == null){
                mostRecentlyProfileId = profileId;
            }else{
                DateTime mostRecentDateTime = DateTime.FromBinary(profileGameData[mostRecentlyProfileId].lastUpdate);
                DateTime currentDateTime = DateTime.FromBinary(gameData.lastUpdate);

                if(currentDateTime > mostRecentDateTime){
                    mostRecentlyProfileId = profileId;
                }
            }
        }
        return mostRecentlyProfileId;
    }

    private string EncryptDecrypt(string data){
        string modifiedData = "";
        for(int i=0; i<data.Length; i++){
            modifiedData += (char) (data[i] ^ encryptionCodeWord[(i % encryptionCodeWord.Length)]);
        }
        return modifiedData;
    }

    private bool AttemptRollback(string fullPath) 
    {
        bool success = false;
        string backupFilePath = fullPath + backupExtension;
        try 
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            else 
            {
                throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
            }
        }
        catch (Exception e) 
        {
            Debug.LogError("Error occured when trying to roll back to backup file at: " + backupFilePath + "\n" + e);
        }

        return success;
    }

}
