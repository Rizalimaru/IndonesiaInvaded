using UnityEngine;
using UnityEngine.AI;

public class EnemyDeadState : EnemyBaseState
{
    float delay;

    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enemy is defeated");

        enemy.GetComponent<NavMeshAgent>().isStopped = true;

        enemy.animator.SetBool("isAttacking", false);
        enemy.animator.SetBool("isResting", false);
        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isDead", true);

        delay = 1f;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Object.Destroy(enemy.gameObject, delay);
    }

    public override void OnCollisionEnter(EnemyStateManager enemy, Collision collision)
    {
        
    }

    public override void OnCollisionExit(EnemyStateManager enemy, Collision collision)
    {
        
    }
}
