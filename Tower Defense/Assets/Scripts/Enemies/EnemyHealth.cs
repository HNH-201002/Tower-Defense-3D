using UnityEngine;
using static EnemySAO;

public class EnemyHealth : HealthBase
{
    [HideInInspector] public EnemyWaveSpawnManager EnemyWaveSpawnManager;
    private EnemyStateController enemyStateController;
    private TypeEnemy _typeEnemy;
    private const string SFX_DEAD = "Enemy_Dead";
    private void Awake()
    { 
        enemyStateController = GetComponent<EnemyStateController>();
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
    private void Start()
    {
        base.Start();
        _uiHealth.gameObject.SetActive(false);
        _typeEnemy = enemyStateController.GetTypeEnemy();
    }
    protected override float GetHealthData()
    {
        return enemyStateController.Health;
    }
    protected override float GetArmorData()
    {
        return enemyStateController.Armor;
    }
    protected override string GetSfxDeadName()
    {
        return SFX_DEAD;
    }
    protected override void AddPool(GameObject gameObject)
    {
        EnemyWaveSpawnManager.DeactivateAndAddToPool(_typeEnemy, this.gameObject);
    }

    public override void Death()
    {
        if (enemyStateController.hasDied) return;
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        GoldManager.Instance.AddGold(enemyStateController.Bounty);
        enemyStateController.hasDied = true;
        EnemyWaveSpawnManager.Instance.OnEnemyDeath();
    }
    private void OnEnable()
    {
        base.OnEnable();
        _uiHealth.gameObject.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

}
