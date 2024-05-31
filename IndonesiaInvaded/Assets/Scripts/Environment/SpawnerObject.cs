using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObject : MonoBehaviour
{

    public GameObject[] container;

    public void Start()
    {
        EnvironmentCutSceneJakarta.instance.CameraDelay();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < container.Length; i++)
            {
                container[i].SetActive(true);

            }
            StartCoroutine(isFinishedEnabler());
        }
    }

    public void DestroyObject()
    {
        StartCoroutine(isFinishedEnabler());
    }

    IEnumerator isFinishedEnabler()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
