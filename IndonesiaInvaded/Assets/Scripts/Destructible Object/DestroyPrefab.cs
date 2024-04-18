using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefab : MonoBehaviour
{
    private void Awake(){
        StartCoroutine(waiter());
    }

    IEnumerator waiter(){
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
