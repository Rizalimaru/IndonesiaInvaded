using UnityEngine;
using UnityEngine.AI;

public class EnemyKnockbackState : EnemyBaseState
{
    float delay;
    float force;
    Vector3 direction;

    public override void EnterState(EnemyStateManager enemy)
    {
        if (enemy.enemyObject.knockbackForce > enemy.enemyObject.knockbackGuard)
        {
            force = enemy.enemyObject.knockbackForce - enemy.enemyObject.knockbackGuard;
        }
        else
        {
            force = 5f;
        }

        delay = enemy.enemyObject.animDelay;

        Debug.Log("Enemy got knockbacked");

        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isAttacking", false);
        enemy.animator.SetBool("isResting", false);
        enemy.animator.SetBool("isDead", false);
        enemy.animator.SetBool("isStunned", true);
        enemy.animator.SetBool("repositioning", false);

        enemy.GetComponent<NavMeshAgent>().enabled = false;

        enemy.enemyObject.GetComponent<Rigidbody>().useGravity = true;
        enemy.enemyObject.GetComponent<Rigidbody>().isKinematic = false;

        direction = enemy.transform.forward * -1;
        
        enemy.GetComponent<Rigidbody>().AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            if (enemy.enemyObject.knockbackForce > 90f)
            {
                enemy.enemyObject.knockbackForce = 25f;
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
