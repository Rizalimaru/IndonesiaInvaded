using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataSaving : MonoBehaviour
{
    public static PlayerDataSaving instance;
    Vector2 look;
    internal Vector3 velocity;


    private void Awake()
    {
        instance = this;

    }
    private void Update(){
        PlayerCrash();
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        Physics.SyncTransforms();
        look.x = rotation.eulerAngles.y;
        look.y = rotation.eulerAngles.z;
        velocity = Vector3.zero;
    }

    private void PlayerCrash()
    {
        if (transform.position.y < -1)
        {
            CheckPointManager.instance.Respawn();
            ScoreManager.instance.ResetAllValues();
        }
    }

}
