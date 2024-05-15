using UnityEngine;
using UnityEngine.AI;

public class EnemyRestState : EnemyBaseState
{
    float attackDelay;

    public override void EnterState(EnemyStateManager enemy)
    {

        if (enemy.enemyObject.enemyTitle == EnemyScriptableObject.title.Boss) Debug.Log("Boss is Resting");
        else Debug.Log("Enemy is Resting");

        enemy.GetComponent<NavMeshAgent>().enabled = true;
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        
        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", false);
        enemy.animator.SetBool("isResting", true);
        enemy.animator.SetBool("isDead", false);
        enemy.animator.SetBool("isStunned", false);
        enemy.animator.SetBool("repositioning", false);

        attackDelay = enemy.enemyObject.attackSpeed;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        bool canSeePlayer = enemy.enemyObject.checkIfSeeTarget();

        if (attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
        }
        else
        {
            if (!canSeePlayer)
            {
                enemy.SwitchState(enemy.repositionState);                
            }
            else
            {
                enemy.SwitchState(enemy.attackState);
            }
        }
    }

    public override void OnCollisionEnter(EnemyStateManager enemy, Collision collision)
    {
        
    }

    public override void OnCollisionExit(EnemyStateManager enemy, Collision collision)
    {
        
    }

    
}
