using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerInteraction : TriggerInteractionBase
{
    public enum DoorToSpawnAt{
        None,
        One,
        Two,
        Three,
        Four,
    }


    [Header("Spawn TO")]
    [SerializeField] private DoorToSpawnAt doorToSpawnTo;
    [SerializeField] private SceneField sceneToLoad;
    
    [Space(10f)]
    [Header("This Door")]
    public DoorToSpawnAt currentDoorPosition;

    public override void Interact()
    {
        SceneSwapManager.SwapSceneFromDoorUse(sceneToLoad, doorToSpawnTo);
    }
}
