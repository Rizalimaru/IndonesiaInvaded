using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class PlayerAttribut : MonoBehaviour
{
    public int maxHealth = 500;
    public int currentHealth;
    public int maxSP = 100;
    public int currentSP = 0;

    public HealthBar healthBar;
    public SkillBar skillBar;

    private Coroutine regenCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        skillBar.SetMaxSkill(maxSP);
        skillBar.SetSkill(currentSP);

        Combat.SuccessfulComboEvent += RegenerateSP;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSkill1();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill2();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy_Melee"))
        {
            TakeDamage(40);
        }
        else if (other.CompareTag("Enemy_Ranged"))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    IEnumerator RegenerateHealth()
    {
        float regenRate = 0.1f; // Laju regenerasi HP per detik
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
 
    void RegenerateSP()
    {
        // Regenerasi SP di sini
        int regenAmount = 10; // Jumlah SP yang akan ditambahkan
        currentSP = Mathf.Min(currentSP + regenAmount, maxSP); // Pastikan SP tidak melebihi maksimum
        skillBar.SetSkill(currentSP); // Update tampilan bar skill
    }

    public void UseSkill1()
    {
        if (currentSP >= 30)
        {
            currentSP -= 30;
            Debug.Log("Skill 1 activated!");    
            skillBar.SetSkill(currentSP);
            // Lakukan tindakan Skill 1 di sini
        }
        else
        {
            Debug.Log("Not enough SP for Skill 1!");
        }
    }

    public void UseSkill2()
    {
        if (currentSP >= 50)
        {
            currentSP -= 50;
            Debug.Log("Skill 2 activated!");
            skillBar.SetSkill(currentSP);
            // Lakukan tindakan Skill 2 di sini
        }
        else
        {
            Debug.Log("Not enough SP for Skill 2!");
        }
    }
}
