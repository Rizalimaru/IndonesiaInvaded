using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    float delay;

    public override void EnterState(EnemyManager enemy)
    {   
        Debug.Log("Enemy is Idle");
        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", false);
        delay = 2.0f;
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
    }

    public override void UpdateState(EnemyManager enemy)
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

    public override void OnCollisionEnter(EnemyManager enemy, Collision collision)
    {
        
    }

    public override void OnCollisionExit(EnemyManager enemy, Collision collision)
    {

    }
}
