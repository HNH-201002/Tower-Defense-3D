using UnityEngine;

[CreateAssetMenu(fileName ="Prefab Enemy" ,menuName ="Create New Data Prefab/Enemy",order = 0)]
public class EnemySAO : ScriptableObject
{
    public enum TypeEnemy
    {
        BlackKnight,
        FlyingDemon,
        Lizard,
        WereWolf
    }
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    [SerializeField] private TypeEnemy _enemyType;
    public GameObject GetPrefab() => _prefab;
    public float Health() => _health;
    public float Speed() => _speed;

    public TypeEnemy GetTypeEnemy() => _enemyType;
}
