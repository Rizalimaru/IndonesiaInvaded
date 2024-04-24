using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector]
    public string objectID;

    private void Awake()
    {
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }
    
    void Start()
    {
        DontDestroy[] dontDestroyObjects = Object.FindObjectsOfType<DontDestroy>();
        for(int i=0; i < dontDestroyObjects.Length; i++)
        {
            if(dontDestroyObjects[i].objectID == objectID)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
