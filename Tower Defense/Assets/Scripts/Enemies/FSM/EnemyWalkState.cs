using UnityEngine;

public class EnemyWalkState : IEnemyState
{
    public void OnEnter(EnemyStateController state)
    {
        state.ani.ResetTrigger("Attack");
        state.ani.SetBool("Combat",false);
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
