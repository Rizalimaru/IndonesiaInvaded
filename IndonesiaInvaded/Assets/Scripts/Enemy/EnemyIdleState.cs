using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    float delay;

    public override void EnterState(EnemyStateManager enemy)
    {
        if (enemy.enemyObject.enemyTitle == EnemyScriptableObject.title.Boss) Debug.Log("Boss is Idle");
        else Debug.Log("Enemy is Idle");

        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", false);
        enemy.animator.SetBool("isResting", false);
        delay = 1f;
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(enemy.enemyObject.Agent.transform.position, enemy.enemyObject.target.transform.position) <= enemy.enemyObject.triggerDistance)
            {
                enemy.SwitchState(enemy.movingState);
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
