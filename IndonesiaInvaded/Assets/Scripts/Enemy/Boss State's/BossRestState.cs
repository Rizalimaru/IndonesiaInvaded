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
                if (boss.bossObject.bossTitle == BossScriptableObject.title.OndelOndel) boss.SwitchState(boss.ondelFirstSkillState);
                else boss.SwitchState(boss.dukunFirstSkillState);
            }
            else if(boss.bossObject.secondSkillCounter >= 3)
            {
                if (boss.bossObject.bossTitle == BossScriptableObject.title.OndelOndel) boss.SwitchState(boss.ondelDashingState);
                else boss.SwitchState(boss.dukunCastingState);
            }
            else
            {
                if (!canSeePlayer || Vector3.Distance(boss.bossObject.spawnPoint.transform.position, boss.bossObject.target.transform.position) <= boss.bossObject.attackDistance)
                {
                    boss.SwitchState(boss.repositionState);
                }
                else
                {
                    if (boss.comboCounter == boss.comboThreshold)
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
