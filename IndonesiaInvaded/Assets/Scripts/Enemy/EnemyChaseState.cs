using UnityEngine;
using UnityEngine.AI;


public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {

        if (enemy.enemyObject.enemyTitle == EnemyScriptableObject.title.Boss) Debug.Log("Boss is Moving");
        else Debug.Log("Enemy is Moving");

        enemy.GetComponent<NavMeshAgent>().isStopped = false;
        enemy.animator.SetBool("isWalking", true);
        enemy.animator.SetBool("isAttacking", false);
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
