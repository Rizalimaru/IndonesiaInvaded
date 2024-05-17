using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Main Declaration
    public EnemyStateManager stateManager;
    public EnemyScriptableObject enemyType;
    public NavMeshAgent Agent;
    private ObjectiveManager objectiveManager;
    public EnemyScriptableObject enemyConfiguration;

    // Offensive Attribute Declaration
    public Transform target;
    public GameObject attackPrefab;
    public Transform spawnPoint;
    public Animator playerAnimator;

    // Attribute Declaration
    [System.NonSerialized] public float health;
    [System.NonSerialized] public float attackPower;
    [System.NonSerialized] public float attackSpeed;
    [System.NonSerialized] public float triggerDistance;
    [System.NonSerialized] public float attackDistance;
    [System.NonSerialized] public float viewAngle;
    [System.NonSerialized] public float animDelay;
    [System.NonSerialized] public EnemyScriptableObject.title enemyTitle;
    [System.NonSerialized] public bool isKnockedBack = false;
    [System.NonSerialized] public float knockbackForce;
    [System.NonSerialized] public float knockbackGuard;
    [System.NonSerialized] public float knockbackDelay;

    // Private Stuff
    private bool isAttacking = false;
    private GameObject attackObject;

    public void Awake()
    {
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        objectiveManager = FindObjectOfType<ObjectiveManager>();
    }

    public void Update()
    {
        bool hit1 = playerAnimator.GetBool("hit1");
        bool hit2 = playerAnimator.GetBool("hit2");
        bool hit3 = playerAnimator.GetBool("hit3");

        if (hit1 || hit2 || hit3)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (health <= 0)
        {
            stateManager.SwitchState(stateManager.deadState);
            
        }

        if (isKnockedBack == true)
        {
            Invoke("knockbackDelayCounter", knockbackDelay);
        }
    }

    private void OnCollisionEnter (Collision collision)
    {

        Collider other = collision.collider;

        if (other.CompareTag("Sword") && isAttacking == true && health > 0)
        {

            CameraShaker.instance.CameraShake(0.5f, 0.1f);

            Debug.Log("Damaged");
            health -= 20;
            knockbackForce = 30f;
            knockbackDelay = 0.2f;

            if (isKnockedBack == false)
            {
                isKnockedBack = true;
                stateManager.SwitchState(stateManager.knockbackState);
            }

            if (health <= 0)
            {
                objectiveManager.UpdateObjective();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SkillRoarCollider") && health > 0)
        {

            Debug.Log("get roar");

            knockbackForce = 65f;
            knockbackDelay = 5f;

            if (isKnockedBack == false)
            {
                stateManager.SwitchState(stateManager.knockbackState);
                isKnockedBack = true;
            }
        }
    }

    public void knockbackDelayCounter()
    {
        isKnockedBack = false; 
    }

    public void Attack()
    {
        attackObject = GameObject.Instantiate(attackPrefab, spawnPoint.transform.position, spawnPoint.rotation) as GameObject;
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

    private void OnDestroy()
    {
        Debug.Log("Enemy Object is Destroyed");

    }

    public void SetupAgent()
    {
        health = enemyType.Health;
        attackPower = enemyType.attackPower;
        attackSpeed = enemyType.attackSpeed;
        triggerDistance = enemyType.triggerDistance;
        attackDistance = enemyType.attackDistance;
        viewAngle = enemyType.viewingAngle;
        knockbackGuard = enemyType.knockbackGuard;

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

}
