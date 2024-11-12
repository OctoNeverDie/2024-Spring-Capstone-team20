using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataController : MonoBehaviour
{
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }

    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    private Dictionary<string, PlayData> playDataCache = new Dictionary<string, PlayData>();

    public PlayData GetPlayDataById(string playDataId)
    {
        if (!playDataCache.TryGetValue(playDataId, out PlayData playData))
        {
            playData = LoadPlayData(playDataId);
            playDataCache[playDataId] = playData;
        }
        return playData;
    }

    public void CreatePlayData(string playDataId)
    {
        if (!playDataCache.ContainsKey(playDataId))
        {
            var newPlayData = new PlayData();
            playDataCache[playDataId] = newPlayData;
            SavePlayData(playDataId, newPlayData);
            UpdateGameDataWithPlayDataId(playDataId);
        }
    }

    public void DeletePlayData(string playDataId)
    {
        string filePath = GetPlayDataFilePath(playDataId);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            playDataCache.Remove(playDataId);
            RemovePlayDataIdFromGameData(playDataId);
        }
    }

    private PlayData LoadPlayData(string playDataId)
    {
        string filePath = GetPlayDataFilePath(playDataId);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayData>(json);
        }
        else
        {
            Debug.LogWarning($"PlayData file '{filePath}' not found, creating new data.");
            return new PlayData();
        }
    }

    private void SavePlayData(string playDataId, PlayData playData)
    {
        string filePath = GetPlayDataFilePath(playDataId);
        File.WriteAllText(filePath, JsonUtility.ToJson(playData, true));
    }

    private string GetPlayDataFilePath(string playDataId)
    {
        return Application.persistentDataPath + $"/{playDataId}.json";
    }

    private void UpdateGameDataWithPlayDataId(string playDataId)
    {
        if (!gameData.save_files_IDs.Contains(playDataId))
        {
            gameData.save_files_IDs.Add(playDataId);
            SaveGameData();
        }
    }

    private void RemovePlayDataIdFromGameData(string playDataId)
    {
        if (gameData.save_files_IDs.Contains(playDataId))
        {
            gameData.save_files_IDs.Remove(playDataId);
            SaveGameData();
        }
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/GameData.json";
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(fromJsonData);
        }
        else
        {
            _gameData = new GameData();  // 기본 GameData 객체 생성
            SaveGameData();
        }
    }

    public void SaveGameData()
    {
        string filePath = Application.persistentDataPath + "/GameData.json";
        File.WriteAllText(filePath, JsonUtility.ToJson(gameData, true));
    }

    void OnApplicationQuit()
    {
        SaveGameData();
        foreach (var kvp in playDataCache)
        {
            SavePlayData(kvp.Key, kvp.Value);
        }
    }
}
