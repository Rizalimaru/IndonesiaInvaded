using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapUltimateScript : MonoBehaviour
{

    public Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();

        Invoke("EnableCollider", 1.5f);

        GameObject.Destroy(gameObject, 1.7f);

    }

    private void EnableCollider()
    {
        col.enabled = true;
    }
}
