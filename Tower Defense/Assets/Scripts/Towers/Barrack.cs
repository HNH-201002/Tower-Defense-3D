using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Barrack : Tower
{
    [SerializeField] private Transform origin;
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private int amountOfSoldier;
    [SerializeField] private SoldierSAO soldierSAO;

    private Queue<SoldierStateController> soldiersWaitForAttack;
    private Dictionary<GameObject, SoldierStateController> assignedTargets;
    private Queue<GameObject> soldierPool;
    private GameObject[] stored;
  

    private bool isFirstTime;

    private void Awake()
    {
        base.Awake();
        soldiersWaitForAttack = new Queue<SoldierStateController>(amountOfSoldier);
        assignedTargets = new Dictionary<GameObject, SoldierStateController>();
        soldierPool = new Queue<GameObject>(amountOfSoldier);
        stored = new GameObject[amountOfSoldier];
        isFirstTime = true;
        InitializeSoldier();
    }

    private void InitializeSoldier()
    {  
        for (int i = 0; i < amountOfSoldier; i++)
        {
            GameObject soldier = GetSoldierFromPool();
            soldiersWaitForAttack.Enqueue(soldier.GetComponent<SoldierStateController>());
            stored[i] = soldier;
        }
    }

    private GameObject GetSoldierFromPool()
    {
        GameObject soldier;
        if (soldierPool.Count > 0)
        {
            SoldierStateController soldierComponent;
            soldier = soldierPool.Dequeue();
            soldier.SetActive(true);
            soldierComponent = soldier.GetComponent<SoldierStateController>();
            soldierComponent._origin = origin;
            soldier.transform.position = origin.position;
            soldier.GetComponent<HealthBase>().Reset();
            soldierComponent.Reset();
        }
        else
        {
            soldier = Instantiate(soldierPrefab, origin.position, Quaternion.identity);
            SoldierStateController soldierComponent = soldier.GetComponent<SoldierStateController>();
            soldierComponent.SetData(soldierSAO);
            soldierComponent._origin = origin;
            soldier.GetComponent<SoldierHealth>().OnSoldierDied += RespawnSolider;
            soldierComponent.OnEnemyDied += AddtoEnemiesWaitAttack;
        }
        return soldier;
    }
    void ReturnSoldierToPool(GameObject soldier)
    {
        soldierPool.Enqueue(soldier);
        soldier.SetActive(false);
    }
    private void RespawnSolider(GameObject soldier,GameObject enemy)
    {
        if (enemy != null)
        {
            assignedTargets.Remove(enemy);
        }
        StartCoroutine(ReturnSoldierToPool(soldier, 1));
        StartCoroutine(Respawn());
    }
    IEnumerator ReturnSoldierToPool(GameObject soldier, float time)
    {
        soldierPool.Enqueue(soldier);
        yield return new WaitForSeconds(time);
        soldier.SetActive(false);
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(soldierSAO.Respawn);
        GameObject soldier = GetSoldierFromPool();
        SoldierStateController soldierComponent = soldier.GetComponent<SoldierStateController>();
        soldiersWaitForAttack.Enqueue(soldierComponent);
        soldier.SetActive(true);
    }
    private void AddtoEnemiesWaitAttack(SoldierStateController gameObject,GameObject enemy)
    {
        assignedTargets.Remove(enemy.gameObject);
        soldiersWaitForAttack.Enqueue(gameObject);
    }
    protected override void Attack(Collider[] enemiesPerAttack)
    {
        AssignTargetsToSoldiers(enemiesPerAttack);
    }
    private void AssignTargetsToSoldiers(Collider[] enemiesPerAttack)
    {
        foreach (var enemyCollider in enemiesPerAttack)
        {
            if (enemyCollider == null || soldiersWaitForAttack.Count == 0) continue;

            GameObject enemy = enemyCollider.gameObject;
            EnemyStateController enemyStateController = enemy.GetComponent<EnemyStateController>();
            if (enemy != null && !assignedTargets.ContainsKey(enemy) && !enemyStateController.hasCombating)
            {
                SoldierStateController soldier = soldiersWaitForAttack.Dequeue();
                soldier._enemiesDetected = enemy;
                enemyStateController.soldier = soldier.gameObject;
                enemyStateController.ChangeState(enemyStateController._combatState);

                assignedTargets.Add(enemy, soldier);
            }
        }
    }
    private void OnEnable()
    {
        base.OnEnable();
        if (!isFirstTime)
        {
            InitializeSoldier();
        }
    }
    private void OnDisable()
    {
        base.OnDisable();
        isFirstTime = false;
        foreach (var target in stored)
        {
            if (target == null) return;
            ReturnSoldierToPool(target);
        }
    }
}