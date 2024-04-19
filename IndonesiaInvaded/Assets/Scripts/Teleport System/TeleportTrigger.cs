using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  TeleportTrigger: TriggerInteractionBase
{
    public enum PortalSpawnAt{
        None,
        One,
    }


    [Header("Spawn TO")]
    public PortalSpawnAt portalSpawnTo;
    public SceneField sceneToLoad;
    
    [Space(10f)]
    [Header("This Door")]
    public PortalSpawnAt currentPortalPosition;

    public void Interact()
    {
        SceneSwapManager.SwapSceneFromDoorUse(sceneToLoad, portalSpawnTo);
    }
}
