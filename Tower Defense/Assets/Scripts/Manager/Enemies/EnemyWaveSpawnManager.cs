using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text timerWaveText;

    private int enemiesRemainingToDie;

    private static EnemyWaveSpawnManager _instance;
    public static EnemyWaveSpawnManager Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    private const string SFX_WAVE_NEXT_WAVE = "NextWave";
    private const string SFX_WAVE_WAVE_COMING = "WaveComing";
    private void Awake()
    {
        UpdateWaveText(0, _timeBetweenOtherWave);
        StartCoroutine(SpawnWaves());
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private IEnumerator SpawnWaves()
    {
        enemiesRemainingToDie = 0;
        for (int i = 0; i < _data.Length; i++)
        {
            SoundManager.Instance.PlaySound(SFX_WAVE_WAVE_COMING);
            UpdateWaveText(i + 1, _timeBetweenOtherWave);
            StartCoroutine(CountdownWaveTimer(_timeBetweenOtherWave));
            yield return StartCoroutine(SpawnWave(_data[i]));
            if (i < _data.Length - 1)
            {
                yield return new WaitForSeconds(_timeBetweenOtherWave);    
            }
        }

        yield return new WaitUntil(() => enemiesRemainingToDie <= 0);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.Win();
    }

    private void UpdateWaveText(int currentWaveIndex,float time)
    {
        waveText.text = $"Wave {currentWaveIndex}/{_data.Length}";
        timerWaveText.text = time.ToString();
    }
    private IEnumerator CountdownWaveTimer(float time)
    {
        float remainingTime = time;
        while (remainingTime > 0)
        {
            timerWaveText.text = Mathf.CeilToInt(remainingTime).ToString();

            yield return new WaitForSeconds(1f);

            remainingTime -= 1f;
        }

        timerWaveText.text = "0";
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

                enemiesRemainingToDie++;
                Enemy enemyScript = enemyGO.GetComponent<Enemy>();
                enemyScript.EnemyWaveSpawnManager = this;
                EnemyHealth enemyHealth = enemyGO.GetComponent<EnemyHealth>();  //singleton
                enemyHealth.EnemyWaveSpawnManager = this;
                enemyScript.SetPatrol(enemyInfo.movePoints.PatrolPoints());
                yield return new WaitForSeconds(_delayBetweenEnemies);
            }
        }
        SoundManager.Instance.PlaySound(SFX_WAVE_NEXT_WAVE);
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
    public void OnEnemyDeath()
    {
        enemiesRemainingToDie--;
    }
}