using UnityEngine;
using UnityEngine.AI;

public class DukunFirstSkillState : BossBaseState
{
    GameObject[] spawnPoints;
    float delay;
    bool checker;

    public override void EnterState(BossStateManager boss)
    {
        if (boss.bossObject.bossTitle == BossScriptableObject.title.Dukun) checker = boss.bossObject.CheckIfEnemySpawned();

        boss.bossObject.castingSkill = true;

        Debug.Log("Boss is Spawning Enemy!");

        spawnPoints = GameObject.FindGameObjectsWithTag("DukunSpawnPoint");
        
        boss.GetComponent<NavMeshAgent>().isStopped = true;

        boss.animator.SetBool("isWalking", false);
        boss.animator.SetBool("isAttacking", false);
        boss.animator.SetBool("isResting", false);
        boss.animator.SetBool("isDead", false);
        boss.animator.SetBool("isStunned", false);
        boss.animator.SetBool("repositioning", false);
        boss.animator.SetBool("firstSkill", true);

        delay = boss.bossObject.firstSkillAnimDelay;

        if (checker == false)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                boss.bossObject.DukunSpawning(spawnPoints[i].transform.position);
            }
        }
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            boss.bossObject.castingSkill = false;
            boss.bossObject.firstSkillCounter = 0;
            boss.SwitchState(boss.restState);
        }
    }
}
