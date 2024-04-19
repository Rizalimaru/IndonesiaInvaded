// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor.PackageManager;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public static class Initializer
// {
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

//     public static void Execute(){
//         Debug.Log("Loaded by the persist Object from the Initializer Script");

//         string currentSceneName = SceneManager.GetActiveScene().name;
//         if (currentSceneName == "MainMenu")
//         {
            
//             if (SceneManager.GetActiveScene().buildIndex >= 0)
//             {
//                 Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PERSISTDATAGAME")));
//             }
//         }
//     }
// }
