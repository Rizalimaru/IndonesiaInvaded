using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Instance already exists");
        }
            instance = this;
            DontDestroyOnLoad(gameObject);
        
    }
}
