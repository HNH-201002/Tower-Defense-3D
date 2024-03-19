using System;
using System.Collections;
using UnityEngine;

public class SoldierStateController : MonoBehaviour
{
    ISoldierState currentState;

    private readonly int _hitAniHash = Animator.StringToHash("hit");

    [SerializeField] private SoldierSAO data;
    private float _speed;
    private float _damage;
    private float _attackSpeed;
    private float _health;
    private float _rangeAttack;
    [HideInInspector] public Transform _origin;
    [HideInInspector] public GameObject _enemiesDetected;

    public float Damage { get { return _damage; } }

    public float AttackSpeed { get { return _attackSpeed; } }

    public float Speed { get { return _speed; } }

    public float Range { get { return _rangeAttack; } }

    public float Health { get { return _health; } }


    [HideInInspector]  public SoldierOrderedState _orderedState;
    [HideInInspector]  public SoldierIdleState _idleState;
    [HideInInspector]  public SoldierAttackState _attackState;
    public Animator ani;
    public bool hasDied;
    public event Action<SoldierStateController,GameObject> OnEnemyDied;
    public event Action OnNewEnemy;
    private void Awake()
    {
        _speed = data.Speed;
        _damage = data.Damage;
        _attackSpeed = data.AttackSpeed;
        _health = data.Health;
        _rangeAttack = data.Range;
        _idleState = new SoldierIdleState();
        _attackState = new SoldierAttackState();
        _orderedState = new SoldierOrderedState();
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
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
            OnEnemyDied?.Invoke(gameObject.GetComponent<SoldierStateController>(),_enemiesDetected);
            _enemiesDetected = null;
            ChangeState(_idleState);
        }
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
    private void OnDisable()
    {
        _enemiesDetected = null;
    }
}
