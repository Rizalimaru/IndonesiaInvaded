using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSatisfyManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Periksa apakah objek yang masuk trigger memiliki tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(StartSlowMotion());
        }
    }

    IEnumerator StartSlowMotion()
    {   
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1f;
    }

    void slowMotion()
    {
        Time.timeScale = 0.5f;
    }



}
