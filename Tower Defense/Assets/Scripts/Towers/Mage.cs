
using System.Collections;
using UnityEngine;

public class Mage : Ranged, IFire
{
    public bool ContinueFiring { get { return false; } }

    public void Fire(Transform projectile,Transform target)
    {
      
    }

    public void UpdateFiring(Transform projectile, Transform target)
    {
        projectile.position = Vector3.MoveTowards(projectile.position, target.position, _projectileSpeed * Time.deltaTime);
    }
}
