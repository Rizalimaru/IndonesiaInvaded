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

        GameObject.Destroy(objectSelf, 2.5f);
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }
}
