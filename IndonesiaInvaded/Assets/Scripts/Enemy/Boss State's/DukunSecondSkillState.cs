using UnityEngine;
using UnityEngine.AI;

public class DukunSecondSkillState : BossBaseState
{
    float delay;

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Overcharge!");

        boss.GetComponent<NavMeshAgent>().isStopped = true;

        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);
        boss.animator.SetBool("secondSkill", true);

        delay = boss.bossObject.secondSkillAnimDelay;
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (delay > 0)
        {
            boss.bossObject.Dukun2ndSkill();
            delay -= Time.deltaTime;
        }
        else
        {
            boss.bossObject.secondSkillCounter = 0;
            boss.SwitchState(boss.restState);
        }
    }
}
