using UnityEngine;
using UnityEngine.AI;

public class BossRestState : BossBaseState
{
    float attackDelay;
    float skillProb;

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss is Resting");

        boss.FirstSkillCounting();
        boss.SecondSkillCounting();

        boss.GetComponent<NavMeshAgent>().isStopped = true;

        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", true);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);
        boss.animator.SetBool("firstSkill", false);
        boss.animator.SetBool("secondSkill", false);
        boss.animator.SetBool("combo", false);

        attackDelay = boss.bossObject.attackSpeed;
    }

    public override void UpdateState(BossStateManager boss)
    {
        bool canSeePlayer = boss.bossObject.checkIfSeeTarget();

        if (attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
        }
        else
        {
            if(boss.bossObject.firstSkillCounter >= 2)
            {
                boss.SwitchState(boss.firstSkillState);
            }
            else if(boss.bossObject.secondSkillCounter >= 5)
            {
                boss.SwitchState(boss.dashingState);
            }
            else
            {
                if (!canSeePlayer)
                {
                    boss.SwitchState(boss.repositionState);
                }
                else
                {
                    if (boss.comboCounter == 2)
                    {
                        boss.SwitchState(boss.comboState);
                    }
                    else
                    {
                        boss.SwitchState(boss.attackState);
                    }
                }
            }
        }
    }
}
