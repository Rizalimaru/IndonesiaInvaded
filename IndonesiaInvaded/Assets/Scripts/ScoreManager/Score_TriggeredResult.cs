using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_TriggeredResult : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UI_ResultGame.instance.ShowResult();
            UI_PauseGame.instance.ShowResult();
        }
    }
}
