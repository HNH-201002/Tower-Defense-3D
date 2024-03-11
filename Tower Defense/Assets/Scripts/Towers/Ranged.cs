using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Tower
{
    [SerializeField] private Transform[] shootOrigins;
    [SerializeField] private GameObject projectilePrefab;
    private bool isReadyToAttack = true;
    protected override void Attack()
    {
        if (isReadyToAttack)
        {
            StartCoroutine(DelayPerShoot());
        }
    }

    private IEnumerator DelayPerShoot()
    {
        isReadyToAttack = false;
        Shoot();
        yield return new WaitForSeconds(_attackSpeed);
        isReadyToAttack = true;
    }
    private void Shoot()
    {
        if (shootOrigins.Length > 0 && Detect()[0] != null)
        {
            GameObject projectileGameObject = Instantiate(projectilePrefab, shootOrigins[0].position, Quaternion.identity);
            Projectile projectileScript = projectileGameObject.GetComponent<Projectile>(); 
            projectileScript.Initialize(Detect()[0].transform, _projectileSpeed); 
        }
    }
}
