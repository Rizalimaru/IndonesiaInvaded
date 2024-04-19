using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLast : MonoBehaviour
{
    public string lastExitName;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("LastExitName") == lastExitName){
            PersistentData.instance.transform.position = transform.position;
            PersistentData.instance.transform.eulerAngles = transform.eulerAngles;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
