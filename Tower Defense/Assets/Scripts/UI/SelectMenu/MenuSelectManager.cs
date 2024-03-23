using UnityEditor;
using UnityEngine;

public class MenuSelectManager : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _selectMapPrefab;
    void Start()
    {
        GameData gameData = SaveManager.LoadData();
        int count = 0;
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
            if (scene.enabled && scene.path.Contains("/M_"))
            {
                GameObject mapSelectGO = Instantiate(_selectMapPrefab);
                mapSelectGO.transform.SetParent(_content, false);
                MenuMapElement menuMapElement = mapSelectGO.GetComponent<MenuMapElement>();
                if (count == 0)
                {
                    menuMapElement.SetLockPanel();
                }
                if (gameData.maps.TryGetValue(i,out MapData data))
                {
                    if (data.isUnlocked)
                    {
                        menuMapElement.SetData(data.starCount,data.health,data.totalHealth,i);
                    }
                }
                menuMapElement.SetScene(i);
                count++;
            }
        }
    }

}
