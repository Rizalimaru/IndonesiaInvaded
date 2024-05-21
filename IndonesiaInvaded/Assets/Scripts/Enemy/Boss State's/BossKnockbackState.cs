using UnityEngine;
using UnityEngine.AI;

public class BossKnockbackState : BossBaseState
{
    float delay;
    float force;
    Vector3 direction;

    public override void EnterState(BossStateManager boss)
    {
        if (boss.bossObject.knockbackForce > boss.bossObject.knockbackGuard)
        {
            force = boss.bossObject.knockbackForce - boss.bossObject.knockbackGuard;
        }
        else
        {
            force = 5f;
        }

        delay = 1f;

        Debug.Log("Boss got knockbacked");

        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", true);
        boss.animator.SetBool("repositioning", false);
        boss.animator.SetBool("combo", false);

        boss.GetComponent<NavMeshAgent>().enabled = false;

        boss.bossObject.GetComponent<Rigidbody>().useGravity = true;
        boss.bossObject.GetComponent<Rigidbody>().isKinematic = false;

        direction = boss.transform.forward * -1;

        boss.GetComponent<Rigidbody>().AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {

            if (boss.bossObject.knockbackForce > 50)
            {
                boss.bossObject.knockbackForce = 35f;
            }

            boss.bossObject.transform.LookAt(boss.bossObject.target.position);
            boss.bossObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            boss.bossObject.GetComponent<Rigidbody>().useGravity = false;
            boss.bossObject.GetComponent<Rigidbody>().isKinematic = true;
            boss.bossObject.agent.Warp(boss.bossObject.transform.position);
            boss.GetComponent<NavMeshAgent>().enabled = true;

            boss.SwitchState(boss.restState);
        }
    }
}
