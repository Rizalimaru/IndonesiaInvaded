using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    private Animator animator;
    private Collider colid;

    [Header("Skill 1")]
    public Image skillImage1;
    public TMP_Text skill1CooldownText;
    public float cooldown1 = 5.0f;
    private bool isCooldown1 = false;
    public KeyCode skill1Key;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float SkillDetectionRadius = 10f;
    public float movementSpeed = 5f;
    private float distanceToMove;
    public float rotationToEnemySpeed = 5.0f;
    public Object SkillRoarCollider;
    public float destroyTimeColliderRoar = 1.5f;
    public GameObject smashExplosion;
    bool isRoarSkill = false;
    string playerLayer = "Player";
    string enemyLayer = "Enemy";

    [Header("Slow Motion Effect")]
    private bool isSlowMotionActive = false;
    public float slowMotionDuration = 1f;
    public float slowMotionTimeScale = 0.5f;
    public float waitBeforeSlowMotion = 0.5f;

    [Header("Skill 2")]
    public Image skillImage2;
    public TMP_Text skill2CooldownText;  
    public float cooldown2 = 8.0f;
    private bool isCooldown2 = false;
    public KeyCode skill2Key;
    public GameObject ruler;
    private int spawnedRulerCount = 0;
    private float lastSpawnTime = 0f;
    private const float timeBetweenSpawns = 0.5f;
    private const int maxRulerCount = 5;
    private bool skill2Active = false;
    private float skill2Duration = 5f;
    private float skill2Timer = 0f;

    [Header("Skill Detection")]
    public Transform player;
    [HideInInspector] public Transform nearestEnemy;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public bool IsRoarSkill
    {
        get { return isRoarSkill; }
        private set { isRoarSkill = value; }
    }

    private void Start()
    {
        colid = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        skillImage1.fillAmount = 0;
        skillImage2.fillAmount = 0;
        skill1CooldownText.text = "";
        skill2CooldownText.text = "";
    }

    private void Update()
    {
        DetectNearestEnemyForSkill();
        Skill1();
        Skill2();

        int LayerPlayer = LayerMask.NameToLayer(playerLayer);
        int LayerEnemy = LayerMask.NameToLayer(enemyLayer);

        if(isRoarSkill)
        {
            Physics.IgnoreLayerCollision(LayerPlayer, LayerEnemy, true);
        }else
        {
            Physics.IgnoreLayerCollision(LayerPlayer, LayerEnemy, false);
        }

        if (nearestEnemy != null)
        {
            distanceToMove = Vector3.Distance(player.position, nearestEnemy.position);
        }
        else
        {
            distanceToMove = 10f;
        }

        if (skill2Active)
        {
            skill2Timer += Time.deltaTime;
            if (skill2Timer >= skill2Duration)
            {
                skill2Active = false;
                skill2Timer = 0f;
                ResetRulerSpawn();
            }

            if (Input.GetMouseButtonDown(0))
            {
                SpawnRuler();
            }
        }
    }

    public bool ApakahSedangSkillRoar
    {
        get { return isRoarSkill; }
    }
    

    #region UsableSkill Function
    public void UseSkill1()
    {   
        bool SedangPakeRangeAtk = Input.GetKey(KeyCode.Mouse1);

        PlayerAttribut player = PlayerAttribut.instance;
        if (player != null && !isCooldown1 && player.currentSP >= 30 && animator.GetBool("isGrounded") && !SedangPakeRangeAtk)
        {
            animator.SetBool("RoarSkill", true);
            CameraShaker.instance.CameraShake(5f, 1f);
            SpawnRoarCollider();
            AudioManager._instance.PlaySFX("Skillplayer", 0);
            StartCoroutine(DelayToCharge(1.5f));
            player.currentSP -= 30;
            Debug.Log("Skill 1 activated!");
            player.skillBar.SetSkill(player.currentSP);
            StartCoroutine(CooldownSkill1());
        }
        else if (player.currentSP < 30)
        {
            Debug.Log("Not enough SP for Skill 1!");
        }
    }

    public void UseSkill2()
    {   
        bool SedangPakeRangeAtk = Input.GetKey(KeyCode.Mouse1);

        PlayerAttribut player = PlayerAttribut.instance;
        if (player != null && !isCooldown2 && player.currentSP >= 50 && !SedangPakeRangeAtk)
        {
            AudioManager._instance.PlaySFX("Skillplayer", 3);
            skill2Active = true;
            skill2Timer = 0f; // Reset the skill timer
            player.currentSP -= 50;
            Debug.Log("Skill 2 activated!");
            player.skillBar.SetSkill(player.currentSP);
            StartCoroutine(CooldownSkill2());
        }
        else if (player.currentSP < 50)
        {
            Debug.Log("Not enough SP for Skill 2!");
        }
    }
    #endregion

    private IEnumerator CooldownSkill1()
    {
        isCooldown1 = true;
        float cooldownTimer = cooldown1;
        while (cooldownTimer > 0)
        {
            skillImage1.fillAmount = cooldownTimer / cooldown1;
            skill1CooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }
        isCooldown1 = false;
        skillImage1.fillAmount = 0;
        skill1CooldownText.text = "";
    }

    private IEnumerator CooldownSkill2()
    {
        isCooldown2 = true;
        float cooldownTimer = cooldown2;
        while (cooldownTimer > 0)
        {
            skillImage2.fillAmount = cooldownTimer / cooldown2;
            skill2CooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }
        isCooldown2 = false;
        skillImage2.fillAmount = 0;
        skill2CooldownText.text = "";
    }

    private void Skill1()
    {
        if (Input.GetKeyDown(skill1Key) && !isCooldown1)
        {
            UseSkill1();
        }
    }

    private void Skill2()
    {
        if (Input.GetKeyDown(skill2Key) && !isCooldown2)
        {
            UseSkill2();
        }
    }

    #region Region function for skill Roar
    public IEnumerator StartSlowMotion()
    {
        if (!isSlowMotionActive)
        {
            yield return new WaitForSeconds(waitBeforeSlowMotion);
            Time.timeScale = slowMotionTimeScale;
            isSlowMotionActive = true;
            Invoke("StopSlowMotion", slowMotionDuration);
        }
    }

    public void StopSlowMotion()
    {
        Time.timeScale = 1f;
        isSlowMotionActive = false;
    }

    public void DetectNearestEnemyForSkill()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, SkillDetectionRadius);
        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Boss"))
            {
                float distance = Vector3.Distance(player.position, collider.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearest = collider.transform;
                }
            }
        }

        nearestEnemy = nearest;
    }

    private IEnumerator MoveToEnemyAfterCharge()
    {
        PlayerMovement.instance.canMove = false; // Disable normal movement
        yield return new WaitForSeconds(.5f);
        LookAtEnemy();
        Vector3 startPosition = player.position;
        Vector3 moveDirection = (nearestEnemy.position - startPosition).normalized;
        Vector3 targetPosition = startPosition + moveDirection * distanceToMove;
        float duration = distanceToMove / movementSpeed;

        AudioManager._instance.PlaySFX("Skillplayer", 1);

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            player.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        player.position = targetPosition;
        if (player.position == targetPosition)
        {
            isRoarSkill = true;
            SpawnSmashExplosion();
            SpawnRoarCollider();
        }
        yield return new WaitForSeconds(2f);
        isRoarSkill = false;

        PlayerMovement.instance.canMove = true; // Re-enable normal movement
    }


    private IEnumerator DelayToCharge(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (nearestEnemy == null)
        {
            animator.SetBool("RoarSkill", false);
        }
        else
        {
            animator.SetTrigger("ChargeAtk");
            animator.SetBool("RoarSkill", false);

            DetectNearestEnemyForSkill();
            StartCoroutine(MoveToEnemyAfterCharge());
        }
    }

    public void LookAtEnemy()
    {
        if (nearestEnemy != null)
        {
            Vector3 targetDirection = nearestEnemy.position - player.position;
            targetDirection.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            player.rotation = Quaternion.Slerp(player.rotation, rotation, rotationToEnemySpeed * Time.deltaTime);
        }
    }

    public void SpawnRoarCollider()
    {
        GameObject roarCollider = Instantiate(SkillRoarCollider, player.position, player.rotation) as GameObject;
        Destroy(roarCollider, destroyTimeColliderRoar);
    }

    public void SpawnSmashExplosion()
    {
        AudioManager._instance.PlaySFX("Skillplayer", 2);

        GameObject smash = Instantiate(smashExplosion, player.position, player.rotation) as GameObject;
        Destroy(smash, 2f);
    }
    #endregion

    #region Region function for skill RulerOrbit
    void SpawnRuler()
    {
        if (spawnedRulerCount < maxRulerCount && Time.time - lastSpawnTime > timeBetweenSpawns)
        {
            AudioManager._instance.PlaySFX("RangedAttack",1 );
            
            quaternion defaultRotation = ruler.transform.rotation;
            GameObject rulerObj = Instantiate(ruler, player.position, defaultRotation) as GameObject;
            spawnedRulerCount++;
            lastSpawnTime = Time.time;
            
            Destroy(rulerObj, 3f);
            
        }

    }

    void ResetRulerSpawn()
    {
        spawnedRulerCount = 0;
        lastSpawnTime = 0f;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            ResetSkills();
        }
    }
    
    public void ResetSkills()
    {
        StopAllCoroutines();
        isCooldown1 = false;
        isCooldown2 = false;
        skillImage1.fillAmount = 0;
        skillImage2.fillAmount = 0;
        skill1CooldownText.text = "";
        skill2CooldownText.text = "";
        Debug.Log("Skills have been reset!");
    }
}
