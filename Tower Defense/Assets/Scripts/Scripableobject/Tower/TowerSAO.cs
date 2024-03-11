using UnityEngine;

[CreateAssetMenu(fileName ="New Tower Data", menuName ="Create New Tower Data/Tower",order =0)]
public class TowerSAO : ScriptableObject
{
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float range;
    [SerializeField] private int enemiesPerAttack;
    [SerializeField] private float projectileSpeed;

    public float Damage { get { return damage; } }

    public float AttackSpeed { get {  return attackSpeed; } }

    public float Range {  get { return range; } }

    public int EnemiesPerAttack { get { return enemiesPerAttack; } }

    public float ProjectileSpeed { get { return projectileSpeed; } }
}
