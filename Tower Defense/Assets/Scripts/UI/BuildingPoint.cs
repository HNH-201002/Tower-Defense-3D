using System.Collections;
using UnityEngine;

public class BuildingPoint : MonoBehaviour
{
    [SerializeField] private GameObject buildSelectPrefab;
    [SerializeField] private Transform buildSelectContain; 
    [SerializeField] private BuildSelectUIManager buildSelectManager;
    [SerializeField] private int heightOffSetBuildSelectPanel = 10;
    private int _id;
    bool hasAlreadyChosen;

    private static Camera mainCamera; 

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void Interact()
    {
        if (hasAlreadyChosen)
        {
            buildSelectManager.HandleOldAndNewPanel(_id);
        }
        else
        {
            GameObject buildSelectGO = Instantiate(buildSelectPrefab);
            buildSelectGO.transform.SetParent(buildSelectContain.transform, false);
            buildSelectGO.transform.position = transform.position + new Vector3(0,heightOffSetBuildSelectPanel, 0);
            buildSelectGO.transform.forward = Camera.main.transform.forward;
            buildSelectManager.Add(_id,buildSelectGO);
            buildSelectGO.GetComponent<BuildSelectController>().SetId(_id);
            hasAlreadyChosen = true;
        }
    }
    public void SetId(int id)
    {
        _id = id;
    }
}
