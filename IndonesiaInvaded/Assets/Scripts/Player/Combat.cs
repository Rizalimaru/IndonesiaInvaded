using UnityEngine;
using System;
using System.Collections;

public class Combat : MonoBehaviour
{
    public static event Action SuccessfulComboEvent; // Event to signal a successful combo

    private PlayerMovement playerMovement;
    private AudioManager audioManagerInstance;
    public static Combat instance;

    [Header("Hit")]
    private Animator animator;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;
    private int currentHit = 0;
    public float hitResetTime = .1f;
    private bool isPerformingHit = false;
    [HideInInspector] public bool isAttacking = false;
    private Coroutine hitResetCoroutine = null;
    KeyCode rangedAtkKey = KeyCode.Mouse1;

    private void Awake()
    {   
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance of Combat exists
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioManagerInstance = AudioManager.Instance;
    }

    void FixedUpdate()
    {

    }

    void Update()
    {   
        bool rangeAtkAktif = Input.GetKey(rangedAtkKey);

        if (Input.GetMouseButtonDown(0) && animator.GetBool("isGrounded") && !rangeAtkAktif)
        {   
            OnClick();
        }
        ResetCombo();

        if (Input.GetMouseButton(0) && currentHit >= 9)
        {
            stopHit();
        }

        skillCast();

        bool hit1 = animator.GetBool("hit1");
        bool hit2 = animator.GetBool("hit2");
        bool hit3 = animator.GetBool("hit3");
        bool hit4 = animator.GetBool("hit4");
        bool RoarSkill = animator.GetBool("RoarSkill");

        if (hit1 || hit2 || hit3 || hit4 || RoarSkill || animator.GetBool("ChargeAtk"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
        if (animator.GetBool("RoarSkill"))
        {   
            currentHit = 0;
        }

        if (playerMovement.IsDodging)
        {
            stopHit();
            currentHit = 0;
        }
    }

    void stopHit()
    {
        animator.SetBool("hit1", false);
        animator.SetBool("hit2", false);
        animator.SetBool("hit3", false);
        animator.SetBool("hit4", false);
    }

    void OnClick()
    {
        float currentTime = Time.time;

        if (currentTime - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        lastClickedTime = currentTime;
        noOfClicks++;
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 4);

        PerformHit();

        if (noOfClicks >= 4 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("hit4"))
            {
                Debug.Log("Hit 4");
                SuccessfulCombo();
            }
        }
    }

    void PerformHit()
    {
        if (animator.GetBool("RoarSkill") || playerMovement.IsDodging)
        {
            return;
        }

        if (currentHit >= 4)
        {
            currentHit = 0;
        }

        switch (currentHit)
        {
            case 0:
                animator.SetBool("hit1", true);
                //StartCoroutine(PlaySoundWithDelay(0.1f, "AttackPlayer", 0));
                Debug.Log("Hit 1");
                break;
            case 1:
                animator.SetBool("hit2", true);
                //StartCoroutine(PlaySoundWithDelay(0.2f, "AttackPlayer", 0));
                Debug.Log("Hit 2");
                break;
            case 2:
                animator.SetBool("hit3", true);
                //StartCoroutine(PlaySoundWithDelay(1f, "AttackPlayer", 1));
                Debug.Log("Hit 3");
                break;
            case 3:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("hit3") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                {
                    animator.SetBool("hit4", true);
                    //StartCoroutine(PlaySoundWithDelay(1f, "AttackPlayer", 2));
                    Debug.Log("Hit 4");
                }
                break;
        }
        
        currentHit++;
    }

#region HitTiming
    IEnumerator slowMotionStart(float tungguawal, float scaleawal, float tunggukedua)
    {
        yield return new WaitForSeconds(tungguawal);
        Time.timeScale = scaleawal;
        yield return new WaitForSeconds(tunggukedua);
        Time.timeScale = 1f;
    }

    void StartSlowMotion()
    {
        StartCoroutine(slowMotionStart(0f, 0.5f, 0.3f));
    }

    void timingHit()
    {   
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (isAttacking == true && stateInfo.normalizedTime > 0f && stateInfo.normalizedTime < 0.3f && stateInfo.IsName("hit1"))
        {
            if (!IsInvoking("StartSlowMotion"))
            {
                Invoke("StartSlowMotion", 0);
            }
        }
    }
#endregion
    
    void ResetCombo()
    {
        if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("hit1", false);
            currentHit = 0;
        }
        else if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {   
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", false);
            currentHit = 0;
        }
        else if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {   
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", false);
            animator.SetBool("hit3", false);
            currentHit = 0;
        }
        else if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit4"))
        {   
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", false);
            animator.SetBool("hit3", false);
            animator.SetBool("hit4", false);
            currentHit = 0;
        }
    }

    void skillCast()
    {
        // Implement the logic to cast skills here
    }

    void SuccessfulCombo()
    {
        Debug.Log("Successful Combo!");
        SuccessfulComboEvent?.Invoke();
    }

    IEnumerator PlaySoundWithDelay(float delay, string soundName, int soundIndex)
    {
        yield return new WaitForSeconds(delay);
        audioManagerInstance.PlaySFX(soundName, soundIndex);
    }
}
