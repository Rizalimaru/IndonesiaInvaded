using UnityEngine;
using UnityEngine.AI;

public class BossRepositionState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        boss.GetComponent<NavMeshAgent>().isStopped = true;
        Debug.Log("Boss is Repositioning");

        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", true);
    }

    public override void UpdateState(BossStateManager boss)
    {
        Vector3 direction = boss.bossObject.target.position - boss.bossObject.agent.transform.position;

        bool canSeePlayer = boss.bossObject.checkIfSeeTarget();
        float step = 8.0f * Time.deltaTime;
        float enemyPlayerDistance = Vector3.Distance(boss.bossObject.spawnPoint.transform.position, boss.bossObject.target.transform.position);

        Debug.Log(enemyPlayerDistance);

        Vector3 facingDirection = Vector3.RotateTowards(boss.bossObject.agent.transform.forward, direction, step, 0.0f);
        boss.bossObject.agent.transform.rotation = Quaternion.LookRotation(facingDirection);

        if (canSeePlayer && enemyPlayerDistance <= boss.bossObject.attackDistance)
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
        else if (canSeePlayer && enemyPlayerDistance <= boss.bossObject.triggerDistance || enemyPlayerDistance >= boss.bossObject.attackDistance + 1)
        {
            boss.SwitchState(boss.movingState);
        }
        else if (canSeePlayer && enemyPlayerDistance > boss.bossObject.triggerDistance)
        {
            boss.SwitchState(boss.idleState);
        }

    }
}
