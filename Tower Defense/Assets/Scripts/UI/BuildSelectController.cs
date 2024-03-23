using System;
using System.Collections;
using TMPro;
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

    [Tooltip("Using for turn off all of UI")]
    public GameObject parentUI; 
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private GameObject fullUpgradePanel;
    [SerializeField] private Slider timeToBuildSilder;
    [SerializeField] private TMP_Text initialMagicBuildingTextPrice;
    [SerializeField] private TMP_Text initialCannonBuildingTextPrice;
    [SerializeField] private TMP_Text initialBarrackBuildingTextPrice;
    [SerializeField] private TMP_Text initialArcherBuildingTextPrice;
    [SerializeField] private TMP_Text upgradePriceText;
    [SerializeField] private TMP_Text sellTextInUpgragePanel;
    [SerializeField] private TMP_Text sellText;


    private GameObject buildPoint;


    private float _timeBuilding;
    public void SetTimeBuilding(float time) => _timeBuilding = time;
    public void SetBuildPoint(GameObject buildPoint) => this.buildPoint = buildPoint; 

    public static event Action<BuildType, int, int> OnBuiltTower;
    public static event Action<BuildType, int, int> OnTowerSold;

    public void SetId(int id) => _id = id;

    private bool _isProcessing;


    private const string SFX_BUILD_TOWER = "TowerBuild";

    private const string SFX_SELL_TOWER = "TowerSell";
    private void Start()
    {
        mainCamera = Camera.main;
        TogglePanels(false, true, false);
        _currentBuildLevel = 0;
        timeToBuildSilder.gameObject.SetActive(false);
        lastCameraRotation = mainCamera.transform.rotation;
        initialMagicBuildingTextPrice.text = GoldManager.Instance.GetTowerPriceInfo(BuildType.Mage,0).ToString();
        initialCannonBuildingTextPrice.text = GoldManager.Instance.GetTowerPriceInfo(BuildType.Cannon, 0).ToString();
        initialArcherBuildingTextPrice.text = GoldManager.Instance.GetTowerPriceInfo(BuildType.Archer, 0).ToString();
        initialBarrackBuildingTextPrice.text = GoldManager.Instance.GetTowerPriceInfo(BuildType.Barrack, 0).ToString();
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
        if (GoldManager.Instance.BuyTown(BuildType.Barrack, 0))
        {
            ToggleBuildPoint(false);
            StartCoroutine(BuildTimer());
        }
    }
    public void BuyMageTower()
    {
        _buildType = BuildType.Mage;
        if(GoldManager.Instance.BuyTown(BuildType.Mage, 0))
        {
            ToggleBuildPoint(false);
            StartCoroutine(BuildTimer());
        }
    }

    public void BuyArcherTower()
    {
        _buildType = BuildType.Archer;
        if(GoldManager.Instance.BuyTown(BuildType.Archer, 0))
        {
            ToggleBuildPoint(false);
            StartCoroutine(BuildTimer());
        }
    }

    public void BuyCanonTower()
    {
        _buildType = BuildType.Cannon;
        if(GoldManager.Instance.BuyTown(BuildType.Cannon, 0))
        {
            ToggleBuildPoint(false);
            StartCoroutine(BuildTimer());
        }
    }
    private void BuyTower(BuildType buildType)
    {
        OnBuiltTower?.Invoke(buildType, _currentBuildLevel, _id);
        if (_currentBuildLevel % 2 == 0)
        {
            TogglePanels(false, false, false);
        }
        else
        {
            TogglePanels(true, false, false);
        }
        _currentBuildLevel++;
        if (_currentBuildLevel > 9)
        {
            ShowFullUpgradePanel();
        }
    }
    private IEnumerator BuildTimer()
    {
        BuyTower(_buildType);
        SoundManager.Instance.PlaySound(SFX_BUILD_TOWER);
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
        sellTextInUpgragePanel.text = GoldManager.Instance.TotalPrice(_buildType,_currentBuildLevel).ToString();
        upgradePriceText.text = GoldManager.Instance.GetTowerPriceInfo(_buildType, _currentBuildLevel).ToString();
        _isProcessing = false;
    }


    public void SellTower()
    {
        SoundManager.Instance.PlaySound(SFX_SELL_TOWER);
        OnTowerSold?.Invoke(_buildType, _currentBuildLevel - 1, _id); //store in Pooling
        GoldManager.Instance.SellTower(_buildType , _currentBuildLevel);
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
        if (GoldManager.Instance.BuyTown(_buildType , _currentBuildLevel))
        {
            StartCoroutine(BuildTimer());
        }
    }
    public void ShowFullUpgradePanel()
    {
        sellText.text = GoldManager.Instance.TotalPrice(_buildType, _currentBuildLevel).ToString();
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