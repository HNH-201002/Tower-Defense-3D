using UnityEngine;

public class Canon : Ranged, IFire
{
    [Range(20.0f, 75.0f)] public float LaunchAngle;

    public bool ContinueFiring { get { return false; } }

    public void Fire(Transform projectile, Transform target)
    {
        Vector3 projectileXZPos = new Vector3(projectile.position.x, transform.position.y, projectile.position.z);
        Vector3 targetXZPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        projectile.LookAt(targetXZPos);

        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);

        float H = target.position.y - projectile.position.y;
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = projectile.TransformDirection(localVelocity);

        Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
        rigidbody.velocity = globalVelocity;
        projectile.rotation = Quaternion.LookRotation(rigidbody.velocity);
    }

    public void UpdateFiring(Transform projectile, Transform target)
    {
       
    }
}