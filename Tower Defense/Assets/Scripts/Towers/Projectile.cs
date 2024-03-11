using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed;

    public void Initialize(Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}