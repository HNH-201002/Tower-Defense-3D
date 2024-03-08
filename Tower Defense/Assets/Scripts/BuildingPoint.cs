using System.Collections;
using UnityEngine;

public class BuildingPoint : MonoBehaviour
{
    [SerializeField] private GameObject buildSelectPrefab;
    [SerializeField] private Transform buildSelectContain; 
    [SerializeField] private BuildSelectManager buildSelectManager;
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
            buildSelectGO.transform.position = transform.position + new Vector3(0,10, 0);
            buildSelectGO.transform.forward = Camera.main.transform.forward;
            buildSelectManager.Add(buildSelectGO);
            _id = buildSelectManager.GetNewId();
            hasAlreadyChosen = true;
        }
    }
}
