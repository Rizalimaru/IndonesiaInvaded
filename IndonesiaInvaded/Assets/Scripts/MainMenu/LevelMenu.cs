using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public SceneField scene;
public void LoadLevel(){
        SceneManager.LoadSceneAsync(scene);
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }
    }
