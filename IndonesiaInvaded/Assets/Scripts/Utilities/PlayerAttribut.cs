using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class PlayerAttribut : MonoBehaviour
{
    // Singleton instance
    public static PlayerAttribut instance;

    private Animator playerAnimator;
    public int maxHealth = 500;
    public int currentHealth;
    public int maxSP = 100;
    public int currentSP = 0;

    public HealthBar healthBar;
    public SkillBar skillBar;

    private Coroutine regenCoroutine;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentHealth = (maxHealth);
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        skillBar.SetMaxSkill(maxSP);
        skillBar.SetSkill(currentSP);

        Combat.SuccessfulComboEvent += RegenerateSP;

        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     TakeDamage(20);
        // }

        if (currentHealth <= 0)
        {   
            PlayerMovement.instance.enabled = false;
            ThirdPersonCam.instance.GetBisaRotasi = false;
            Combat.instance.enabled = false;
            playerAnimator.SetBool("Death", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;

        if (other.CompareTag("EnemyMeleeCollider"))
        {
            Debug.Log("Damaged by melee");
            TakeDamage(15);
            StopRegenerateHealth();
        }
        else if (other.CompareTag("EnemyRangedCollider"))
        {
            Debug.Log("Damaged by ranged");
            TakeDamage(10);
            StopRegenerateHealth();
        }
        else if (other.CompareTag("BossMeleeCollider") && PlayerMovement.instance.lagiKnock == false)
        {
            Debug.Log("Colliding with boss's hand");
            TakeDamage(25);
            StopRegenerateHealth();
        }
        else if (other.CompareTag("BossRangedCollider") && PlayerMovement.instance.lagiKnock == false)
        {
            Debug.Log("Colliding with boss ranged");
            TakeDamage(15);
            StopRegenerateHealth();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DukunUltimate"))
        {
            Debug.Log("Explosion!");
            TakeDamage(45);
            StopRegenerateHealth();
        }
        else if (other.CompareTag("OndelUltimate"))
        {
            Debug.Log("Smashed!");
            TakeDamage(60);
            StopRegenerateHealth();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    IEnumerator RegenerateHealth()
    {
        float regenRate = 0.05f; // Laju regenerasi HP per detik
        while (true)
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += Mathf.RoundToInt(regenRate * maxHealth);
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                healthBar.SetHealth(currentHealth);
            }
            yield return new WaitForSeconds(6f);

        }
    }

    // Method untuk memulai serangan
    public void StopRegenerateHealth()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    // Method untuk memulai kembali regenerasi HP saat pemain selesai menyerang
    public void StartRegenerateHealth()
    {
        if (regenCoroutine == null)
        {
            regenCoroutine = StartCoroutine(RegenerateHealth());
        }
    }
    public void RegenHPOrb(int hpAmount)
    {
        currentHealth += hpAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        Debug.Log("Player restored " + hpAmount + " HP using HP orb!");
    }

    public void RegenSPOrb(int spAmount)
    {
        currentSP += spAmount;
        currentSP = Mathf.Clamp(currentSP, 0, maxSP);
        skillBar.SetSkill(currentSP);
        Debug.Log("Player restored " + spAmount + " SP using SP orb!");
    }

    void RegenerateSP()
    {
        // Regenerasi SP di sini
        int regenAmount = 10; // Jumlah SP yang akan ditambahkan
        currentSP = Mathf.Min(currentSP + regenAmount, maxSP); // Pastikan SP tidak melebihi maksimum
        skillBar.SetSkill(currentSP); // Update tampilan bar skill
    }

    public void ResetTotal()
    {
        currentHealth = maxHealth;
        currentSP = maxSP;

        // Update the health and skill bars
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        skillBar.SetMaxSkill(maxSP);
        skillBar.SetSkill(currentSP);

        // Restart health regeneration if needed
        StartRegenerateHealth();

    }

}
