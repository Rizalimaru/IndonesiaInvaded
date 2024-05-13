using UnityEngine;
using UnityEngine.AI;

public class EnemyConfusedState : EnemyBaseState
{

    float delay;

    public override void EnterState(EnemyStateManager enemy)
    {

        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", false);
        enemy.animator.SetBool("isResting", false);
        enemy.animator.SetBool("isDead", false);
        enemy.animator.SetBool("isStunned", false);
        enemy.animator.SetBool("repositioning", false);
        enemy.animator.SetBool("confused", true);

        enemy.GetComponent<NavMeshAgent>().isStopped = false;

        delay = enemy.enemyObject.knockbackDelay;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            enemy.enemyObject.isKnockedBack = false;

            enemy.SwitchState(enemy.restState);
        }
    }

    public override void OnCollisionEnter(EnemyStateManager enemy, Collision collision)
    {
    }

    public override void OnCollisionExit(EnemyStateManager enemy, Collision collision)
    {
    }
}
