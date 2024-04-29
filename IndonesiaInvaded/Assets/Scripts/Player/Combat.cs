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
    private Animator anim;
    public float cooldownTime = 2f;

    [Header("Skill")]
    public KeyCode skillRoarKey = KeyCode.E;
    public KeyCode skillEnhance = KeyCode.Q;
    public CameraShake cameraShake;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;


    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioManagerInstance = AudioManager.Instance;
        
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            anim.SetBool("hit1", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            anim.SetBool("hit2", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            anim.SetBool("hit3", false);
            noOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }

        skillCast();
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            anim.SetBool("hit1", true);
            StartCoroutine(PlaySoundWithDelay(0.1f, "AttackPlayer", 0));
            Debug.Log("Hit 1");
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", true);
            StartCoroutine(PlaySoundWithDelay(0.2f, "AttackPlayer", 0));
            Debug.Log("Hit 2");
        }
        if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", true);
            StartCoroutine(PlaySoundWithDelay(1f, "AttackPlayer", 1));
            Debug.Log("Hit 3");
            SuccessfulCombo(); // Panggil method baru untuk menangani combo sukses
        }
    }

    void skillCast()
    {
        if(Input.GetKeyDown(skillRoarKey))
        {   
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
            anim.SetTrigger("SkillRoar");
            
        }

    }

    void SuccessfulCombo()
    {
        // Method ini akan dipanggil setelah combo hit ke-3 berhasil
        Debug.Log("Successful Combo!");
        SuccessfulComboEvent?.Invoke(); // Panggil event untuk mengirim sinyal combo berhasil
    }  

    
    // Method untuk memainkan suara dengan delay
    IEnumerator PlaySoundWithDelay(float delay, string soundName, int soundIndex)
    {
        yield return new WaitForSeconds(delay);
        audioManagerInstance.PlaySFX(soundName, soundIndex);
    }
}
