using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildManager;

public class BuildSelectController : MonoBehaviour
{
    private Camera mainCamera;
    private int _id;
    public void SetId(int id) => _id = id;

    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private GameObject fullUpgradePanel;
    public static event Action<BuildType, int, int> BuiltTower;
    public static event Action<BuildType, int, int> TowerSold;

    private int _currentBuildLevel = 0;
    private BuildType _buildType;
    private void Start()
    {
        mainCamera = Camera.main;
        upgradePanel.SetActive(false);
    }
    void Update()
    {
        transform.forward = mainCamera.transform.forward;
    }
    public void DefenseTowerBuyButton()
    {
        BuiltTower?.Invoke(BuildType.Barrack,_currentBuildLevel,_id);
        _buildType = BuildType.Barrack;
        Buy();
    }
    public void MageTowerBuyButton()
    {
        BuiltTower?.Invoke(BuildType.Mage, _currentBuildLevel, _id);
        _buildType = BuildType.Mage;
        Buy();
    }
    public void ArcherTowerBuyButton()
    {
        BuiltTower?.Invoke(BuildType.Archer, _currentBuildLevel, _id);
        _buildType = BuildType.Archer;
        Buy();
    }
    public void CanonTowerBuyButton()
    {
        BuiltTower?.Invoke(BuildType.Canon, _currentBuildLevel, _id);
        _buildType = BuildType.Canon;
        Buy();
    }
    public void SellTower()
    {
        TowerSold?.Invoke(_buildType, _currentBuildLevel - 1,_id);
        fullUpgradePanel.SetActive(false);
        upgradePanel.SetActive(false);
        buildPanel.SetActive(true);
        _currentBuildLevel = 0;
    }
    public void FullUpgradePanel()
    {
        fullUpgradePanel.SetActive(true);
        upgradePanel.SetActive(false);
    }
    private void Buy()
    {
        upgradePanel.SetActive(true);
        buildPanel.SetActive(false);
        _currentBuildLevel++;
    }
}
