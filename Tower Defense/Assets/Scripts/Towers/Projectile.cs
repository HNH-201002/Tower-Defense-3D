using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Ranged _ranged;
    private Transform target;
    private IFire _fire;
    [SerializeField] private Transform flashPrefab;
    [SerializeField] private Transform hitPrefab;
    public void Initialize(Transform target,IFire fire)
    {
        this.target = target;
        _fire = fire;
        if(flashPrefab != null) flashPrefab.gameObject.SetActive(true);
        if(hitPrefab  != null) hitPrefab.gameObject.SetActive(false);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.SetPositionAndRotation(transform.position,transform.rotation);

        if (target != null)
        {
            if(flashPrefab != null)
            {
                if (flashPrefab.gameObject.activeInHierarchy)
                {
                    flashPrefab.transform.forward = gameObject.transform.forward;
                    flashPrefab.rotation = Quaternion.identity;
                    flashPrefab.transform.position = gameObject.transform.position;
                    StartCoroutine(DelayBeforeDisapper(flashPrefab.gameObject, 1));
                }
            }
            _fire.Fire(gameObject.transform, target);
        }
    }

    private void Update()
    {
        if (target != null && _fire != null)
        {
            _fire.UpdateFiring(gameObject.transform, target);
        }
    }
    IEnumerator DelayBeforeDisapper(GameObject objectToHandle,float time)
    {
        yield return new WaitForSeconds(time);
        objectToHandle.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {

            if (hitPrefab != null) hitPrefab.gameObject.SetActive(true);
            StartCoroutine(DelayBeforeDisapper(hitPrefab.gameObject));
        }
    }
    IEnumerator DelayBeforeDisapper(GameObject objectToHandle)
    {
        yield return new WaitForSeconds(0.5f);
        objectToHandle.SetActive(false);
        _ranged.AddProjectileToPool(gameObject);
    }
    public void SetRanged(Ranged ranged)
    {
        _ranged = ranged;
    }
}