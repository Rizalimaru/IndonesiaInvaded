using UnityEngine;
using UnityEngine.AI;

public class BossComboState : BossBaseState
{
    float delay;
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Combo Attack!");

        boss.comboCounter = 0;

        boss.GetComponent<NavMeshAgent>().isStopped = true;

        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);
        boss.animator.SetBool("combo", true);

        delay = boss.bossObject.animDelay;

        if(boss.bossObject.bossTitle == BossScriptableObject.title.OndelOndel)
        {
            boss.bossObject.Attack();

            AudioManager._instance.PlaySFX("SkillBoss", 1);
        }
        else
        {
            boss.bossObject.DukunCombo();
        }

    }

    public override void UpdateState(BossStateManager boss)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            if (boss.bossObject.bossTitle == BossScriptableObject.title.OndelOndel)
            {
                boss.bossObject.OndelDisableMeleeAttack();
            }
            boss.SwitchState(boss.restState);
        }
    }
}
