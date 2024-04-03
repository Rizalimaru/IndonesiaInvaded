using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerCombat : MonoBehaviour
{   
    [Header("Button")]
    public KeyCode attackButton = KeyCode.Mouse0;
    Animator animator;

    //Combat Variables
    int comboCounter;
    float cooldownTime = 0.1f;
    float lastClicked;
    float lastComboEnd;

    //Character Info
    [SerializeField] WeaponPlayer currentWeapon;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(currentWeapon != null)
        {
            Attack(currentWeapon.weaponName);
        }
    }

    void Attack(string weapon)
    {
        if(Input.GetKeyDown(attackButton) && Time.time - lastComboEnd > cooldownTime)
        {   
            comboCounter ++;
            comboCounter = Mathf.Clamp(comboCounter,0, currentWeapon.comboLength);

            //Create atk Names
            for(int i = 0; i < comboCounter; i++)
            {
                if (i == 0)
                {
                    if (comboCounter == 1 && animator.GetCurrentAnimatorStateInfo(0).IsTag("Movement"))
                    {
                        animator.SetBool("AttackStart", true);
                        animator.SetBool(weapon + "Attack" + (i + 1), true);
                        lastClicked = Time.time;
                    }

                }else
                {
                    if (comboCounter >= (i+1) && animator.GetCurrentAnimatorStateInfo(0).IsName(weapon + "Attack" + i))
                    {
                        animator.SetBool(weapon + "Attack" + (i + 1), true);
                        animator.SetBool(weapon + "Attack" + (i), true);
                        lastClicked = Time.time;
                    }
                }
            }
        }

        //animation exit bool reset
        for(int i=0; i < currentWeapon.comboLength; i++)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName(weapon + "Attack" + (i + 1)))
            {
                comboCounter =0;
                lastComboEnd = Time.time;
                animator.SetBool(weapon + "Attack" + (i + 1), false);
                animator.SetBool("AttackStart", false);
            }
        }
    }
}
