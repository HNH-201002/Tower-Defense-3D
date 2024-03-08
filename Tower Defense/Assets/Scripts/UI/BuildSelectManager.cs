using System.Collections.Generic;
using UnityEngine;

public class BuildSelectManager : MonoBehaviour
{
    private Dictionary<int,GameObject> buildSelectGOs = new Dictionary<int, GameObject>();
    private int _currentBuildSelectId = 0;
    private int countId = 0;

    public int GetNewId() => countId - 1;
    public void Add(GameObject buildSelectGO)
    {
        buildSelectGOs.Add(countId, buildSelectGO);
        HandleOldAndNewPanel(countId);
        countId++;
    }
    public void HandleOldAndNewPanel(int newId)
    {

        if (_currentBuildSelectId != -1 && _currentBuildSelectId != newId)
        {
            if (buildSelectGOs.TryGetValue(_currentBuildSelectId, out var oldGO))
            {
                oldGO.SetActive(false);
            }
        }
        if (buildSelectGOs.TryGetValue(newId, out var newGO))
        {
            newGO.SetActive(true);
            _currentBuildSelectId = newId;
        }
    }
    public void TurnOffCurrentBuildSelect()
    {
        if (_currentBuildSelectId != -1 && buildSelectGOs.TryGetValue(_currentBuildSelectId, out var currentGO))
        {
            currentGO.SetActive(false);
        }
    }
    private void OnEnable()
    {
        RayCastFromMouse.PlayerClickedOut += TurnOffCurrentBuildSelect;
    }

    private void OnDisable()
    {
        RayCastFromMouse.PlayerClickedOut -= TurnOffCurrentBuildSelect;
    }
}
