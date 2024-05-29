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

    // Offensive Attribute Declaration
    public Transform target;
    public GameObject attackPrefab;
    public Transform spawnPoint;
    public Animator playerAnimator;

    // Attribute Declaration
    [HideInInspector] public float health;
    [HideInInspector] public float attackPower;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float triggerDistance;
    [HideInInspector] public float attackDistance;
    [HideInInspector] public float viewAngle;
    [HideInInspector] public float animDelay;
    [HideInInspector] public EnemyScriptableObject.title enemyTitle;
    [HideInInspector] public bool isKnockedBack = false;
    [HideInInspector] public float knockbackForce;
    [HideInInspector] public float knockbackGuard;
    [HideInInspector] public float knockbackDelay;

    // Private Stuff
    private bool isAttacking = false;
    private GameObject attackObject;

    //Add some object
    public GameObject hitVFX;

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

        if (other.CompareTag("Sword") | other.CompareTag("RangedCollider") | other.CompareTag("FootCollider") && isAttacking == true && health > 0)
        {

            CameraShaker.instance.CameraShake(5f, 0.1f);
            spawnVfxhit();
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

        if (other.CompareTag("RangedCollider") && health > 0)
        {

            CameraShaker.instance.CameraShake(5f, 0.1f);
            spawnVfxhit();
            Debug.Log("Damaged");
            health -= 10;
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
            health -= 30;
            knockbackForce = 65f;
            knockbackDelay = 5f;

            if (isKnockedBack == false)
            {
                stateManager.SwitchState(stateManager.knockbackState);
                isKnockedBack = true;
            }
            
            if (health <= 0)
            {
                objectiveManager.UpdateObjective();
            }
        }
    }

    void spawnVfxhit()
    {   
        Vector3 newPosition = transform.position + new Vector3(0, 1, 0);
        GameObject vfx = Instantiate(hitVFX, newPosition, Quaternion.identity);
        Destroy(vfx, .5f);
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

        enemyTitle = enemyType.enemyTitle;
        animDelay = enemyType.animationDelay;
    }

}
