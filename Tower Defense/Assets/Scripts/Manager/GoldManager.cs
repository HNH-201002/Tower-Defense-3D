using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BuildManager;

public class GoldManager : MonoBehaviour
{
    [System.Serializable]
    public class TowerGoldData
    {
        public BuildType buildType;
        public TowerSAO[] data;
    }

    [SerializeField]
    private List<TowerGoldData> towerGoldDataList = new List<TowerGoldData>();
    private Dictionary<BuildType, TowerSAO[]> towerGolds;
    [SerializeField] private int gold;
    [SerializeField] private TMP_Text goldText;
    private static GoldManager _instance;
    public static GoldManager Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            InitializeTowerGoldsDictionary();
        }
        goldText.text = gold.ToString();
    }
    private void InitializeTowerGoldsDictionary()
    {
        towerGolds = new Dictionary<BuildType, TowerSAO[]>();
        foreach (TowerGoldData data in towerGoldDataList)
        {
            if (!towerGolds.ContainsKey(data.buildType))
            {
                towerGolds.Add(data.buildType, data.data);
            }
        }
    }
    public bool BuyTown(BuildType buildType, int level)
    {
        int _realLevel = GetRealLevel(level);
        if (towerGolds.TryGetValue(buildType, out TowerSAO[] data) && gold >= data[_realLevel].Price)
        {
            gold -= data[_realLevel].Price;
            UpdateGoldText();
            return true;
        }
        return false;
    }

    public void SellTower(BuildType buildType, int level)
    {
        gold += TotalPrice(buildType, level);
        UpdateGoldText();
    }

    public int TotalPrice(BuildType buildType, int level)
    {
        int _realLevel;
        _realLevel = level / 2;
        if (towerGolds.TryGetValue(buildType, out TowerSAO[] data))
        {
            int total = 0;
            for (int i = 0; i < _realLevel; i++)
            {
                total += data[i].Price;
            }
            return total;
        }
        return 0;
    }

    public int GetTowerPriceInfo(BuildType buildType, int level)
    {
        int _realLevel = GetRealLevel(level);
        if (towerGolds.TryGetValue(buildType, out TowerSAO[] data) && _realLevel < data.Length)
        {
            return data[_realLevel].Price;
        }
        return 0;
    }
    private void UpdateGoldText()
    {
        goldText.text = gold.ToString();
    }
    private int GetRealLevel(int level)
    {
        return (level != 0) ? level / 2 : level;
    }
}
