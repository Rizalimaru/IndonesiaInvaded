using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using System.Data.Common;
using Unity.VisualScripting;

public class Combat : MonoBehaviour
{
    public static event Action SuccessfulComboEvent; // Event untuk mengirim sinyal bahwa combo berhasil

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
        //timingHit();
        if (Input.GetMouseButtonDown(0) && animator.GetBool("isGrounded") && !rangeAtkAktif)
        {   
            PerformHit();
        }
        ResetCombo();
        // Pengecekan jika klik masih ditekan setelah mencapai hit terakhir
        if (Input.GetMouseButton(0) && currentHit >= 9)
        {
            // Kembali ke hit pertama
            stopHit();
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
        if(animator.GetBool("RoarSkill"))
        {   
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

    void PerformHit()
    {
        // Memeriksa apakah RoarSkill aktif, jika ya, keluar dari metode
        if (animator.GetBool("RoarSkill"))
        {   
            return;
        }

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
        if(isAttacking == true && stateInfo.normalizedTime > 0f && stateInfo.normalizedTime < 0.3f && stateInfo.IsName("hit1"))
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
        else if(isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
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
    void OnClick()
    {
        float currentTime = Time.time;

        if (currentTime - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        lastClickedTime = currentTime;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
            {
                animator.SetBool("hit1", true);
                StartCoroutine(PlaySoundWithDelay(0.1f, "AttackPlayer", 0));
                Debug.Log("Hit 1");
            }
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 4);

        if (noOfClicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
            {
                animator.SetBool("hit1", false);
                animator.SetBool("hit2", true);
                StartCoroutine(PlaySoundWithDelay(0.2f, "AttackPlayer", 0));
                Debug.Log("Hit 2");
            }
        }
        if (noOfClicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
            {
                animator.SetBool("hit2", false);
                animator.SetBool("hit3", true);
                StartCoroutine(PlaySoundWithDelay(1f, "AttackPlayer", 1));
                Debug.Log("Hit 3");
            }
        }
        if (noOfClicks >= 4 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("hit4"))
            {
                animator.SetBool("hit3", false);
                animator.SetBool("hit4", true);
                StartCoroutine(PlaySoundWithDelay(1f, "AttackPlayer", 2));
                Debug.Log("Hit 4");
                SuccessfulCombo();
            }
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
