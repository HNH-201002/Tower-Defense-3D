using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private TowerSAO towerData;
    [SerializeField] private LayerMask mask;
    protected float _attackSpeed;
    protected float _range;
    protected float _damage;
    protected int _enemiesPerAttack;
    protected float _projectileSpeed;
    Collider[] enemiesPerAttack;
    private void Awake()
    {
        _attackSpeed = towerData.AttackSpeed;
        _range = towerData.Range;
        _damage = towerData.Damage;
        _enemiesPerAttack = towerData.EnemiesPerAttack;
        _projectileSpeed = towerData.ProjectileSpeed;
        enemiesPerAttack = new Collider[_enemiesPerAttack];
    }
    private void Update()
    {
        Detect();
    }
    protected abstract void Attack();

    protected virtual Collider[] Detect()
    {
        int enemyDetected = Physics.OverlapSphereNonAlloc(transform.position,_range,enemiesPerAttack,mask);
        if (enemyDetected > 0) Attack();
        return enemiesPerAttack;
    }
}
