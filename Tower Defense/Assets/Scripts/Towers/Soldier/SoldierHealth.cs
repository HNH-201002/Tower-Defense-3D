using System;
using System.Collections;
using UnityEngine;
using static EnemySAO;

public class SoldierHealth : HealthBase
{
    public event Action<GameObject, GameObject> OnSoldierDied;
    private bool isRestorationStopped = false; // Renamed 'check' to 'isRestorationStopped'

    private void Start()
    {
        base.Start();
        _uiHealth.gameObject.SetActive(true);
    }

    protected override void AddPool(GameObject gameObject)
    {
        OnSoldierDied?.Invoke(gameObject, GetComponent<SoldierStateController>()._enemiesDetected);
    }

    protected override float GetHealthData()
    {
        return 200;
    }

    public override void Death()
    {
        _uiHealth.gameObject.SetActive(true);
        SoldierStateController soldierStateController = GetComponent<SoldierStateController>();
        soldierStateController.hasDied = true;
    }

    private void StopRestoreHealth()
    {
        isRestorationStopped = true; // Use the renamed variable here
    }

    private void RestoreHealth(SoldierStateController soldier, GameObject enemy)
    {
        isRestorationStopped = false; // Use the renamed variable here
        StartCoroutine(DelayedRestoreHealth(3f));
    }

    IEnumerator DelayedRestoreHealth(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isRestorationStopped) // Only proceed if restoration is not stopped
        {
            StartCoroutine(RestoreHealth());
        }
    }

    IEnumerator RestoreHealth()
    {
        while (_health < 200 && !isRestorationStopped) // Check the renamed variable in the condition
        {
            _health += 200 * (20f / 100);
            _uiHealth.value = _health;
            if (_health > 200) _health = 200;
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnEnable()
    {
        base.OnEnable();
        SoldierStateController soldierStateController = GetComponent<SoldierStateController>();
        soldierStateController.OnEnemyDied += RestoreHealth;
        soldierStateController.OnNewEnemy += StopRestoreHealth;
    }

    private void OnDisable()
    {
        base.OnDisable();
        SoldierStateController soldierStateController = GetComponent<SoldierStateController>();
        soldierStateController.OnEnemyDied -= RestoreHealth;
        soldierStateController.OnNewEnemy -= StopRestoreHealth;
    }
}