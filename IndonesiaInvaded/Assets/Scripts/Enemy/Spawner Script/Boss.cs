using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public GameObject hitVFX;
    [HideInInspector] public Animator playerAnimator;

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
    [HideInInspector] public bool castingSkill = false;
    public GameObject areaSkillPrefab;

    // Dukun Spesific Skill Declaration
    public GameObject enemyToSpawn;
    public GameObject comboAttackPrefab;
    public GameObject ultimateAttackPrefab;

    // Private Stuff
    private bool isAttacking = false;

    private bool AddScore = false;
    private GameObject attackObject;

    public void Awake()
    {
        if (bossTitle == BossScriptableObject.title.OndelOndel)
        {
            attackPrefab = null;
            enemyToSpawn = null;
            comboAttackPrefab = null;
            ultimateAttackPrefab = null;
        }

        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;

    }
    private void Start()
    {
        SetupAgent();
        BossHealthBar.instance.Initialize(health);
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
            if (bossTitle == BossScriptableObject.title.OndelOndel)
            {
                AudioManager._instance.StopBackgroundMusicWithTransition("GameJakarta", 1f);

                if (AddScore == false)
                {
                    AddScore = true;
                    ScoreManager.instance.AddBossDefeats(1);
                }
                
                EnvironmentCutSceneJakarta.instance.CutSceneBoss();

            }

            // Jikascene yang aktif adalah scene gameplay3 atau level 3
            if (SceneManager.GetActiveScene().name == "Gameplay3" || SceneManager.GetActiveScene().name == "Level3")
            {
                AudioManager._instance.StopBackgroundMusicWithTransition("GameBandung", 1f);

                AudioManager._instance.PlayBackgroundMusicWithTransition("Win",0,1f);

                if (AddScore == false)
                {
                    AddScore = true;
                    ScoreManager.instance.AddBossDefeats(1);
                }
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;

        if (other.CompareTag("Sword") && isAttacking && health > 0 && castingSkill == false)
        {
            CameraShaker.instance.CameraShake(5f, 0.1f);

            AudioManager._instance.PlayBossHitSFX("BossHit", 0);
            AudioManager._instance.PlaySFX("EnemyHit",0);

            Debug.Log("Damaged by Sword. Current health: " + health);
            spawnVfxhit();
            health -= 20;
            Debug.Log("Health after damage: " + health);

            BossHealthBar.instance.UpdateHealthBar(health);
        }

        if (other.CompareTag("RangedCollider") && isAttacking && health > 0 && castingSkill == false)
        {
            CameraShaker.instance.CameraShake(5f, 0.1f);

            AudioManager._instance.PlayBossHitSFX("BossHit", 0);
            AudioManager._instance.PlaySFX("EnemyHit",0);

            Debug.Log("Damaged by Sword. Current health: " + health);
            spawnVfxhit();
            health -= 10;
            Debug.Log("Boss Kena Ranged: " + health);

            BossHealthBar.instance.UpdateHealthBar(health);
        }

        if (other.CompareTag("RangedCollider") && health > 0)
        {   
            CameraShaker.instance.CameraShake(3f, 0.1f);
            AudioManager._instance.PlayBossHitSFX("BossHit", 0);
            AudioManager._instance.PlaySFX("EnemyHit",0);

            Debug.Log("Damaged by Ranged. Current health: " + health);
            spawnVfxhit();
            health -= 10;
            Debug.Log("Health after damage: " + health);

            BossHealthBar.instance.UpdateHealthBar(health);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SkillRoarCollider") && health > 0 && castingSkill == false)
        {
            AudioManager._instance.PlayBossHitSFX("BossHit", 0);
            
            Debug.Log("Damaged by Roar. Current health: " + health);
            health -= 50;
            Debug.Log("Health after damage: " + health);

            BossHealthBar.instance.UpdateHealthBar(health);

            knockbackForce = 65f;
            knockbackDelay = 75f;

            if (isKnockedBack == false)
            {
                isKnockedBack = true;
                Invoke("knockbackDelayCounter", knockbackDelay);
                stateManager.SwitchState(stateManager.knockbackState);
            }
        }
    }

    void spawnVfxhit()
    {
        Vector3 newPosition = transform.position + new Vector3(-1, 1, 0);
        GameObject vfx = Instantiate(hitVFX, newPosition, Quaternion.identity);
        Destroy(vfx, .5f);
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
            AudioManager._instance.PlaySFX("SkillBoss",3);
        }
        else
        {
            AudioManager._instance.PlaySFX("BossDukun",0);
            attackObject = GameObject.Instantiate(attackPrefab, spawnPoint.transform.position, spawnPoint.rotation) as GameObject;
        }
    }

    // Ondel-Ondel Skill Logic
    public void OndelDisableMeleeAttack()
    {
        Collider meleeCollider = GameObject.FindGameObjectWithTag("BossMeleeCollider").GetComponent<Collider>();
        meleeCollider.enabled = false;
    }

    public void OndelSkill1()
    {
        Collider meleeCollider = GameObject.FindGameObjectWithTag("BossMeleeCollider").GetComponent<Collider>();
        meleeCollider.enabled = true;
    }

    public void OndelSkill2()
    {
        attackObject = GameObject.Instantiate(areaSkillPrefab, transform.position, transform.rotation) as GameObject;

    }

    public void OndelCastSkill2()
    {
        Invoke("OndelSkill2", 1.8f);
    }

    // Dukun Skill Logic

    public void DukunSpawning(Vector3 position)
    {
        Instantiate(enemyToSpawn, position, Quaternion.identity);
        AudioManager._instance.PlaySFX("Teleport",0);
    }

    public bool CheckIfEnemySpawned()
    {
        int checkerInt = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (checkerInt >= 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DukunCombo()
    {
        Instantiate(comboAttackPrefab, target.position, Quaternion.identity);

        StartCoroutine(SFXDukunCombo());

    }

    IEnumerator SFXDukunCombo()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager._instance.PlaySFX("BossDukun",1);
    }

    public void Dukun2ndSkill()
    {
        GameObject zapObj = Instantiate(ultimateAttackPrefab);
        zapObj.transform.SetParent(transform);
        zapObj.transform.localPosition = new Vector3(Random.Range(-15, 15), 0, Random.Range(-15, 15));
    }

    // Other stuff

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
