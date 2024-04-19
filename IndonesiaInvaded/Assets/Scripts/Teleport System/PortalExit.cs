using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalExit : MonoBehaviour
{
   public SceneField sceneField;
   public static bool loadPortal;
   public string exitName;

   public void OnTriggerEnter(Collider other)
   {
      loadPortal = true;
      PlayerPrefs.SetString("LastExitName", exitName);
      
      SceneManager.LoadSceneAsync(sceneField);
   }
}
