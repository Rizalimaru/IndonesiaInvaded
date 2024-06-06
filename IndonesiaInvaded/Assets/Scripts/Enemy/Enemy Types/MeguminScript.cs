using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeguminScript : MonoBehaviour
{
    public Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();

        Invoke("EnableCollider", 8f);

        GameObject.Destroy(gameObject, 10);
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }
}
