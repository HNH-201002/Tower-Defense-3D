using UnityEngine;

public class BuildingPoint : MonoBehaviour
{
    [SerializeField] private GameObject buildSelectPrefab;
    [SerializeField] private Transform buildSelectContainer;
    [SerializeField] private BuildSelectUIManager buildSelectManager;
    [SerializeField] private int heightOffsetBuildSelectPanel = 10;
    [SerializeField] private GameObject buildingPoint;

    private int _id;
    private bool hasAlreadyChosen;
    private static Camera mainCamera;
    private float _timeBuilding;

    private void Awake()
    {
        mainCamera = Camera.main ?? mainCamera; // null-coalescing 
    }

    public void Interact()
    {
        if (hasAlreadyChosen)
        {
            buildSelectManager.HandleOldAndNewPanel(_id);
        }
        else
        {
            SpawnBuildSelect();
            hasAlreadyChosen = true;
        }
    }
    private void SpawnBuildSelect()
    {
        GameObject buildSelectGO = Instantiate(buildSelectPrefab, buildSelectContainer, false);
        Vector3 positionOffset = new Vector3(0, heightOffsetBuildSelectPanel, 0);
        buildSelectGO.transform.position = transform.position + positionOffset;
        buildSelectGO.transform.forward = mainCamera.transform.forward;

        buildSelectManager.Add(_id, buildSelectGO);
        BuildSelectController buildSelectComponent = buildSelectGO.GetComponent<BuildSelectController>();
        buildSelectComponent.SetId(_id);
        buildSelectComponent.SetBuildPoint(buildingPoint);
        buildSelectComponent.SetTimeBuilding(_timeBuilding);
    }
    public void SetId(int id)
    {
        _id = id;
    }
    public void SetTimeBuilding(float time)
    {
        _timeBuilding = time;
    }
}