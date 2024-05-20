using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    // Main Declaration
    public BossStateManager stateManager;
    public BossScriptableObject bossName;
    public NavMeshAgent agent;

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
    [HideInInspector] public float knockbackForce;
    [HideInInspector] public float knockbackGuard;
    [HideInInspector] public float knockbackDelay;
    [HideInInspector] public BossScriptableObject.title bossTitle;
    [HideInInspector] public bool isKnockedBack = false;

    // Skill Declaration
    [HideInInspector] public float firstSkillChance;
    [HideInInspector] public float secondSkillChance;
    [HideInInspector] public int firstSkillCounter = 0;
    [HideInInspector] public int secondSkillCounter = 0;
    [HideInInspector] public float firstSkillAnimDelay;
    [HideInInspector] public float secondSkillAnimDelay;


    // Private Stuff
    private bool isAttacking = false;
    private GameObject attackObject;

    public void Awake()
    {
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Ada 5");
            secondSkillCounter = 5;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        Collider other = collision.collider;

        if (other.CompareTag("Sword") && isAttacking == true && health > 0)
        {

            CameraShaker.instance.CameraShake(0.5f, 0.1f);

            Debug.Log("Damaged");
            health -= 20;
            knockbackForce = 30f;
            knockbackDelay = 7f;

            if (isKnockedBack == false)
            {
                isKnockedBack = true;
                stateManager.SwitchState(stateManager.knockbackState);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SkillRoarCollider") && health > 0)
        {

            Debug.Log("get roar");
            health -= 50;
            knockbackForce = 65f;
            knockbackDelay = 15f;

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
        Vector3 direction = target.position - agent.transform.position;
        float angle = Vector3.Angle(direction, agent.transform.forward);

        if (direction.magnitude < float.MaxValue && angle < viewAngle)
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
        health = bossName.Health;
        attackPower = bossName.attackPower;
        attackSpeed = bossName.attackSpeed;
        triggerDistance = bossName.triggerDistance;
        attackDistance = bossName.attackDistance;
        viewAngle = bossName.viewingAngle;
        knockbackGuard = bossName.knockbackGuard;

        firstSkillChance = bossName.firstSkillChance;
        secondSkillChance = bossName.secondSkillChance;
        firstSkillAnimDelay = bossName.firstSkillAnimDelay;
        secondSkillAnimDelay = bossName.secondSkillAnimDelay;

        agent.speed = bossName.Speed;
        agent.angularSpeed = bossName.AngularSpeed;
        agent.acceleration = bossName.Acceleration;
        agent.stoppingDistance = bossName.StoppingDistance;
        agent.autoBraking = bossName.Braking;

        bossTitle = bossName.bossTitle;
        animDelay = bossName.animationDelay;
    }
}
