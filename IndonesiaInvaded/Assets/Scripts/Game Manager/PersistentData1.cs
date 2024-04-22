using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData1 : MonoBehaviour
{
    public static PersistentData1 instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Instance already exists");
            Destroy(gameObject);
        }
            instance = this;
            DontDestroyOnLoad(gameObject);
        
    }
}
