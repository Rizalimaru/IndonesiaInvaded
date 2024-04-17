using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyManager enemy)
    {
        enemy.GetComponent<NavMeshAgent>().isStopped = false;
        Debug.Log("Enemy is Attacking");
        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", true);
    }

    public override void UpdateState(EnemyManager enemy)
    {
        // Logic here

        if (Vector3.Distance(enemy.agent.transform.position, enemy.target.transform.position) >= enemy.attackDistance + 1)
        {
            enemy.SwitchState(enemy.movingState);
        }
    }

    public override void OnCollisionEnter(EnemyManager enemy, Collision collision)
    {
        
    }

    public override void OnCollisionExit(EnemyManager enemy, Collision collision)
    {
        
    }
}
