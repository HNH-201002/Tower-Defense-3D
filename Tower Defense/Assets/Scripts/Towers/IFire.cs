using UnityEngine;

public interface IFire
{
    bool ContinueFiring { get; }
    void Fire(Transform projectile, Transform target);
    void UpdateFiring(Transform projectile, Transform target);
}
