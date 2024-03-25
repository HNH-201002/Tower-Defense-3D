using UnityEngine;

[CreateAssetMenu(fileName ="Prefab Enemy" ,menuName ="Create New Data Prefab/Enemy",order = 0)]
public class EnemySAO : ScriptableObject
{
    public enum TypeEnemy
    {
        BlackKnight,
        FlyingDemon,
        Lizard,
        WereWolf,
        Rat,
        Turtle,
        Orc,
        Skeleton
    }
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackRange;
    [Tooltip("Percentage %")]
    [SerializeField] private float armor;
    [SerializeField] private int bounty;
    [SerializeField] private TypeEnemy _enemyType;
    [Space(2)]
    [SerializeField] private LayerMask _soldierLayerMask;
    public GameObject GetPrefab() => _prefab;
    public float Health() => _health;
    public float Speed() => _speed;

    public float Damage() => _damage;

    public float AttackSpeed() => _attackSpeed;

    public float AttackRange() => _attackRange;

    public float Armor { get { return armor; } }

    public int Bounty { get { return bounty; } }

    public LayerMask SoldierLayerMask() => _soldierLayerMask;
    public TypeEnemy GetTypeEnemy() => _enemyType;
}
