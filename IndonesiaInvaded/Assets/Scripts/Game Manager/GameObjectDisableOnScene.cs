using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectDisableOnScene : MonoBehaviour
{
    public SceneField[] sceneToEnable;
    public SceneField sceneToDisable;
    public GameObject[] objectsToDisable;

    void Start()
    {
        // Mendaftarkan metode OnSceneLoaded sebagai delegat yang akan dipanggil ketika scene dimuat.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Menonaktifkan GameObject saat memulai scene.
        DisableObjects();
    }

    void OnDestroy()
    {
        // Menghapus delegat agar tidak terjadi kebocoran memori.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == sceneToDisable.SceneName)
        {
            DisableObjects();
            
        }

        // Mengaktifkan kembali GameObject saat scene baru dimuat.
        foreach (SceneField sceneField in sceneToEnable)
        {
            if (scene.name == sceneField.SceneName)
            {
                EnableObjects();
                break;
            }
        }

    }

    void DisableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }

    void EnableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(true);
        }
    }
}
