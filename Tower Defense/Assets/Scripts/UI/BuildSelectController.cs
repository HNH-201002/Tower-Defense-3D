using System;
using System.Collections;
using UnityEngine;
using static BuildManager;

public class BuildSelectController : MonoBehaviour
{
    private Camera mainCamera;
    private int _id;
    private int _currentBuildLevel = 0;
    private BuildType _buildType;

    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private GameObject fullUpgradePanel;
    [SerializeField] private GameObject buildPoint;

    private float _timeBuilding;
    public void SetTimeBuilding(float time) => _timeBuilding = time;
    public void SetBuildPoint(GameObject buildPoint) => this.buildPoint = buildPoint; 

    public static event Action<BuildType, int, int> OnBuiltTower;
    public static event Action<BuildType, int, int> OnTowerSold;

    public void SetId(int id) => _id = id;
    private void Start()
    {
        mainCamera = Camera.main;
        TogglePanels(false, true, false);
    }

    private void Update()
    {
        AlignWithCamera();
    }
    // Button event
    public void BuyDefenseTower() 
    {
        BuyTower(BuildType.Barrack);
        StartCoroutine(DelayBuildForContruction(BuildType.Barrack));
    }
    public void BuyMageTower()
    {
        BuyTower(BuildType.Mage);
        StartCoroutine(DelayBuildForContruction(BuildType.Mage));
    }

    public void BuyArcherTower()
    {
        BuyTower(BuildType.Archer);
        StartCoroutine(DelayBuildForContruction(BuildType.Archer));
    }

    public void BuyCanonTower()
    {
        BuyTower(BuildType.Canon);
        StartCoroutine(DelayBuildForContruction(BuildType.Canon));
    }
    private void BuyTower(BuildType buildType)
    {
        OnBuiltTower?.Invoke(buildType, _currentBuildLevel, _id);
        _buildType = buildType;
        _currentBuildLevel++;
        TogglePanels(true, false, false);
        ToggleBuildPoint(false);
    }
    IEnumerator DelayBuildForContruction(BuildType buildType)
    {
        yield return new WaitForSeconds(_timeBuilding);
        BuyTower(buildType);
    }
    public void SellTower()
    {
        OnTowerSold?.Invoke(_buildType, _currentBuildLevel - 1, _id);
        ResetTower();
    }
    public void Upgrade()
    {
        if (_currentBuildLevel >= 10) 
        {
            ShowFullUpgradePanel();
            return;
        }
        BuyTower(_buildType);
        StartCoroutine(DelayBuildForContruction(_buildType));
    }
    public void ShowFullUpgradePanel()
    {
        TogglePanels(false, false, true);
    }

    private void TogglePanels(bool upgrade, bool build, bool fullUpgrade)
    {
        upgradePanel.SetActive(upgrade);
        buildPanel.SetActive(build);
        fullUpgradePanel.SetActive(fullUpgrade);
    }

    private void ResetTower()
    {
        TogglePanels(false, true, false);
        _currentBuildLevel = 0;
        ToggleBuildPoint(true);
    }

    private void ToggleBuildPoint(bool state)
    {
        buildPoint.SetActive(state);
    }

    private void AlignWithCamera()
    {
        if (mainCamera != null)
        {
            transform.forward = mainCamera.transform.forward;
        }
    }
}