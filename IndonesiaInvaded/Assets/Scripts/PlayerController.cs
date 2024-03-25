using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 look;
    internal Vector3 velocity;
    
    public void Teleport(Vector3 position, Quaternion rotation)
    {
        
        transform.position = position;
        Physics.SyncTransforms();
        look.x = rotation.eulerAngles.y;
        look.y = rotation.eulerAngles.z;
        velocity = Vector3.zero;
    }
}
