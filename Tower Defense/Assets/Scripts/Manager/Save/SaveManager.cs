using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MapData
{
    public string mapName;
    public int starCount;
    public int health;
    public int totalHealth;
    public bool isUnlocked;

    public MapData(string number, int stars, int health,int totalHealth, bool unlocked)
    {
        mapName = number;
        starCount = stars;
        isUnlocked = unlocked;
        this.health = health;
        this.totalHealth = totalHealth;
    }
}
[System.Serializable]
public class MapDataListWrapper
{
    public List<MapData> mapDataList;
}
public class GameData
{
    public Dictionary<string, MapData> maps;
    public GameData()
    {
        maps = new Dictionary<string, MapData>();
    }
}
public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance;
    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(gameObject);
            }
        }
    }
    public static void SaveData(MapData data)
    {
        GameData gameData = LoadData();
        string path = Application.persistentDataPath + "/saveFile.json";
        if (gameData.maps.TryGetValue(data.mapName, out MapData value))
        {
            if (value.starCount < data.starCount || value.health < data.health)
            {
                gameData.maps[data.mapName] = data;
            }
        }
        else
        {
            gameData.maps.Add(data.mapName, data);
        }

        MapDataListWrapper wrapper = new MapDataListWrapper
        {
            mapDataList = new List<MapData>(gameData.maps.Values)
        };

        string json = JsonUtility.ToJson(wrapper, true);

        FileInfo file = new FileInfo(path);
        file.Directory.Create();

        File.WriteAllText(path, json);
    }
    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MapDataListWrapper wrapper = JsonUtility.FromJson<MapDataListWrapper>(json);
            GameData gameData = new GameData();
            foreach (var mapData in wrapper.mapDataList)
            {
                gameData.maps.Add(mapData.mapName, mapData);
            }
            return gameData;
        }
        else
        {
            return new GameData();
        }
    }
}