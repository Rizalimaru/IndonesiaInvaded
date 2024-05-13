using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyStateManager : MonoBehaviour
{
    // State Declaration
    public EnemyBaseState currentState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState movingState = new EnemyChaseState();
    public EnemyRestState restState = new EnemyRestState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyRepositionState repositionState = new EnemyRepositionState();
    public EnemyDeadState deadState = new EnemyDeadState();
    public EnemyKnockbackState knockbackState = new EnemyKnockbackState();
    public EnemyConfusedState confusedState = new EnemyConfusedState();

    // Enemy Declaration
    public Enemy enemyObject;
    public Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyObject.SetupAgent();
        currentState = idleState;
    }

    void Start()
    { 
        currentState.EnterState(this);
    }

    public void StartAgent()
    {
        currentState = idleState;
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    private void OnCollisionExit(Collision collision) 
    { 
        currentState.OnCollisionExit(this, collision); 
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

}
