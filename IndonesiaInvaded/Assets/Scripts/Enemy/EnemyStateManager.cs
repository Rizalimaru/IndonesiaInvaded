using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyStateManager : MonoBehaviour
{
    // State Declaration
    EnemyBaseState currentState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState movingState = new EnemyChaseState();
    public EnemyAttackState attackState = new EnemyAttackState();

    // Config Loader
    public EnemyScriptableObject enemyType;

    // Unity Stuff Declaration
    public Animator animator;
    public NavMeshAgent agent;
    public Transform target;

    // Object Declaration
    public GameObject attackType;
    public Transform spawnPoint;

    // Attribute Declaration
    [System.NonSerialized] public float attackPower;
    [System.NonSerialized] public float attackSpeed;
    [System.NonSerialized] public float triggerDistance;
    [System.NonSerialized] public float attackDistance;
    [System.NonSerialized] public float attackForce;
    [System.NonSerialized] public float attackDecay;

    void Start()
    {
        animator = GetComponent<Animator>();
        SetupAgent();

        currentState.EnterState(this);
    }

    public void StartAgent()
    {
        currentState = idleState;
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    private void OnCollisionExit(Collision collision) 
    { 
        currentState.OnCollisionExit(this, collision); 
    }


    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void SetupAgent()
    {
        attackPower = enemyType.attackPower;
        attackSpeed = enemyType.attackSpeed;
        triggerDistance = enemyType.triggerDistance;
        attackDistance = enemyType.attackDistance;
        attackForce = enemyType.attackForce;
        attackDecay = enemyType.attackDecay;

        agent.speed = enemyType.Speed;
        agent.angularSpeed = enemyType.AngularSpeed;
        agent.acceleration = enemyType.Acceleration;
        agent.stoppingDistance = enemyType.StoppingDistance;
        agent.autoBraking = enemyType.Braking;

        agent.obstacleAvoidanceType = enemyType.ObstacleAvoidanceType;
        agent.radius = enemyType.Radius;
        agent.height = enemyType.Height;
        agent.avoidancePriority = enemyType.AvoidancePriority;

        agent.areaMask = enemyType.AreaMask;
    }
}
