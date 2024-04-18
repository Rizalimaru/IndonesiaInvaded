using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExistenceManager : MonoBehaviour
{

    public GameObject self;

    // Config Loader
    public EnemyScriptableObject enemyType;

    // Attribute Declaration
    [System.NonSerialized] public float health;

    // Start is called before the first frame update
    void Awake()
    {
        health = enemyType.Health;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Damaged");
            health -= 10;
        }

        if (health <= 0)
        {
            Destroy(self);
            Debug.Log("Dead");
        }
    }


}
