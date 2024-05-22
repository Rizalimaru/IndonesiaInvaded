using UnityEngine;
using UnityEngine.AI;

public class BossAttackState : BossBaseState
{
    float delay;
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss is Attacking");
        boss.comboCounter++;

        boss.GetComponent<NavMeshAgent>().isStopped = true;
        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", true);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);

        delay = boss.bossObject.animDelay;

        boss.bossObject.Attack();

    }

    public override void UpdateState(BossStateManager boss)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            if (boss.bossObject.bossTitle == BossScriptableObject.title.OndelOndel)
            {
                boss.bossObject.DisableMeleeAttack();
            }
            boss.SwitchState(boss.restState);
        }
    }
}
