using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    // Main Declaration
    public EnemyStateManager stateManager;
    public EnemyScriptableObject enemyType;
    public NavMeshAgent Agent;

    // Offensive Attribute Declaration
    public Transform target;
    public GameObject attackType;
    public Transform spawnPoint;

    // Attribute Declaration
    [System.NonSerialized] public float health;
    [System.NonSerialized] public float attackPower;
    [System.NonSerialized] public float attackSpeed;
    [System.NonSerialized] public float triggerDistance;
    [System.NonSerialized] public float attackDistance;
    [System.NonSerialized] public float attackForce;
    [System.NonSerialized] public float attackDecay;
    [System.NonSerialized] public float viewAngle;
    [System.NonSerialized] public float animDelay;
    [System.NonSerialized] public EnemyScriptableObject.title enemyTitle;

    public void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    public void Update()
    {
        if(health <= 0)
        {
            stateManager.currentState = stateManager.deadState;
        }   
    }

    public void Attack()
    {
        GameObject attackObj = GameObject.Instantiate(attackType, spawnPoint.transform.position, spawnPoint.rotation) as GameObject;
        Rigidbody attackRigidBody = attackObj.GetComponent<Rigidbody>();
        attackRigidBody.AddForce(attackRigidBody.transform.forward * attackForce);
        GameObject.Destroy(attackObj, attackDecay);
    }

    public bool checkIfSeeTarget()
    {
        Vector3 direction = target.position - Agent.transform.position;
        float angle = Vector3.Angle(direction, Agent.transform.forward);

        if(direction.magnitude < float.MaxValue && angle < viewAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetupAgent()
    {
        health = enemyType.Health;
        attackPower = enemyType.attackPower;
        attackSpeed = enemyType.attackSpeed;
        triggerDistance = enemyType.triggerDistance;
        attackDistance = enemyType.attackDistance;
        attackForce = enemyType.attackForce;
        attackDecay = enemyType.attackDecay;
        viewAngle = enemyType.viewingAngle;

        Agent.speed = enemyType.Speed;
        Agent.angularSpeed = enemyType.AngularSpeed;
        Agent.acceleration = enemyType.Acceleration;
        Agent.stoppingDistance = enemyType.StoppingDistance;
        Agent.autoBraking = enemyType.Braking;

        Agent.obstacleAvoidanceType = enemyType.ObstacleAvoidanceType;
        Agent.radius = enemyType.Radius;
        Agent.height = enemyType.Height;
        Agent.avoidancePriority = enemyType.AvoidancePriority;

        Agent.areaMask = enemyType.AreaMask;

        enemyTitle = enemyType.enemyTitle;
        animDelay = enemyType.animationDelay;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }
}
