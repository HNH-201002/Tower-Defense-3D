using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Tower 
{
    [SerializeField] private Transform shootOrigins;
    [SerializeField] private GameObject projectilePrefab;
    private Queue<GameObject> poolProjectile = new Queue<GameObject>();
    private Queue<GameObject> poolFlash = new Queue<GameObject>();
    private bool isReadyToAttack = true;
    IFire fire;
    private void Awake()
    {
        base.Awake();
        fire = GetComponent<IFire>();
    }
    private void Start()
    {
        base.Start();
    }
    protected override void Attack(Collider[] enemiesPerAttack)
    {

        if (isReadyToAttack)
        {
            StartCoroutine(DelayPerShoot(enemiesPerAttack));
        }
    }

    private IEnumerator DelayPerShoot(Collider[] enemiesPerAttack)
    {
        isReadyToAttack = false;
        Shoot(enemiesPerAttack);
        yield return new WaitForSeconds(_attackSpeed);
        isReadyToAttack = true;
    }
    private void Shoot(Collider[] enemiesPerAttack)
    {
        if (shootOrigins && enemiesPerAttack[0] != null)
        {
            GameObject projectileGameObject = GetProjectileFromPool();

            if (projectileGameObject == null)
            {
                projectileGameObject = Instantiate(projectilePrefab, shootOrigins.position, Quaternion.identity);
            }
            else
            {
                projectileGameObject.transform.position = shootOrigins.position;
                projectileGameObject.transform.rotation = Quaternion.identity;
                projectileGameObject.SetActive(true);
            }

            Projectile projectileScript = projectileGameObject.GetComponent<Projectile>();
            projectileScript.Initialize(enemiesPerAttack[0].transform, fire);
            projectileScript.SetRanged(this);
        }
    }
    private GameObject GetProjectileFromPool()
    {
        if (poolProjectile.Count <= 0) return null;
        return poolProjectile.Dequeue();
    }
    public void AddProjectileToPool(GameObject projectile)
    {
        projectile.SetActive(false);
        poolProjectile.Enqueue(projectile);
    }
}
