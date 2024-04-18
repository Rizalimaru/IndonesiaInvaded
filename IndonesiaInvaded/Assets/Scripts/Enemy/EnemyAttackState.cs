using UnityEngine;
using UnityEngine.AI;

// The attacking window is calculated with enemy.attackSpeed + 1 from idle state delay

public class EnemyAttackState : EnemyBaseState
{
    float delay;

    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        Debug.Log("Enemy is Attacking");
        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", true);
        delay = enemy.attackSpeed;

        GameObject attackObj = GameObject.Instantiate(enemy.attackType, enemy.spawnPoint.transform.position, enemy.spawnPoint.rotation) as GameObject;
        Rigidbody attackRigidBody = attackObj.GetComponent<Rigidbody>();
        attackRigidBody.AddForce(attackRigidBody.transform.forward * enemy.attackForce);
        GameObject.Destroy(attackObj, enemy.attackDecay);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            enemy.SwitchState(enemy.idleState);
        }
        
        if (Vector3.Distance(enemy.agent.transform.position, enemy.target.transform.position) >= enemy.attackDistance + 1)
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
