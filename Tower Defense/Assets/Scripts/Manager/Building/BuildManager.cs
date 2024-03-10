using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public enum BuildType
    {
        Canon,
        Archer,
        Mage,
        Barrack
    }
    [SerializeField] private float timeBuilding;
    [SerializeField] private GameObject[] canonPrefabs;
    [SerializeField] private GameObject[] archerPrefabs;
    [SerializeField] private GameObject[] magePrefabs;
    [SerializeField] private GameObject[] barrackPrefabs;
    [SerializeField] private BuildingPoint[] buildingPoints;

    private Dictionary<BuildType, GameObject[]> prefabLookup;
    private Dictionary<BuildType, Queue<GameObject>[]> objectPools;
    private Dictionary<BuildType, Dictionary<int, GameObject[]>> currentBuildings; 
    //gameObject[] even index are tower , odd index are contruction tower

    private void Awake()
    {
        InitializePrefabLookup();
        InitializePools();
        InitializeBuildingPoints();
    }

    private void InitializePrefabLookup()
    {
        prefabLookup = new Dictionary<BuildType, GameObject[]>
        {
            { BuildType.Canon, canonPrefabs },
            { BuildType.Archer, archerPrefabs },
            { BuildType.Mage, magePrefabs },
            { BuildType.Barrack, barrackPrefabs }
        };
    }

    private void InitializePools()
    {
        objectPools = new Dictionary<BuildType, Queue<GameObject>[]>();
        currentBuildings = new Dictionary<BuildType, Dictionary<int, GameObject[]>>();

        foreach (BuildType type in Enum.GetValues(typeof(BuildType)))
        {
            objectPools[type] = new Queue<GameObject>[buildingPoints.Length];
            currentBuildings[type] = new Dictionary<int, GameObject[]>();

            for (int i = 0; i < buildingPoints.Length; i++)
            {
                objectPools[type][i] = new Queue<GameObject>();
                currentBuildings[type][i] = new GameObject[prefabLookup[type].Length];
            }
        }
    }

    private void InitializeBuildingPoints()
    {
        for (int i = 0; i < buildingPoints.Length; i++)
        {
            buildingPoints[i].SetId(i);
            buildingPoints[i].SetTimeBuilding(timeBuilding);
        }
    }

    public void Build(BuildType buildType, int level, int pointId)
    {
        var prefab = prefabLookup[buildType][level];
        var buildingPoint = buildingPoints[pointId].transform;

        var buildGO = GetPooledObject(buildType, level) ?? Instantiate(prefab, buildingPoint.position, Quaternion.identity);

        currentBuildings[buildType][pointId][level] = buildGO;
        buildGO.transform.SetParent(buildingPoint);
        buildGO.transform.position = buildingPoint.position;
        buildGO.transform.rotation = Quaternion.identity;
        buildGO.SetActive(true);
        DeableContructionTower(buildType, level, pointId);
    }

    private void DeableContructionTower(BuildType buildType, int level, int pointId)
    {
        if (level % 2 != 0)
        {
            AddTowerToPool(buildType, level - 1, pointId);
        }
    }

    private GameObject GetPooledObject(BuildType buildType, int level)
    {
        var pool = objectPools[buildType][level];
        while (pool.Count > 0)
        {
            var pooledObject = pool.Dequeue();
            if (!pooledObject.activeInHierarchy)
            {
                return pooledObject;
            }
        }

        return null;
    }

    public void AddTowerToPool(BuildType buildType, int level, int pointId)
    {
        var tower = currentBuildings[buildType][pointId][level];
        tower.SetActive(false);
        objectPools[buildType][level].Enqueue(tower);
    }

    private void OnEnable()
    {
        BuildSelectController.OnBuiltTower += Build;
        BuildSelectController.OnTowerSold += AddTowerToPool;
    }

    private void OnDisable()
    {
        BuildSelectController.OnBuiltTower -= Build;
        BuildSelectController.OnTowerSold -= AddTowerToPool;
    }
}