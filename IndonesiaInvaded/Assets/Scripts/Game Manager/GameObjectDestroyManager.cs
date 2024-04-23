using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectDestroyManager : MonoBehaviour
{
    public static GameObjectDestroyManager instance;
    public GameObject gameObjectToDestroy;
    public SceneField sceneToDestroy;


   private void Awake()
    {
        if (instance != null)
        {
            instance = this;
            // Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
    }
    public void DestroyGameObject()
    {
        if(SceneManager.GetActiveScene().name == sceneToDestroy)
        {
            Destroy(gameObjectToDestroy);
        }
        if(SceneManager.GetActiveScene().name != sceneToDestroy)
        {
            Instantiate(gameObjectToDestroy);
        }
    }
}
