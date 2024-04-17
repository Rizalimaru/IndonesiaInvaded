using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<NavMeshAgent>().isStopped = false;
        Debug.Log("Enemy is Moving");
        enemy.animator.SetBool("isWalking", true);
        enemy.animator.SetBool("isAttacking", false);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (Vector3.Distance(enemy.agent.transform.position, enemy.target.transform.position) <= enemy.triggerDistance)
        {
            enemy.agent.SetDestination(enemy.target.transform.position);
            if(Vector3.Distance(enemy.agent.transform.position, enemy.target.transform.position) <= enemy.attackDistance)
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
        /** GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            enemy.SwitchState(enemy.attackState);
        } **/
    }

    public override void OnCollisionExit(EnemyStateManager enemy, Collision collision)
    {
        
    }
}
