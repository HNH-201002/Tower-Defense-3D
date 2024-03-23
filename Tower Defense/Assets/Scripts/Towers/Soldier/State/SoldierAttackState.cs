using UnityEngine;

public class SoldierAttackState : ISoldierState
{
    private SoldierStateController _controller;
    GameObject _target;
    private const string SFX_COMBAT = "Combat_Barrack";
    public void OnEnter(SoldierStateController state)
    {
        _controller = state;
        _target = state._enemiesDetected.gameObject;     
    }

    public void UpdateState(SoldierStateController state)
    {
        if (state.hasDied) return;
        Vector3 targetDirection = _target.transform.position - state.transform.position;

        if (Vector3.Distance(state.transform.position, _target.transform.position) > 4.5f)
        {
            state.ani.SetBool("Chase",true);
            state.gameObject.transform.position = Vector3.MoveTowards(state.gameObject.transform.position,
                                                                      _target.transform.position,
                                                                      _controller.Speed * Time.deltaTime);
        }
        else
        {
            state.ani.SetBool("Chase", false);
            state.ani.SetBool("Attack",true);
        }
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection.normalized);

        state.gameObject.transform.rotation = Quaternion.Slerp(state.gameObject.transform.rotation,
                                                               lookRotation,
                                                               Time.deltaTime * 20);
    }
    public void OnExit(SoldierStateController state, ISoldierState state1)
    {

    }

    public void OnHurt(SoldierStateController state)
    {

    }
}
