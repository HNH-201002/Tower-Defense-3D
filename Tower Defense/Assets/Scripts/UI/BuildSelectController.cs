using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSelectController : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private GameObject fullUpgradePanel;
    private void Start()
    {
        mainCamera = Camera.main;
        upgradePanel.SetActive(false);
    }
    void Update()
    {
        transform.forward = mainCamera.transform.forward;
    }
    public void DefenseTowerSellButton()
    {
        Upgrade();
    }
    public void MageTowerSellButton()
    {
        Upgrade();
    }
    public void ArcherTowerSellButton()
    {
        Upgrade();
    }
    public void CanonTowerSellButton()
    {
        Upgrade();
    }
    public void SellTower()
    {
        fullUpgradePanel.SetActive(false);
        upgradePanel.SetActive(false);
        buildPanel.SetActive(true);
    }
    public void FullUpgradePanel()
    {
        fullUpgradePanel.SetActive(true);
        upgradePanel.SetActive(false);
    }
    private void Upgrade()
    {
        upgradePanel.SetActive(true);
        buildPanel.SetActive(false);
    }
}
