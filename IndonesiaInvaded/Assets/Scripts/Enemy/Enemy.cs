using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    public EnemyStateManager StateManager;
    public NavMeshAgent Agent;
    public int Health;

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }
}
