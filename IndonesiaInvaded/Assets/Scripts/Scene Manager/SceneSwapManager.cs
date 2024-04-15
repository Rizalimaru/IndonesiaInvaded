using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;
    public static bool loadFromPortal;
    private GameObject player;
    private Collider portalCollider;
    private Collider playerCollider;
    private Vector3 playerSpawnPosition;
    
    private TeleportTrigger.PortalSpawnAt portalSpawnTo;

    private void Awake()
{
    if (instance == null)
    {
        instance = this;
    }
    player = GameObject.FindWithTag("Player");
    playerCollider = player.GetComponent<Collider>();
}

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void SwapSceneFromDoorUse(SceneField myScene, TeleportTrigger.PortalSpawnAt portalSpawnAt)
    {
        loadFromPortal = true;
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, portalSpawnAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, TeleportTrigger.PortalSpawnAt portalSpawnAt = TeleportTrigger.PortalSpawnAt.None)
    {
        SceneFadeManager.instance.StartFadeOut();

        while(SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }
        portalSpawnTo = portalSpawnAt;
        SceneManager.LoadScene(myScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.instance.StartFadeIn();
        if(loadFromPortal)
        {
            FindPortal(portalSpawnTo);
            player.transform.position = playerSpawnPosition;
            loadFromPortal = false;
        }
    }

    private void FindPortal(TeleportTrigger.PortalSpawnAt portalSpawnNumber){
        TeleportTrigger[] portal = FindObjectsOfType<TeleportTrigger>();

        for(int i = 0; i< portal.Length; i++){
            if(portal[i].currentPortalPosition == portalSpawnNumber){
                portalCollider = portal[i].gameObject.GetComponent<Collider>();
                CalculateSpawnPosition();
                return;
            }
        }
    }

    private void CalculateSpawnPosition(){
        float colliderHeight = playerCollider.bounds.extents.y;
        playerSpawnPosition = portalCollider.transform.position - new Vector3(0f, colliderHeight, 0f);
    }
}
