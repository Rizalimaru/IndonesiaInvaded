using UnityEngine;
using UnityEngine.AI;

public class BossChaseState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss is Moving");

        boss.GetComponent<NavMeshAgent>().isStopped = false;
        boss.animator.SetBool("isWalking", true);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (Vector3.Distance(boss.bossObject.spawnPoint.transform.position, boss.bossObject.target.transform.position) < boss.bossObject.triggerDistance)
        {
            boss.bossObject.agent.SetDestination(boss.bossObject.target.transform.position);
            if (Vector3.Distance(boss.bossObject.spawnPoint.transform.position, boss.bossObject.target.transform.position) < boss.bossObject.attackDistance)
            {
                if(boss.comboCounter == boss.comboThreshold)
                {
                    boss.SwitchState(boss.comboState);
                }
                else
                {
                    boss.SwitchState(boss.attackState);
                }
            }
        }
        else
        {
            boss.SwitchState(boss.idleState);
        }
    }
}
