using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static BuildManager;

public class BuildSelectController : MonoBehaviour
{
    private Camera mainCamera;
    private int _id;
    private int _currentBuildLevel;
    private BuildType _buildType;
    private Quaternion lastCameraRotation;

    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private GameObject fullUpgradePanel;
    [SerializeField] private Slider timeToBuildSilder;

    [SerializeField] private GameObject buildPoint;


    private float _timeBuilding;
    public void SetTimeBuilding(float time) => _timeBuilding = time;
    public void SetBuildPoint(GameObject buildPoint) => this.buildPoint = buildPoint; 

    public static event Action<BuildType, int, int> OnBuiltTower;
    public static event Action<BuildType, int, int> OnTowerSold;

    public void SetId(int id) => _id = id;

    private bool _isProcessing;
    private void Start()
    {
        mainCamera = Camera.main;
        TogglePanels(false, true, false);
        _currentBuildLevel = 0;
        timeToBuildSilder.gameObject.SetActive(false);
        lastCameraRotation = mainCamera.transform.rotation;
    }
    private void Update()
    {
        if (mainCamera.transform.rotation != lastCameraRotation)
        {
            AlignWithCamera();
            lastCameraRotation = mainCamera.transform.rotation;
        }
    }
    // Button event
    public void BuyDefenseTower() 
    {
        _buildType = BuildType.Barrack;
        ToggleBuildPoint(false);
        StartCoroutine(BuildTimer());
    }
    public void BuyMageTower()
    {
        _buildType = BuildType.Mage;
        ToggleBuildPoint(false);
        StartCoroutine(BuildTimer());
    }

    public void BuyArcherTower()
    {
        _buildType = BuildType.Archer;
        ToggleBuildPoint(false);
        StartCoroutine(BuildTimer());
    }

    public void BuyCanonTower()
    {
        _buildType = BuildType.Canon;
        ToggleBuildPoint(false);
        StartCoroutine(BuildTimer());
    }
    private void BuyTower(BuildType buildType)
    {
        OnBuiltTower?.Invoke(buildType, _currentBuildLevel, _id);
        if (_currentBuildLevel >= 9)
        {
            ShowFullUpgradePanel();
            return;
        }
        if (_currentBuildLevel % 2 == 0)
        {
            TogglePanels(false, false, false);
        }
        else
        {
            TogglePanels(true, false, false);
        }
        _currentBuildLevel++;
    }
    private IEnumerator BuildTimer()
    {
        BuyTower(_buildType);
        timeToBuildSilder.gameObject.SetActive(true);
        float elapsedTime = 0f;
        _isProcessing = true;
        while (elapsedTime < _timeBuilding)
        {
            timeToBuildSilder.value = elapsedTime / _timeBuilding;
            elapsedTime += Time.deltaTime;
            yield return null;// Wait for the next frame before continuing the loop
        }
        timeToBuildSilder.value = 0;
        timeToBuildSilder.gameObject.SetActive(false);
        BuyTower(_buildType);
        _isProcessing = false;
    }
    public void SellTower()
    {
        OnTowerSold?.Invoke(_buildType, _currentBuildLevel - 1, _id); //store in Pooling
        ResetTower();
    }
    private void ResetTower()
    {
        TogglePanels(false, true, false);
        _currentBuildLevel = 0;
        ToggleBuildPoint(true);
    }
    public void Upgrade()
    {
        StartCoroutine(BuildTimer());
    }
    public void ShowFullUpgradePanel()
    {
        TogglePanels(false, false, true);
    }

    private void TogglePanels(bool upgrade, bool build, bool fullUpgrade)
    {
        upgradePanel.SetActive(upgrade);
        fullUpgradePanel.SetActive(fullUpgrade);
        buildPanel.SetActive(build);
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
    public bool IsProcessing()
    {
        return _isProcessing;
    }
}