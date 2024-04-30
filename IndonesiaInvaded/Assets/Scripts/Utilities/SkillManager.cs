using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    // Singleton instance
    public static SkillManager instance;

    [Header("Skill 1")]
    public Image skillImage1;
    public float cooldown1 = 5;
    private bool isCooldown1 = false;
    public KeyCode skill1Key;

    [Header("Skill 2")]
    public Image skillImage2;
    public float cooldown2 = 8; // Cooldown for Skill 2
    private bool isCooldown2 = false;
    public KeyCode skill2Key;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        skillImage1.fillAmount = 0;
        skillImage2.fillAmount = 0;
    }

    private void Update()
    {
        Skill1();
        Skill2(); // Call Skill2 method in the Update loop
    }

    public void UseSkill1()
    {
        PlayerAttribut player = PlayerAttribut.instance;
        if (player != null && !isCooldown1 && player.currentSP >= 30)
        {
            player.currentSP -= 30;
            Debug.Log("Skill 1 activated!");
            player.skillBar.SetSkill(player.currentSP);
            // Start cooldown
            StartCoroutine(CooldownSkill1());
        }
        else if (player.currentSP < 30)
        {
            Debug.Log("Not enough SP for Skill 1!");
        }
    }

    public void UseSkill2()
    {
        PlayerAttribut player = PlayerAttribut.instance;
        if (player != null && !isCooldown2 && player.currentSP >= 50) // Check if enough SP and skill is not on cooldown
        {
            player.currentSP -= 50;
            Debug.Log("Skill 2 activated!");
            player.skillBar.SetSkill(player.currentSP);
            // Start cooldown
            StartCoroutine(CooldownSkill2());
        }
        else if (player.currentSP < 50)
        {
            Debug.Log("Not enough SP for Skill 2!");
        }
    }

    private IEnumerator CooldownSkill1()
    {
        isCooldown1 = true;
        float cooldownTimer = cooldown1;
        while (cooldownTimer > 0)
        {
            skillImage1.fillAmount = cooldownTimer / cooldown1;
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }
        isCooldown1 = false;
        skillImage1.fillAmount = 0;
    }

    private IEnumerator CooldownSkill2()
    {
        isCooldown2 = true;
        float cooldownTimer = cooldown2;
        while (cooldownTimer > 0)
        {
            skillImage2.fillAmount = cooldownTimer / cooldown2;
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }
        isCooldown2 = false;
        skillImage2.fillAmount = 0;
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
        if (Input.GetKeyDown(skill2Key) && !isCooldown2) // Check for key press and skill cooldown
        {
            UseSkill2();
        }
    }
}
