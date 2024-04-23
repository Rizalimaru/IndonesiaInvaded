using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectDestroyManager : MonoBehaviour
{
    public static GameObjectDestroyManager instance;
    public GameObject[] gameObjectToDestroy;


    private void Awake()
    {
        if (instance != null)
        {
            instance = this;
            // Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void DestroyGameObject()
    {
        for (int i = 0; i < gameObjectToDestroy.Length; i++)
        {
            Destroy(gameObjectToDestroy[i]);
        }
    }

    public void SpawnGameObject()
    {
        for (int i = 0; i < gameObjectToDestroy.Length; i++)
        {
            Instantiate(gameObjectToDestroy[i]);
        }
    }
}
