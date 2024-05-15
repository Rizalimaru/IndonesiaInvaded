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
    public GameObject rangedBullet;
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

    // Whatever this is
    private bool isAttacking = false;
    public GameObject meleeCollider;
    public GameObject bossMeleeCollider;



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
            if (enemyTitle == EnemyScriptableObject.title.Basic_Melee || enemyTitle == EnemyScriptableObject.title.Basic_Ranged)
            {
                Invoke("knockbackDelayCounter", knockbackDelay);
            }
            else
            {
                Invoke("knockbackDelayCounter", 5f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && isAttacking == true && health > 0)
        {
            Debug.Log("Damaged");
            health -= 5;
            knockbackForce = 35f;
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

        if (other.CompareTag("SkillRoarCollider") && health > 0)
        {
            knockbackForce = 100f;
            knockbackDelay = 3f;

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

    public void enableAttack()
    {
        if (enemyTitle == EnemyScriptableObject.title.Basic_Melee)
        {
            meleeCollider.GetComponent<Collider>().enabled = true;
        }
        else if (enemyTitle == EnemyScriptableObject.title.Basic_Ranged)
        {
            GameObject attackObj = GameObject.Instantiate(rangedBullet, spawnPoint.transform.position, spawnPoint.rotation) as GameObject;
        }
        else if (enemyTitle == EnemyScriptableObject.title.Boss)
        {
            bossMeleeCollider.GetComponent<Collider>().enabled = true;
        }
    }

    public void disableAttack()
    {
        if (enemyTitle == EnemyScriptableObject.title.Basic_Melee)
        {
            meleeCollider.GetComponent<Collider>().enabled = false;
        }
        else if (enemyTitle == EnemyScriptableObject.title.Boss)
        {
            bossMeleeCollider.GetComponent<Collider>().enabled = false;
        }
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
