
public interface ISoldierState
{
    public void OnEnter(SoldierStateController state);
    public void UpdateState(SoldierStateController state);
    public void OnHurt(SoldierStateController state);
    public void OnExit(SoldierStateController state, ISoldierState state1);
}
