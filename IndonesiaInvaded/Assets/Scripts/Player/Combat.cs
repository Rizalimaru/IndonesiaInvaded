using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    public static event Action SuccessfulComboEvent; // Event untuk mengirim sinyal bahwa combo berhasil

    private AudioManager audioManagerInstance;

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

    public static Combat instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
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
        if (Input.GetMouseButtonDown(0) && animator.GetBool("isGrounded"))
        {
            PerformHit();
        }
        ResetCombo();
        // Pengecekan jika klik masih ditekan setelah mencapai hit terakhir
        if (Input.GetMouseButton(0) && currentHit >= 9)
        {
            // Kembali ke hit pertama
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", false);
            animator.SetBool("hit3", false);
            animator.SetBool("hit4", false);
        }

        skillCast();

        bool hit1 = animator.GetBool("hit1");
        bool hit2 = animator.GetBool("hit2");
        bool hit3 = animator.GetBool("hit3");
        bool hit4 = animator.GetBool("hit4");
        bool RoarSkill =  animator.GetBool("RoarSkill");

        if (hit1 || hit2 || hit3 || hit4 || RoarSkill || animator.GetBool("ChargeAtk"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    void PerformHit()
    {
        // Mengecek apakah hit terakhir sudah mencapai hit ke-4, jika ya, maka reset ke hit pertama
        if (currentHit >= 9)
        {
            currentHit = 0;
        }

        // Mengaktifkan animator controller yang sesuai dengan hit saat ini
        switch (currentHit)
        {
            case 0:
                animator.SetBool("hit1", true);
                break;
            case 1:
                animator.SetBool("hit2", true);
                break;
            case 2:
                animator.SetBool("hit3", true);
                break;
            case 3:
                animator.SetBool("hit4", true);
                break;
            default:
                break;
        }

        // Menambah hit saat ini untuk persiapan hit berikutnya
        currentHit++;
    }

    // IEnumerator ResetCombo()
    // {
    //     yield return new WaitForSeconds(hitResetTime);
    //     animator.SetBool("hit1", false);
    //     animator.SetBool("hit2", false);
    //     animator.SetBool("hit3", false);
    //     animator.SetBool("hit4", false);
    //     currentHit = 0;
    // }
    
    void ResetCombo()
    {
        if(isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("hit1", false);
            currentHit=0;
        }
        else if(isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {   
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", false);
            currentHit=0;
        }
        else if(isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {   
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", false);
            animator.SetBool("hit3", false);
            currentHit=0;
        }
        else if(isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit4"))
        {   
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", false);
            animator.SetBool("hit3", false);
            animator.SetBool("hit4", false);
            currentHit=0;
        }
    }

    void skillCast()
    {
        // Implementasikan logika untuk melempar skill di sini
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
