using System.Collections;
using System.Collections.Generic;
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
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Vector3 direction = enemy.target.position - enemy.agent.transform.position;
        float angle = Vector3.Angle(direction, enemy.agent.transform.forward);
        bool canSeePlayer = direction.magnitude < enemy.triggerDistance && angle < 1.0f;
        float step = 3.0f * Time.deltaTime;

        Vector3 facingDirection = Vector3.RotateTowards(enemy.agent.transform.forward, direction, step, 0.0f);
        
        enemy.agent.transform.rotation = Quaternion.LookRotation(facingDirection);

        if (canSeePlayer && Vector3.Distance(enemy.agent.transform.position, enemy.target.transform.position) <= enemy.attackDistance + 1)
        {
            enemy.SwitchState(enemy.attackState);
        }
        else if (canSeePlayer && Vector3.Distance(enemy.agent.transform.position, enemy.target.transform.position) <= 1000.0f)
        {
            enemy.SwitchState(enemy.movingState);
        }
    }

    public override void OnCollisionEnter(EnemyStateManager enemy, Collision collision)
    {
        
    }

    public override void OnCollisionExit(EnemyStateManager enemy, Collision collision)
    {
        
    }
}
