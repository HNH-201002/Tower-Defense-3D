using UnityEngine;


public class EnemyCombatState : IEnemyState
{
    private const float AttackRangeSqr = 4.5f * 4.5f;
    private const float RotationSpeed = 20f;
    private readonly int ANI_ATTACK = Animator.StringToHash("Attack");
    public void OnEnter(EnemyStateController state)
    {
        state.ani.SetBool("Combat", true);
        state.hasCombating = true;
    }
    public void UpdateState(EnemyStateController state)
    {
        if (state.hasDied) return;

        if (!state.soldier.gameObject.activeInHierarchy)
        {
            state.ChangeState(state._walkState);
            return; 
        }
        Vector3 targetDirection = state.soldier.transform.position - state.transform.position;
        targetDirection.y = 0; 

        if (targetDirection.sqrMagnitude < AttackRangeSqr) 
        {
            state.ani.SetTrigger(ANI_ATTACK);
        }

        if (targetDirection != Vector3.zero) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection.normalized);
            state.gameObject.transform.rotation = Quaternion.Slerp(state.gameObject.transform.rotation,
                                                                   lookRotation,
                                                                   Time.deltaTime * RotationSpeed);
        }
    }
    public void OnHurt(EnemyStateController state)
    {

    }
    public void OnExit(EnemyStateController state, IEnemyState nextState)
    {

    }
}
