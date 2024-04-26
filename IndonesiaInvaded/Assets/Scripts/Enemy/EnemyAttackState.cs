using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    float delay;
    public override void EnterState(EnemyStateManager enemy)
    {
        if (enemy.enemyTitle == EnemyScriptableObject.title.Boss) Debug.Log("Boss is Attacking");
        else Debug.Log("Enemy is Attacking");

        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", true);
        enemy.animator.SetBool("isResting", false);
        delay = enemy.animDelay;

        GameObject attackObj = GameObject.Instantiate(enemy.attackType, enemy.spawnPoint.transform.position, enemy.spawnPoint.rotation) as GameObject;
        Rigidbody attackRigidBody = attackObj.GetComponent<Rigidbody>();
        attackRigidBody.AddForce(attackRigidBody.transform.forward * enemy.attackForce);
        GameObject.Destroy(attackObj, enemy.attackDecay);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            enemy.SwitchState(enemy.restState);
        }
    }

    public override void OnCollisionEnter(EnemyStateManager enemy, Collision collision)
    {
        
    }

    public override void OnCollisionExit(EnemyStateManager enemy, Collision collision)
    {
        
    }
}
