using UnityEngine;
using UnityEngine.AI;

public class DukunCastingState : BossBaseState
{
    float delay;

    public override void EnterState(BossStateManager boss)
    {
        boss.bossObject.castingSkill = true;  

        boss.GetComponent<NavMeshAgent>().isStopped = true;

        boss.animator.SetBool("isWalking", true);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);

        delay = 2f;

        boss.bossObject.OndelSkill2();
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            boss.SwitchState(boss.dukunSecondSkillState);
        }
    }
}
