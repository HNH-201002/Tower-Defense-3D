using Unity.VisualScripting;
using UnityEngine;

public class SoldierIdleState : ISoldierState
{
    private SoldierStateController _controller;
    public void OnEnter(SoldierStateController state)
    {
        _controller = state;
        state.ani.SetBool("Attack", false);
        state.ani.SetBool("Chase", true);
    }

    public void UpdateState(SoldierStateController state)
    {
        if (state.hasDied) return;
        if (!state._origin) return;
        if (state._enemiesDetected)
        {
            state.ChangeState(state._attackState);
        }
        if (Mathf.Abs(state._origin.position.x - state.transform.position.x) >= 0.2f)
        {
            state.gameObject.transform.position = Vector3.MoveTowards(state.gameObject.transform.position,
                                                                      _controller._origin.position,
                                                                      _controller.Speed * Time.deltaTime);

            Vector3 directionToOrigin = (_controller._origin.position - state.transform.position);
            directionToOrigin.y = 0; 
            directionToOrigin = directionToOrigin.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToOrigin);
            state.transform.rotation = Quaternion.Slerp(state.transform.rotation, targetRotation,20 * Time.deltaTime);
        }
        else
        {
            state.ani.SetBool("Chase", false);
        }
    }
    public void OnExit(SoldierStateController state, ISoldierState state1)
    {

    }

    public void OnHurt(SoldierStateController state)
    {

    }
}
