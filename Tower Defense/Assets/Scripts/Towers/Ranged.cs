using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ranged : Tower
{
    [SerializeField] private Transform shootOrigins;
    [SerializeField] private GameObject projectilePrefab;
    private Queue<GameObject> poolProjectile = new Queue<GameObject>();
    private bool isReadyToAttack = true;
    IFire fire;
    private string SFX_RELEASE;
    private string SFX_HIT;
    private void Awake()
    {
        base.Awake();
        fire = GetComponent<IFire>();
        SFX_RELEASE = SetSfxRelease();
        SFX_HIT = SetSfxHit();
    }
    private void Start()
    {
        base.Start();
    }
    protected abstract string SetSfxRelease();
    protected abstract string SetSfxHit();
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
        SoundManager.Instance.PlaySound(SFX_RELEASE);
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
            projectileScript.Initialize(enemiesPerAttack[0].transform, fire,_damage,SFX_HIT); //magic number
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
    protected override void OnEnable()
    {
        base.OnEnable();
        isReadyToAttack = true;
    }
}