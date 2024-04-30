using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_TriggeredResult : MonoBehaviour
{

    public GameObject triggeredResult;
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
            triggeredResult.SetActive(true);
            UI_ResultGame.instance.ShowResult();
        }
    }
}
