using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyCombatState : IEnemyState
{
    
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

        if (targetDirection.sqrMagnitude < 4.5f * 4.5f) 
        {
            state.ani.SetTrigger("Attack");
        }

        if (targetDirection != Vector3.zero) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection.normalized);
            state.gameObject.transform.rotation = Quaternion.Slerp(state.gameObject.transform.rotation,
                                                                   lookRotation,
                                                                   Time.deltaTime * 20);
        }
    }
    public void OnHurt(EnemyStateController state)
    {

    }
    public void OnExit(EnemyStateController state, IEnemyState state1)
    {

    }
}
