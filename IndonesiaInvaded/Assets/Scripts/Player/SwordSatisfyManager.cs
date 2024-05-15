using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class SwordSatisfyManager : MonoBehaviour
{   
    private Combat combat;
    private Animator animator;

    private void Awake()
    {   
        combat = Combat.instance;
    }
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        // Periksa apakah objek yang masuk trigger memiliki tag "Enemy"
        if (other.CompareTag("Enemy"))
        {   
            CameraShaker.instance.CameraShake(0.5f, 0.1f);
            StartCoroutine(slowMotionStart());
        }
    }
    IEnumerator slowMotionStart()
    {
        Time.timeScale = 0.6f;
        yield return new WaitForSeconds(0f);
        Time.timeScale = 1f;
    }

    void StratSlowMotion()
    {
        StartCoroutine(slowMotionStart());
    }

}
