using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class SwordSatisfyManager : MonoBehaviour
{   
    private Combat combat;
    private bool isSlowMotionActive = false;

    private void Awake()
    {
        combat = Combat.instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Periksa apakah objek yang masuk trigger memiliki tag "Enemy"
        if (other.CompareTag("Enemy") && combat.isAttacking == true)
        {
            StartCoroutine(slowMotionStart());
        }
    }

    IEnumerator slowMotionStart()
    {
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0.7f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1f;

    }

    IEnumerator slowMotionStop()
    {   
        yield return new WaitForSeconds(0.1f);  
        Time.timeScale = 1f;
    }



}
