using UnityEngine;

public class EnemyWalkState : IEnemyState
{
    private readonly int ANI_ATTACK = Animator.StringToHash("Attack");
    private readonly int ANI_COMBAT = Animator.StringToHash("Combat");
    public void OnEnter(EnemyStateController state)
    {
        state.ani.ResetTrigger(ANI_ATTACK);
        state.ani.SetBool(ANI_COMBAT, false);
        state.hasCombating = false;
    }
    public void UpdateState(EnemyStateController state)
    {
        if (state.hasDied) return;
        state.GetEnemy.Walk();
    }

    public void OnHurt(EnemyStateController state)
    {

    }
    public void OnExit(EnemyStateController state, IEnemyState state1)
    {
        
    }

}
