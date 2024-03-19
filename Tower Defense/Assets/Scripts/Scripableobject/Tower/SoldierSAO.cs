using UnityEngine;

[CreateAssetMenu(fileName = "New Soldier Data", menuName = "Create New Soldier Data/Soldier", order = 0)]
public class SoldierSAO : ScriptableObject
{
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float health;

    public float Damage { get { return damage; } }

    public float AttackSpeed { get { return attackSpeed; } }

    public float Speed { get { return speed; } }

    public float Range { get { return range; } }

    public float Health { get { return health; } }

}