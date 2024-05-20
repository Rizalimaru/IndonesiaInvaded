using UnityEngine;
using UnityEngine.AI;


public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enemy is Moving");

        enemy.GetComponent<NavMeshAgent>().isStopped = false;
        enemy.animator.SetBool("isWalking", true);
        enemy.animator.SetBool("isAttacking", false);
        enemy.animator.SetBool("isResting", false);
        enemy.animator.SetBool("isDead", false);
        enemy.animator.SetBool("isStunned", false);
        enemy.animator.SetBool("repositioning", false);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (Vector3.Distance(enemy.enemyObject.spawnPoint.transform.position, enemy.enemyObject.target.transform.position) <= enemy.enemyObject.triggerDistance)
        {
            enemy.enemyObject.Agent.SetDestination(enemy.enemyObject.target.transform.position);
            if(Vector3.Distance(enemy.enemyObject.spawnPoint.transform.position, enemy.enemyObject.target.transform.position) <= enemy.enemyObject.attackDistance)
            {
                enemy.SwitchState(enemy.attackState);
            }
        }
        else
        {
            enemy.SwitchState(enemy.idleState);
        }
    }

    public override void OnCollisionEnter(EnemyStateManager enemy, Collision collision)
    {
        
    }

    public override void OnCollisionExit(EnemyStateManager enemy, Collision collision)
    {
        
    }
}
