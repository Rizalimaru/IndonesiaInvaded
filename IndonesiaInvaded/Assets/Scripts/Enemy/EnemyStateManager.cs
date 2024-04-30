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

    // Enemy Declaration
    public Enemy enemyObject;
    public Animator animator;
    

    void Start()
    { 
        // For single enemy testing uncomment StartAgent() and enable NavMeshAgent component in inspector

        animator = GetComponent<Animator>();
        
        enemyObject.SetupAgent();
        
        StartAgent();

        currentState.EnterState(this);
    }

    public void StartAgent()
    {
        // StartAgent is used for external script to enable each enemy statemanager

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
