using UnityEngine;
using UnityEngine.AI;

public class BossDashToTargetState : BossBaseState
{
    float delay;
    float tempAccel;
    float tempSpeed;

    public override void EnterState(BossStateManager boss)
    {
        tempAccel = boss.bossObject.agent.acceleration;
        tempSpeed = boss.bossObject.agent.speed;

        boss.bossObject.agent.acceleration = tempAccel * 10;
        boss.bossObject.agent.speed = tempSpeed * 50;

        Debug.Log("Boss is dashing");

        boss.GetComponent<NavMeshAgent>().isStopped = false;

        boss.animator.SetBool("isWalking", true);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);

        delay = 1f;

        boss.bossObject.agent.SetDestination(boss.bossObject.target.transform.position);
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            boss.bossObject.agent.speed = tempSpeed;
            boss.bossObject.agent.acceleration = tempAccel;

            boss.SwitchState(boss.secondSkill2ndState);
        }
    }
}
