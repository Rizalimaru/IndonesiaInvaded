using UnityEngine;

public class BossDeadState : BossBaseState
{
    float delay;

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Boss is defeated");

        boss.bossObject.agent.enabled = false;

        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isDead", true);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);

        delay = 1f;
    }

    public override void UpdateState(BossStateManager boss)
    {
        Object.Destroy(boss.gameObject, delay);
    }
}
