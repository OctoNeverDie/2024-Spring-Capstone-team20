using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DataController : MonoBehaviour
{
    #region SingleTon
    private static DataController instance;
    public static DataController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataController>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<DataController>();
                    singleton.name = typeof(DataController).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }
    #endregion

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
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
                ToGameJson();
            }
            return _gameData;
        }
    }

    public PlayData _playData;

    public PlayData playData
    {
        get
        {
            if (_playData == null)
            {
                LoadPlayData(gameData.cur_save_file_ID);
                ToPlayJson(gameData.cur_save_file_ID);
            }
            return _playData;
        }
    }

    System.IO.StreamWriter SW = null;

    private string GetGameDataFilePath()
    {
        //return Application.persistentDataPath + $"/GameData.json";
        return Application.dataPath + "/Scripts/JSON/GameData.json";
    }
    private string GetPlayDataFilePath(string playDataId)
    {
        //return Application.persistentDataPath + $"/{playDataId}.json";
        return Application.dataPath + "/Scripts/JSON/"+ playDataId+".json";
    }

    public void LoadGameData()
    {
        string filePath = GetGameDataFilePath();

        Debug.Log("이거 호출");
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            print(filePath);
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            // 새 파일이 없을 경우, 새로운 GameData 객체 생성
            _gameData = new GameData();  // 기본 GameData 객체 생성
            ToGameJson(); // 생성된 GameData를 JSON 파일로 저장
        }
    }


    [ContextMenu("To Game Json")]
    public void ToGameJson()
    {
        File.WriteAllText(GetGameDataFilePath(), JsonUtility.ToJson(gameData, true));
    }


    public void LoadPlayData(string playDataID)
    {
        string filePath = GetPlayDataFilePath(playDataID);
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            Debug.Log(filePath);
            string FromJsonData = File.ReadAllText(filePath);
            _playData = JsonUtility.FromJson<PlayData>(FromJsonData);
        }
        else
        {
            Debug.Log("파일 새로 생성");
            // 새 파일이 없을 경우, 새로운 PlayData 객체 생성
            _playData = new PlayData();  // 기본 PlayData 객체 생성
            ToPlayJson(playDataID); // 생성된 PlayData를 JSON 파일로 저장
            gameData.save_files_IDs.Add(playDataID);
        }
    }

    public void DeletePlayData(string playDataID)
    {
        string filePath = GetPlayDataFilePath(playDataID);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            if (gameData.save_files_IDs.Contains(playDataID))
            {
                gameData.save_files_IDs.Remove(playDataID);
            }
        }
    }

    public void ToPlayJson(string playDataID)
    {
        File.WriteAllText(GetPlayDataFilePath(playDataID), JsonUtility.ToJson(playData, true));
    }


    void OnApplicationQuit()
    {
        ToGameJson();
        ToPlayJson(gameData.cur_save_file_ID);//종료할때 있어야함
    }

    IEnumerator SaveGameData()
    {
        while (true)
        {
            File.WriteAllText(GetGameDataFilePath(), JsonUtility.ToJson(gameData, true));

            try
            {
                SW = new System.IO.StreamWriter(GetGameDataFilePath());
                //SW = new System.IO.StreamWriter(Application.dataPath + "/Scripts/JSON/GameData.json");
            }
            catch (Exception exp)
            {
                UnityEngine.Debug.Log(exp);
            }
            finally
            {
                if (SW != null)
                {
                    SW.Close();
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

    }

    IEnumerator SavePlayData(string playDataID)
    {
        while (true)
        {
            File.WriteAllText(GetPlayDataFilePath(playDataID), JsonUtility.ToJson(playData, true));

            try
            {
                SW = new System.IO.StreamWriter(GetPlayDataFilePath(playDataID));
                //SW = new System.IO.StreamWriter(Application.dataPath + "/Scripts/JSON/PlayData.json");
            }
            catch (Exception exp)
            {
                UnityEngine.Debug.Log(exp);
            }
            finally
            {
                if (SW != null)
                {
                    SW.Close();
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

    }

}