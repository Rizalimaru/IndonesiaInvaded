using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    public int maxHealth = 500;
    public int currentHealth;
    public float regenRate = 0.1f;
    public int maxSP = 100;
    public int currentSP = 0;

    public HealthBar healthBar;
    public SkillBar skillBar;

    private int mouseClickCount = 0;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        skillBar.SetMaxSkill(maxSP);
        skillBar.SetSkill(currentSP);

        StartCoroutine(RegenerateHealth());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

        if (Input.GetMouseButtonDown(0))
        {
            mouseClickCount++;
            if (mouseClickCount >= 3)
            {
                mouseClickCount = 0;
                ComboSucceed();
            }
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator RegenerateHealth()
    {
        while (true)
        {
            if (!IsAttacked() && currentHealth < maxHealth)
            {
                currentHealth += Mathf.RoundToInt(regenRate * maxHealth);
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
                healthBar.SetHealth(currentHealth);
            }

            yield return new WaitForSeconds(3f);
        }
    }

    bool IsAttacked()
    {
        return false;
    }

    void ComboSucceed()
    {
        if (currentSP < maxSP)
        {
            currentSP += 10;
            skillBar.SetSkill(currentSP);
        }
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

    public  void UseSkill2()
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
