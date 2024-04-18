using UnityEngine;
using UnityEngine.Pool;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool Parent;

    public virtual void OnDisable()
    {
        Parent.ReturnObjectToPool(this);
    }
}