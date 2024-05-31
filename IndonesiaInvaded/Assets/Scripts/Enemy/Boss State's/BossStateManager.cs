using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossStateManager : MonoBehaviour
{
    // State Declaration
    public BossBaseState currentState;
    public BossIdleState idleState = new BossIdleState();
    public BossChaseState movingState = new BossChaseState();
    public BossRestState restState = new BossRestState();
    public BossAttackState attackState = new BossAttackState();
    public BossRepositionState repositionState = new BossRepositionState();
    public BossDeadState deadState = new BossDeadState();
    public BossKnockbackState knockbackState = new BossKnockbackState();
    public BossComboState comboState = new BossComboState();

    // Ondel-Ondel Skills State
    public OndelOndelFirstSkillState ondelFirstSkillState = new OndelOndelFirstSkillState();
    public BossDashToTargetState ondelDashingState = new BossDashToTargetState();
    public OndelOndelSecondSkillState ondelSecondSkillState = new OndelOndelSecondSkillState();

    // Dukun Skill State
    public DukunFirstSkillState dukunFirstSkillState = new DukunFirstSkillState();
    public DukunCastingState dukunCastingState = new DukunCastingState();
    public DukunSecondSkillState dukunSecondSkillState = new DukunSecondSkillState();

    // Combo Counter
    [HideInInspector] public int comboCounter = 0;

    // Boss Declaration
    public Boss bossObject;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bossObject.SetupAgent();
        currentState = idleState;
    }

    void Start()
    {
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Ada 3");
            bossObject.firstSkillCounter = 3;
        }
    }

    public void StartAgent()
    {
        currentState = idleState;
    }

    public void SwitchState(BossBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void FirstSkillCounting()
    {
        float randNum;
        randNum = Random.Range(0.1f, 100f);

        if (randNum < bossObject.firstSkillChance)
        {
            bossObject.firstSkillCounter++;
        }
    }

    public void SecondSkillCounting()
    {
        float randNum;
        randNum = Random.Range(0.1f, 100f);

        if (randNum < bossObject.secondSkillChance)
        {
            bossObject.secondSkillCounter++;
        }
    }
}
