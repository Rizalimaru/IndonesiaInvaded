using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapUltimateScript : MonoBehaviour
{
    public GameObject objectSelf;
    public Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();

        Invoke("EnableCollider", 0.3f);
        Invoke("DisableCollider", 0.5f);

        GameObject.Destroy(objectSelf, 2.5f);
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }

    private void DisableCollider()
    {
        col.enabled = false;
    }
}
