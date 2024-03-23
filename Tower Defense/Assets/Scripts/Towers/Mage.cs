
using System.Collections;
using UnityEngine;

public class Mage : Ranged, IFire
{
    public bool ContinueFiring { get { return false; } }
    private const string SFX_RELEASE = "Release_Mage";
    private const string SFX_HIT = "Hit_Mage";

    public void Fire(Transform projectile,Transform target)
    {
        ani.SetTrigger("Attack");
    }

    public void UpdateFiring(Transform projectile, Transform target)
    {
        projectile.position = Vector3.MoveTowards(projectile.position, target.position, _projectileSpeed * Time.deltaTime);
    }
    protected override string SetSfxRelease()
    {
        return SFX_RELEASE;
    }
    protected override string SetSfxHit()
    {
        return SFX_HIT;
    }
}
