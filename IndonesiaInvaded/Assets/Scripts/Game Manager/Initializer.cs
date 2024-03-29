using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Initializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void Execute(){
        Debug.Log("Loaded by the persist Object from the Initializer Script");
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("GameManager")));
    }
}
