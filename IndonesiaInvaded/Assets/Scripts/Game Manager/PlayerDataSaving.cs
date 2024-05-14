using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataSaving : MonoBehaviour
{    
    public static PlayerDataSaving instance;

    [Header("Player Component")]
    public GameObject player;
    public GameObject playerCamera;

    [Header("Animator ReSpawn")]
    public Animator animatorReSpawn;
    Vector2 look;
    internal Vector3 velocity;
    

    private void Awake(){
        instance = this;
    }

    private void Update() 
    {
        if (InputManager.instance.GetExitPressed()) 
        {
            GameManager.instance.SaveGame();
            SceneManager.LoadSceneAsync("MainMenu");
        }

    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        Physics.SyncTransforms();
        look.x = rotation.eulerAngles.y;
        look.y = rotation.eulerAngles.z;
        velocity = Vector3.zero;
    }

    public void Teleports(Vector3 position)
    {
        transform.position = position;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void Die()
    {
        StartCoroutine(DieAnim());
    }

    IEnumerator DieAnim(){
        animatorReSpawn.SetTrigger("End");
        DisablePlayer();
        yield return new WaitForSeconds(1);

        CheckPointManager.instance.Respawn();

        EnablePlayer();

        animatorReSpawn.SetTrigger("Start");
        GameManager.instance.SaveGame();

    }

    private void DisablePlayer(){
        player.gameObject.SetActive(false);
        playerCamera.SetActive(false);
    }

    private void EnablePlayer(){
        player.gameObject.SetActive(true);
        playerCamera.SetActive(true);
    }
}
