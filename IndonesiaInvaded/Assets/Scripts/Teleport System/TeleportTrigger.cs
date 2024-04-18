using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  TeleportTrigger: TriggerInteractionBase
{
    public enum PortalSpawnAt{
        None,
        One,
        Two,
    }


    [Header("Spawn TO")]
    public PortalSpawnAt portalSpawnTo;
    public SceneField sceneToLoad;
    public string exitName;
    
    [Space(10f)]
    [Header("This Door")]
    public PortalSpawnAt currentPortalPosition;

    public void Interact()
    {
        PlayerPrefs.SetString("LastExitName", exitName);
        SceneSwapManager.SwapSceneFromDoorUse(sceneToLoad, portalSpawnTo);
    }
}
