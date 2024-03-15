using System.Collections;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private TowerSAO towerData;
    protected LayerMask _mask;
    protected float _attackSpeed;
    protected float _range;
    protected float _damage;
    protected int _enemiesPerAttack;
    protected float _projectileSpeed;
    private Coroutine attackRoutine;

    protected void Awake()
    {
        _attackSpeed = towerData.AttackSpeed;
        _range = towerData.Range;
        _damage = towerData.Damage;
        _enemiesPerAttack = towerData.EnemiesPerAttack;
        _projectileSpeed = towerData.ProjectileSpeed;
        _mask = towerData.Mask;
    }

    protected void Start()
    {
        attackRoutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            Detect();
            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    protected abstract void Attack(Collider[] enemies);

    protected virtual void Detect()
    {
        Collider[] enemies = new Collider[_enemiesPerAttack];
        int enemiesDetected = Physics.OverlapSphereNonAlloc(transform.position, _range, enemies,_mask);

        if (enemiesDetected > 0)
        {
            Attack(enemies);
        }
    }
    private void OnDisable()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}