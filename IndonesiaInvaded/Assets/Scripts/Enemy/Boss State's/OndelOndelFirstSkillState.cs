using UnityEngine;
using UnityEngine.AI;

public class OndelOndelFirstSkillState : BossBaseState
{
    float delay;
    float tempSpeed;
    float tempAccel;

    public override void EnterState(BossStateManager boss)
    {
        boss.bossObject.castingSkill = true;

        tempAccel = boss.bossObject.agent.acceleration;
        tempSpeed = boss.bossObject.agent.speed;

        boss.bossObject.agent.acceleration = tempAccel * 10;
        boss.bossObject.agent.speed = tempSpeed * 50;

        Debug.Log("Boss is rotating");

        boss.GetComponent<NavMeshAgent>().isStopped = false;

        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);
        boss.animator.SetBool("firstSkill", true);

        delay = boss.bossObject.firstSkillAnimDelay;

        boss.bossObject.OndelSkill1();

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
            boss.bossObject.firstSkillCounter = 0;

            boss.bossObject.agent.speed = tempSpeed;
            boss.bossObject.agent.acceleration = tempAccel;

            boss.bossObject.OndelStopSkill1();

            boss.bossObject.castingSkill = false;

            boss.SwitchState(boss.restState);
        }
    }
}
