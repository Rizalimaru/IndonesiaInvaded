using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRestState : EnemyBaseState
{
    float attackDelay;

    public override void EnterState(EnemyStateManager enemy)
    {

        if (enemy.enemyTitle == EnemyScriptableObject.title.Boss) Debug.Log("Boss is Resting");
        else Debug.Log("Enemy is Resting");

        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        enemy.animator.SetBool("isAttacking", false);
        enemy.animator.SetBool("isResting", true);

        attackDelay = enemy.attackSpeed;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Vector3 direction = enemy.target.position - enemy.agent.transform.position;
        float angle = Vector3.Angle(direction, enemy.agent.transform.forward);
        bool canSeePlayer = direction.magnitude < enemy.triggerDistance && angle < 30.0f;

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
