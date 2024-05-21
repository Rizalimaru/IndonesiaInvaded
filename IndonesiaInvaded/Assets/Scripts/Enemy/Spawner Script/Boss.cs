using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    // Main Declaration
    public BossStateManager stateManager;
    public BossScriptableObject bossName;
    public NavMeshAgent agent;
    public BossHealthBar bossHealthBar;

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
    // public GameObject skill1Prefab;
    public GameObject skill2Prefab;
    
    // Private Stuff
    private bool isAttacking = false;
    private GameObject attackObject;

    public void Awake()
    {
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }
    private void Start()
    {
        SetupAgent();

        if (bossHealthBar != null)
        {
            Debug.Log("Boss: Menginisialisasi health bar");
            bossHealthBar.Initialize(health);
        }
        else
        {
            Debug.LogError("Boss: BossHealthBar tidak ditemukan");
        }
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
            Debug.Log("Ada 2");
            firstSkillCounter = 2;
        }
    }

   private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;

        if (other.CompareTag("Sword") && isAttacking && health > 0)
        {
            CameraShaker.instance.CameraShake(0.5f, 0.1f);

            Debug.Log("Damaged by Sword. Current health: " + health);
            health -= 20;
            Debug.Log("Health after damage: " + health);

            if (bossHealthBar != null)
            {
                Debug.Log("Boss: Memperbarui health bar");
                bossHealthBar.UpdateHealthBar(health);
            }

            knockbackForce = 30f;
            knockbackDelay = 7f;

            if (!isKnockedBack)
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
            Debug.Log("Damaged by Roar. Current health: " + health);
            health -= 50;
            Debug.Log("Health after damage: " + health);

            if (bossHealthBar != null)
            {
                Debug.Log("Boss: Memperbarui health bar");
                bossHealthBar.UpdateHealthBar(health);
            }

            knockbackForce = 65f;
            knockbackDelay = 15f;

            if (!isKnockedBack)
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
        if (bossTitle == BossScriptableObject.title.OndelOndel)
        {
            Collider meleeCollider = GameObject.FindGameObjectWithTag("BossMeleeCollider").GetComponent<Collider>();
            meleeCollider.enabled = true;
        }
        else
        {
            attackObject = GameObject.Instantiate(attackPrefab, spawnPoint.transform.position, spawnPoint.rotation) as GameObject;
        }
    }

    public void DisableMeleeAttack()
    {
        Collider meleeCollider = GameObject.FindGameObjectWithTag("BossMeleeCollider").GetComponent<Collider>();
        meleeCollider.enabled = false;
    }

    public void Skill1()
    {
        Collider meleeCollider = GameObject.FindGameObjectWithTag("BossMeleeCollider").GetComponent<Collider>();
        meleeCollider.enabled = true;
    }

    private void Skill2()
    {
        attackObject = GameObject.Instantiate(skill2Prefab, transform.position, transform.rotation) as GameObject;
    }

    public void StopSkill1()
    {
        Collider meleeCollider = GameObject.FindGameObjectWithTag("BossMeleeCollider").GetComponent<Collider>();
        meleeCollider.enabled = false;
    }

    public void CastSkill2()
    {
        Invoke("Skill2", 1.8f);
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
