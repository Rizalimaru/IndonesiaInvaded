using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    float delay;

    public override void EnterState(EnemyStateManager enemy)
    {   
        Debug.Log("Enemy is Idle");
        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", false);
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
            if (Vector3.Distance(enemy.agent.transform.position, enemy.target.transform.position) <= enemy.triggerDistance)
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
