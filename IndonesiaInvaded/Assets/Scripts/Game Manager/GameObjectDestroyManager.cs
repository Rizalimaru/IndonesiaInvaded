using System.Collections;
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
            Debug.LogWarning("More than one instance of GameObjectDestroyManager found!");
        }
        else
        {
            instance = this;
        }
    }
    public void DestroyGameObject()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            for (int i = 0; i < gameObjectToDestroy.Length; i++)
            {
                gameObjectToDestroy[i].SetActive(false);
            }

        }
    }

    public void SpawnGameObject()
    {
        for (int i = 0; i < gameObjectToDestroy.Length; i++)
        {
            gameObjectToDestroy[i].SetActive(true);
        }


    }
}
