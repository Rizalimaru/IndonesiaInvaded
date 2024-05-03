using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_TriggeredResult : MonoBehaviour
{
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UI_ResultGame.instance.ShowResult();
            UI_PauseGame.instance.ShowResult();
        }
    }
}
