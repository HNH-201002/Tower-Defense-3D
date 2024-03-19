using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    IEnemyState currentState;

    private readonly int _hitAniHash = Animator.StringToHash("hit");

    [SerializeField] private EnemySAO data;
    private float _speed;
    private float _damage;
    private float _attackSpeed;
    private float _health;
    private float _rangeAttack;
    private LayerMask _layerMask;
    [HideInInspector] public Transform _origin;

    public float Damage { get { return _damage; } }

    public float AttackSpeed { get { return _attackSpeed; } }

    public float Speed { get { return _speed; } }

    public float Range { get { return _rangeAttack; } }

    public float Health { get { return _health; } }

    public LayerMask LayerEnemy { get { return _layerMask; } }

    [HideInInspector] public EnemyWalkState _walkState;
    [HideInInspector] public EnemyCombatState _combatState;

    Enemy _enemy;

    public Enemy GetEnemy { get { return _enemy; } }

    [HideInInspector]  public Animator ani;

    [HideInInspector] public bool hasCombating;

    [HideInInspector] public GameObject soldier;

    public bool hasDied;

    public static event Action<EnemyStateController> OnSoldierDied;
    private void Awake()
    {
        _speed = data.Speed();
        _damage = data.Damage();
        _attackSpeed = data.AttackSpeed();
        _health = data.Health();
        _rangeAttack = data.AttackRange();
        _layerMask = data.LayerMask();
        _walkState = new EnemyWalkState();
        _combatState = new EnemyCombatState();
        _enemy = GetComponent<Enemy>();
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        ChangeState(_walkState);
    }
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }
    public void ChangeState(IEnemyState newState)
    {
        currentState = newState;
        currentState.OnEnter(this);
    }
    // Animation Event
    public void AttackSoldier()
    {
        if (soldier == null) return;
        SoldierHealth soldierHealth = soldier.GetComponent<SoldierHealth>();
        soldierHealth.TakeDamage(_damage);
        if (soldierHealth.HasDied)
        {
            soldier = null;
            OnSoldierDied?.Invoke(gameObject.GetComponent<EnemyStateController>());
            ChangeState(_walkState);
        }
    }
    public void OnEnable()
    {
        hasDied = false;
    }
}
