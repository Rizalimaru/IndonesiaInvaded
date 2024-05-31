using UnityEngine;
using UnityEngine.AI;

public class OndelOndelSecondSkillState : BossBaseState
{
    float delay;

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss is angry");

        boss.GetComponent<NavMeshAgent>().isStopped = false;

        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);
        boss.animator.SetBool("secondSkill", true);

        delay = boss.bossObject.secondSkillAnimDelay;
        boss.bossObject.OndelCastSkill2();
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            boss.bossObject.castingSkill = false;
            boss.bossObject.secondSkillCounter = 0;
            boss.SwitchState(boss.restState);
        }
    }
}
