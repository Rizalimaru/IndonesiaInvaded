using UnityEngine;
using UnityEngine.AI;

public class BossIdleState : BossBaseState
{
    float delay;

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss is Idle");

        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);

        delay = 1f;
        boss.GetComponent<NavMeshAgent>().isStopped = true;
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(boss.bossObject.spawnPoint.transform.position, boss.bossObject.target.transform.position) <= boss.bossObject.triggerDistance)
            {
                boss.SwitchState(boss.movingState);
            }
        }
    }
}
