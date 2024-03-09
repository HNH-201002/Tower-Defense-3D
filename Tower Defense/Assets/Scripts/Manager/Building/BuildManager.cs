using System;
using System.Collections.Generic;
using UnityEngine;

struct BuildPooling
{
    public Queue<GameObject> builds;
}
struct BuildInfo
{
    public GameObject build;
}
struct BuildCurrentStored
{
    public Dictionary<int, BuildInfo> builds;
}
public class BuildManager : MonoBehaviour
{
    public enum BuildType
    {
        Canon,
        Archer,
        Mage,
        Barrack
    }
    [SerializeField] private GameObject[] canonPrefab;
    [SerializeField] private GameObject[] archerPrefab;
    [SerializeField] private GameObject[] magePrefab;
    [SerializeField] private GameObject[] barrackPrefab;
    [SerializeField] private BuildingPoint[] buildPoint;

    Dictionary<BuildType, BuildPooling[]>  objectPoolingBuild = new Dictionary<BuildType, BuildPooling[]>();
    Dictionary<BuildType, BuildCurrentStored[]> currentBuildings = new Dictionary<BuildType, BuildCurrentStored[]>();
    GameObject buildPrefab;
    private void Awake()
    {
        foreach (BuildType type in Enum.GetValues(typeof(BuildType)))
        {
            currentBuildings[type] = new BuildCurrentStored[buildPoint.Length];
            objectPoolingBuild[type] = new BuildPooling[buildPoint.Length];

            for (int i = 0; i < buildPoint.Length; i++)
            {

                objectPoolingBuild[type][i].builds = new Queue<GameObject>();
                currentBuildings[type][i].builds = new Dictionary<int, BuildInfo>();
            }
        }

        for (int i = 0; i < buildPoint.Length; i++)
        {
            buildPoint[i].SetId(i);
        }
    }
    public void Build(BuildType buildType,int level,int idGOParent)
    {
        switch (buildType) 
        {
            case BuildType.Canon:
                buildPrefab = canonPrefab[level];
                break;
            case BuildType.Archer:
                buildPrefab = archerPrefab[level];
                break;
            case BuildType.Mage:
                buildPrefab = magePrefab[level];
                break;
            case BuildType.Barrack:
                buildPrefab = barrackPrefab[level];
                break;
        }
        Spawn(buildPrefab, buildPoint[idGOParent].gameObject, buildType,level,idGOParent);
    }
    private void Spawn(GameObject gameobjectPrefab, GameObject parentGameObject, BuildType buildType, int level,int id)
    {
        if (gameobjectPrefab == null)
        {
            Debug.LogError("gameobjectPrefab is null");
            return;
        }

        if (parentGameObject == null)
        {
            Debug.LogError("parentGameObject is null");
            return;
        }

        GameObject buildGO = GetPooledEnemy(buildType, level);
        if (buildGO == null)
        {
            buildGO = Instantiate(gameobjectPrefab, parentGameObject.transform.position, Quaternion.identity);
            if (buildGO == null)
            {
                buildGO = Instantiate(gameobjectPrefab, parentGameObject.transform.position, Quaternion.identity);
                BuildInfo buildInfo = new BuildInfo() { build = buildGO};
                currentBuildings[buildType][level].builds.Add(id, buildInfo);
            }
            else
            {
                BuildInfo build = new BuildInfo() { build = buildGO };
                currentBuildings[buildType][level].builds[id] = build;
            }
        }
        else
        {
            buildGO.SetActive(true);
            buildGO.transform.position = parentGameObject.transform.position;
            buildGO.transform.rotation = Quaternion.identity;
            BuildInfo build = new BuildInfo() { build = buildGO };
            if (currentBuildings[buildType][level].builds.ContainsKey(id))
            {
                currentBuildings[buildType][level].builds[id] = build;
            }
            else
            {
                currentBuildings[buildType][level].builds.Add(id,build);
            }
        }
        buildGO.transform.SetParent(parentGameObject.transform);
    }
    private GameObject GetPooledEnemy(BuildType buildType, int level)
    {
        if (level < 0 || level >= objectPoolingBuild[buildType].Length || objectPoolingBuild[buildType][level].builds == null)
        {
            Debug.LogError($"No pool available for '{buildType}' at level '{level}'.");
            return null;
        }

        var buildPool = objectPoolingBuild[buildType][level];

        if (buildPool.builds.Count > 0)
        {
            var pooledEnemy = buildPool.builds.Peek();
            if (!pooledEnemy.activeInHierarchy)
            {
                return buildPool.builds.Dequeue();
            }
        }

        return null;
    }
    public void AddTowerToStored(BuildType buildType, int level, int id)
    {
        objectPoolingBuild[buildType][level].builds.Enqueue(currentBuildings[buildType][level].builds[id].build);
        currentBuildings[buildType][level].builds[id].build.SetActive(false);
    }
    private void OnEnable()
    {
        BuildSelectController.BuiltTower += Build;
        BuildSelectController.TowerSold += AddTowerToStored;
    }
    private void OnDisable()
    {
        BuildSelectController.BuiltTower -= Build;
        BuildSelectController.TowerSold -= AddTowerToStored;
    }
}
