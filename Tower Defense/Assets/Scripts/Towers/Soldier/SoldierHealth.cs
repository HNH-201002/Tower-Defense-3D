using System;
using System.Collections;
using UnityEngine;

public class SoldierHealth : HealthBase
{
    private SoldierStateController SoldierStateController;
    public event Action<GameObject, GameObject> OnSoldierDied;
    private bool isRestorationStopped = false;
    private const string SFX_DEAD = "Soldier_Dead";
    private void Awake()
    {
        SoldierStateController = GetComponent<SoldierStateController>();
    }
    private void Start()
    {
        base.Start();
        _uiHealth.gameObject.SetActive(true);
    }

    protected override void AddPool(GameObject gameObject)
    {
        OnSoldierDied?.Invoke(gameObject, SoldierStateController._enemiesDetected);
    }

    protected override string GetSfxDeadName()
    {
        return SFX_DEAD;
    }
    protected override float GetHealthData()
    {
        return SoldierStateController.Health;
    }
    protected override float GetArmorData()
    {
        return SoldierStateController.Armor;
    }
    public override void Death()
    {
        _uiHealth.gameObject.SetActive(true);
        SoldierStateController.hasDied = true;
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
        SoldierStateController.OnEnemyDied += RestoreHealth;
        SoldierStateController.OnNewEnemy += StopRestoreHealth;
    }

    private void OnDisable()
    {
        base.OnDisable();
        SoldierStateController.OnEnemyDied -= RestoreHealth;
        SoldierStateController.OnNewEnemy -= StopRestoreHealth;
    }
}