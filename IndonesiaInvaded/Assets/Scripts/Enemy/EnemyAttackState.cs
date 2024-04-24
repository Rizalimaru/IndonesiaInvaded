using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    float animDelay;
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        Debug.Log("Enemy is Attacking");
        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", true);
        enemy.animator.SetBool("isResting", false);
        animDelay = 1.0f;

        GameObject attackObj = GameObject.Instantiate(enemy.attackType, enemy.spawnPoint.transform.position, enemy.spawnPoint.rotation) as GameObject;
        Rigidbody attackRigidBody = attackObj.GetComponent<Rigidbody>();
        attackRigidBody.AddForce(attackRigidBody.transform.forward * enemy.attackForce);
        GameObject.Destroy(attackObj, enemy.attackDecay);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (animDelay > 0)
        {
            animDelay -= Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(enemy.agent.transform.position, enemy.target.transform.position) >= enemy.attackDistance + 1)
            {
                enemy.SwitchState(enemy.movingState);
            }

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
