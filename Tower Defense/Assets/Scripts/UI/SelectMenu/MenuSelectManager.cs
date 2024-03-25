using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuSelectManager : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _selectMapPrefab;
    [SerializeField] private List<string> sceneNames;
    private bool isPreviousMapUnclock = false;
    void Start()
    {
        GameData gameData = SaveManager.LoadData();

        for (int i = 0; i < sceneNames.Count; i++)
        {
            string sceneName = sceneNames[i];
            GameObject mapSelectGO = Instantiate(_selectMapPrefab);
            mapSelectGO.transform.SetParent(_content, false);
            MenuMapElement menuMapElement = mapSelectGO.GetComponent<MenuMapElement>();
            menuMapElement.nameIndexMapText.text = (i + 1).ToString();

            if (gameData.maps.TryGetValue(sceneName, out MapData data))
            {
                menuMapElement.SetData(data.starCount, data.health, data.totalHealth, i, data.isUnlocked);
                isPreviousMapUnclock = data.isUnlocked; 
            }
            else
            {
                bool unlockStatus = (i == 0 || isPreviousMapUnclock);
                menuMapElement.SetData(0, 0, 20, i, unlockStatus);
                if (!unlockStatus)
                {
                    menuMapElement.SetLockPanel();
                }
            }

            menuMapElement.SetScene(sceneName);
        }
    }

}
