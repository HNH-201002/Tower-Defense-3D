using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnemySAO;

public class EnemyHealth : HealthBase
{
    [HideInInspector] public EnemyWaveSpawnManager EnemyWaveSpawnManager;
    [SerializeField] private EnemySAO data;
    private TypeEnemy _typeEnemy;
    private void Start()
    {
        base.Start();
        _uiHealth.gameObject.SetActive(false);
        _typeEnemy = data.GetTypeEnemy();
    }
    protected override float GetHealthData()
    {
        return data.Health();
    }

    protected override void AddPool(GameObject gameObject)
    {
        EnemyWaveSpawnManager.DeactivateAndAddToPool(_typeEnemy, this.gameObject);
    }

    public override void Death()
    {
        GetComponent<EnemyStateController>().hasDied = true;
    }
}
