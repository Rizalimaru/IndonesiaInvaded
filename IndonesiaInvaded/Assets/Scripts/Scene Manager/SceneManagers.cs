using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public static SceneManagers instance;

    [Header("Scenes To Load")]
    public SceneField blockOutJakarta;
    public SceneField blockOutInvert;
    public SceneField blockOutBandung;
    public SceneField MainMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NewGame(){
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }
}