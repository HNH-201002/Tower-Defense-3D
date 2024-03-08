using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct EnemySpawnInfo
{
    public EnemySAO enemyPrefab;
    public int numberOfEnemy;
    public PatrolMove movePoints;
}

[Serializable]
struct WaveSpawnInfo
{
    public EnemySpawnInfo[] enemySpawnDataStructure;
    public bool isHappened;
}

public class EnemyWaveSpawnManager : MonoBehaviour
{
    [SerializeField] private WaveSpawnInfo[] _data;
    [SerializeField] private float _timeBetweenOtherWave;
    [SerializeField] private float _delayBetweenEnemies; 
    [SerializeField] private Transform _spawnLocation;
    private Dictionary<EnemySAO.TypeEnemy, List<GameObject>> storedGameObject = new Dictionary<EnemySAO.TypeEnemy, List<GameObject>>();
    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        foreach (var waveInfo in _data)
        {
            yield return StartCoroutine(SpawnWave(waveInfo));
            yield return new WaitForSeconds(_timeBetweenOtherWave);
        }
    }

    private IEnumerator SpawnWave(WaveSpawnInfo waveInfo)
    {
        foreach (var enemyInfo in waveInfo.enemySpawnDataStructure)
        {
            for (int i = 0; i < enemyInfo.numberOfEnemy; i++)
            {
                GameObject enemyGO = GetPooledEnemy(enemyInfo.enemyPrefab.GetTypeEnemy());
                
                if (enemyGO == null)
                {
                    enemyGO = Instantiate(enemyInfo.enemyPrefab.GetPrefab(), _spawnLocation.position, Quaternion.identity);
                }
                else
                {
                    enemyGO.transform.position = _spawnLocation.position;
                    enemyGO.transform.rotation = Quaternion.identity;
                    enemyGO.SetActive(true);
                }
                Enemy enemyScript = enemyGO.GetComponent<Enemy>();
                enemyScript.EnemyWaveSpawnManager = this;
                enemyScript.SetPatrol(enemyInfo.movePoints.PatrolPoints());
                yield return new WaitForSeconds(_delayBetweenEnemies);
            }
        }
    }

    public GameObject GetPooledEnemy(EnemySAO.TypeEnemy typeEnemy)
    {
        if (storedGameObject.ContainsKey(typeEnemy))
        {
            foreach (var pooledEnemy in storedGameObject[typeEnemy])
            {
                if (!pooledEnemy.activeInHierarchy)
                {
                    return pooledEnemy;
                }
            }
        }
        return null;
    }
    //ObjectPooling
    public void AddEnemyToStored(EnemySAO.TypeEnemy typeEnemy,GameObject enemy)
    {
        if (storedGameObject.ContainsKey(typeEnemy))
        {
            storedGameObject[typeEnemy].Add(enemy);
        }
        else
        {
            List<GameObject> newList = new List<GameObject> { enemy };
            storedGameObject.Add(typeEnemy, newList);
        }
    }
    public void DeactivateAndAddToPool(EnemySAO.TypeEnemy typeEnemy, GameObject enemy)
    {
        enemy.SetActive(false);
        AddEnemyToStored(typeEnemy, enemy);
    }
}