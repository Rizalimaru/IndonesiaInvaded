using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCheck : MonoBehaviour
{
    public string exitName;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetString("LastExitName", exitName);
            SceneController.instance.NextLevel();
        }
    }
}
