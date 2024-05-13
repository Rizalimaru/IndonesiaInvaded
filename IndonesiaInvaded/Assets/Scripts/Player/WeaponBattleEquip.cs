using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBattleEquip : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject weaponHolder;

    public bool isAttacking = false;
    float timeSinceLastHit = 0f;
    float hideDelay = 5f;

    PlayerAttribut playerAttribute;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Hide weapon at the start of the game
        HideWeapon();

        playerAttribute = GetComponent<PlayerAttribut>();
    }

    void Update()
    {
        // If currently attacking, check for hit animation
        if (animator.GetBool("hit1")==true || 
        animator.GetBool("hit2")==true || 
        animator.GetBool("hit3")==true ||
        animator.GetBool("hit4")==true ||
        animator.GetBool("RoarSkill")==true
        )
        {
            // If hit1 animation is playing, show weapon    
                ShowWeapon();
                HideWeaponHolder();
                timeSinceLastHit = 0f;
                playerAttribute.StopRegenerateHealth();

        }
        else
            {
                // Otherwise, update time since last hit
                timeSinceLastHit += Time.deltaTime;
                // If no hit animation is playing for a certain duration, hide weapon
                if (timeSinceLastHit >= hideDelay)
                {
                    HideWeapon();
                    ShowWeaponHolder();
                    playerAttribute.StartRegenerateHealth();    
                }
            }
    }

    // Function to be called when starting an attack
    public void StartAttack()
    {
        isAttacking = true;
        timeSinceLastHit = 0f;
    }

    // Function to be called when finishing an attack
    public void EndAttack()
    {
        isAttacking = false;
        // Reset time since last hit when attack ends
        timeSinceLastHit = 0f;
    }

    // Function to hide the weapon
    void HideWeapon()
    {
        if (weapon != null)
            weapon.SetActive(false);
    }

    // Function to show the weapon
    void ShowWeapon()
    {
        if (weapon != null)
            weapon.SetActive(true);
    }

    // Function to hide the weapon holder
    public void HideWeaponHolder()
    {
        if (weaponHolder != null)
            weaponHolder.SetActive(false);
    }

    // Function to show the weapon holder
    public void ShowWeaponHolder()
    {
        if (weaponHolder != null)
            weaponHolder.SetActive(true);
    }
}
