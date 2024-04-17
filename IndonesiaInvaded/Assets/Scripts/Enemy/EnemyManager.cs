using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyManager : MonoBehaviour
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

    // Attribute Declaration
    private float health;
    private float attackPower;
    private float attackSpeed;
    public float triggerDistance;
    public float attackDistance;

    void Start()
    {
        animator = GetComponent<Animator>();
        triggerDistance = 6.0f;
        SetupAgent();

        currentState = idleState;
        currentState.EnterState(this);
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
        health = enemyType.Health;
        attackPower = enemyType.attackPower;
        attackSpeed = enemyType.attackSpeed;
        triggerDistance = enemyType.triggerDistance;
        attackDistance = enemyType.attackDistance;

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
