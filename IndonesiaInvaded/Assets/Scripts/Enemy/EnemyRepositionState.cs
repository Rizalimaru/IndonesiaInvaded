using UnityEngine;
using UnityEngine.AI;

public class EnemyRepositionState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        Debug.Log("Repositioning");
        enemy.animator.SetBool("isAttacking", false);
        enemy.animator.SetBool("isResting", true);
        enemy.animator.SetBool("isWalking", false);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Vector3 direction = enemy.enemyObject.target.position - enemy.enemyObject.Agent.transform.position;

        bool canSeePlayer = enemy.enemyObject.checkIfSeeTarget();
        float step = 8.0f * Time.deltaTime;
        float enemyPlayerDistance = Vector3.Distance(enemy.enemyObject.Agent.transform.position, enemy.enemyObject.target.transform.position);

        Vector3 facingDirection = Vector3.RotateTowards(enemy.enemyObject.Agent.transform.forward, direction, step, 0.0f);
        enemy.enemyObject.Agent.transform.rotation = Quaternion.LookRotation(facingDirection);

        if (canSeePlayer && enemyPlayerDistance <= enemy.enemyObject.attackDistance + 1)
        {
            enemy.SwitchState(enemy.attackState);
        }
        else if (canSeePlayer && enemyPlayerDistance <= enemy.enemyObject.attackDistance && enemyPlayerDistance > enemy.enemyObject.attackDistance)
        {
            enemy.SwitchState(enemy.movingState);
        }
        else if (canSeePlayer && enemyPlayerDistance > enemy.enemyObject.attackDistance)
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
