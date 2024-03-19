
public interface IEnemyState
{
    public void OnEnter(EnemyStateController state);
    public void UpdateState(EnemyStateController state);
    public void OnHurt(EnemyStateController state);
    public void OnExit(EnemyStateController state, IEnemyState state1);
}
