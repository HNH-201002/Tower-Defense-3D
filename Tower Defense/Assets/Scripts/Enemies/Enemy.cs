using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySAO enemyData;
    private EnemySAO.TypeEnemy _typeEnemy;
    private float health;
    private float speed;
    private Queue<Transform> _pointToMove;
    protected Rigidbody rb;
    private Transform _NextPointToMove;
    Animator ani;

    private readonly int _speedAniHash = Animator.StringToHash("Speed");
    public EnemySAO.TypeEnemy GetTypeEnemy() => _typeEnemy;
    [HideInInspector] public EnemyWaveSpawnManager EnemyWaveSpawnManager;
    private void Awake()
    {
        _typeEnemy = enemyData.GetTypeEnemy();
        ani = GetComponent<Animator>();
        health = enemyData.Health();
        speed = enemyData.Speed();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _NextPointToMove = _pointToMove.Dequeue();
    }
    private void Update()
    {
        Walk();
    }
    protected abstract void Attack();

    protected virtual void Hurt(float damage)
    {
        health -= damage;
    }
    protected virtual void Walk()
    {
        if (_pointToMove == null || _pointToMove.Count < 0) return;

        if (Vector3.Distance(_NextPointToMove.transform.position, transform.position) <= 0.03f)
        {
            if (_pointToMove.Count > 0)
            {
                _NextPointToMove = _pointToMove.Dequeue();
            }
            ani.SetFloat(_speedAniHash, 0);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position
                                                    , _NextPointToMove.transform.position
                                                    , speed * Time.deltaTime);
            ani.SetFloat(_speedAniHash, speed * Time.deltaTime);
            SetNextPoint(_NextPointToMove);
        }
    }

    private void SetNextPoint(Transform point)
    {
        Vector3 directionToPoint = point.transform.position - transform.position;
        if (directionToPoint.magnitude <= 0.0001f) return;
        _NextPointToMove = point;
        Quaternion rotation = Quaternion.LookRotation(point.transform.position - transform.position);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Lerp(current, rotation,7 * Time.deltaTime);
    }
    public void SetPatrol(List<Transform> patrolPoints) => _pointToMove = new Queue<Transform>(patrolPoints);

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("End"))
        {
            EnemyWaveSpawnManager.DeactivateAndAddToPool(_typeEnemy,this.gameObject);
        }
    }
}
