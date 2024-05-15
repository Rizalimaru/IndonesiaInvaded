using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class PlayerAttribut : MonoBehaviour
{
    // Singleton instance
    public static PlayerAttribut instance;
    public int maxHealth = 500;
    public int currentHealth;
    public int maxSP = 100;
    public int currentSP = 0;

    public HealthBar healthBar;
    public SkillBar skillBar;

    private Coroutine regenCoroutine;

    private void Start()
    {
        currentHealth = (maxHealth);
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        skillBar.SetMaxSkill(maxSP);
        skillBar.SetSkill(currentSP);

        Combat.SuccessfulComboEvent += RegenerateSP;

        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    TakeDamage(20);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyMeleeCollider"))
        {
            Debug.Log("Damaged by melee");
            TakeDamage(100);
            StopRegenerateHealth();
        }
        else if (other.CompareTag("EnemyRangedCollider"))
        {
            Debug.Log("Damaged by ranged");
            TakeDamage(50);
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
        float regenRate = 0.01f; // Laju regenerasi HP per detik
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

}
