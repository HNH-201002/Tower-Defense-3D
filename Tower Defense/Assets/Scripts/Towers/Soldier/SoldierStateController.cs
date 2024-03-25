using System;
using System.Collections;
using UnityEngine;

public class SoldierStateController : MonoBehaviour
{
    ISoldierState currentState;

    private readonly int _hitAniHash = Animator.StringToHash("hit");

    private float _speed;
    private float _damage;
    private float _attackSpeed;
    private float _health;
    private float _rangeAttack;
    private float _armor;
    [HideInInspector] public Transform _origin;
    [HideInInspector] public GameObject _enemiesDetected;

    public float Damage { get { return _damage; } }

    public float AttackSpeed { get { return _attackSpeed; } }

    public float Speed { get { return _speed; } }

    public float Range { get { return _rangeAttack; } }

    public float Health { get { return _health; } }

    public float Armor { get { return _armor; } }


    [HideInInspector]  public SoldierOrderedState _orderedState;
    [HideInInspector]  public SoldierIdleState _idleState;
    [HideInInspector]  public SoldierAttackState _attackState;
    public Animator ani;
    public bool hasDied;
    public event Action<SoldierStateController,GameObject> OnEnemyDied;
    public event Action OnNewEnemy;

    private SoldierSAO _data;
    public void SetData(SoldierSAO data) => _data = data;
    private void Awake()
    {
        _idleState = new SoldierIdleState();
        _attackState = new SoldierAttackState();
        _orderedState = new SoldierOrderedState();
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        _speed = _data.Speed;
        _damage = _data.Damage;
        _attackSpeed = _data.AttackSpeed;
        _health = _data.Health;
        _rangeAttack = _data.Range;
        _armor = _data.Armor;
        ChangeState(_idleState);
    }
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }
    public void ChangeState(ISoldierState newState)
    {
        currentState = newState;
        currentState.OnEnter(this);
        if (currentState == _attackState)
        {
            OnNewEnemy?.Invoke();
        }
    }
    //Event animation
    public void Attack()
    {
        if (!_enemiesDetected) return;
        EnemyHealth enemyHealth = _enemiesDetected.GetComponent<EnemyHealth>();
        enemyHealth.TakeDamage(_damage);
        if (enemyHealth.HasDied)
        {
            EnemyDied();
        }
    }

    public void EnemyDied()
    {
        OnEnemyDied?.Invoke(gameObject.GetComponent<SoldierStateController>(), _enemiesDetected);
        _enemiesDetected = null;
        ChangeState(_idleState);
    }

    private void OnEnable()
    {
        hasDied = false;
        ChangeState(_idleState);
    }
    public void Reset()
    {
        _enemiesDetected = null;
        hasDied = false;
        ChangeState(_idleState);
    }
}
